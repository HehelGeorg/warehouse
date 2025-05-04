namespace WareHousePipline.ContractSystem;

public class Contract
{
    public int ContractId { get; }
    public List<int> SupplierIds { get; } = new List<int>();
    public decimal StandardRate { get; private set; }
    public Dictionary<string, decimal> SpecialRates { get; private set; } = new Dictionary<string, decimal>();

    public decimal TotalAmount { get; private set; } = 0; // Для учета общей суммы по контракту

    //иммитация БД
    private static int nextContractId = 0;

    public Contract(decimal standardRate, params int[] supplierIds)
    {
        ContractId = nextContractId++;

        StandardRate = standardRate;
        foreach (int id in supplierIds)
        {
            SupplierIds.Add(id); // да, я знаю о наличии {ListTypeObject}.AddRange()
        }
    }

    public void AddSpecialRate(string itemCategory, decimal rate)
    {
        SpecialRates[itemCategory] = rate;
    }

    public decimal GetRate(string itemCategory)
    {
        if (SpecialRates.TryGetValue(itemCategory, out decimal rate))
        {
            return rate;
        }

        return StandardRate;
    }

    public void AddToTotalAmount(decimal amount)
    {
        TotalAmount += amount;
    }

    public bool IsSupplierSigned(int supplierId)
    {
        foreach (int id in SupplierIds)
        {
            if (id == supplierId)
            {
                return true;
            }
        }

        return false;
    }

}