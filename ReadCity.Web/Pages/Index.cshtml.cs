using Microsoft.AspNetCore.Mvc.RazorPages;
using ReadCity.Data.Services;
using ReadCity.Web.ViewModels;

namespace ReadCity.Web.Pages
{
    /// <summary>
    /// Модель страницы каталога товаров.
    /// Загружает список товаров из базы данных и передаёт их в представление.
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly ProductService _productService;

        /// <summary>
        /// Список товаров для отображения на странице.
        /// </summary>
        public List<ProductViewModel> Products { get; set; } = new();

        /// <summary>
        /// Инициализирует новый экземпляр модели страницы.
        /// Создает экземпляр сервиса для работы с товарами.
        /// </summary>
        public IndexModel()
        {
            _productService = new ProductService();
        }

        /// <summary>
        /// Выполняется при GET-запросе к странице.
        /// Загружает все товары из базы данных и преобразует их в модели представления.
        /// </summary>
        public void OnGet()
        {
            try
            {
                var products = _productService.GetAllProducts();
                Products = products.Select(p => new ProductViewModel
                {
                    Id = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    DiscountedPrice = p.DiscountedPrice,
                    Manufacturer = p.Manufacturer?.Name,
                    Stock = p.Stock,
                    Photo = p.Photo
                }).ToList();
            }
            catch
            {
                Products = new List<ProductViewModel>();
            }
        }
    }
}