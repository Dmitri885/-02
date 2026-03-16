using System;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using Kursych.Forms.Main;

namespace Kursych.Forms.Products
{
    public partial class ProductsForm : Form
    {
        private DatabaseService dbService;
        private DataTable productsData;
        private DataView filteredView;
        private List<Category> categoriesList;
        private List<Supplier> suppliersList;

        public ProductsForm()
        {
            InitializeComponent();
            dbService = new DatabaseService();

            // Подключаем обработчики событий для кнопок
            this.btnAdd.Click += btnAdd_Click;
            this.btnEdit.Click += btnEdit_Click;
            this.btnDelete.Click += btnDelete_Click;
            this.btnRefresh.Click += btnRefresh_Click;
            this.btnClearFilters.Click += btnClearFilters_Click;

            // Подписываемся на события DataGridView
            this.dataGridView.DataBindingComplete += DataGridView_DataBindingComplete;
            this.dataGridView.CellDoubleClick += dataGridView_CellDoubleClick;

            // Подписываемся на события фильтров
            this.txtSearch.TextChanged += txtSearch_TextChanged;
            this.cmbCategories.SelectedIndexChanged += FilterComboBox_SelectedIndexChanged;
            this.cmbSuppliers.SelectedIndexChanged += FilterComboBox_SelectedIndexChanged;

            // Настраиваем видимость кнопок в зависимости от роли пользователя
            ConfigureButtonsByRole();

            // Загружаем данные после полной инициализации формы
            this.Shown += (s, e) => LoadAllData();
        }

