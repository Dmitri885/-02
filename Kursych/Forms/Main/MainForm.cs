using System;
using System.Windows.Forms;
using Kursych.Forms.Directories;
using Kursych.Forms.Orders;
using Kursych.Forms.Products;
using Kursych.Forms.Reports;
using Kursych.Forms.Users;
using Kursych.Forms.Auth;
using Kursych.Forms.Config; // Добавьте пространство имен для настроек

namespace Kursych.Forms.Main
{
    public partial class MainForm : Form
    {
        // Флаг для отслеживания, была ли блокировка
        private bool isLockedByInactivity = false;

        public MainForm()
        {
            InitializeComponent();
            LoadUserInfo();
            ConfigureMenuVisibility();

            // Подписываемся на событие блокировки
            InactivityTracker.InactivityLock += OnInactivityLock;
        }

        private void LoadUserInfo()
        {
            if (UserSession.CurrentUser != null)
            {
                lblWelcome.Text = $"{UserSession.CurrentUser.FullName}\n({UserSession.CurrentUser.RoleName})";
            }
            else
            {
                lblWelcome.Text = "Добро пожаловать!\n(Не авторизован)";
            }
        }

        private void ConfigureMenuVisibility()
        {
            // Скрываем всё по умолчанию
            btnUsers.Visible = false;
            btnProducts.Visible = false;
            btnOrders.Visible = false;
            btnReports.Visible = false;
            btnCategories.Visible = false;
            btnSuppliers.Visible = false;
            btnSettings.Visible = false; // Добавляем кнопку настроек

            if (UserSession.CurrentUser == null) return;

            // Администратор (RoleID = 1)
            if (UserSession.IsAdmin)
            {
                btnUsers.Visible = true;
                btnProducts.Visible = true;
                btnOrders.Visible = true;
                btnReports.Visible = true;
                btnCategories.Visible = true;
                btnSuppliers.Visible = true;
                btnSettings.Visible = true; // Только админ видит настройки
            }
            // Менеджер (RoleID = 2)
            else if (UserSession.IsManager)
            {
                btnProducts.Visible = true;
                btnOrders.Visible = true;
                btnReports.Visible = true;
                btnCategories.Visible = true;
                btnSuppliers.Visible = true;
            }
            // Кассир (RoleID = 3)
            else if (UserSession.IsCashier)
            {
                btnProducts.Visible = true;
                btnOrders.Visible = true;
            }
        }

        // Обработчик события блокировки от InactivityTracker
        private void OnInactivityLock(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    ShowLockScreen();
                });
            }
            else
            {
                ShowLockScreen();
            }
        }

        // Показ формы блокировки
        private void ShowLockScreen()
        {
            if (isLockedByInactivity) return;

            isLockedByInactivity = true;

            // Создаем форму авторизации в режиме блокировки
            LoginForm lockForm = new LoginForm();
            lockForm.IsLockMode = true;
            lockForm.Text = "Блокировка системы";
            lockForm.TopMost = true;
            lockForm.StartPosition = FormStartPosition.CenterScreen;

            // Скрываем главную форму
            this.Hide();

            // Показываем форму блокировки
            if (lockForm.ShowDialog() == DialogResult.OK)
            {
                // Успешная разблокировка
                isLockedByInactivity = false;
                InactivityTracker.UnlockSystem();
                this.Show();
                this.Activate();
            }
            else
            {
                // Если пользователь закрыл форму блокировки - выходим
                Application.Exit();
            }
        }

        // Обработчик для кнопки "Сменить пользователя"
        private void btnSwitchUser_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Вы хотите сменить пользователя?\nТекущая сессия будет завершена.",
                "Смена пользователя",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Останавливаем трекер бездействия
                InactivityTracker.Stop();

                // Очищаем текущую сессию
                UserSession.Clear();

                // Вместо закрытия формы, мы создаем событие для Program.cs
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
        }

        // Обработчик для кнопки "Настройки"
        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (UserSession.CurrentUser == null)
            {
                MessageBox.Show("Вы не авторизованы!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!UserSession.IsAdmin)
            {
                MessageBox.Show("У вас нет прав доступа к настройкам!",
                    "Доступ запрещен", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var settingsForm = new SettingsForm())
            {
                settingsForm.ShowDialog();
            }
        }

        // Остальные обработчики (без изменений)
        private void btnUsers_Click(object sender, EventArgs e)
        {
            if (UserSession.CurrentUser == null)
            {
                MessageBox.Show("Вы не авторизованы!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!UserSession.IsAdmin)
            {
                MessageBox.Show("У вас нет прав доступа к управлению пользователями!",
                    "Доступ запрещен", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            new UsersForm().ShowDialog();
            LoadUserInfo();
            ConfigureMenuVisibility();
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            if (UserSession.CurrentUser == null)
            {
                MessageBox.Show("Вы не авторизованы!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            new ProductsForm().ShowDialog();
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            if (UserSession.CurrentUser == null)
            {
                MessageBox.Show("Вы не авторизованы!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            new OrdersForm().ShowDialog();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            if (UserSession.CurrentUser == null)
            {
                MessageBox.Show("Вы не авторизованы!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!UserSession.IsAdmin && !UserSession.IsManager)
            {
                MessageBox.Show("У вас нет прав доступа к отчетам!",
                    "Доступ запрещен", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            new ReportsForm().ShowDialog();
        }

        private void btnCategories_Click(object sender, EventArgs e)
        {
            if (UserSession.CurrentUser == null)
            {
                MessageBox.Show("Вы не авторизованы!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!UserSession.IsAdmin && !UserSession.IsManager)
            {
                MessageBox.Show("У вас нет прав доступа к категориям!",
                    "Доступ запрещен", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            new CategoriesForm().ShowDialog();
        }

        private void btnSuppliers_Click(object sender, EventArgs e)
        {
            if (UserSession.CurrentUser == null)
            {
                MessageBox.Show("Вы не авторизованы!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!UserSession.IsAdmin && !UserSession.IsManager)
            {
                MessageBox.Show("У вас нет прав доступа к поставщикам!",
                    "Доступ запрещен", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            new SuppliersForm().ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Вы уверены, что хотите выйти из приложения?",
                "Выход",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Останавливаем трекер бездействия
                InactivityTracker.Stop();

                // Очищаем сессию
                UserSession.Clear();

                // Полный выход из приложения
                Application.Exit();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Останавливаем трекер при закрытии формы
            InactivityTracker.Stop();
        }
    }
}