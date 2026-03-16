//using System.Configuration;

//namespace Kursych
//{
//    public static class Settings
//    {
//        public static string MySqlServer
//        {
//            get => GetSetting("MySqlServer", "localhost");
//            set => SaveSetting("MySqlServer", value);
//        }

//        public static string MySqlDatabase
//        {
//            get => GetSetting("MySqlDatabase", "constructionbase");
//            set => SaveSetting("MySqlDatabase", value);
//        }

//        public static string MySqlUid
//        {
//            get => GetSetting("MySqlUid", "root");
//            set => SaveSetting("MySqlUid", value);
//        }

//        public static string MySqlPassword
//        {
//            get => GetSetting("MySqlPassword", "");
//            set => SaveSetting("MySqlPassword", value);
//        }

//        public static string MySqlPort
//        {
//            get => GetSetting("MySqlPort", "3306");
//            set => SaveSetting("MySqlPort", value);
//        }

//        private static string GetSetting(string key, string defaultValue)
//        {
//            try
//            {
//                return ConfigurationManager.AppSettings[key] ?? defaultValue;
//            }
//            catch
//            {
//                return defaultValue;
//            }
//        }

//        private static void SaveSetting(string key, string value)
//        {
//            try
//            {
//                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

//                if (config.AppSettings.Settings[key] != null)
//                {
//                    config.AppSettings.Settings[key].Value = value;
//                }
//                else
//                {
//                    config.AppSettings.Settings.Add(key, value);
//                }

//                config.Save(ConfigurationSaveMode.Modified);
//                ConfigurationManager.RefreshSection("appSettings");
//            }
//            catch
//            {
//                // Игнорируем ошибки сохранения
//            }
//        }
//    }
//}