namespace ReadCity.Data.Models
{
    /// <summary>
    /// Производитель товара.
    /// </summary>
    public class Manufacturer
    {
        /// <summary>
        /// Уникальный идентификатор производителя.
        /// </summary>
        public int ManufacturerId { get; set; }

        /// <summary>
        /// Название производителя.
        /// </summary>
        public string Name { get; set; }
    }
}
