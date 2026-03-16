//using System;
//using System.Windows.Forms;

//namespace Kursych.Forms.Directories
//{
//    public partial class ManufacturersForm : Form
//    {
//        public ManufacturersForm()
//        {
//            InitializeComponent9();
//        }

//        private DataGridView dataGridView;
//        private Button btnAdd;
//        private Button btnEdit;
//        private Button btnDelete;

//        private void InitializeComponent9()
//        {
//            this.dataGridView = new DataGridView();
//            this.btnAdd = new Button();
//            this.btnEdit = new Button();
//            this.btnDelete = new Button();

//            // dataGridView
//            this.dataGridView.Location = new System.Drawing.Point(12, 12);
//            this.dataGridView.Name = "dataGridView";
//            this.dataGridView.Size = new System.Drawing.Size(500, 300);
//            this.dataGridView.TabIndex = 0;
//            this.dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
//            this.dataGridView.ReadOnly = true;
//            this.dataGridView.AllowUserToAddRows = false;
//            this.dataGridView.AllowUserToDeleteRows = false;

//            // btnAdd
//            this.btnAdd.Location = new System.Drawing.Point(12, 320);
//            this.btnAdd.Name = "btnAdd";
//            this.btnAdd.Size = new System.Drawing.Size(100, 30);
//            this.btnAdd.TabIndex = 1;
//            this.btnAdd.Text = "Добавить";
//            this.btnAdd.UseVisualStyleBackColor = true;
//            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);

//            // btnEdit
//            this.btnEdit.Location = new System.Drawing.Point(120, 320);
//            this.btnEdit.Name = "btnEdit";
//            this.btnEdit.Size = new System.Drawing.Size(100, 30);
//            this.btnEdit.TabIndex = 2;
//            this.btnEdit.Text = "Редактировать";
//            this.btnEdit.UseVisualStyleBackColor = true;
//            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);

//            // btnDelete
//            this.btnDelete.Location = new System.Drawing.Point(228, 320);
//            this.btnDelete.Name = "btnDelete";
//            this.btnDelete.Size = new System.Drawing.Size(100, 30);
//            this.btnDelete.TabIndex = 3;
//            this.btnDelete.Text = "Удалить";
//            this.btnDelete.UseVisualStyleBackColor = true;
//            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);

//            // ManufacturersForm
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(524, 361);
//            this.Controls.Add(this.btnDelete);
//            this.Controls.Add(this.btnEdit);
//            this.Controls.Add(this.btnAdd);
//            this.Controls.Add(this.dataGridView);
//            this.Name = "ManufacturersForm";
//            this.StartPosition = FormStartPosition.CenterScreen;
//            this.Text = "Справочник производителей";
//            this.Load += new EventHandler(this.ManufacturersForm_Load);
//        }

//        private void ManufacturersForm_Load(object sender, EventArgs e)
//        {
//            LoadManufacturers();
//        }

//        private void LoadManufacturers()
//        {
//            dataGridView.Columns.Clear();
//            dataGridView.Columns.Add("Id", "ID");
//            dataGridView.Columns.Add("Name", "Название");
//            dataGridView.Columns.Add("Country", "Страна");
//            dataGridView.Columns.Add("Address", "Адрес");
//            dataGridView.Columns.Add("Phone", "Телефон");
//            dataGridView.Columns.Add("Email", "Email");

//            // TODO: Загрузка данных из базы данных
//            dataGridView.Rows.Add(1, "Apple Inc.", "США", "1 Apple Park Way, Cupertino, CA 95014", "+1-408-996-1010", "info@apple.com");
//            dataGridView.Rows.Add(2, "Samsung Electronics", "Южная Корея", "129 Samsung-ro, Yeongtong-gu, Suwon-si, Gyeonggi-do", "+82-31-200-1114", "contact@samsung.com");
//            dataGridView.Rows.Add(3, "Sony Corporation", "Япония", "1-7-1 Konan, Minato-ku, Tokyo 108-0075", "+81-3-6748-2111", "info@sony.com");
//            dataGridView.Rows.Add(4, "LG Electronics", "Южная Корея", "128 Yeoui-daero, Yeongdeungpo-gu, Seoul 07336", "+82-2-3777-1114", "help@lg.com");
//            dataGridView.Rows.Add(5, "Xiaomi Corporation", "Китай", "Building A, No.68 Qinghe Middle Street, Haidian District, Beijing", "+86-10-6060-6688", "service@xiaomi.com");
//        }

