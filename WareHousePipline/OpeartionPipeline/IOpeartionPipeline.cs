using OperationContext;
using OperationStep;
namespace WareHousePipline;

public interface IOperationPipeline
{
    public Task<bool> ProcessAsync(OperationContextBase context);
    
    public void AddStep(IOperationStep step);
    
    IReadOnlyCollection<IOperationStep> GetSteps();
}