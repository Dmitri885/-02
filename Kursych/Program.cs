using System;
using System.Windows.Forms;
using Kursych.Forms.Auth;
using Kursych.Forms.Config;
using Kursych.Forms.Main;

namespace Kursych
{
    static class Program
    {
        private static MainForm mainForm;
        private static bool isLocked = false;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Инициализируем трекер бездействия (но пока не запускаем)
            InactivityTracker.InactivityLock += OnInactivityLock;

            // Бесконечный цикл для возврата на форму авторизации
            while (true)
            {
                // Запускаем форму авторизации
                LoginForm loginForm = new LoginForm();

                // Если это блокировка, устанавливаем специальный режим
                if (isLocked)
                {
                    loginForm.IsLockMode = true;
                    loginForm.Text = "Блокировка системы";
                    loginForm.TopMost = true; // Поверх всех окон
                }

                // Если пользователь отменил вход (нажал Выход) - выходим из приложения
                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    if (isLocked)
                    {
                        // Если это блокировка и пользователь закрыл форму - всё равно блокируем
                        continue;
                    }
                    break;
                }

                // Если это была разблокировка
                if (isLocked)
                {
                    isLocked = false;
                    // Показываем главную форму
                    if (mainForm != null)
                    {
                        mainForm.Show();
                        mainForm.Activate();
                    }
                    continue;
                }

                // Если пользователь успешно авторизовался, создаем и открываем главную форму
                mainForm = new MainForm();

                // Запускаем трекер бездействия после успешного входа
                InactivityTracker.Initialize(mainForm);

                var result = mainForm.ShowDialog();

                // Если главная форма закрылась с результатом Abort - возвращаемся на авторизацию
                if (result == DialogResult.Abort)
                {
                    continue;
                }

                // Если главная форма закрылась с результатом Cancel или OK - выходим
                break;
            }

            // Останавливаем трекер перед выходом
            InactivityTracker.Stop();

            // Завершаем приложение
            Application.Exit();
        }

        // Обработчик события блокировки
        private static void OnInactivityLock(object sender, EventArgs e)
        {
            if (mainForm != null && !isLocked)
            {
                isLocked = true;

                // Скрываем главную форму
                mainForm.Hide();

                // Запускаем форму авторизации в режиме блокировки
                // (используем Invoke, так как событие может прийти из другого потока)
                if (mainForm.InvokeRequired)
                {
                    mainForm.Invoke((MethodInvoker)delegate
                    {
                        ShowLockScreen();
                    });
                }
                else
                {
                    ShowLockScreen();
                }
            }
        }

        private static void ShowLockScreen()
        {
            // Создаем форму авторизации для разблокировки
            LoginForm lockForm = new LoginForm();
            lockForm.IsLockMode = true;
            lockForm.Text = "Блокировка системы";
            lockForm.TopMost = true;
            lockForm.StartPosition = FormStartPosition.CenterScreen;

            if (lockForm.ShowDialog() == DialogResult.OK)
            {
                // Успешная разблокировка
                isLocked = false;
                if (mainForm != null)
                {
                    mainForm.Show();
                    mainForm.Activate();
                }
            }
            else
            {
                // Если пользователь закрыл форму без авторизации, завершаем приложение
                Application.Exit();
            }
        }
    }
}