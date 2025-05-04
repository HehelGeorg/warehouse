using OperationContext;
using OperationStep;
using warehouse;
using WareHousePipline.PipelineSteps;

namespace WareHousePipline.OpreationContext;

public class AddItemsToWarehouseStep : IOperationStep
{
    public async Task<bool> ExecuteAsync(OperationContextBase context)
    {
        if (context is not SupplyOperationContext supplyContext)
        {
            Console.WriteLine("AddItemsToWarehouseStep: Некорректный тип контекста.");
            return false;
        }

        var warehouse = supplyContext.Get<Warehouse>("Warehouse");
        if (warehouse == null)
        {
            Console.WriteLine("AddItemsToWarehouseStep: Warehouse не найден в контексте.");
            return false;
        }

        if (supplyContext.ItemsToSupply == null || !supplyContext.ItemsToSupply.Any())
        {
            Console.WriteLine("AddItemsToWarehouseStep: Нет товаров для добавления.");
            return true; // Ничего не делаем, но и не ошибка
        }

        Console.WriteLine("AddItemsToWarehouseStep: Добавление товаров на склад...");
        foreach (var item in supplyContext.ItemsToSupply)
        {
            warehouse.AddItemToWarehouse(item);
        }
        Console.WriteLine("AddItemsToWarehouseStep: Товары успешно добавлены на склад.");
        return true;
    }
}