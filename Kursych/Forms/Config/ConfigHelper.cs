using System.Configuration;

namespace Kursych.Helpers
{
    public static class ConfigHelper
    {
        /// <summary>
        /// Проверка учетных данных администратора из конфига
        /// </summary>
        public static bool ValidateAdminCredentials(string login, string password)
        {
            string defaultLogin = ConfigurationManager.AppSettings["DefaultAdminLogin"] ?? "admin";
            string defaultPassword = ConfigurationManager.AppSettings["DefaultAdminPassword"] ?? "admin";

            return login == defaultLogin && password == defaultPassword;
        }

        /// <summary>
        /// Получение таймаута бездействия из конфига
        /// </summary>
        public static int GetInactivityTimeout()
        {
            string timeout = ConfigurationManager.AppSettings["InactivityTimeoutSeconds"] ?? "30";
            if (int.TryParse(timeout, out int seconds))
                return seconds;
            return 30;
        }
    }
}