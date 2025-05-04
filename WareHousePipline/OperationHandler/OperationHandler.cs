using OperationStep;
using SuplierSystem;
using warehouse;
using warehouse.Item;
using WareHousePipline.ContractSystem;
using WareHousePipline.OperationSteps;
using WareHousePipline.OpreationContext;

namespace WareHousePipline.OperationHandlers;


using WareHousePipline.PipelineSteps;

public class SupplyOperationHandler : AbstractOperationHandler
{
    private readonly SupplierAuthenticator _authenticator;
    private readonly Warehouse _warehouse;
    private readonly ContractSystem.ContractSystem _contractSystem;

    public SupplyOperationHandler(IOperationPipeline pipeline, SupplierAuthenticator authenticator, Warehouse warehouse,
        ContractSystem.ContractSystem contractSystem)
        : base(pipeline)
    {
        _authenticator = authenticator;
        _warehouse = warehouse;
        _contractSystem = contractSystem;
    }

    protected override void ConfigurePipeline()
    {
        _pipeline.AddStep(new AuthenticationStep());
        _pipeline.AddStep(new CheckContractStep());
        _pipeline.AddStep(new AddItemsToWarehouseStep());
        _pipeline.AddStep(new RecordSupplyOperationStep());
    }

    public async Task<bool> ProcessSupplyAsync(int supplierId, string password, List<StoredItem> itemsToSupply,
        bool registerNewSupplier = false, string? newSupplierPassword = null)
    {
        var context = new SupplyOperationContext()
        {

            SupplierId = supplierId,
            Password = password,
            ItemsToSupply = itemsToSupply,
            Warehouse = _warehouse,
            ContractSystem = _contractSystem,
            Authenticator = _authenticator,
            ShouldRegisterNewSupplier = registerNewSupplier,
            NewSupplierPassword = newSupplierPassword
        };
        context.Set("Warehouse", _warehouse);
        context.Set("ContractSystem", _contractSystem);
        context.Set("Authenticator", _authenticator);

        return await HandleAsync(context);
    }
}