//        private void btnAdd_Click(object sender, EventArgs e)
//        {
//            using (var dialog = new ManufacturerEditForm())
//            {
//                dialog.Text = "Добавление производителя";
//                dialog.SetManufacturerData(0, "", "", "", "", "");

//                if (dialog.ShowDialog() == DialogResult.OK)
//                {
//                    // TODO: Сохранение в базу данных
//                    int newId = dataGridView.Rows.Count + 1;
//                    dataGridView.Rows.Add(newId, dialog.ManufacturerName, dialog.Country,
//                        dialog.Address, dialog.Phone, dialog.Email);

//                    MessageBox.Show("Производитель успешно добавлен", "Успех",
//                        MessageBoxButtons.OK, MessageBoxIcon.Information);
//                }
//            }
//        }

//        private void btnEdit_Click(object sender, EventArgs e)
//        {
//            if (dataGridView.SelectedRows.Count == 0)
//            {
//                MessageBox.Show("Выберите производителя для редактирования", "Информация",
//                    MessageBoxButtons.OK, MessageBoxIcon.Information);
//                return;
//            }

//            DataGridViewRow selectedRow = dataGridView.SelectedRows[0];
//            int id = Convert.ToInt32(selectedRow.Cells["Id"].Value);
//            string name = selectedRow.Cells["Name"].Value.ToString();
//            string country = selectedRow.Cells["Country"].Value?.ToString() ?? "";
//            string address = selectedRow.Cells["Address"].Value?.ToString() ?? "";
//            string phone = selectedRow.Cells["Phone"].Value?.ToString() ?? "";
//            string email = selectedRow.Cells["Email"].Value?.ToString() ?? "";

//            using (var dialog = new ManufacturerEditForm())
//            {
//                dialog.Text = "Редактирование производителя";
//                dialog.SetManufacturerData(id, name, country, address, phone, email);

//                if (dialog.ShowDialog() == DialogResult.OK)
//                {
//                    // TODO: Обновление в базе данных
//                    selectedRow.Cells["Name"].Value = dialog.ManufacturerName;
//                    selectedRow.Cells["Country"].Value = dialog.Country;
//                    selectedRow.Cells["Address"].Value = dialog.Address;
//                    selectedRow.Cells["Phone"].Value = dialog.Phone;
//                    selectedRow.Cells["Email"].Value = dialog.Email;

//                    MessageBox.Show("Производитель успешно обновлен", "Успех",
//                        MessageBoxButtons.OK, MessageBoxIcon.Information);
//                }
//            }
//        }

//        private void btnDelete_Click(object sender, EventArgs e)
//        {
//            if (dataGridView.SelectedRows.Count == 0)
//            {
//                MessageBox.Show("Выберите производителя для удаления", "Информация",
//                    MessageBoxButtons.OK, MessageBoxIcon.Information);
//                return;
//            }

//            DataGridViewRow selectedRow = dataGridView.SelectedRows[0];
//            int id = Convert.ToInt32(selectedRow.Cells["Id"].Value);
//            string name = selectedRow.Cells["Name"].Value.ToString();

//            DialogResult result = MessageBox.Show(
//                $"Вы уверены, что хотите удалить производителя \"{name}\"?",
//                "Подтверждение удаления",
//                MessageBoxButtons.YesNo,
//                MessageBoxIcon.Question);

//            if (result == DialogResult.Yes)
//            {
//                try
//                {
//                    // TODO: Проверка на использование производителя в товарах
//                    // TODO: Удаление из базы данных
//                    dataGridView.Rows.Remove(selectedRow);

