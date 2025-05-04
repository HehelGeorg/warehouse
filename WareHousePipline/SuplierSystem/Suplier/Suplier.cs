namespace SuplierSystem.Suplier;

public class Supplier
{
    public int SupplierId { get; }
    public string Name { get; set; }
    public string ContactInformation { get; set; }
    
    private static int NextSuplierId = 0;

    public Supplier(int supplierId, string name, string contactInformation)
    {
        SupplierId = NextSuplierId++;
        Name = name;
        ContactInformation = contactInformation;
    }
}