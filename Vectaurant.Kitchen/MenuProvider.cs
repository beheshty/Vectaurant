using Vectaurant.Shared;

namespace Vectaurant.Kitchen;

public static class MenuProvider
{
    //This method can be replaced with any implementation that reads data from a database or another data source.
    public static List<MenuItem> GetMenuItems()
    {
        return
        [
            new() { Id = 1, Name = "Bruschetta", Description = "Grilled bread topped with garlic, tomatoes, olive oil, and basil.", Category = "Appetizer", AvailableCount = 10, Price = 6.5 },
            new() { Id = 2, Name = "Caesar Salad", Description = "Fresh romaine lettuce with Caesar dressing, croutons, and parmesan.", Category = "Appetizer", AvailableCount = 8, Price = 7.0 },
            new() { Id = 3, Name = "Grilled Salmon", Description = "Tender grilled salmon served with lemon butter sauce and vegetables.", Category = "Main Course", AvailableCount = 5, Price = 18.0 },
            new() { Id = 4, Name = "Spaghetti Carbonara", Description = "Classic Italian pasta with pancetta, eggs, parmesan, and pepper.", Category = "Main Course", AvailableCount = 7, Price = 14.0 },
            new() { Id = 5, Name = "Margherita Pizza", Description = "Traditional pizza with tomato sauce, mozzarella, and fresh basil.", Category = "Main Course", AvailableCount = 4, Price = 10.0 },
            new() { Id = 6, Name = "Pepperoni Pizza", Description = "Spicy pepperoni slices layered over melted mozzarella and tomato sauce.", Category = "Main Course", AvailableCount = 3, Price = 12.0 },
            new() { Id = 7, Name = "BBQ Chicken Pizza", Description = "Grilled chicken, red onions, BBQ sauce, and mozzarella on a crispy crust.", Category = "Main Course", AvailableCount = 2, Price = 13.5 },
            new() { Id = 8, Name = "Vegetarian Pizza", Description = "Loaded with bell peppers, olives, mushrooms, onions, and mozzarella.", Category = "Main Course", AvailableCount = 5, Price = 11.5 },
            new() { Id = 9, Name = "Tiramisu", Description = "Layered dessert with coffee-soaked ladyfingers and mascarpone cream.", Category = "Dessert", AvailableCount = 6, Price = 6.5 },
            new() { Id = 10, Name = "Chocolate Lava Cake", Description = "Warm chocolate cake with a molten center, served with vanilla ice cream.", Category = "Dessert", AvailableCount = 5, Price = 7.0 },
            new() { Id = 11, Name = "Espresso", Description = "Strong and rich Italian coffee shot.", Category = "Beverage", AvailableCount = 15, Price = 3.0 },
            new() { Id = 12, Name = "Lemonade", Description = "Refreshing homemade lemonade with mint and a touch of honey.", Category = "Beverage", AvailableCount = 10, Price = 4.0 },
            new() { Id = 13, Name = "Grilled Chicken Sandwich", Description = "Juicy grilled chicken breast with lettuce, tomato, and aioli on a brioche bun.", Category = "Main Course", AvailableCount = 6, Price = 12.5 }
        ];
    }
}
