using Microsoft.EntityFrameworkCore;
using ReadCity.Data.Context;
using ReadCity.Data.Models;

namespace ReadCity.Data.Services
{
    /// <summary>
    /// Сервис для работы с товарами и связанными данными.
    /// </summary>
    public class ProductService
    {
        /// <summary>
        /// Возвращает все товары с загруженными производителями и категориями.
        /// </summary>
        /// <returns>Список всех товаров.</returns>
        public List<Product> GetAllProducts()
        {
            try
            {
                using var context = new ReadCityDbContext();
                return context.Products
                    .Include(p => p.Manufacturer)
                    .Include(p => p.Category)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки товаров: {ex.Message}");
                return new List<Product>();
            }
        }

        /// <summary>
        /// Возвращает список всех производителей, отсортированных по названию.
        /// </summary>
        /// <returns>Список производителей.</returns>
        public List<Manufacturer> GetAllManufacturers()
        {
            try
            {
                using var context = new ReadCityDbContext();
                return context.Manufacturers.OrderBy(m => m.Name).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки производителей: {ex.Message}");
                return new List<Manufacturer>();
            }
        }

        /// <summary>
        /// Возвращает товар по его идентификатору с загруженными производителем и категорией.
        /// </summary>
        /// <param name="id">Идентификатор товара.</param>
        /// <returns>Объект Product или null, если товар не найден.</returns>
        public Product? GetProductById(int id)
        {
            try
            {
                using var context = new ReadCityDbContext();
                return context.Products
                    .Include(p => p.Manufacturer)
                    .Include(p => p.Category)
                    .FirstOrDefault(p => p.ProductId == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения товара: {ex.Message}");
                return null;
            }
        }
    }
}
