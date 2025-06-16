

namespace Vectaurant.FrontDesk.Orders;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<OrderItem> Items { get; set; } = [];
}
