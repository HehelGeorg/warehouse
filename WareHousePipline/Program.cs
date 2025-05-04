using OperationStep;
using SuplierSystem;
using warehouse;
using warehouse.Item;
using WareHousePipline;
using WareHousePipline.ContractSystem;
using WareHousePipline.OperationHandlers;
using WareHousePipline.OperationSteps;
using WareHousePipline.OpreationContext;


namespace WareHouseConsole
{
    class Program
    {
        private static Warehouse _warehouse;
        private static ContractSystem _contractSystem;
        private static SupplierAuthenticator _authenticator;
        private static OperationPipeline _pipeline;
        private static SupplyOperationHandler _supplyHandler;

        static async Task Main(string[] args)
        {
            // Инициализация
            _warehouse = new Warehouse("Основной склад", "ул. Консольная, 1", "Консольный Менеджер", WarehouseType.General);
            _contractSystem = new ContractSystem();
            _authenticator = new SupplierAuthenticator();
            _pipeline = new OperationPipeline();
            _supplyHandler = new SupplyOperationHandler(_pipeline, _authenticator, _warehouse, _contractSystem);

            bool running = true;
            while (running)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Зарегистрировать и поставить товары");
                Console.WriteLine("2. Создать новый контракт");
                Console.WriteLine("3. Поставить товары на склад (существующий поставщик)");
                Console.WriteLine("4. Посмотреть содержимое склада");
                Console.WriteLine("5. Посмотреть информацию о контракте поставщика");
                Console.WriteLine("0. Выход");

                Console.Write("Ваш выбор: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await RegisterAndSupply();
                        break;
                    case "2":
                        CreateContract();
                        break;
                    case "3":
                        await SupplyItems();
                        break;
                    case "4":
                        ViewWarehouseContents();
                        break;
                    case "5":
                        ViewSupplierContractInfo();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("Выход из программы.");
                        break;
                    default:
                        Console.WriteLine("Некорректный выбор. Пожалуйста, попробуйте снова.");
                        break;
                }
            }
        }

        static async Task RegisterAndSupply()
        {
            Console.Write("Введите ID нового поставщика: ");
            if (int.TryParse(Console.ReadLine(), out int supplierId))
            {
                Console.Write("Введите пароль для нового поставщика: ");
                string password = Console.ReadLine();

                List<StoredItem> itemsToSupply =  await GetItemsToSupply();

                if (itemsToSupply.Count > 0)
                {
                    bool success = await _supplyHandler.ProcessSupplyAsync(supplierId, password, itemsToSupply, true, password); // Регистрируем при поставке
                    Console.WriteLine($"Поставка от нового поставщика ID {supplierId}: {(success ? "Успешно" : "Неудачно")}");
                }
                else
                {
                    Console.WriteLine("Нет товаров для поставки.");
                }
            }
            else
            {
                Console.WriteLine("Некорректный формат ID поставщика.");
            }
        }

        static async Task SupplyItems()
        {
            Console.Write("Введите ID поставщика: ");
            if (int.TryParse(Console.ReadLine(), out int supplierId))
            {
                Console.Write("Введите пароль поставщика: ");
                string password = Console.ReadLine();

                List<StoredItem> itemsToSupply = await GetItemsToSupply();

                if (itemsToSupply.Count > 0)
                {
                    bool success = await _supplyHandler.ProcessSupplyAsync(supplierId, password, itemsToSupply); // Поставка для существующего
                    Console.WriteLine($"Поставка от поставщика ID {supplierId}: {(success ? "Успешно" : "Неудачно")}");
                }
                else
                {
                    Console.WriteLine("Нет товаров для поставки.");
                }
            }
            else
            {
                Console.WriteLine("Некорректный формат ID поставщика.");
            }
        }

