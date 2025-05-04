using OperationContext;
using OperationStep;


namespace  WareHousePipline;


public class OperationPipeline : IOperationPipeline
{
    private readonly List<IOperationStep> _steps = new List<IOperationStep>();

    public async Task<bool> ProcessAsync(OperationContextBase context)
    {
        foreach (var step in _steps)
        {
            Console.WriteLine($"Выполнение шага: {step.GetType().Name}");
            // Проверяем выполнилась ли операция
            if (!await step.ExecuteAsync(context))
            {
                Console.WriteLine($"Шаг '{step.GetType().Name}' завершился неудачей. Операция прервана.");
                return false;
            }
        }
        Console.WriteLine("Операция успешно завершена.");
        return true;
    }
    
    
    
    public void AddStep(IOperationStep step)
    {
        _steps.Add(step);
    }

    public IReadOnlyCollection<IOperationStep> GetSteps()
    {
        return _steps.AsReadOnly();
    }
}