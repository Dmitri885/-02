using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Kursych.Forms.Directories
{
    public partial class CategoriesForm : Form
    {
        public CategoriesForm()
        {
            InitializeComponent();

            // Подключаем обработчики событий для кнопок
            this.btnAdd.Click += btnAdd_Click;
            this.btnEdit.Click += btnEdit_Click;
            this.btnDelete.Click += btnDelete_Click;
            this.btnRefresh.Click += btnRefresh_Click;

            // Загружаем данные СРАЗУ в конструкторе
            LoadCategories();
        }

        private void CategoriesForm_Load(object sender, EventArgs e)
        {
            // Данные уже загружены в конструкторе
        }

        private void LoadCategories()
        {
            try
            {
                dataGridView.Rows.Clear();
                dataGridView.Columns.Clear();

                // Создание колонок согласно вашей БД
                dataGridView.Columns.Add("categoriesID", "ID");
                dataGridView.Columns.Add("categoriesName", "Название категории");
                dataGridView.Columns.Add("Description", "Описание");
                dataGridView.Columns.Add("CreatedDate", "Дата создания");

                // Загрузка данных из базы данных
                string query = @"SELECT categoriesID, categoriesName, Description, CreatedDate 
                               FROM categories 
                               ORDER BY categoriesName";

                var dbHelper = new Kursych.MySQLHelper();
                DataTable dt = dbHelper.ExecuteQuery(query);

                foreach (DataRow row in dt.Rows)
                {
                    dataGridView.Rows.Add(
                        row["categoriesID"],
                        row["categoriesName"],
                        row["Description"],
                        row["CreatedDate"]
                    );
                }

                // Настройка ширины колонок
                if (dataGridView.Columns.Count > 0)
                {
                    dataGridView.Columns["categoriesID"].Width = 50;
                    dataGridView.Columns["categoriesName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView.Columns["CreatedDate"].Width = 120;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке категорий: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var dialog = new CategoryEditForm())
            {
                dialog.Text = "Добавление категории";
                dialog.SetCategoryData(0, "", "");

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Сохранение в базу данных
                        string name = MySqlHelper.EscapeString(dialog.CategoryName);
                        string description = MySqlHelper.EscapeString(dialog.Description);

                        string query = $@"INSERT INTO categories (categoriesName, Description) 
                                       VALUES ('{name}', '{description}')";

                        var dbHelper = new Kursych.MySQLHelper();
                        int result = dbHelper.ExecuteNonQuery(query);

                        if (result > 0)
                        {
                            MessageBox.Show("Категория успешно добавлена", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadCategories();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при добавлении категории: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите категорию для редактирования", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow selectedRow = dataGridView.SelectedRows[0];
            int id = Convert.ToInt32(selectedRow.Cells["categoriesID"].Value);
            string name = selectedRow.Cells["categoriesName"].Value.ToString();
            string description = selectedRow.Cells["Description"].Value?.ToString() ?? "";

            using (var dialog = new CategoryEditForm())
            {
                dialog.Text = "Редактирование категории";
                dialog.SetCategoryData(id, name, description);

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Обновление в базе данных
                        string escapedName = MySqlHelper.EscapeString(dialog.CategoryName);
                        string escapedDescription = MySqlHelper.EscapeString(dialog.Description);

                        string query = $@"UPDATE categories 
                                       SET categoriesName = '{escapedName}', 
                                           Description = '{escapedDescription}' 
                                       WHERE categoriesID = {id}";

                        var dbHelper = new Kursych.MySQLHelper();
                        int result = dbHelper.ExecuteNonQuery(query);

                        if (result > 0)
                        {
                            MessageBox.Show("Категория успешно обновлена", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadCategories();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при обновлении категории: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите категорию для удаления", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow selectedRow = dataGridView.SelectedRows[0];
            int id = Convert.ToInt32(selectedRow.Cells["categoriesID"].Value);
            string name = selectedRow.Cells["categoriesName"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Вы уверены, что хотите удалить категорию \"{name}\"?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var dbHelper = new Kursych.MySQLHelper();

                    // Проверка на использование категории в товарах
                    string checkQuery = $"SELECT COUNT(*) FROM product WHERE categoriesID = {id}";
                    object countResult = dbHelper.ExecuteScalar(checkQuery);

                    int productCount = countResult != null ? Convert.ToInt32(countResult) : 0;

                    if (productCount > 0)
                    {
                        MessageBox.Show($"Невозможно удалить категорию. Она используется в {productCount} товар(ах).",
                            "Ошибка удаления",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Удаление из базы данных
                    string deleteQuery = $"DELETE FROM categories WHERE categoriesID = {id}";
                    int deleteResult = dbHelper.ExecuteNonQuery(deleteQuery);

                    if (deleteResult > 0)
                    {
                        MessageBox.Show("Категория успешно удалена", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCategories();
                    }
                    else
                    {
                        MessageBox.Show("Категория не найдена", "Информация",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении категории: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadCategories();
        }
    }
}