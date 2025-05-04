using SuplierSystem.Suplier;
namespace SuplierSystem;

public class SupplierAuthenticator
{
    private Dictionary<int, string> _supplierCredentials = new Dictionary<int, string>();

    public void RegisterSupplier(int supplierId, string password)
    {
        _supplierCredentials[supplierId] = password;
        Console.WriteLine($"Зарегистрирован поставщик с ID '{supplierId}'.");
    }

    public Supplier? Authenticate(int supplierId, string password)
    {
        if (_supplierCredentials.TryGetValue(supplierId, out var storedPassword) && storedPassword == password)
        {
            return new Supplier(supplierId, $"Поставщик {supplierId}", "Информация");
        }
        Console.WriteLine($"Аутентификация поставщика ID '{supplierId}' не удалась. Тока не брутфорсай, бро");
        return null;
    }

    public bool IsSupplierSAuth(int id)
    {
        foreach (int _id in _supplierCredentials.Keys)
        {
            if (id == _id)
            {
                return true;
            }

            
        }
        return false;
    }
}
