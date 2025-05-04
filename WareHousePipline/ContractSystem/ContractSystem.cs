namespace WareHousePipline.ContractSystem;

public class ContractSystem
{
    private Dictionary<int, Contract> _contracts = new Dictionary<int, Contract>();

    public Contract? GetContractBySupplierId(int supplierId)
    {
        return _contracts.Values.FirstOrDefault(c => c.IsSupplierSigned(supplierId));
    }

    

    public void RecordOperation(int contractId, string description, decimal amount)
    {
        if (_contracts.TryGetValue(contractId, out var contract))
        {
            Console.WriteLine($"Операция по контракту №'{contractId}': {description}, Сумма: {amount} ({description})");
            contract.AddToTotalAmount(amount);
        }
        else
        {
            Console.WriteLine($"Контракт с ID '{contractId}' не найден.");
        }
    }

    public void AddNewContract(Contract contract)
    {
        _contracts[contract.ContractId] = contract;
    }
}