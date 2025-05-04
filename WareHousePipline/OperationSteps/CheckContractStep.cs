using OperationContext;
using OperationStep;
using WareHousePipline.PipelineSteps;

namespace WareHousePipline.OpreationContext;


public class CheckContractStep : IOperationStep
{
    public async Task<bool> ExecuteAsync(OperationContextBase context)
    {
        if (context is not SupplyOperationContext supplyContext)
        {
            Console.WriteLine("CheckContractStep: Некорректный тип контекста.");
            return false;
        }

        var contractSystem = supplyContext.Get<ContractSystem.ContractSystem>("ContractSystem");
        if (contractSystem == null)
        {
            Console.WriteLine("CheckContractStep: ContractSystem не найден в контексте.");
            return false;
        }

        var authenticatedSupplier = supplyContext.AuthenticatedSupplier;
        if (authenticatedSupplier == null)
        {
            Console.WriteLine("CheckContractStep: Аутентифицированный поставщик не найден в контексте.");
            return false;
        }

        supplyContext.SupplierContract = contractSystem.GetContractBySupplierId(authenticatedSupplier.SupplierId);
        if (supplyContext.SupplierContract == null)
        {
            Console.WriteLine($"CheckContractStep: Для поставщика '{authenticatedSupplier.Name}' не найден действующий контракт.");
            return false;
        }

        Console.WriteLine($"CheckContractStep: Найден контракт №'{supplyContext.SupplierContract.ContractId}'.");
        context.Set("SupplierContract", supplyContext.SupplierContract); // Сохраняем для следующих шагов
        return true;
    }
}