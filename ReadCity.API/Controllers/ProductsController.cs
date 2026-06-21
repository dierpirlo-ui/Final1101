using Microsoft.AspNetCore.Mvc;
using ReadCity.Data.Services;

namespace ReadCity.API.Controllers
{
    /// <summary>
    /// Контроллер для работы с товарами через API.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера товаров.
        /// Создает экземпляр сервиса для работы с товарами.
        /// </summary>
        public ProductsController()
        {
            _productService = new ProductService();
        }

        /// <summary>
        /// Получить список всех товаров.
        /// </summary>
        /// <returns>Список товаров с краткой информацией (без описания).</returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var products = _productService.GetAllProducts();

                var result = products.Select(p => new
                {
                    Id = p.ProductId,
                    Article = p.Article,
                    Name = p.Name,
                    Price = p.Price,
                    DiscountedPrice = p.DiscountedPrice,
                    Manufacturer = p.Manufacturer?.Name,
                    Category = p.Category?.Name,
                    Stock = p.Stock,
                    Photo = p.Photo
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Получить товар по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор товара.</param>
        /// <returns>Полная информация о товаре или ошибка.</returns>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var product = _productService.GetProductById(id);

                if (product == null)
                {
                    return NotFound(new { error = "Товар не найден" });
                }

                return Ok(new
                {
                    Id = product.ProductId,
                    Article = product.Article,
                    Name = product.Name,
                    Price = product.Price,
                    DiscountedPrice = product.DiscountedPrice,
                    Manufacturer = product.Manufacturer?.Name,
                    Category = product.Category?.Name,
                    Stock = product.Stock,
                    Description = product.Description,
                    Photo = product.Photo
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}