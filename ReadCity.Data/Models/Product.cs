namespace ReadCity.Data.Models
{
    /// <summary>
    /// Товар в каталоге магазина.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Уникальный идентификатор товара.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Артикул товара.
        /// </summary>
        public string Article { get; set; }

        /// <summary>
        /// Название товара.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Единица измерения.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Цена товара.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Автор товара.
        /// </summary>
        public string? Author { get; set; }

        /// <summary>
        /// Идентификатор производителя.
        /// </summary>
        public int ManufacturerId { get; set; }

        /// <summary>
        /// Идентификатор категории.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Размер скидки.
        /// </summary>
        public int Discount { get; set; }

        /// <summary>
        /// Количество товара на складе.
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Описание товара.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Имя файла изображения товара.
        /// </summary>
        public string? Photo { get; set; }


        /// <summary>
        /// Производитель товара (навигационное свойство).
        /// </summary>
        public Manufacturer? Manufacturer { get; set; }

        /// <summary>
        /// Категория товара (навигационное свойство).
        /// </summary>
        public Category? Category { get; set; }

        /// <summary>
        /// Цена товара с учётом скидки.
        /// </summary>
        public decimal DiscountedPrice
        {
            get
            {
                if (Discount > 0)
                    return Price - (Price * Discount / 100);
                return Price;
            }
        }

        /// <summary>
        /// Путь к изображению товара для отображения.
        /// Если фото отсутствует, возвращает путь к заглушке.
        /// </summary>
        public string? PhotoPath
        {
            get
            {
                if (string.IsNullOrEmpty(Photo))
                    return $"/Resources/picture.png" ;
                return $"/Resources/Products/{Photo}";
            }
        }
    }
}
