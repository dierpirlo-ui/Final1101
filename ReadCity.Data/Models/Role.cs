namespace ReadCity.Data.Models
{
    /// <summary>
    /// Роль пользователя в системе.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Уникальный идентификатор роли.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Название роли.
        /// </summary>
        public string Name { get; set; }
    }
}
