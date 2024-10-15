using BD_lab_2.Models;
using Microsoft.IdentityModel.Tokens;

namespace BD_lab_2.Models
{
    internal class Program
    {
        static void Print<T>(string sqlText, IEnumerable<T>? items)
        {
            Console.WriteLine(sqlText);
            Console.WriteLine("Записи: ");
            if (!items.IsNullOrEmpty())
            {
                foreach (var item in items)
                {
                    Console.WriteLine(item!.ToString());
                }
            }
            else
            {
                Console.WriteLine("Пусто");
            }
            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу");
            Console.ReadKey();
        }

        static void Select(ProductionContext db)
        {
            var queryLINQ1 = from pt in db.ProductTypes
                             select new
                             {
                                 Название_Типа_Продукта = pt.Name
                             };
            string comment = "1. Результат выполнения запроса на выборку всех данных из таблицы ProductTypes";
            Print(comment, queryLINQ1.Take(10).ToList());

            var queryLINQ2 = from p in db.Products
                             where p.Name.Contains("Product A") 
                             select new
                             {
                                 Название_Продукта = p.Name
                             };
            comment = "2. Результат выполнения запроса на выборку данных из таблицы Products с фильтрацией по названию";
            Print(comment, queryLINQ2.ToList());

            var queryLINQ3 = db.ProductionPlans
                               .GroupBy(pp => pp.EnterpriseId)
                               .Select(gr => new
                               {
                                   Идентификатор_Предприятия = gr.Key,
                                   Средний_Запланированный_Объем = gr.Average(pp => pp.PlannedVolume)
                               });
            comment = "3. Результат выполнения запроса на выборку сгруппированных данных из таблицы ProductionPlans с вычислением среднего объема";
            Print(comment, queryLINQ3.Take(10).ToList());

            var queryLINQ4 = from p in db.Products
                             from pt in p.ProductTypes
                             select new
                             {
                                 Название_Продукта = p.Name,
                                 Тип_Продукта = pt.Name
                             };
            comment = "4. Результат выполнения запроса на выборку данных из таблиц Products и ProductTypes";
            Print(comment, queryLINQ4.Take(10).ToList());

            var queryLINQ5 = from pp in db.ProductionPlans
                             join e in db.Enterprises on pp.EnterpriseId equals e.EnterpriseId
                             where pp.PlannedVolume > 5000 
                             select new
                             {
                                 Название_Предприятия = e.Name,
                                 Запланированный_Объем = pp.PlannedVolume
                             };
            comment = "5. Результат выполнения запроса на выборку данных из ProductionPlans и Enterprises с фильтрацией";
            Print(comment, queryLINQ5.Take(10).ToList());
        }

        static void Insert(ProductionContext db)
        {
            var newProductType = new ProductType
            {
                Name = "New Product Type"
            };
            db.ProductTypes.Add(newProductType);
            db.SaveChanges();

            string comment = "Выборка типов продуктов после вставки нового типа";
            var queryLINQ1 = from pt in db.ProductTypes
                             where pt.Name == "New Product Type"
                             select new
                             {
                                 Название_Типа = pt.Name
                             };
            Print(comment, queryLINQ1.ToList());

            var newProduct = new Product
            {
                Name = "New Product"
            };
            db.Products.Add(newProduct);
            db.SaveChanges();

            comment = "Выборка продуктов после вставки нового продукта";
            var queryLINQ2 = from p in db.Products
                             where p.Name == "New Product"
                             select new
                             {
                                 Название_Продукта = p.Name
                             };
            Print(comment, queryLINQ2.ToList());
        }

        static void Delete(ProductionContext db)
        {
            var productType = db.ProductTypes.FirstOrDefault(pt => pt.Name == "New Product Type");
            if (productType != null)
            {
                db.ProductTypes.Remove(productType);
                db.SaveChanges();
            }

            string comment = "Выборка типов продуктов после удаления";
            var queryLINQ1 = from pt in db.ProductTypes
                             where pt.Name == "New Product Type"
                             select new
                             {
                                 Название_Типа = pt.Name
                             };
            Print(comment, queryLINQ1.ToList());

            var product = db.Products.FirstOrDefault(p => p.Name == "New Product");
            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
            }

            comment = "Выборка продуктов после удаления";
            var queryLINQ2 = from p in db.Products
                             where p.Name == "New Product"
                             select new
                             {
                                 Название_Продукта = p.Name
                             };
            Print(comment, queryLINQ2.ToList());
        }

        static void Update(ProductionContext db)
        {
            var plansToUpdate = db.ProductionPlans.Where(pp => pp.PlannedVolume < 5000);
            if (!plansToUpdate.IsNullOrEmpty())
            {
                foreach (var plan in plansToUpdate)
                {
                    plan.PlannedVolume += 1000;
                }
                db.SaveChanges();
            }

            string comment = "Выборка ProductionPlans после обновления";
            var queryLINQ1 = from pp in db.ProductionPlans
                             where pp.PlannedVolume < 6000
                             select new
                             {
                                 Название_Предприятия = pp.Enterprise.Name,
                                 Запланированный_Объем = pp.PlannedVolume
                             };
            Print(comment, queryLINQ1.ToList());
        }

        static void Main(string[] args)
        {
            using (var db = new ProductionContext())
            {
                Console.WriteLine("Выполнение выборки данных ============");
                Console.ReadKey();
                Select(db);

                Console.WriteLine("Выполнение вставки данных ============");
                Console.ReadKey();
                Insert(db);

                Console.WriteLine("Выполнение удаления данных ============");
                Console.ReadKey();
                Delete(db);

                Console.WriteLine("Выполнение обновления данных ============");
                Console.ReadKey();
                Update(db);
            }
        }
    }
}
