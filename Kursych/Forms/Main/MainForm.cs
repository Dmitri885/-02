using System;
using System.Windows.Forms;
using Kursych.Forms.Directories;
using Kursych.Forms.Orders;
using Kursych.Forms.Products;
using Kursych.Forms.Reports;
using Kursych.Forms.Users;
using Kursych.Forms.Auth;

namespace Kursych.Forms.Main
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            LoadUserInfo();
            ConfigureMenuVisibility();
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
                // Очищаем текущую сессию
                UserSession.Clear();

                // Вместо закрытия формы, мы создаем событие для Program.cs
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
        }

        // Обработчики для кнопок меню
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
                // Очищаем сессию
                UserSession.Clear();

                // Полный выход из приложения
                Application.Exit();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Ничего не делаем
        }
    }
}