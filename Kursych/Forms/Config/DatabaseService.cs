using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Kursych.Forms.Main
{
    public class DatabaseService
    {
        private string connectionString;

        public DatabaseService()
        {
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection"]?.ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = "Server=localhost;Database=kursych;Uid=root;Pwd=root;";
                }
            }
            catch
            {
                connectionString = "Server=localhost;Database=kursych;Uid=root;Pwd=root;";
            }
        }

        public bool TestConnection()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    return connection.State == ConnectionState.Open;
                }
            }
            catch
            {
                return false;
            }
        }

        public string GetConnectionString()
        {
            return connectionString ?? "";
        }

        public DataTable ExecuteQuery(string query)
        {
            DataTable dt = new DataTable();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        connection.Open();
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Не показываем ошибку, просто возвращаем пустую таблицу
                System.Diagnostics.Debug.WriteLine($"Ошибка выполнения запроса: {ex.Message}");
            }

            return dt;
        }

        public int ExecuteNonQuery(string query)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        connection.Open();
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                return 0;
            }
        }

        public object ExecuteScalar(string query)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        connection.Open();
                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        // Получение категорий
        public DataTable GetCategories()
        {
            DataTable dt = new DataTable();

            try
            {
                string query = "SELECT categoriesID, categoriesName FROM categories ORDER BY categoriesName";
                dt = ExecuteQuery(query);
            }
            catch { }

            // Убеждаемся, что таблица имеет правильную структуру
            if (!dt.Columns.Contains("categoriesID"))
                dt.Columns.Add("categoriesID", typeof(int));
            if (!dt.Columns.Contains("categoriesName"))
                dt.Columns.Add("categoriesName", typeof(string));

            return dt;
        }

        // Получение поставщиков
        public DataTable GetSuppliers()
        {
            DataTable dt = new DataTable();

            try
            {
                string query = "SELECT SuppliersID, Name FROM suppliers ORDER BY Name";
                dt = ExecuteQuery(query);
            }
            catch { }

            if (!dt.Columns.Contains("SuppliersID"))
                dt.Columns.Add("SuppliersID", typeof(int));
            if (!dt.Columns.Contains("Name"))
                dt.Columns.Add("Name", typeof(string));

            return dt;
        }

        // Получение продуктов
        // Получение продуктов
        // Получение продуктов
        public DataTable GetProducts()
        {
            DataTable dt = new DataTable();

            try
            {
                string query = @"
            SELECT 
                p.ProductID,
                p.Name,
                p.Price,
                p.categoriesID,
                p.Description,
                p.SuppliersID,
                p.StockQualitu,
                p.ImagePath,
                IFNULL(c.categoriesName, '') as categoriesName,
                IFNULL(s.Name, '') as SupplierName
            FROM product p
            LEFT JOIN categories c ON p.categoriesID = c.categoriesID
            LEFT JOIN suppliers s ON p.SuppliersID = s.SuppliersID
            ORDER BY p.Name";

                dt = ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка в GetProducts: {ex.Message}");
            }

            // Убеждаемся, что все нужные колонки существуют
            if (!dt.Columns.Contains("ProductID"))
                dt.Columns.Add("ProductID", typeof(int));
            if (!dt.Columns.Contains("Name"))
                dt.Columns.Add("Name", typeof(string));
            if (!dt.Columns.Contains("Price"))
                dt.Columns.Add("Price", typeof(decimal));
            if (!dt.Columns.Contains("categoriesID"))
                dt.Columns.Add("categoriesID", typeof(int));
            if (!dt.Columns.Contains("Description"))
                dt.Columns.Add("Description", typeof(string));
            if (!dt.Columns.Contains("SuppliersID"))
                dt.Columns.Add("SuppliersID", typeof(int));
            if (!dt.Columns.Contains("StockQualitu"))
                dt.Columns.Add("StockQualitu", typeof(int));
            if (!dt.Columns.Contains("ImagePath"))
                dt.Columns.Add("ImagePath", typeof(string));
            if (!dt.Columns.Contains("categoriesName"))
                dt.Columns.Add("categoriesName", typeof(string));
            if (!dt.Columns.Contains("SupplierName"))
                dt.Columns.Add("SupplierName", typeof(string));

            return dt;
        }

        public DataTable GetOrders()
        {
            DataTable dt = new DataTable();

            try
            {
                string query = @"
                    SELECT 
                        o.OrderID as 'ID заказа',
                        COALESCE(CONCAT(u.UserSurname, ' ', u.UserName), 'Неизвестный клиент') as 'Клиент',
                        GROUP_CONCAT(CONCAT(p.Name, ' (', oi.Quantity, ' шт.)') SEPARATOR '; ') as 'Товары',
                        COUNT(oi.OrderItemID) as 'Кол-во позиций',
                        SUM(oi.Quantity) as 'Всего единиц',
                        DATE_FORMAT(o.OrderDate, '%d.%m.%Y %H:%i') as 'Дата заказа',
                        o.Status as 'Статус',
                        o.TotalAmount as 'Общая стоимость',
                        o.DiscountAmount as 'Скидка',
                        o.FinalAmount as 'Итого'
                    FROM orders o
                    LEFT JOIN users u ON o.UserID = u.UserID
                    LEFT JOIN order_items oi ON o.OrderID = oi.OrderID
                    LEFT JOIN product p ON oi.ProductID = p.ProductID
                    GROUP BY o.OrderID
                    ORDER BY o.OrderDate DESC";

                dt = ExecuteQuery(query);
            }
            catch { }

            return dt;
        }

        public DataTable GetUsers()
        {
            DataTable dt = new DataTable();

            try
            {
                string query = @"
                    SELECT 
                        u.UserID,
                        u.UserName,
                        u.UserSurname,
                        u.UserPatronymic,
                        u.UserLogin,
                        u.UserPassword,
                        r.RoleID,
                        r.RoleName,
                        u.Phone,
                        u.Email,
                        u.Address,
                        u.CreatedDate
                    FROM users u 
                    LEFT JOIN role r ON u.RoleID = r.RoleID 
                    ORDER BY u.UserSurname, u.UserName";

                dt = ExecuteQuery(query);
            }
            catch { }

            return dt;
        }

        public User AuthenticateUser(string login, string password)
        {
            try
            {
                string query = @"
                    SELECT 
                        u.UserID,
                        u.UserName,
                        u.UserSurname,
                        u.UserPatronymic,
                        u.UserLogin,
                        u.UserPassword,
                        r.RoleID,
                        r.RoleName
                    FROM users u 
                    LEFT JOIN role r ON u.RoleID = r.RoleID 
                    WHERE u.UserLogin = @Login AND u.UserPassword = @Password";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Login", login);
                        cmd.Parameters.AddWithValue("@Password", password);

                        connection.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    UserID = reader.GetInt32("UserID"),
                                    UserName = reader.GetString("UserName"),
                                    UserSurname = reader.GetString("UserSurname"),
                                    UserPatronymic = reader.IsDBNull(reader.GetOrdinal("UserPatronymic")) ?
                                        string.Empty : reader.GetString("UserPatronymic"),
                                    UserLogin = reader.GetString("UserLogin"),
                                    UserPassword = reader.GetString("UserPassword"),
                                    RoleID = reader.GetInt32("RoleID"),
                                    RoleName = reader.GetString("RoleName")
                                };
                            }
                        }
                    }
                }
            }
            catch { }

            return null;
        }

        public bool IsLoginUnique(string login, int excludeUserId = 0)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM users WHERE UserLogin = @Login AND UserID != @ExcludeID";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Login", login);
                        cmd.Parameters.AddWithValue("@ExcludeID", excludeUserId);

                        connection.Open();
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count == 0;
                    }
                }
            }
            catch
            {
                return true;
            }
        }

        public bool AddUser(User user)
        {
            try
            {
                string query = @"
                    INSERT INTO users (UserName, UserSurname, UserPatronymic, UserLogin, UserPassword, RoleID, Phone, Email)
                    VALUES (@Name, @Surname, @Patronymic, @Login, @Password, @RoleID, @Phone, @Email)";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Name", user.UserName ?? "");
                        cmd.Parameters.AddWithValue("@Surname", user.UserSurname ?? "");
                        cmd.Parameters.AddWithValue("@Patronymic", user.UserPatronymic ?? "");
                        cmd.Parameters.AddWithValue("@Login", user.UserLogin ?? "");
                        cmd.Parameters.AddWithValue("@Password", user.UserPassword ?? "");
                        cmd.Parameters.AddWithValue("@RoleID", user.RoleID);
                        cmd.Parameters.AddWithValue("@Phone", user.Phone ?? "");
                        cmd.Parameters.AddWithValue("@Email", user.Email ?? "");

                        connection.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateUser(User user)
        {
            try
            {
                string query = @"
                    UPDATE users 
                    SET UserName = @Name, 
                        UserSurname = @Surname, 
                        UserPatronymic = @Patronymic, 
                        UserLogin = @Login,
                        RoleID = @RoleID,
                        Phone = @Phone,
                        Email = @Email
                    WHERE UserID = @UserID";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", user.UserID);
                        cmd.Parameters.AddWithValue("@Name", user.UserName ?? "");
                        cmd.Parameters.AddWithValue("@Surname", user.UserSurname ?? "");
                        cmd.Parameters.AddWithValue("@Patronymic", user.UserPatronymic ?? "");
                        cmd.Parameters.AddWithValue("@Login", user.UserLogin ?? "");
                        cmd.Parameters.AddWithValue("@RoleID", user.RoleID);
                        cmd.Parameters.AddWithValue("@Phone", user.Phone ?? "");
                        cmd.Parameters.AddWithValue("@Email", user.Email ?? "");

                        connection.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteUser(int userId)
        {
            try
            {
                string query = "DELETE FROM users WHERE UserID = @UserID";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        connection.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateOrderStatus(int orderId, string status)
        {
            try
            {
                string query = $"UPDATE orders SET Status = '{status}' WHERE OrderID = {orderId}";
                return ExecuteNonQuery(query) > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}