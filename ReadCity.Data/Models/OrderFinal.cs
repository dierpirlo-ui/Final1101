namespace ReadCity.Data.Models
{
    /// <summary>
    /// Заказ покупателя.
    /// </summary>
    public class OrderFinal
    {
        /// <summary>
        /// Уникальный идентификатор заказа.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Номер заказа.
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Дата создания заказа.
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Дата доставки заказа.
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Код для получения заказа.
        /// </summary>
        public string ReceiptCode { get; set; }

        /// <summary>
        /// Идентификатор статуса заказа.
        /// </summary>
        public int StatusId { get; set; }


        /// <summary>
        /// Пользователь, оформивший заказ (навигационное свойство).
        /// </summary>
        public User? User { get; set; }

        /// <summary>
        /// Статус заказа (навигационное свойство).
        /// </summary>
        public OrderStatus? Status { get; set; }

        /// <summary>
        /// Список позиций в заказе (навигационное свойство).
        /// </summary>
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
