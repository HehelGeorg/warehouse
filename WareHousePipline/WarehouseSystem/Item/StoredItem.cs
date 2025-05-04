namespace warehouse.Item;

public class StoredItem : Item
{
    public int Quantity { get; set; }
    public ItemStatus Status { get; set; }
    public decimal StorageCost { get; set; }
    public DateTime ArrivalDateTime { get; set; }
    public DateTime? ExpirationDateTime { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? DeliveryPlace { get; set; }

    public StoredItem(string manufacturer, string model, Dimensions dimensions, float weight, string storageConditions, string category, int quantity, DateTime arrivalDateTime, decimal price = 0)
        : base(manufacturer, model, dimensions, weight, storageConditions, category, price)
    {
        Quantity = quantity;
        Status = ItemStatus.Available;
        ArrivalDateTime = arrivalDateTime;
    }
}

public enum ItemStatus
{
    Available,
    Reserved,
    Defective,
    Stopped
}