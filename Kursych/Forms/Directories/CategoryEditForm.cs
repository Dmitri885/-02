using System;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Kursych.Forms.Directories
{
    public partial class CategoryEditForm : Form
    {
        public string CategoryName => txtName.Text.Trim();
        public string Description => txtDescription.Text.Trim();

        public CategoryEditForm()
        {
            InitializeComponent();

            // Подключаем обработчики событий
            this.btnOk.Click += BtnOk_Click;
            this.btnCancel.Click += BtnCancel_Click;
        }

        public void SetCategoryData(int id, string name, string description)
        {
            txtName.Text = name;
            txtDescription.Text = description;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название категории", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return;
            }

            if (txtName.Text.Length > 100)
            {
                MessageBox.Show("Название категории не должно превышать 100 символов", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}