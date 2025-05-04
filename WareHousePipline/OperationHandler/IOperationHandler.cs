using OperationContext;
using WareHousePipline;

public abstract class AbstractOperationHandler
{
    protected readonly IOperationPipeline _pipeline;

    protected AbstractOperationHandler(IOperationPipeline pipeline)
    {
        _pipeline = pipeline;
        ConfigurePipeline(); // Настраиваем конвейер при создании обработчика
    }

    public virtual async Task<bool> HandleAsync(OperationContextBase context)
    {
        return await _pipeline.ProcessAsync(context);
    }
    
    protected abstract void ConfigurePipeline();
}