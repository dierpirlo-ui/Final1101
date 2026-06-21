namespace ReadCity.API.Models
{
    /// <summary>
    /// Модель для обновления заказа через API.
    /// Используется в методе PUT /api/orders/{id}.
    /// </summary>
    public class UpdateOrderModel
    {
        /// <summary>
        /// Новая дата доставки заказа.
        /// Если не указана — дата доставки остаётся без изменений.
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// Новый идентификатор статуса заказа.
        /// </summary>
        public int StatusId { get; set; }
    }
}
