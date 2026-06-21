namespace ReadCity.Data.Models
{
    /// <summary>
    /// Статус заказа.
    /// </summary>
    public class OrderStatus
    {
        /// <summary>
        /// Уникальный идентификатор статуса.
        /// </summary>
        public int StatusId { get; set; }

        /// <summary>
        /// Название статуса.
        /// </summary>
        public string Name { get; set; }
    }
}
