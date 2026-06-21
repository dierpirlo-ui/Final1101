using Microsoft.AspNetCore.Mvc.RazorPages;
using ReadCity.Data.Services;

namespace ReadCity.Web.Pages
{
    /// <summary>
    /// Модель страницы подтверждения добавления товара в заказ.
    /// </summary>
    public class AddToOrderModel : PageModel
    {
        private readonly ProductService _productService;

        /// <summary>
        /// Сообщение для пользователя.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Заголовок ошибки.
        /// </summary>
        public string ErrorTitle { get; set; } = "Ошибка";

        /// <summary>
        /// Название добавленного товара.
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Цена добавленного товара со скидкой.
        /// </summary>
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// Флаг успешного добавления товара.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр модели страницы.
        /// Создает экземпляр сервиса для работы с товарами.
        /// </summary>
        public AddToOrderModel()
        {
            _productService = new ProductService();
        }

        /// <summary>
        /// Выполняется при GET-запросе к странице.
        /// Проверяет наличие товара и его доступность на складе.
        /// </summary>
        /// <param name="id">Идентификатор товара.</param>
        public void OnGet(int id)
        {
            try
            {
                var product = _productService.GetProductById(id);
                if (product == null)
                {
                    Message = "Товар не найден";
                    IsSuccess = false;
                    return;
                }

                if (product.Stock <= 0)
                {
                    Message = "Товара нет в наличии";
                    ErrorTitle = "Нет в наличии";
                    IsSuccess = false;
                    return;
                }

                ProductName = product.Name;
                ProductPrice = product.DiscountedPrice;
                Message = $"Товар «{product.Name}» добавлен в заказ!";
                IsSuccess = true;
            }
            catch (Exception ex)
            {
                Message = $"Ошибка: {ex.Message}";
                IsSuccess = false;
            }
        }
    }
}