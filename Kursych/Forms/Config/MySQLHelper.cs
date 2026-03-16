using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System.Windows.Forms;

namespace Kursych
{
    public class MySQLHelper
    {
        private string connectionString;
        private string configFile;

        public MySQLHelper()
        {
            // Файл конфигурации в папке с программой
            configFile = Path.Combine(Application.StartupPath, "dbconfig.ini");
            LoadConnectionString();
        }

        private void LoadConnectionString()
        {
            try
            {
                if (File.Exists(configFile))
                {
                    // Читаем строку подключения из файла
                    string[] lines = File.ReadAllLines(configFile);
                    foreach (string line in lines)
                    {
                        if (line.StartsWith("ConnectionString="))
                        {
                            connectionString = line.Substring("ConnectionString=".Length);
                            return;
                        }
                    }
                }

                // Если файла нет - используем значения по умолчанию
                connectionString = "server=localhost;port=3306;database=kursych;uid=root;pwd=root;";
            }
            catch
            {
                connectionString = "server=localhost;port=3306;database=kursych;uid=root;pwd=root;";
            }
        }

        public string GetConnectionString()
        {
            return connectionString;
        }

        // Тест подключения (без параметров)
        public bool TestConnection()
        {
            return TestConnection(connectionString);
        }

        // Тест подключения (с параметрами)
        public bool TestConnection(string server, string database, string uid, string password, string port)
        {
            string testConnectionString = $"server={server};port={port};database={database};uid={uid};pwd={password};";
            return TestConnection(testConnectionString);
        }

        // Тест подключения (со строкой)
        public bool TestConnection(string connString)
        {
            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        // Проверка существования базы данных
        public bool CheckDatabaseExists()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Проверяем наличие таблиц
                    string checkQuery = @"
                        SELECT COUNT(*) FROM information_schema.tables 
                        WHERE table_schema = DATABASE() 
                        AND table_name IN ('users', 'role', 'product', 'categories', 'suppliers', 'orders', 'order_items')";

                    using (MySqlCommand cmd = new MySqlCommand(checkQuery, connection))
                    {
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count >= 7; // Должно быть минимум 7 таблиц
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public void SaveConnectionSettings(string server, string database, string uid, string password, string port)
        {
            string newConnectionString = $"server={server};port={port};database={database};uid={uid};pwd={password};";

            // Сохраняем в файл
            File.WriteAllText(configFile, $"ConnectionString={newConnectionString}");

            this.connectionString = newConnectionString;
        }

        public (string server, string database, string uid, string password, string port) LoadConnectionSettings()
        {
            // Парсим строку подключения
            return ParseConnectionString(connectionString);
        }

        private (string server, string database, string uid, string password, string port) ParseConnectionString(string connStr)
        {
            string server = "localhost";
            string database = "kursych";
            string uid = "root";
            string password = "root";
            string port = "3306";

            string[] parts = connStr.Split(';');
            foreach (string part in parts)
            {
                if (part.StartsWith("server=", StringComparison.OrdinalIgnoreCase))
                    server = part.Substring("server=".Length);
                else if (part.StartsWith("database=", StringComparison.OrdinalIgnoreCase))
                    database = part.Substring("database=".Length);
                else if (part.StartsWith("uid=", StringComparison.OrdinalIgnoreCase))
                    uid = part.Substring("uid=".Length);
                else if (part.StartsWith("pwd=", StringComparison.OrdinalIgnoreCase))
                    password = part.Substring("pwd=".Length);
                else if (part.StartsWith("password=", StringComparison.OrdinalIgnoreCase))
                    password = part.Substring("password=".Length);
                else if (part.StartsWith("port=", StringComparison.OrdinalIgnoreCase))
                    port = part.Substring("port=".Length);
            }

            return (server, database, uid, password, port);
        }

        public DataTable ExecuteQuery(string query)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public int ExecuteNonQuery(string query)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public object ExecuteScalar(string query)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    return cmd.ExecuteScalar();
                }
            }
        }
    }
}