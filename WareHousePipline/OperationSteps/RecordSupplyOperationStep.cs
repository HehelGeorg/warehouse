using OperationContext;
using OperationStep;
using WareHousePipline.PipelineSteps;

namespace WareHousePipline.OperationSteps;

public class RecordSupplyOperationStep : IOperationStep
{
    public async Task<bool> ExecuteAsync(OperationContextBase context)
    {
        if (context is not SupplyOperationContext supplyContext)
        {
            Console.WriteLine("RecordSupplyOperationStep: Некорректный тип контекста.");
            return false;
        }

        var contractSystem = supplyContext.Get<ContractSystem.ContractSystem>("ContractSystem");
        if (contractSystem == null)
        {
            Console.WriteLine("RecordSupplyOperationStep: ContractSystem не найден в контексте.");
            return false;
        }

        var supplierContract = supplyContext.SupplierContract;
        var itemsToSupply = supplyContext.ItemsToSupply;

        if (supplierContract == null || itemsToSupply == null || itemsToSupply.Count() == 0)
        {
            Console.WriteLine("RecordSupplyOperationStep: Недостаточно данных для учета операции.");
            return false;
        }

        decimal totalAmount = itemsToSupply.Sum(item => item.Price * item.Quantity);
        contractSystem.RecordOperation(supplierContract.ContractId, $"Поставка {itemsToSupply.Count} товаров", totalAmount);
        Console.WriteLine($"RecordSupplyOperationStep: Операция поставки по контракту №'{supplierContract.ContractId}' учтена на сумму '{totalAmount}'.");
        return true;
    }
}