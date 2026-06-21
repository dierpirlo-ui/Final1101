using Microsoft.EntityFrameworkCore;
using ReadCity.Data.Context;
using ReadCity.Data.Models;

namespace ReadCity.Data.Services
{
    /// <summary>
    /// Сервис для работы с заказами и связанными данными.
    /// </summary>
    public class OrderService
    {
        /// <summary>
        /// Создаёт новый заказ в базе данных.
        /// Автоматически присваивает номер заказа, текущую дату, статус "Новый" и код получения.
        /// </summary>
        /// <param name="order">Объект заказа без OrderNumber и OrderId.</param>
        /// <returns>true — при успешном сохранении, false — при ошибке.</returns>
        public bool CreateOrder(OrderFinal order)
        {
            try
            {
                using var context = new ReadCityDbContext();

                var maxNumber = context.OrderFinals.Max(o => (int?)o.OrderNumber) ?? 0;
                order.OrderNumber = maxNumber + 1;

                order.OrderDate = DateTime.Now;
                order.StatusId = 1;

                var random = new Random();
                order.ReceiptCode = random.Next(100, 999).ToString();

                context.OrderFinals.Add(order);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка создания заказа: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Возвращает все заказы с полной информацией:
        /// пользователь, статус, позиции и товары в каждой позиции.
        /// Заказы отсортированы по дате (сначала новые).
        /// </summary>
        /// <returns>Список всех заказов.</returns>
        public List<OrderFinal> GetAllOrders()
        {
            try
            {
                using var context = new ReadCityDbContext();
                return context.OrderFinals
                    .Include(o => o.User)
                    .Include(o => o.Status)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                    .OrderByDescending(o => o.OrderDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки заказов: {ex.Message}");
                return new List<OrderFinal>();
            }
        }

        /// <summary>
        /// Обновляет дату доставки и статус заказа.
        /// </summary>
        /// <param name="orderId">Идентификатор заказа.</param>
        /// <param name="deliveryDate">Новая дата доставки.</param>
        /// <param name="statusId">Новый идентификатор статуса.</param>
        /// <returns>true — при успешном обновлении, false — если заказ не найден.</returns>
        public bool UpdateOrder(int orderId, DateTime? deliveryDate, int statusId)
        {
            try
            {
                using var context = new ReadCityDbContext();
                var order = context.OrderFinals.Find(orderId);
                if (order == null)
                {
                    return false;
                }

                order.DeliveryDate = deliveryDate;
                order.StatusId = statusId;
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обновления заказа: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Возвращает заказ по его идентификатору с полной информацией:
        /// пользователь, статус, позиции и товары в каждой позиции.
        /// </summary>
        /// <param name="id">Идентификатор заказа.</param>
        /// <returns>Объект OrderFinal или null, если заказ не найден.</returns>
        public OrderFinal? GetOrderById(int id)
        {
            try
            {
                using var context = new ReadCityDbContext();
                return context.OrderFinals
                    .Include(o => o.User)
                    .Include(o => o.Status)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                    .FirstOrDefault(o => o.OrderId == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения заказа: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Возвращает список всех статусов заказов.
        /// </summary>
        /// <returns>Список статусов заказов.</returns>
        public List<OrderStatus> GetAllStatuses()
        {
            try
            {
                using var context = new ReadCityDbContext();
                return context.OrderStatuses.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки статусов: {ex.Message}");
                return new List<OrderStatus>();
            }
        }
    }
}
