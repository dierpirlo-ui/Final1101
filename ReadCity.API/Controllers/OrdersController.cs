using Microsoft.AspNetCore.Mvc;
using ReadCity.Data.Services;
using ReadCity.API.Models;

namespace ReadCity.API.Controllers
{
    /// <summary>
    /// Контроллер для работы с заказами через API.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера заказов.
        /// Создает экземпляр сервиса для работы с заказами.
        /// </summary>
        public OrdersController()
        {
            _orderService = new OrderService();
        }

        /// <summary>
        /// Получить список всех заказов с полной информацией.
        /// Включает пользователя, статус, позиции и товары.
        /// </summary>
        /// <returns>Список заказов с деталями.</returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var orders = _orderService.GetAllOrders();

                var result = orders.Select(o => new
                {
                    Id = o.OrderId,
                    Number = o.OrderNumber,
                    Date = o.OrderDate,
                    DeliveryDate = o.DeliveryDate,
                    Customer = o.User?.FullName ?? "Гость",
                    Status = o.Status?.Name ?? "Неизвестно",
                    ReceiptCode = o.ReceiptCode,
                    Items = o.OrderItems.Select(i => new
                    {
                        Product = i.Product?.Name ?? "Неизвестно",
                        Quantity = i.Quantity,
                        Price = i.Product?.Price ?? 0,
                        Total = (i.Product?.Price ?? 0) * i.Quantity
                    }),
                    TotalAmount = o.OrderItems.Sum(i => (i.Product?.Price ?? 0) * i.Quantity)
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Получить заказ по идентификатору с полной информацией.
        /// </summary>
        /// <param name="id">Идентификатор заказа.</param>
        /// <returns>Полная информация о заказе или ошибка.</returns>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var order = _orderService.GetOrderById(id);

                if (order == null)
                {
                    return NotFound(new { error = "Заказ не найден" });
                }

                return Ok(new
                {
                    Id = order.OrderId,
                    Number = order.OrderNumber,
                    Date = order.OrderDate,
                    DeliveryDate = order.DeliveryDate,
                    Customer = order.User?.FullName ?? "Гость",
                    Status = order.Status?.Name ?? "Неизвестно",
                    ReceiptCode = order.ReceiptCode,
                    Items = order.OrderItems.Select(i => new
                    {
                        Product = i.Product?.Name ?? "Неизвестно",
                        Quantity = i.Quantity,
                        Price = i.Product?.Price ?? 0,
                        Total = (i.Product?.Price ?? 0) * i.Quantity
                    }),
                    TotalAmount = order.OrderItems.Sum(i => (i.Product?.Price ?? 0) * i.Quantity)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Обновить дату доставки и статус заказа.
        /// </summary>
        /// <param name="id">Идентификатор заказа.</param>
        /// <param name="model">Модель с новыми данными.</param>
        /// <returns>Сообщение об успехе или ошибка.</returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateOrderModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(new { error = "Неверные данные" });
                }

                var success = _orderService.UpdateOrder(id, model.DeliveryDate, model.StatusId);

                if (!success)
                {
                    return NotFound(new { error = "Заказ не найден" });
                }

                return Ok(new { message = "Заказ обновлен" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}