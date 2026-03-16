using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Kursych.Forms.Directories
{
    public partial class SupplierEditForm : Form
    {
        private bool isEditMode = false;
        private int supplierId = 0;

        public string SupplierName => txtName.Text.Trim();
        public string ContactInfo => txtContactInfo.Text.Trim();
        public string Phone => txtPhone.Text.Trim();
        public string Email => txtEmail.Text.Trim();
        public string Address => txtAddress.Text.Trim();

        public SupplierEditForm()
        {
            InitializeComponent();

            // Подключаем обработчики событий
            this.btnOk.Click += btnOk_Click;
            this.btnCancel.Click += btnCancel_Click;

            // Ограничения для полей
            this.txtPhone.KeyPress += txtPhone_KeyPress;
        }

        public void SetAddMode()
        {
            isEditMode = false;
            txtName.Text = "";
            txtContactInfo.Text = "";
            txtAddress.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            this.Text = "Добавление поставщика";
        }

        public void SetEditMode(int id, string name, string contactInfo, string address, string phone, string email)
        {
            isEditMode = true;
            supplierId = id;
            txtName.Text = name;
            txtContactInfo.Text = contactInfo;
            txtAddress.Text = address;
            txtPhone.Text = phone;
            txtEmail.Text = email;
            this.Text = "Редактирование поставщика";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название компании", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Введите телефон", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPhone.Focus();
                return false;
            }

            // Проверка телефона (только цифры, 11 символов)
            string digitsOnly = Regex.Replace(txtPhone.Text, @"[^\d]", "");
            if (digitsOnly.Length != 11)
            {
                MessageBox.Show("Телефон должен содержать 11 цифр", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPhone.Focus();
                return false;
            }

            // Проверка email если он заполнен
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(txtEmail.Text, emailPattern))
                {
                    MessageBox.Show("Введите корректный email адрес", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtEmail.Focus();
                    return false;
                }
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем только цифры и управляющие клавиши
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}