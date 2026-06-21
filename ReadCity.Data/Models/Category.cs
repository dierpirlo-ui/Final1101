namespace ReadCity.Data.Models
{
    /// <summary>
    /// Категория товара.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Уникальный идентификатор категории.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Название категории.
        /// </summary>
        public string Name { get; set; }
    }
}
