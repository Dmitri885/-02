using System;
using System.Text;

namespace Kursych.Forms.Auth
{
    internal class CaptchaGenerator
    {
        private static readonly Random random = new Random();

        // Разрешенные символы (исключаем похожие: O, 0, I, l, 1 и т.д.)
        private const string AllowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789";

        /// <summary>
        /// Генерирует случайную строку для CAPTCHA
        /// </summary>
        public static string Generate(int length = 4)
        {
            if (length <= 0)
                throw new ArgumentException("Длина должна быть больше 0");

            StringBuilder result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(AllowedChars.Length);
                result.Append(AllowedChars[index]);
            }

            return result.ToString();
        }

        /// <summary>
        /// Проверяет, совпадает ли введенный текст с CAPTCHA (регистронезависимо)
        /// </summary>
        public static bool Validate(string input, string captcha)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(captcha))
                return false;

            return input.Trim().ToUpper() == captcha.ToUpper();
        }
    }
}