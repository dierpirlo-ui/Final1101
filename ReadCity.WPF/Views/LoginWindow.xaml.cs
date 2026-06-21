using System.Windows;
using System.Windows.Controls;
using ReadCity.Data.Services;

namespace ReadCity.WPF.Views
{
    /// <summary>
    /// Окно авторизации пользователя в системе.
    /// Позволяет ввести логин и пароль для входа.
    public partial class LoginWindow : Window
    {
        private readonly UserService _userService = new UserService();

        /// <summary>
        /// Инициализирует окно авторизации.
        /// </summary>
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginTextBox.Text.Trim();
            var password = PasswordBox.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ShowError("Введите логин и пароль.");
                return;
            }

            try
            {
                var user = _userService.Authenticate(login, password);
                if (user == null)
                {
                    ShowError("Неверный логин или пароль.");
                    return;
                }

                App.CurrentUser = user;

                Properties.Settings.Default.UserId = user.UserId;
                Properties.Settings.Default.IsAuthorized = true;
                Properties.Settings.Default.Save();

                Close();
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка: {ex.Message}");
            }
        }

        private void ShowError(string message)
        {
            ErrorTextBlock.Text = message;
            ErrorTextBlock.Visibility = Visibility.Visible;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}