        static async Task<List<StoredItem>> GetItemsToSupply()
        {
            List<StoredItem> itemsToSupply = new List<StoredItem>();
            bool addingItems = true;
            while (addingItems)
            {
                Console.WriteLine("\nДобавление товара для поставки (или 'готово'):");
                Console.Write("Производитель: ");
                string manufacturer = Console.ReadLine();
                if (manufacturer.ToLower() == "готово")
                {
                    addingItems = false;
                    break;
                }
                Console.Write("Модель: "); string model = Console.ReadLine();
                Console.Write("Длина: "); float.TryParse(Console.ReadLine(), out float length);
                Console.Write("Ширина: "); float.TryParse(Console.ReadLine(), out float width);
                Console.Write("Высота: "); float.TryParse(Console.ReadLine(), out float height);
                Console.Write("Вес: "); float.TryParse(Console.ReadLine(), out float weight);
                Console.Write("Условия хранения: "); string storageConditions = Console.ReadLine();
                Console.Write("Категория: "); string category = Console.ReadLine();
                Console.Write("Количество: "); int.TryParse(Console.ReadLine(), out int quantity);
                Console.Write("Цена: "); decimal.TryParse(Console.ReadLine(), out decimal price);

                itemsToSupply.Add(new StoredItem(manufacturer, model, new Dimensions(), weight, storageConditions, category, quantity, DateTime.Now, price));
            }
            return itemsToSupply;
        }

        static void CreateContract()
        {
            Console.Write("Введите стандартный тариф контракта: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal standardRate))
            {
                Console.Write("Введите ID поставщиков, которые будут подписаны на этот контракт (через запятую): ");
                string[] supplierIdsStr = Console.ReadLine().Split(',');
                List<int> supplierIds = new List<int>();
                foreach (var idStr in supplierIdsStr)
                {
                    if (int.TryParse(idStr.Trim(), out int id))
                    {
                        supplierIds.Add(id);
                    }
                    else
                    {
                        Console.WriteLine($"Некорректный ID поставщика: {idStr}. Пропущен.");
                    }
                }
                var newContract = new Contract(standardRate, supplierIds.ToArray());
                _contractSystem.AddNewContract(newContract);
                Console.WriteLine($"Контракт ID {newContract.ContractId} создан.");
            }
            else
            {
                Console.WriteLine("Некорректный формат стандартного тарифа.");
            }
        }

        static void ViewWarehouseContents()
        {
            Console.WriteLine("\nСодержимое склада:");
            if (_warehouse.StoredItems.Count == 0)
            {
                Console.WriteLine("Склад пуст.");
            }
            else
            {
                foreach (var item in _warehouse.StoredItems)
                {
                    Console.WriteLine($"- ID: {item.ItemId}, {item.Manufacturer} {item.Model}, Кол-во: {item.Quantity}");
                }
            }
        }

        static void ViewSupplierContractInfo()
        {
            Console.Write("Введите ID поставщика для просмотра информации о контракте: ");
            if (int.TryParse(Console.ReadLine(), out int supplierId))
            {
                var contract = _contractSystem.GetContractBySupplierId(supplierId);
                if (contract != null)
                {
                    Console.WriteLine($"\nИнформация о контракте для поставщика ID {supplierId}:");
                    Console.WriteLine($"  ID контракта: {contract.ContractId}");
                    Console.WriteLine($"  Стандартный тариф: {contract.StandardRate}");
                    Console.WriteLine($"  Подписаны поставщики: {string.Join(", ", contract.SupplierIds)}");
                    Console.WriteLine($"  Общая сумма по контракту: {contract.TotalAmount}");
                    if (contract.SpecialRates.Count > 0)
                    {
                        Console.WriteLine("  Специальные тарифы:");
                        foreach (var rate in contract.SpecialRates)
                        {
                            Console.WriteLine($"    - {rate.Key}: {rate.Value}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("  Нет специальных тарифов.");
                    }
                }
                else
                {
                    Console.WriteLine($"Контракт для поставщика ID {supplierId} не найден.");
                }
            }
            else
            {
                Console.WriteLine("Некорректный формат ID поставщика.");
            }
        }
    }
}