        /// <summary>
        /// Настройка видимости кнопок в зависимости от роли пользователя
        /// </summary>
        private void ConfigureButtonsByRole()
        {
            try
            {
                // По умолчанию скрываем все кнопки
                btnAdd.Visible = false;
                btnEdit.Visible = false;
                btnDelete.Visible = false;

                // Если пользователь не авторизован - выходим
                if (UserSession.CurrentUser == null)
                {
                    return;
                }

                // Администратор (RoleID = 1) - все кнопки видны
                if (UserSession.IsAdmin)
                {
                    btnAdd.Visible = true;
                    btnEdit.Visible = true;
                    btnDelete.Visible = true;
                }
                // Менеджер (RoleID = 2) - все кнопки видны
                else if (UserSession.IsManager)
                {
                    btnAdd.Visible = true;
                    btnEdit.Visible = true;
                    btnDelete.Visible = true;
                }
                // Кассир/Продавец (RoleID = 3) - ТОЛЬКО ПРОСМОТР, кнопки скрыты
                else if (UserSession.IsCashier || UserSession.IsCashier)
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                }
                // Другие роли - скрываем все кнопки
                else
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                }
            }
            catch (Exception ex)
            {
                // В случае ошибки просто скрываем все кнопки
                btnAdd.Visible = false;
                btnEdit.Visible = false;
                btnDelete.Visible = false;
                Console.WriteLine($"Ошибка в ConfigureButtonsByRole: {ex.Message}");
            }
        }

        public class Category
        {
            public int categoriesID { get; set; }
            public string categoriesName { get; set; }
            public override string ToString() => categoriesName;
        }

        public class Supplier
        {
            public int SuppliersID { get; set; }
            public string Name { get; set; }
            public string Contacinformation { get; set; }
            public override string ToString() => Name;
        }

        private void LoadAllData()
        {
            try
            {
                LoadCategories();
                LoadSuppliers();
                LoadProductsData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCategories()
        {
            try
            {
                categoriesList = new List<Category>();
                categoriesList.Add(new Category { categoriesID = 0, categoriesName = "Все категории" });

                DataTable dt = dbService.GetCategories();

                if (dt != null && dt.Rows != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row != null)
                        {
                            object catId = row["categoriesID"];
                            object catName = row["categoriesName"];

                            if (catId != null && catName != null && catId != DBNull.Value && catName != DBNull.Value)
                            {
                                categoriesList.Add(new Category
                                {
                                    categoriesID = Convert.ToInt32(catId),
                                    categoriesName = catName.ToString()
                                });
                            }
                        }
                    }
                }

                if (cmbCategories != null)
                {
                    cmbCategories.DataSource = null;
                    cmbCategories.DataSource = categoriesList;
                    cmbCategories.DisplayMember = "categoriesName";
                    cmbCategories.ValueMember = "categoriesID";

                    if (cmbCategories.Items.Count > 0)
                        cmbCategories.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке категорий:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSuppliers()
        {
            try
            {
                suppliersList = new List<Supplier>();
                suppliersList.Add(new Supplier { SuppliersID = 0, Name = "Все поставщики" });

                DataTable dt = dbService.GetSuppliers();

                if (dt != null && dt.Rows != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row != null)
                        {
                            object supId = row["SuppliersID"];
                            object supName = row["Name"];

                            if (supId != null && supName != null && supId != DBNull.Value && supName != DBNull.Value)
                            {
                                suppliersList.Add(new Supplier
                                {
                                    SuppliersID = Convert.ToInt32(supId),
                                    Name = supName.ToString()
                                });
                            }
                        }
                    }
                }

                if (cmbSuppliers != null)
                {
                    cmbSuppliers.DataSource = null;
                    cmbSuppliers.DataSource = suppliersList;
                    cmbSuppliers.DisplayMember = "Name";
                    cmbSuppliers.ValueMember = "SuppliersID";

                    if (cmbSuppliers.Items.Count > 0)
                        cmbSuppliers.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке поставщиков:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable GetProductsFromDatabase()
        {
            DataTable dataTable = new DataTable();

            try
            {
                dataTable = dbService.GetProducts();

                // Проверяем, что таблица не null
                if (dataTable == null)
                {
                    dataTable = CreateProductsDataTableStructure();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке товаров:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataTable = CreateProductsDataTableStructure();
            }

            return dataTable;
        }

        private DataTable CreateProductsDataTableStructure()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Price", typeof(decimal));
            dt.Columns.Add("categoriesID", typeof(int));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("SuppliersID", typeof(int));
            dt.Columns.Add("StockQualitu", typeof(int));
            dt.Columns.Add("ImagePath", typeof(string));
            dt.Columns.Add("categoriesName", typeof(string));
            dt.Columns.Add("SupplierName", typeof(string));
            return dt;
        }

        private void LoadProductsData()
        {
            productsData = GetProductsFromDatabase();

            if (productsData != null && productsData.Rows.Count > 0)
            {
                filteredView = new DataView(productsData);
                dataGridView.DataSource = filteredView;
            }
            else
            {
                // Если данных нет, показываем пустую таблицу с колонками
                dataGridView.DataSource = null;
                dataGridView.Columns.Clear();
                dataGridView.Columns.Add("ProductID", "ID");
                dataGridView.Columns.Add("Name", "Название");
                dataGridView.Columns.Add("Price", "Цена");
                dataGridView.Columns.Add("categoriesName", "Категория");
                dataGridView.Columns.Add("Description", "Описание");
                dataGridView.Columns.Add("SupplierName", "Поставщик");
                dataGridView.Columns.Add("StockQualitu", "Количество");
                dataGridView.Columns.Add("ImagePath", "Путь к фото");

                // Скрываем служебные колонки
                dataGridView.Columns["ImagePath"].Visible = false;

                // Устанавливаем ширину колонок
                dataGridView.Columns["ProductID"].Width = 50;
                dataGridView.Columns["Name"].Width = 180;
                dataGridView.Columns["Price"].Width = 80;
                dataGridView.Columns["categoriesName"].Width = 120;
                dataGridView.Columns["SupplierName"].Width = 120;
                dataGridView.Columns["Description"].Width = 200;
                dataGridView.Columns["StockQualitu"].Width = 80;

                dataGridView.RowTemplate.Height = 80;
            }
        }

        private void ConfigureDataGridViewColumns()
        {
            // Проверяем, что dataGridView не null и имеет колонки
            if (dataGridView == null || dataGridView.Columns.Count == 0)
                return;

            // Настройка заголовков
            if (dataGridView.Columns.Contains("ProductID"))
                dataGridView.Columns["ProductID"].HeaderText = "ID";

            if (dataGridView.Columns.Contains("Name"))
                dataGridView.Columns["Name"].HeaderText = "Название";

            if (dataGridView.Columns.Contains("Price"))
            {
                dataGridView.Columns["Price"].HeaderText = "Цена";
                dataGridView.Columns["Price"].DefaultCellStyle.Format = "C2";
                dataGridView.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dataGridView.Columns.Contains("categoriesName"))
                dataGridView.Columns["categoriesName"].HeaderText = "Категория";

            if (dataGridView.Columns.Contains("Description"))
                dataGridView.Columns["Description"].HeaderText = "Описание";

            if (dataGridView.Columns.Contains("SupplierName"))
                dataGridView.Columns["SupplierName"].HeaderText = "Поставщик";

            if (dataGridView.Columns.Contains("StockQualitu"))
            {
                dataGridView.Columns["StockQualitu"].HeaderText = "Количество";
                dataGridView.Columns["StockQualitu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            // Скрываем служебные колонки
            if (dataGridView.Columns.Contains("ImagePath"))
                dataGridView.Columns["ImagePath"].Visible = false;

            if (dataGridView.Columns.Contains("categoriesID"))
                dataGridView.Columns["categoriesID"].Visible = false;

            if (dataGridView.Columns.Contains("SuppliersID"))
                dataGridView.Columns["SuppliersID"].Visible = false;

            // Устанавливаем ширину колонок
            if (dataGridView.Columns.Contains("ProductID"))
                dataGridView.Columns["ProductID"].Width = 50;

            if (dataGridView.Columns.Contains("Name"))
                dataGridView.Columns["Name"].Width = 180;

            if (dataGridView.Columns.Contains("Price"))
                dataGridView.Columns["Price"].Width = 80;

            if (dataGridView.Columns.Contains("categoriesName"))
                dataGridView.Columns["categoriesName"].Width = 120;

            if (dataGridView.Columns.Contains("SupplierName"))
                dataGridView.Columns["SupplierName"].Width = 120;

            if (dataGridView.Columns.Contains("Description"))
                dataGridView.Columns["Description"].Width = 200;

            if (dataGridView.Columns.Contains("StockQualitu"))
                dataGridView.Columns["StockQualitu"].Width = 80;

            // Устанавливаем высоту строк
            dataGridView.RowTemplate.Height = 80;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        // ================ РАБОТА С ИЗОБРАЖЕНИЯМИ ================

        private void DataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Сначала настраиваем колонки
            ConfigureDataGridViewColumns();
            // Затем добавляем колонку с изображением
            AddImageColumn();
        }

        private void AddImageColumn()
        {
            // Проверяем, что dataGridView не null
            if (dataGridView == null)
                return;

            // Удаляем старую колонку изображения если есть
            if (dataGridView.Columns.Contains("Image"))
                dataGridView.Columns.Remove("Image");

            // Создаем колонку для изображения
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn.Name = "Image";
            imageColumn.HeaderText = "Фото";
            imageColumn.Width = 80;
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            imageColumn.ReadOnly = true;

            // Вставляем колонку первой
            dataGridView.Columns.Insert(0, imageColumn);

            // Заполняем изображения
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.IsNewRow) continue;

                // Получаем путь к изображению из БД
                string imagePath = null;
                if (row.Cells["ImagePath"].Value != null && row.Cells["ImagePath"].Value != DBNull.Value)
                {
                    imagePath = row.Cells["ImagePath"].Value.ToString();
                }

                // Загружаем изображение
                Image img = LoadProductImage(imagePath);

                if (img != null)
                {
                    row.Cells["Image"].Value = img;
                }
                else
                {
                    // Если изображения нет - показываем заглушку
                    row.Cells["Image"].Value = CreateNoImageBitmap(80, 80);
                }
            }
        }

        private Image LoadProductImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return null;

            // Если путь уже абсолютный - оставляем как есть
            // Если относительный - комбинируем с Application.StartupPath
            string fullPath = imagePath;

            if (!Path.IsPathRooted(imagePath))
            {
                // Это относительный путь
                fullPath = Path.Combine(Application.StartupPath, imagePath);
            }

            if (File.Exists(fullPath))
            {
                try
                {
                    return Image.FromFile(fullPath);
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        private Bitmap CreateNoImageBitmap(int width, int height)
        {
            Bitmap noImage = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(noImage))
            {
                g.Clear(Color.LightGray);

                using (Pen pen = new Pen(Color.Gray, 1))
                {
                    g.DrawRectangle(pen, 0, 0, width - 1, height - 1);
                }

                using (Pen pen = new Pen(Color.Gray, 2))
                {
                    g.DrawLine(pen, 5, 5, width - 5, height - 5);
                    g.DrawLine(pen, width - 5, 5, 5, height - 5);
                }

                string text = "Нет фото";
                using (Font font = new Font("Arial", 8))
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    SizeF textSize = g.MeasureString(text, font);
                    g.DrawString(text, font, brush,
                        (width - textSize.Width) / 2,
                        (height - textSize.Height) / 2);
                }
            }
            return noImage;
        }

        // ================ ФИЛЬТРЫ ================

        private void ApplyFilters()
        {
            try
            {
                string searchText = txtSearch.Text.Trim();
                string filter = "";

                if (!string.IsNullOrEmpty(searchText))
                {
                    filter += $"Name LIKE '%{searchText.Replace("'", "''")}%'";
                }

                if (cmbCategories.SelectedIndex > 0 && cmbCategories.SelectedItem is Category selectedCategory)
                {
                    if (!string.IsNullOrEmpty(filter)) filter += " AND ";
                    filter += $"categoriesName = '{selectedCategory.categoriesName.Replace("'", "''")}'";
                }

                if (cmbSuppliers.SelectedIndex > 0 && cmbSuppliers.SelectedItem is Supplier selectedSupplier)
                {
                    if (!string.IsNullOrEmpty(filter)) filter += " AND ";
                    filter += $"SupplierName = '{selectedSupplier.Name.Replace("'", "''")}'";
                }

                if (filteredView != null)
                {
                    filteredView.RowFilter = filter;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при применении фильтров:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) => ApplyFilters();

        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilters();

        private void btnClearFilters_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbCategories.SelectedIndex = 0;
            cmbSuppliers.SelectedIndex = 0;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAllData();
            btnClearFilters_Click(sender, e);
        }

        // ================ CRUD ОПЕРАЦИИ ================

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверка прав доступа
                if (UserSession.CurrentUser == null)
                {
                    MessageBox.Show("Вы не авторизованы!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!UserSession.IsAdmin && !UserSession.IsManager)
                {
                    MessageBox.Show("У вас нет прав для добавления товаров!", "Доступ запрещен",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var editForm = new ProductEditForm(categoriesList, suppliersList);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadAllData(); // Перезагружаем данные
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении товара:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Проверка прав доступа
            if (UserSession.CurrentUser == null)
            {
                MessageBox.Show("Вы не авторизованы!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!UserSession.IsAdmin && !UserSession.IsManager)
            {
                MessageBox.Show("У вас нет прав для редактирования товаров!", "Доступ запрещен",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для редактирования", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataGridViewRow selectedRow = dataGridView.SelectedRows[0];
                int productId = Convert.ToInt32(selectedRow.Cells["ProductID"].Value);

                // Используем ProductData из пространства имен Kursych.Forms.Products
                ProductData productData = new ProductData
                {
                    ProductID = productId,
                    Name = selectedRow.Cells["Name"].Value?.ToString() ?? "",
                    Price = Convert.ToDecimal(selectedRow.Cells["Price"].Value),
                    CategoryID = selectedRow.Cells["categoriesID"].Value != DBNull.Value ? Convert.ToInt32(selectedRow.Cells["categoriesID"].Value) : 0,
                    Description = selectedRow.Cells["Description"].Value?.ToString() ?? "",
                    SupplierID = selectedRow.Cells["SuppliersID"].Value != DBNull.Value ? Convert.ToInt32(selectedRow.Cells["SuppliersID"].Value) : 0,
                    StockQuantity = Convert.ToInt32(selectedRow.Cells["StockQualitu"].Value),
                    ImagePath = selectedRow.Cells["ImagePath"].Value?.ToString() ?? ""
                };

                var editForm = new ProductEditForm(productData, categoriesList, suppliersList);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadAllData(); // Перезагружаем данные
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании товара:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверка прав доступа
                if (UserSession.CurrentUser == null)
                {
                    MessageBox.Show("Вы не авторизованы!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!UserSession.IsAdmin && !UserSession.IsManager)
                {
                    MessageBox.Show("У вас нет прав для удаления товаров!", "Доступ запрещен",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите товар для удаления", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow selectedRow = dataGridView.SelectedRows[0];
                int productId = Convert.ToInt32(selectedRow.Cells["ProductID"].Value);
                string productName = selectedRow.Cells["Name"].Value?.ToString() ?? "Неизвестный товар";

                DialogResult result = MessageBox.Show($"Вы уверены, что хотите удалить товар:\n\"{productName}\"?",
                    "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    using (MySqlConnection connection = new MySqlConnection(dbService.GetConnectionString()))
                    {
                        string query = "DELETE FROM product WHERE ProductID = @ProductID";
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ProductID", productId);
                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Товар успешно удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadAllData();
                            }
                        }
                    }
                }
            }
            catch (MySqlException mysqlEx)
            {
                if (mysqlEx.Number == 1451)
                {
                    MessageBox.Show("Невозможно удалить товар. Существуют связанные записи в заказах.", "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Ошибка MySQL: {mysqlEx.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении товара:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Проверяем права доступа перед двойным кликом
                if (UserSession.CurrentUser != null && (UserSession.IsAdmin || UserSession.IsManager))
                {
                    btnEdit_Click(sender, e);
                }
            }
        }
    }
}