using Microsoft.EntityFrameworkCore;
using ReadCity.Data.Context;
using ReadCity.Data.Models;

namespace ReadCity.Data.Services
{
    /// <summary>
    /// Сервис для работы с пользователями и авторизацией.
    /// </summary>
    public class UserService
    {
        /// <summary>
        /// Авторизует пользователя по логину и паролю.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>Объект User при успешной авторизации, иначе null.</returns>
        public User? Authenticate(string login, string password)
        {
            try
            {
                using var context = new ReadCityDbContext();
                return context.Users
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.Login == login && u.Password == password);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка авторизации: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Получить пользователя по ID с его ролью.
        /// </summary>
        /// <param name="userId">ID пользователя.</param>
        /// <returns>Объект User или null, если не найден.</returns>
        public User? GetUserById(int userId)
        {
            try
            {
                using var context = new ReadCityDbContext();
                return context.Users
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.UserId == userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения пользователя: {ex.Message}");
                return null;
            }
        }
    }
}
