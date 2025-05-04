using OperationContext;
namespace OperationStep;

public interface IOperationStep
{

    public Task<bool> ExecuteAsync(OperationContextBase context);
    
}