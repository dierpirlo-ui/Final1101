using Microsoft.Win32;
using ReadCity.Data.Models;
using System.IO;
using System.Text;
using System.Windows;

namespace ReadCity.WPF.Views
{
    /// <summary>
    /// Окно отображения талона заказа.
    /// Показывает информацию о заказе и позволяет сохранить талон в текстовый файл.
    /// </summary>
    public partial class ReceiptWindow : Window
    {
        private readonly OrderFinal _order;
        private readonly List<OrderItem> _items;
        private readonly string _receiptText;

        /// <summary>
        /// Инициализирует окно талона и формирует его содержимое.
        /// </summary>
        /// <param name="order">Объект заказа.</param>
        /// <param name="items">Список позиций заказа.</param>
        public ReceiptWindow(OrderFinal order, List<OrderItem> items)
        {
            InitializeComponent();
            _order = order;
            _items = items;
            _receiptText = BuildReceipt();
            ReceiptTextBlock.Text = _receiptText;
        }

        private string BuildReceipt()
        {
            var sb = new StringBuilder();

            sb.AppendLine("                        ТАЛОН ЗАКАЗА");
            sb.AppendLine();
            sb.AppendLine($"  Дата заказа:   {_order.OrderDate:dd.MM.yyyy HH:mm}");
            sb.AppendLine($"  Номер заказа:  {_order.OrderNumber}");
            sb.AppendLine($"  Код получения: {_order.ReceiptCode}");
            sb.AppendLine();
            sb.AppendLine("  Состав заказа:");
            sb.AppendLine();

            decimal total = 0;
            foreach (var item in _items)
            {
                var price = item.Product?.DiscountedPrice ?? 0;
                var sum = price * item.Quantity;
                total += sum;
                sb.AppendLine($"  {item.Product?.Name ?? "Неизвестно"}");
                sb.AppendLine($"    {item.Quantity} x {price:N2} руб. = {sum:N2} руб.");
                sb.AppendLine();
            }

            sb.AppendLine($"  ИТОГО: {total:N2} руб.");
            sb.AppendLine("  Спасибо за покупку!");

            return sb.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt",
                FileName = $"Заказ_{_order.OrderNumber}_{DateTime.Now:dd-MM-yyyy}.txt",
                DefaultExt = "txt",
                Title = "Сохранить талон"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(dialog.FileName, _receiptText, Encoding.UTF8);
                    StatusTextBlock.Text = "Файл успешно сохранён!";
                    StatusTextBlock.Visibility = Visibility.Visible;
                    StatusTextBlock.Foreground = System.Windows.Media.Brushes.Green;
                }
                catch (Exception ex)
                {
                    StatusTextBlock.Text = $"Ошибка: {ex.Message}";
                    StatusTextBlock.Visibility = Visibility.Visible;
                    StatusTextBlock.Foreground = System.Windows.Media.Brushes.Red;
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}