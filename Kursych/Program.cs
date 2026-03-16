using System;
using System.Windows.Forms;
using Kursych.Forms.Auth;
using Kursych.Forms.Main;

namespace Kursych
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Бесконечный цикл для возврата на форму авторизации
            while (true)
            {
                // Запускаем форму авторизации
                LoginForm loginForm = new LoginForm();

                // Если пользователь отменил вход (нажал Выход) - выходим из приложения
                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    break;
                }

                // Если пользователь успешно авторизовался, открываем главную форму
                MainForm mainForm = new MainForm();
                var result = mainForm.ShowDialog();

                // Если главная форма закрылась с результатом Abort - возвращаемся на авторизацию
                if (result == DialogResult.Abort)
                {
                    continue;
                }

                // Если главная форма закрылась с результатом Cancel или OK - выходим
                break;
            }

            // Завершаем приложение
            Application.Exit();
        }
    }
}