using System;
using System.Windows.Forms;
using Kursych.Forms.Auth;

namespace Kursych.Forms.Config
{
    public partial class DatabaseConfigForm : Form
    {
        private Kursych.MySQLHelper db;

        public DatabaseConfigForm()
        {
            InitializeComponent();

            // Подключаем обработчики событий
            this.btnTest.Click += new EventHandler(this.btnTest_Click);
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.chkShowPassword.CheckedChanged += new EventHandler(this.chkShowPassword_CheckedChanged);

            db = new Kursych.MySQLHelper();
            LoadCurrentSettings();
        }

        private void LoadCurrentSettings()
        {
            var settings = db.LoadConnectionSettings();
            txtServer.Text = settings.server;
            txtDatabase.Text = settings.database;
            txtUsername.Text = settings.uid;
            txtPassword.Text = settings.password;
            txtPort.Text = settings.port;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                // Используем метод с 5 параметрами
                if (db.TestConnection(
                    txtServer.Text,
                    txtDatabase.Text,
                    txtUsername.Text,
                    txtPassword.Text,
                    txtPort.Text))
                {
                    MessageBox.Show("✅ Подключение к базе данных успешно!",
                        "Тест подключения",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("❌ Не удалось подключиться к базе данных!\nПроверьте параметры подключения.",
                        "Ошибка подключения",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                db.SaveConnectionSettings(
                    txtServer.Text,
                    txtDatabase.Text,
                    txtUsername.Text,
                    txtPassword.Text,
                    txtPort.Text
                );

                MessageBox.Show("✅ Настройки успешно сохранены!",
                    "Сохранение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtServer.Text))
            {
                MessageBox.Show("Введите адрес сервера!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtServer.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDatabase.Text))
            {
                MessageBox.Show("Введите название базы данных!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDatabase.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Введите имя пользователя!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPort.Text))
            {
                MessageBox.Show("Введите порт!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPort.Focus();
                return false;
            }

            return true;
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '●';
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}