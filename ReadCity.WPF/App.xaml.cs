using System.Windows;
using ReadCity.Data.Models;

namespace ReadCity.WPF
{
    /// <summary>
    /// Главный класс приложения WPF.
    /// Содержит глобальные данные, доступные из любого окна.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Текущий авторизованный пользователь.
        /// null — если пользователь не авторизован.
        /// </summary>
        public static User? CurrentUser { get; set; }

        /// <summary>
        /// Текущий заказ.
        /// </summary>
        public static OrderFinal CurrentOrder { get; set; } = new OrderFinal();

        /// <summary>
        /// Список позиций текущего заказа.
        /// Хранится в памяти до момента оформления заказа.
        /// </summary>
        public static List<OrderItem> CurrentOrderItems { get; set; } = new List<OrderItem>();
    }
}