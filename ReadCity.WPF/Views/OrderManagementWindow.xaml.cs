using System.Windows;
using System.Windows.Controls;
using System.Linq;
using ReadCity.Data.Models;
using ReadCity.Data.Services;

namespace ReadCity.WPF.Views
{
    /// <summary>
    /// Окно управления заказами для администратора и менеджера.
    /// Позволяет просматривать заказы, изменять дату доставки и статус.
    /// </summary>
    public partial class OrderManagementWindow : Window
    {
        private readonly OrderService _orderService = new OrderService();

        /// <summary>
        /// Инициализирует окно управления заказами и загружает данные.
        /// </summary>
        public OrderManagementWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var orders = _orderService.GetAllOrders();
                OrdersDataGrid.ItemsSource = orders;

                var statuses = _orderService.GetAllStatuses();
                StatusComboBox.ItemsSource = statuses;
                StatusComboBox.SelectedValuePath = "StatusId";
                StatusComboBox.DisplayMemberPath = "Name";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных:\n{ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OrdersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is OrderFinal order)
            {
                DeliveryDatePicker.SelectedDate = order.DeliveryDate;
                StatusComboBox.SelectedValue = order.StatusId;
            }
        }

        private void SaveOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is not OrderFinal selectedOrder)
            {
                MessageBox.Show("Выберите заказ.",
                    "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var statusId = StatusComboBox.SelectedValue != null
                    ? (int)StatusComboBox.SelectedValue
                    : selectedOrder.StatusId;

                var deliveryDate = DeliveryDatePicker.SelectedDate;

                var success = _orderService.UpdateOrder(selectedOrder.OrderId, deliveryDate, statusId);
                if (success)
                {
                    MessageBox.Show("Заказ обновлён.",
                        "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Не удалось обновить заказ.",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении:\n{ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}