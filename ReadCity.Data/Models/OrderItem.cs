namespace ReadCity.Data.Models
{
    /// <summary>
    /// Позиция в заказе.
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// Уникальный идентификатор позиции.
        /// </summary>
        public int OrderItemId { get; set; }

        /// <summary>
        /// Идентификатор заказа.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Идентификатор товара.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Количество товара.
        /// </summary>
        public int Quantity { get; set; }


        /// <summary>
        /// Заказ (навигационное свойство).
        /// </summary>
        public OrderFinal? Order { get; set; }

        /// <summary>
        /// Товар (навигационное свойство).
        /// </summary>
        public Product? Product { get; set; }

        /// <summary>
        /// Общая стоимость позиции.
        /// </summary>
        public decimal TotalPrice => (Product?.DiscountedPrice ?? 0) * Quantity;
    }
}
