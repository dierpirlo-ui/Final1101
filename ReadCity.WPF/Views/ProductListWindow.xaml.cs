using System.Windows;
using System.Windows.Controls;
using ReadCity.Data.Models;
using ReadCity.Data.Services;

namespace ReadCity.WPF.Views
{
    /// <summary>
    /// Главное окно приложения — каталог товаров.
    /// Отображает список товаров в виде карточек с возможностью фильтрации, поиска и добавления в заказ.
    /// </summary>
    public partial class ProductListWindow : Window
    {
        private readonly ProductService _productService = new();
        private List<Product> _allProducts = new();

        /// <summary>
        /// Инициализирует главное окно, загружает данные и настраивает интерфейс.
        /// </summary>
        public ProductListWindow()
        {
            InitializeComponent();
            AddPlaceholders();
            LoadData();
            LoadUserFromSettings();
            UpdateUserPanel();
        }

        private void AddPlaceholders()
        {
            SetPlaceholder(SearchTextBox, "Поиск по названию...");
            SetPlaceholder(MinPriceTextBox, "Цена от");
            SetPlaceholder(MaxPriceTextBox, "Цена до");
        }

        private void SetPlaceholder(TextBox textBox, string placeholder)
        {
            textBox.Text = placeholder;
            textBox.Foreground = System.Windows.Media.Brushes.Gray;

            textBox.GotFocus += (s, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.Foreground = System.Windows.Media.Brushes.Black;
                }
            };

            textBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.Foreground = System.Windows.Media.Brushes.Gray;
                }
            };
        }

        private void LoadData()
        {
            try
            {
                _allProducts = _productService.GetAllProducts();

                var manufacturers = _productService.GetAllManufacturers();
                ManufacturerComboBox.Items.Clear();
                ManufacturerComboBox.Items.Add(new ComboBoxItem { Content = "Все производители", Tag = 0 });
                foreach (var m in manufacturers)
                    ManufacturerComboBox.Items.Add(new ComboBoxItem { Content = m.Name, Tag = m.ManufacturerId });
                ManufacturerComboBox.SelectedIndex = 0;

                SortComboBox.SelectedIndex = 0;

                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных:\n{ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyFilters()
        {
            try
            {
                if (_allProducts == null || !_allProducts.Any())
                {
                    ProductsItemsControl.ItemsSource = new List<Product>();
                    CounterTextBlock.Text = "Показано: 0 из 0";
                    return;
                }
                var filtered = _allProducts.AsEnumerable();

                var searchText = SearchTextBox.Text?.Trim().ToLower();
                if (!string.IsNullOrEmpty(searchText) && searchText != "поиск по названию...")
                    filtered = filtered.Where(p => p.Name.ToLower().Contains(searchText));

                if (ManufacturerComboBox.SelectedItem is ComboBoxItem { Tag: int mId } && mId > 0)
                    filtered = filtered.Where(p => p.ManufacturerId == mId);

                if (decimal.TryParse(MinPriceTextBox.Text, out decimal minPrice) && minPrice > 0)
                    filtered = filtered.Where(p => p.DiscountedPrice >= minPrice);

                if (decimal.TryParse(MaxPriceTextBox.Text, out decimal maxPrice) && maxPrice > 0)
                    filtered = filtered.Where(p => p.DiscountedPrice <= maxPrice);

                var sortTag = (SortComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString();
                filtered = sortTag switch
                {
                    "NameAsc" => filtered.OrderBy(p => p.Name),
                    "NameDesc" => filtered.OrderByDescending(p => p.Name),
                    "PriceAsc" => filtered.OrderBy(p => p.DiscountedPrice),
                    "PriceDesc" => filtered.OrderByDescending(p => p.DiscountedPrice),
                    _ => filtered
                };

                var result = filtered.ToList();
                ProductsItemsControl.ItemsSource = result;
                CounterTextBlock.Text = $"Показано: {result.Count} из {_allProducts.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации:\n{ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterChanged(object sender, EventArgs e) => ApplyFilters();

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: Product product })
            {
                var existing = App.CurrentOrderItems.FirstOrDefault(i => i.ProductId == product.ProductId);
                if (existing != null)
                    existing.Quantity++;
                else
                {
                    App.CurrentOrderItems.Add(new OrderItem
                    {
                        ProductId = product.ProductId,
                        Product = product,
                        Quantity = 1
                    });
                }

                ViewOrderButton.Visibility = Visibility.Visible;
                MessageBox.Show($"«{product.Name}» добавлен в заказ.",
                    "Добавлено", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser != null)
            {
                App.CurrentUser = null;

                Properties.Settings.Default.IsAuthorized = false;
                Properties.Settings.Default.UserId = 0;
                Properties.Settings.Default.Save();

                UpdateUserPanel();
            }
            else
            {
                var loginWindow = new LoginWindow();
                loginWindow.Owner = this;
                loginWindow.ShowDialog();
                UpdateUserPanel();
            }
        }

        private void UpdateUserPanel()
        {
            if (App.CurrentUser != null)
            {
                UserNameTextBlock.Text = App.CurrentUser.FullName;
                LoginButton.Content = "Выйти";
                LoginButton.Margin = new Thickness(10, 0, 0, 0);

                bool isStaff = App.CurrentUser.Role?.Name == "Администратор" ||
                               App.CurrentUser.Role?.Name == "Менеджер";
                OrdersManagementButton.Visibility = isStaff ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                UserNameTextBlock.Text = string.Empty;
                LoginButton.Content = "Войти";
                LoginButton.Margin = new Thickness(0);
                OrdersManagementButton.Visibility = Visibility.Collapsed;
            }

            ViewOrderButton.Visibility = App.CurrentOrderItems.Any() ? Visibility.Visible : Visibility.Collapsed;
        }

        private void LoadUserFromSettings()
        {
            if (Properties.Settings.Default.IsAuthorized)
            {
                var userId = Properties.Settings.Default.UserId;
                var userService = new UserService();
                var user = userService.GetUserById(userId);

                if (user != null)
                {
                    App.CurrentUser = user;
                }
                else
                {
                    Properties.Settings.Default.IsAuthorized = false;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void ViewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var orderWindow = new OrderWindow();
            orderWindow.Owner = this;
            orderWindow.ShowDialog();
            ViewOrderButton.Visibility = App.CurrentOrderItems.Any() ? Visibility.Visible : Visibility.Collapsed;
        }

        private void OrdersManagementButton_Click(object sender, RoutedEventArgs e)
        {
            var managementWindow = new OrderManagementWindow();
            managementWindow.Owner = this;
            managementWindow.ShowDialog();
        }
    }
}