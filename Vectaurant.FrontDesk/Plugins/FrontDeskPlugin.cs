using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using Vectaurant.FrontDesk.Orders;
using Vectaurant.Shared;
using Vectaurant.Shared.MenuItems;

namespace Vectaurant.FrontDesk.Plugins
{
    public class FrontDeskPlugin
    {
        private readonly VectorStore _vectorStore;
        private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;
        private readonly MenuItemRepository _menuItemRepository;
        private readonly List<Order> _orderList;
        private Order _order;

        public FrontDeskPlugin(VectorStore memoryStore, 
            IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator,
            MenuItemRepository menuItemRepository)
        {
            _vectorStore = memoryStore;
            _embeddingGenerator = embeddingGenerator;
            _menuItemRepository = menuItemRepository;
            _orderList = [];
        }

        [KernelFunction, Description("Searches the restaurant menu to answer questions about available items. return matching items as a json objects on each line.")]
        public async Task<string> SearchMenuAsync(
            [Description("A natural language question or request that helps retrieve relevant items from the menu")] string question)
        {
            var qdrandConfig = ConfigurationLoader.GetSection<QdrandOptions>(nameof(QdrandOptions));

            var collection = _vectorStore.GetCollection<ulong, MenuItem>(qdrandConfig.CollectionName);

            var searchVector = await _embeddingGenerator.GenerateAsync(question);

            var searchResults = collection.SearchAsync(searchVector.Vector, 3);

            var responseBuilder = new StringBuilder();
            int count = 0;
            await foreach (var result in searchResults)
            {
                if (result.Score > 0.3)
                {
                    responseBuilder.AppendLine(JsonSerializer.Serialize(result.Record));
                    count++;
                }
            }
            return count == 0 ? "I couldn't find any item on the menu that match your request." : responseBuilder.ToString();
        }

        [KernelFunction, Description("Adds a menu item to the current order. If no order is open, a new order is created. Returns the current order as a JSON string or an error message.")]
        public async Task<string> AddItemToOrderAsync(
           [Description("The unique ID of the menu item to add")] ulong menuItemId,
           [Description("The quantity of the menu item to add")] short quantity)
        {
            if (quantity <= 0)
                return "Quantity must be greater than zero.";

            var menuItem = _menuItemRepository.GetMenuItemById(menuItemId);
            if (menuItem == null)
                return $"Menu item with ID {menuItemId} not found.";

            if (!menuItem.TryReserve(quantity))
                return $"Not enough '{menuItem.Name}' available to reserve {quantity}.";

            _order ??= new Order();

            var orderItem = _order.Items.FirstOrDefault(oi => oi.MenuItemId == menuItemId);
            if (orderItem != null)
            {
                orderItem.Quantity += quantity;
            }
            else
            {
                _order.Items.Add(new OrderItem
                {
                    MenuItemId = menuItemId,
                    Quantity = quantity
                });
            }

            return await Task.FromResult(System.Text.Json.JsonSerializer.Serialize(_order));
        }

        [KernelFunction, Description("Completes the current order, adds it to the order list, and closes the order. Returns a confirmation message or an error if no order is open.")]
        public async Task<string> CompleteOrderAsync()
        {
            if (_order == null || _order.Items == null || _order.Items.Count == 0)
                return "No open order to complete.";

            _orderList.Add(_order);
            var completedOrderId = _order.Id;
            _order = null;
            return await Task.FromResult($"Order {completedOrderId} has been completed and added to the order list.");
        }
    }
}
