using ReadCity.Data.Models;
using ReadCity.Data.Services;
using System.Windows;
using System.Windows.Controls;

namespace ReadCity.WPF.Views
{
    /// <summary>
    /// Окно просмотра и оформления заказа.
    /// Отображает добавленные товары, позволяет изменять количество, удалять товары и оформлять заказ.
    /// </summary>
    public partial class OrderWindow : Window
    {
        private readonly OrderService _orderService = new OrderService();

        /// <summary>
        /// Инициализирует окно корзины и загружает текущие позиции заказа.
        /// </summary>
        public OrderWindow()
        {
            InitializeComponent();
            LoadOrder();
        }

        private void LoadOrder()
        {
            if (App.CurrentUser != null)
            {
                ClientNameTextBlock.Text = $"Клиент: {App.CurrentUser.FullName}";
                ClientNameTextBlock.Visibility = Visibility.Visible;
            }

            OrderDataGrid.ItemsSource = App.CurrentOrderItems;
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            var total = App.CurrentOrderItems
                .Sum(i => (i.Product?.DiscountedPrice ?? 0) * i.Quantity);
            TotalTextBlock.Text = $"Итого: {total:N2} руб.";
        }

        private void QuantityTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.DataContext is OrderItem item)
            {
                if (int.TryParse(textBox.Text, out int newQuantity))
                {
                    if (newQuantity <= 0)
                    {
                        App.CurrentOrderItems.Remove(item);
                        OrderDataGrid.ItemsSource = null;
                        OrderDataGrid.ItemsSource = App.CurrentOrderItems;
                        UpdateTotal();
                        MessageBox.Show($"Товар удалён из заказа.",
                            "Удалено", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        item.Quantity = newQuantity;
                        UpdateTotal();
                    }
                }
            }
        }

        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: OrderItem item })
            {
                var result = MessageBox.Show(
                    $"Удалить «{item.Product?.Name}» из заказа?",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    App.CurrentOrderItems.Remove(item);
                    OrderDataGrid.ItemsSource = null;
                    OrderDataGrid.ItemsSource = App.CurrentOrderItems;
                    UpdateTotal();
                }
            }
        }

        private void ConfirmOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (!App.CurrentOrderItems.Any())
            {
                MessageBox.Show("Добавьте хотя бы один товар в заказ.",
                    "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var itemsCopy = App.CurrentOrderItems.ToList();

                var order = new OrderFinal
                {
                    OrderDate = DateTime.Now,
                    UserId = App.CurrentUser?.UserId,
                    StatusId = 1,
                    OrderItems = App.CurrentOrderItems.Select(i => new OrderItem
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity
                    }).ToList()
                };

                var random = new Random();
                order.ReceiptCode = random.Next(100, 999).ToString();

                var saved = _orderService.CreateOrder(order);
                if (!saved)
                {
                    MessageBox.Show("Не удалось сохранить заказ. Попробуйте снова.",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var receiptWindow = new ReceiptWindow(order, itemsCopy);
                receiptWindow.Owner = this;
                receiptWindow.ShowDialog();

                App.CurrentOrderItems.Clear();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при оформлении заказа:\n{ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}