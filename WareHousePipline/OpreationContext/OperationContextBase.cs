namespace OperationContext;

public abstract class OperationContextBase
{
    // Общие свойства для всех контекстов операций могут быть определены здесь
    public Dictionary<string, object> Data { get; } = new Dictionary<string, object>();

    public T? Get<T>(string key) where T : class
    {
        if (Data.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }
        return null;
    }

    public void Set(string key, object value)
    {
        Data[key] = value;
    }
}