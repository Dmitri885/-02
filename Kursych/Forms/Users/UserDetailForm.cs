using System;
using System.Drawing;
using System.Windows.Forms;

namespace Kursych.Forms.Users
{
    public partial class UserDetailForm : Form
    {
        private User _user;

        // Элементы управления
        private Label lblLoginValue;
        private Label lblRoleValue;
        private Label lblCreatedValue;
        private Label lblStatusValue;
        private TextBox txtFullName;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private TextBox txtAddress;
        private TextBox txtBirthDate;

        public UserDetailForm(User user)
        {
            InitializeComponent();
            _user = user;
            InitializeCustomComponents();
            LoadUserData();
        }

        private void InitializeCustomComponents()
        {
            this.SuspendLayout();

            // Настройка формы
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(240, 242, 245);

            // Основная панель
            var mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                ColumnCount = 2,
                RowCount = 13,
                BackColor = Color.White
            };

            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

            // Заголовок
            var lblHeader = new Label
            {
                Text = "👤 Информация о пользователе",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Dock = DockStyle.Fill
            };
            mainPanel.Controls.Add(lblHeader, 0, 0);
            mainPanel.SetColumnSpan(lblHeader, 2);

            // Разделитель
            var separator1 = new Panel
            {
                Height = 1,
                BackColor = Color.FromArgb(200, 200, 200),
                Dock = DockStyle.Fill
            };
            mainPanel.Controls.Add(separator1, 0, 1);
            mainPanel.SetColumnSpan(separator1, 2);

            // Основная информация
            AddLabel(mainPanel, "Логин:", 2);
            lblLoginValue = AddValueLabel(mainPanel, "", 2);

            AddLabel(mainPanel, "Роль:", 3);
            lblRoleValue = AddValueLabel(mainPanel, "", 3);

            AddLabel(mainPanel, "Дата создания:", 4);
            lblCreatedValue = AddValueLabel(mainPanel, "", 4);

            AddLabel(mainPanel, "Статус:", 5);
            lblStatusValue = AddValueLabel(mainPanel, "", 5);

            // Разделитель
            var separator2 = new Panel
            {
                Height = 1,
                BackColor = Color.FromArgb(200, 200, 200),
                Dock = DockStyle.Fill
            };
            mainPanel.Controls.Add(separator2, 0, 6);
            mainPanel.SetColumnSpan(separator2, 2);

            // Заголовок персональных данных
            var lblPersonalHeader = new Label
            {
                Text = "Персональные данные",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Dock = DockStyle.Fill
            };
            mainPanel.Controls.Add(lblPersonalHeader, 0, 7);
            mainPanel.SetColumnSpan(lblPersonalHeader, 2);

            // Персональные данные
            AddLabel(mainPanel, "ФИО:", 8);
            txtFullName = AddTextBox(mainPanel, 8);

            AddLabel(mainPanel, "Телефон:", 9);
            txtPhone = AddTextBox(mainPanel, 9);

            AddLabel(mainPanel, "Email:", 10);
            txtEmail = AddTextBox(mainPanel, 10);

            AddLabel(mainPanel, "Адрес:", 11);
            txtAddress = AddTextBox(mainPanel, 11);

            AddLabel(mainPanel, "Дата рождения:", 12);
            txtBirthDate = AddTextBox(mainPanel, 12);

            // Кнопка закрытия
            var btnClose = new Button
            {
                Text = "Закрыть",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 134, 222),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(120, 35),
                Anchor = AnchorStyles.None
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();

            var btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.White,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(10)
            };
            btnPanel.Controls.Add(btnClose);

            // Добавляем на форму
            this.Controls.Add(mainPanel);
            this.Controls.Add(btnPanel);

            this.ResumeLayout(false);
        }

        private void AddLabel(TableLayoutPanel panel, string text, int row)
        {
            var label = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                TextAlign = ContentAlignment.MiddleRight,
                Dock = DockStyle.Fill,
                Padding = new Padding(5)
            };
            panel.Controls.Add(label, 0, row);
        }

        private Label AddValueLabel(TableLayoutPanel panel, string text, int row)
        {
            var label = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(44, 62, 80),
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Padding = new Padding(5)
            };
            panel.Controls.Add(label, 1, row);
            return label;
        }

        private TextBox AddTextBox(TableLayoutPanel panel, int row)
        {
            var textBox = new TextBox
            {
                ReadOnly = true,
                BackColor = Color.FromArgb(240, 242, 245),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10F),
                Dock = DockStyle.Fill,
                Padding = new Padding(5)
            };
            panel.Controls.Add(textBox, 1, row);
            return textBox;
        }

        private void LoadUserData()
        {
            try
            {
                // Основная информация
                lblLoginValue.Text = _user.UserLogin ?? "";
                lblRoleValue.Text = _user.RoleName ?? "";
                lblCreatedValue.Text = _user.CreatedDate.ToString("dd.MM.yyyy HH:mm");
                lblStatusValue.Text = _user.IsActive ? "Активен" : "Заблокирован";
                lblStatusValue.ForeColor = _user.IsActive ? Color.Green : Color.Red;

                // Персональные данные (полностью видимы)
                txtFullName.Text = _user.FullName ?? "";
                txtPhone.Text = string.IsNullOrEmpty(_user.Phone) ? "не указан" : _user.Phone;
                txtEmail.Text = string.IsNullOrEmpty(_user.Email) ? "не указан" : _user.Email;
                txtAddress.Text = string.IsNullOrEmpty(_user.Address) ? "не указан" : _user.Address;
                txtBirthDate.Text = _user.BirthDate?.ToString("dd.MM.yyyy") ?? "не указана";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}