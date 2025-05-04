using OperationContext;
using SuplierSystem;
using WareHousePipline.PipelineSteps;

namespace OperationStep;


public class AuthenticationStep : IOperationStep
{
    public async Task<bool> ExecuteAsync(OperationContextBase context)
    {
        if (context is not SupplyOperationContext supplyContext)
        {
            Console.WriteLine("AuthenticationStep: Некорректный тип контекста.");
            return false;
        }

        var authenticator = supplyContext.Get<SupplierAuthenticator>("Authenticator");
        if (authenticator == null)
        {
            Console.WriteLine("AuthenticationStep: SupplierAuthenticator не найден в контексте.");
            return false;
        }

        // Попытка аутентификации
        supplyContext.AuthenticatedSupplier = authenticator.Authenticate(supplyContext.SupplierId, supplyContext.Password);

        // Если аутентификация не удалась и установлен флаг регистрации
        if (supplyContext.AuthenticatedSupplier == null && supplyContext.ShouldRegisterNewSupplier)
        {
            if (string.IsNullOrEmpty(supplyContext.NewSupplierPassword))
            {
                Console.WriteLine($"AuthenticationStep: Регистрация нового поставщика запрошена, но пароль не указан.");
                return false;
            }

            authenticator.RegisterSupplier(supplyContext.SupplierId, supplyContext.NewSupplierPassword);
            Console.WriteLine($"AuthenticationStep: Новый поставщик ID '{supplyContext.SupplierId}' успешно зарегистрирован.");
            // Попытка аутентификации снова после регистрации
            supplyContext.AuthenticatedSupplier = authenticator.Authenticate(supplyContext.SupplierId, supplyContext.Password);
        }

        if (supplyContext.AuthenticatedSupplier == null)
        {
            Console.WriteLine($"AuthenticationStep: Аутентификация поставщика ID '{supplyContext.SupplierId}' не удалась.");
            return false;
        }

        Console.WriteLine($"AuthenticationStep: Поставщик '{supplyContext.AuthenticatedSupplier.Name}' успешно аутентифицирован.");
        context.Set("AuthenticatedSupplier", supplyContext.AuthenticatedSupplier);
        return true;
    }
}