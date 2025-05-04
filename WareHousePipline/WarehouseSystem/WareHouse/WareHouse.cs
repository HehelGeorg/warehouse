using warehouse.Item;
namespace warehouse;

public class Warehouse
{
    public int WarehouseId { get; }
    public string WarehouseName { get; set; }
    public string WarehouseAddress { get; set; }
    public string Owner { get; set; }
    public WarehouseType WarehouseType { get; set; }
    public List<StoredItem> StoredItems { get; private set; } = new List<StoredItem>();
    //иммитация работы БД
    private static int nextWarehouseId = 0;

    public Warehouse(string warehouseName, string warehouseAddress, string owner, WarehouseType warehouseType)
    {
        WarehouseId = nextWarehouseId++;
        WarehouseName = warehouseName;
        WarehouseAddress = warehouseAddress;
        Owner = owner;
        WarehouseType = warehouseType;
    }

    public void AddItemToWarehouse(StoredItem item)
    {
        StoredItems.Add(item);
        Console.WriteLine($"Товар '{item.Manufacturer} {item.Model}' (ID: {item.ItemId}) добавлен на склад '{WarehouseName}'.");
    }

    public StoredItem? GetItemById(int itemId)
    {
        return StoredItems.FirstOrDefault(item => item.ItemId == itemId);
    }
}

public enum WarehouseType
{
    General,
    Refrigerated,
    Hazardous
}