//                    MessageBox.Show("Производитель успешно удален", "Успех",
//                        MessageBoxButtons.OK, MessageBoxIcon.Information);
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show($"Ошибка при удалении производителя: {ex.Message}", "Ошибка",
//                        MessageBoxButtons.OK, MessageBoxIcon.Error);
//                }
//            }
//        }

//        // Вспомогательный класс для диалога редактирования
//        private class ManufacturerEditForm : Form
//        {
//            private TextBox txtName;
//            private TextBox txtCountry;
//            private TextBox txtAddress;
//            private TextBox txtPhone;
//            private TextBox txtEmail;
//            private Button btnOk;
//            private Button btnCancel;
//            private Label lblName;
//            private Label lblCountry;
//            private Label lblAddress;
//            private Label lblPhone;
//            private Label lblEmail;

//            public string ManufacturerName => txtName.Text.Trim();
//            public string Country => txtCountry.Text.Trim();
//            public string Address => txtAddress.Text.Trim();
//            public string Phone => txtPhone.Text.Trim();
//            public string Email => txtEmail.Text.Trim();

//            public ManufacturerEditForm()
//            {
//                InitializeComponent();
//            }

//            private void InitializeComponent()
//            {
//                this.txtName = new TextBox();
//                this.txtCountry = new TextBox();
//                this.txtAddress = new TextBox();
//                this.txtPhone = new TextBox();
//                this.txtEmail = new TextBox();
//                this.btnOk = new Button();
//                this.btnCancel = new Button();
//                this.lblName = new Label();
//                this.lblCountry = new Label();
//                this.lblAddress = new Label();
//                this.lblPhone = new Label();
//                this.lblEmail = new Label();

//                int y = 12;
//                int labelWidth = 80;
//                int textBoxWidth = 250;

//                // lblName
//                this.lblName.AutoSize = true;
//                this.lblName.Location = new System.Drawing.Point(12, y);
//                this.lblName.Name = "lblName";
//                this.lblName.Size = new System.Drawing.Size(60, 13);
//                this.lblName.TabIndex = 0;
//                this.lblName.Text = "Название:";

//                // txtName
//                this.txtName.Location = new System.Drawing.Point(labelWidth, y - 3);
//                this.txtName.Name = "txtName";
//                this.txtName.Size = new System.Drawing.Size(textBoxWidth, 20);
//                this.txtName.TabIndex = 1;

//                y += 30;

//                // lblCountry
//                this.lblCountry.AutoSize = true;
//                this.lblCountry.Location = new System.Drawing.Point(12, y);
//                this.lblCountry.Name = "lblCountry";
//                this.lblCountry.Size = new System.Drawing.Size(46, 13);
//                this.lblCountry.TabIndex = 2;
//                this.lblCountry.Text = "Страна:";

//                // txtCountry
//                this.txtCountry.Location = new System.Drawing.Point(labelWidth, y - 3);
//                this.txtCountry.Name = "txtCountry";
//                this.txtCountry.Size = new System.Drawing.Size(textBoxWidth, 20);
//                this.txtCountry.TabIndex = 3;

//                y += 30;

//                // lblAddress
//                this.lblAddress.AutoSize = true;
//                this.lblAddress.Location = new System.Drawing.Point(12, y);
//                this.lblAddress.Name = "lblAddress";
//                this.lblAddress.Size = new System.Drawing.Size(41, 13);
//                this.lblAddress.TabIndex = 4;
//                this.lblAddress.Text = "Адрес:";

//                // txtAddress
//                this.txtAddress.Location = new System.Drawing.Point(labelWidth, y - 3);
//                this.txtAddress.Name = "txtAddress";
//                this.txtAddress.Size = new System.Drawing.Size(textBoxWidth, 20);
//                this.txtAddress.TabIndex = 5;

//                y += 30;

//                // lblPhone
//                this.lblPhone.AutoSize = true;
//                this.lblPhone.Location = new System.Drawing.Point(12, y);
//                this.lblPhone.Name = "lblPhone";
//                this.lblPhone.Size = new System.Drawing.Size(55, 13);
//                this.lblPhone.TabIndex = 6;
//                this.lblPhone.Text = "Телефон:";

//                // txtPhone
//                this.txtPhone.Location = new System.Drawing.Point(labelWidth, y - 3);
//                this.txtPhone.Name = "txtPhone";
//                this.txtPhone.Size = new System.Drawing.Size(textBoxWidth, 20);
//                this.txtPhone.TabIndex = 7;

//                y += 30;

//                // lblEmail
//                this.lblEmail.AutoSize = true;
//                this.lblEmail.Location = new System.Drawing.Point(12, y);
//                this.lblEmail.Name = "lblEmail";
//                this.lblEmail.Size = new System.Drawing.Size(35, 13);
//                this.lblEmail.TabIndex = 8;
//                this.lblEmail.Text = "Email:";

//                // txtEmail
//                this.txtEmail.Location = new System.Drawing.Point(labelWidth, y - 3);
//                this.txtEmail.Name = "txtEmail";
//                this.txtEmail.Size = new System.Drawing.Size(textBoxWidth, 20);
//                this.txtEmail.TabIndex = 9;

//                y += 35;

//                // btnOk
//                this.btnOk.Location = new System.Drawing.Point(180, y);
//                this.btnOk.Name = "btnOk";
//                this.btnOk.Size = new System.Drawing.Size(75, 23);
//                this.btnOk.TabIndex = 10;
//                this.btnOk.Text = "ОК";
//                this.btnOk.UseVisualStyleBackColor = true;
//                this.btnOk.Click += new EventHandler(this.btnOk_Click);

//                // btnCancel
//                this.btnCancel.Location = new System.Drawing.Point(261, y);
//                this.btnCancel.Name = "btnCancel";
//                this.btnCancel.Size = new System.Drawing.Size(75, 23);
//                this.btnCancel.TabIndex = 11;
//                this.btnCancel.Text = "Отмена";
//                this.btnCancel.UseVisualStyleBackColor = true;
//                this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

//                // ManufacturerEditForm
//                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//                this.AutoScaleMode = AutoScaleMode.Font;
//                this.ClientSize = new System.Drawing.Size(348, y + 40);
//                this.Controls.Add(this.btnCancel);
//                this.Controls.Add(this.btnOk);
//                this.Controls.Add(this.txtEmail);
//                this.Controls.Add(this.lblEmail);
//                this.Controls.Add(this.txtPhone);
//                this.Controls.Add(this.lblPhone);
//                this.Controls.Add(this.txtAddress);
//                this.Controls.Add(this.lblAddress);
//                this.Controls.Add(this.txtCountry);
//                this.Controls.Add(this.lblCountry);
//                this.Controls.Add(this.txtName);
//                this.Controls.Add(this.lblName);
//                this.FormBorderStyle = FormBorderStyle.FixedDialog;
//                this.MaximizeBox = false;
//                this.MinimizeBox = false;
//                this.StartPosition = FormStartPosition.CenterParent;
//            }

//            public void SetManufacturerData(int id, string name, string country, string address, string phone, string email)
//            {
//                txtName.Text = name;
//                txtCountry.Text = country;
//                txtAddress.Text = address;
//                txtPhone.Text = phone;
//                txtEmail.Text = email;
//            }

//            private void btnOk_Click(object sender, EventArgs e)
//            {
//                if (string.IsNullOrWhiteSpace(txtName.Text))
//                {
//                    MessageBox.Show("Введите название производителя", "Ошибка",
//                        MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    return;
//                }

//                if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !IsValidEmail(txtEmail.Text))
//                {
//                    MessageBox.Show("Введите корректный email адрес", "Ошибка",
//                        MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    return;
//                }

//                this.DialogResult = DialogResult.OK;
//                this.Close();
//            }

//            private bool IsValidEmail(string email)
//            {
//                try
//                {
//                    var addr = new System.Net.Mail.MailAddress(email);
//                    return addr.Address == email;
//                }
//                catch
//                {
//                    return false;
//                }
//            }

//            private void btnCancel_Click(object sender, EventArgs e)
//            {
//                this.DialogResult = DialogResult.Cancel;
//                this.Close();
//            }
//        }
//    }
//}