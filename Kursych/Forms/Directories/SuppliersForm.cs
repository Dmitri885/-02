using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Kursych.Forms.Directories
{
    public partial class SuppliersForm : Form
    {
        // Переменная для хранения правильного имени поля контактной информации
        private string contactFieldName = "";

        public SuppliersForm()
        {
            InitializeComponent();

            // Подключаем обработчики событий
            this.btnAdd.Click += btnAdd_Click;
            this.btnEdit.Click += btnEdit_Click;
            this.btnDelete.Click += btnDelete_Click;
            this.btnRefresh.Click += btnRefresh_Click;

            // Загружаем данные сразу в конструкторе
            DetectContactFieldName();
            LoadSuppliers();
        }

        private void SuppliersForm_Load(object sender, EventArgs e)
        {
            // Данные уже загружены в конструкторе
        }

        private void DetectContactFieldName()
        {
            try
            {
                var dbHelper = new Kursych.MySQLHelper();
                string query = @"SHOW COLUMNS FROM suppliers";
                DataTable columns = dbHelper.ExecuteQuery(query);

                // Ищем поле, которое может быть полем контактов
                foreach (DataRow col in columns.Rows)
                {
                    string fieldName = col["Field"].ToString().ToLower();
                    if (fieldName.Contains("contact") || fieldName.Contains("contain") ||
                        fieldName.Contains("person") || fieldName.Contains("info"))
                    {
                        contactFieldName = col["Field"].ToString();
                        return;
                    }
                }

                // Если не нашли, используем первую попытку
                contactFieldName = "Contactinformation";
            }
            catch
            {
                contactFieldName = "Contactinformation";
            }
        }

        private void LoadSuppliers()
        {
            try
            {
                dataGridView.Rows.Clear();
                dataGridView.Columns.Clear();

                // Определяем правильные имена полей
                DetectContactFieldName();

                var dbHelper = new Kursych.MySQLHelper();

                // Сначала проверим, какие поля существуют
                string checkQuery = @"SHOW COLUMNS FROM suppliers";
                DataTable columns = dbHelper.ExecuteQuery(checkQuery);

                // Собираем список существующих полей
                List<string> existingFields = new List<string>();
                foreach (DataRow col in columns.Rows)
                {
                    existingFields.Add(col["Field"].ToString());
                }

                // Формируем запрос только с существующими полями
                List<string> selectFields = new List<string>();
                List<string> displayFields = new List<string>();

                // Основные поля, которые нам нужны
                Dictionary<string, string> neededFields = new Dictionary<string, string>
                {
                    {"SuppliersID", "ID"},
                    {"Name", "Название"},
                    {"Address", "Адрес"},
                    {"Phone", "Телефон"},
                    {"Email", "Email"},
                    {"CreatedDate", "Дата создания"}
                };

                // Добавляем поле контактов, если оно есть
                if (!string.IsNullOrEmpty(contactFieldName) && existingFields.Contains(contactFieldName))
                {
                    neededFields[contactFieldName] = "Контактная информация";
                }
                else
                {
                    // Ищем альтернативное поле
                    foreach (string field in existingFields)
                    {
                        string fieldLower = field.ToLower();
                        if ((fieldLower.Contains("contact") || fieldLower.Contains("contain") ||
                             fieldLower.Contains("person") || fieldLower.Contains("info")) &&
                            !fieldLower.Contains("id") && !fieldLower.Contains("date"))
                        {
                            contactFieldName = field;
                            neededFields[field] = "Контактная информация";
                            break;
                        }
                    }
                }

                // Формируем списки полей для запроса и отображения
                foreach (var field in neededFields)
                {
                    if (existingFields.Contains(field.Key))
                    {
                        selectFields.Add(field.Key);
                        displayFields.Add(field.Value);
                    }
                }

                if (selectFields.Count == 0)
                {
                    MessageBox.Show("Не удалось определить поля таблицы suppliers", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Создаем запрос
                string query = $"SELECT {string.Join(", ", selectFields)} FROM suppliers ORDER BY Name";
                DataTable dt = dbHelper.ExecuteQuery(query);

                // Создаем колонки
                for (int i = 0; i < selectFields.Count; i++)
                {
                    dataGridView.Columns.Add(selectFields[i], displayFields[i]);
                }

                // Заполняем данными
                foreach (DataRow row in dt.Rows)
                {
                    var values = new object[selectFields.Count];
                    for (int i = 0; i < selectFields.Count; i++)
                    {
                        values[i] = row[selectFields[i]];
                    }
                    dataGridView.Rows.Add(values);
                }

                // Настраиваем ширину колонок
                if (dataGridView.Columns.Count > 0)
                {
                    if (dataGridView.Columns.Contains("SuppliersID"))
                        dataGridView.Columns["SuppliersID"].Width = 50;
                    if (dataGridView.Columns.Contains("Name"))
                        dataGridView.Columns["Name"].Width = 150;
                    if (dataGridView.Columns.Contains("Phone"))
                        dataGridView.Columns["Phone"].Width = 100;
                    if (dataGridView.Columns.Contains("Email"))
                        dataGridView.Columns["Email"].Width = 150;
                    if (dataGridView.Columns.Contains("Address"))
                        dataGridView.Columns["Address"].Width = 200;
                    if (dataGridView.Columns.Contains("CreatedDate"))
                        dataGridView.Columns["CreatedDate"].Width = 120;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке поставщиков: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var editForm = new SupplierEditForm())
            {
                editForm.Text = "Добавление поставщика";
                editForm.SetAddMode();

                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string name = MySqlHelper.EscapeString(editForm.SupplierName);
                        string contactInfo = MySqlHelper.EscapeString(editForm.ContactInfo);
                        string address = MySqlHelper.EscapeString(editForm.Address);
                        string phone = MySqlHelper.EscapeString(editForm.Phone);
                        string email = MySqlHelper.EscapeString(editForm.Email);

                        string query = $@"INSERT INTO suppliers 
                                       (Name, {contactFieldName}, Address, Phone, Email) 
                                       VALUES ('{name}', '{contactInfo}', '{address}', '{phone}', '{email}')";

                        var dbHelper = new Kursych.MySQLHelper();
                        int result = dbHelper.ExecuteNonQuery(query);

                        if (result > 0)
                        {
                            MessageBox.Show("Поставщик успешно добавлен", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadSuppliers();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при добавлении поставщика: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите поставщика для редактирования", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow selectedRow = dataGridView.SelectedRows[0];
            int id = Convert.ToInt32(selectedRow.Cells["SuppliersID"].Value);
            string name = selectedRow.Cells["Name"].Value?.ToString() ?? "";

            // Получаем контактную информацию
            string contactInfo = "";
            if (!string.IsNullOrEmpty(contactFieldName) && dataGridView.Columns.Contains(contactFieldName))
            {
                contactInfo = selectedRow.Cells[contactFieldName].Value?.ToString() ?? "";
            }

            string address = selectedRow.Cells["Address"].Value?.ToString() ?? "";
            string phone = selectedRow.Cells["Phone"].Value?.ToString() ?? "";
            string email = selectedRow.Cells["Email"].Value?.ToString() ?? "";

            using (var editForm = new SupplierEditForm())
            {
                editForm.Text = "Редактирование поставщика";
                editForm.SetEditMode(id, name, contactInfo, address, phone, email);

                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string escapedName = MySqlHelper.EscapeString(editForm.SupplierName);
                        string escapedContactInfo = MySqlHelper.EscapeString(editForm.ContactInfo);
                        string escapedAddress = MySqlHelper.EscapeString(editForm.Address);
                        string escapedPhone = MySqlHelper.EscapeString(editForm.Phone);
                        string escapedEmail = MySqlHelper.EscapeString(editForm.Email);

                        string query = $@"UPDATE suppliers 
                                       SET Name = '{escapedName}', 
                                           {contactFieldName} = '{escapedContactInfo}',
                                           Address = '{escapedAddress}',
                                           Phone = '{escapedPhone}',
                                           Email = '{escapedEmail}'
                                       WHERE SuppliersID = {id}";

                        var dbHelper = new Kursych.MySQLHelper();
                        int result = dbHelper.ExecuteNonQuery(query);

                        if (result > 0)
                        {
                            MessageBox.Show("Поставщик успешно обновлен", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadSuppliers();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при обновлении поставщика: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите поставщика для удаления", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow selectedRow = dataGridView.SelectedRows[0];
            int id = Convert.ToInt32(selectedRow.Cells["SuppliersID"].Value);
            string name = selectedRow.Cells["Name"].Value?.ToString() ?? "поставщик";

            DialogResult result = MessageBox.Show(
                $"Вы уверены, что хотите удалить поставщика \"{name}\"?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var dbHelper = new Kursych.MySQLHelper();

                    // Проверка на использование поставщика в товарах
                    string checkQuery = $"SELECT COUNT(*) FROM product WHERE SuppliersID = {id}";
                    object countResult = dbHelper.ExecuteScalar(checkQuery);
                    int productCount = countResult != null ? Convert.ToInt32(countResult) : 0;

                    if (productCount > 0)
                    {
                        MessageBox.Show($"Невозможно удалить поставщика. Он используется в {productCount} товар(ах).",
                            "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string deleteQuery = $"DELETE FROM suppliers WHERE SuppliersID = {id}";
                    int deleteResult = dbHelper.ExecuteNonQuery(deleteQuery);

                    if (deleteResult > 0)
                    {
                        MessageBox.Show("Поставщик успешно удален", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadSuppliers();
                    }
                    else
                    {
                        MessageBox.Show("Поставщик не найден", "Информация",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении поставщика: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadSuppliers();
        }
    }
}