namespace warehouse.Item;

public class Item
{
    public int ItemId { get; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    //заглушка
    public Dimensions Dimensions { get; set; }
    public float Weight { get; set; }
    public string StorageConditions { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; } // Цена товара (не складская)
    //иммитация работы БД
    private static int nextItemId = 1;

    public Item(string manufacturer, string model, Dimensions dimensions, float weight, string storageConditions, string category, decimal price = 0)
    {
        ItemId = nextItemId++;
        Manufacturer = manufacturer;
        Model = model;
        Dimensions = dimensions;
        Weight = weight;
        StorageConditions = storageConditions;
        Category = category;
        Price = price;
    }
}