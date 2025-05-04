using OperationContext;
using OperationStep;
using SuplierSystem;
using SuplierSystem.Suplier;
using warehouse;
using warehouse.Item;
using WareHousePipline.ContractSystem;

namespace WareHousePipline.PipelineSteps;


public class SupplyOperationContext : OperationContextBase
{
    public int SupplierId { get; set; }
    public string Password { get; set; }
    public List<StoredItem> ItemsToSupply { get; set; } = new List<StoredItem>();
    public Supplier? AuthenticatedSupplier { get; set; }
    public Contract? SupplierContract { get; set; }
    public Warehouse? Warehouse { get; set; }
    public ContractSystem.ContractSystem? ContractSystem { get; set; }
    public SupplierAuthenticator? Authenticator { get; set; }

    // Новые поля для регистрации поставщика (могут быть nullable, если не используются всегда)
    public bool ShouldRegisterNewSupplier { get; set; } = false;
    public string? NewSupplierPassword { get; set; }
}