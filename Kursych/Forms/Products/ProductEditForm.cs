using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.IO;
using System.Drawing;
using Kursych.Forms.Main;

namespace Kursych.Forms.Products
{
    public partial class ProductEditForm : Form
    {
        private ProductData productData;
        private List<ProductsForm.Category> categories;
        private List<ProductsForm.Supplier> suppliers;
        private bool isEditMode;
        private DatabaseService dbService;
        private string selectedImagePath = "";
        private string selectedImageFileName = "";

        // Конструктор для добавления нового товара
        public ProductEditForm(List<ProductsForm.Category> categories, List<ProductsForm.Supplier> suppliers)
        {
            InitializeComponent();
            this.categories = categories;
            this.suppliers = suppliers;
            this.isEditMode = false;
            this.dbService = new DatabaseService();
            InitializeForm();
        }

        // Конструктор для редактирования существующего товара
        public ProductEditForm(ProductData productData, List<ProductsForm.Category> categories, List<ProductsForm.Supplier> suppliers)
        {
            InitializeComponent();
            this.productData = productData;
            this.categories = categories;
            this.suppliers = suppliers;
            this.isEditMode = true;
            this.dbService = new DatabaseService();
            InitializeForm();
            LoadProductData();
        }

        private void InitializeForm()
        {
            // Настройка заголовка формы
            this.Text = isEditMode ? "Редактирование товара" : "Добавление нового товара";

            // Загрузка категорий (исключаем "Все категории")
            var filteredCategories = categories.Where(c => c.categoriesID != 0).ToList();
            cmbCategory.DataSource = filteredCategories;
            cmbCategory.DisplayMember = "categoriesName";
            cmbCategory.ValueMember = "categoriesID";

            // Загрузка поставщиков (исключаем "Все поставщики")
            var filteredSuppliers = suppliers.Where(s => s.SuppliersID != 0).ToList();
            cmbSupplier.DataSource = filteredSuppliers;
            cmbSupplier.DisplayMember = "Name";
            cmbSupplier.ValueMember = "SuppliersID";

            // Привязка событий
            this.btnSave.Click += btnSave_Click;
            this.btnCancel.Click += btnCancel_Click;
            this.btnSelectImage.Click += btnSelectImage_Click;
            this.btnClearImage.Click += btnClearImage_Click;

            // Валидация ввода
            txtPrice.KeyPress += txtPrice_KeyPress;
            txtStockQuantity.KeyPress += txtStockQuantity_KeyPress;

            // Добавляем обработчик для проверки дубликатов при изменении названия
            txtName.TextChanged += txtName_TextChanged;

            // Устанавливаем изображение по умолчанию
            pictureBox.Image = CreateNoImageBitmap(200, 150);
        }

        private void LoadProductData()
        {
            if (productData != null)
            {
                txtName.Text = productData.Name;
                txtPrice.Text = productData.Price.ToString("F2");
                txtDescription.Text = productData.Description;
                txtStockQuantity.Text = productData.StockQuantity.ToString();

                // Выбор категории
                var selectedCategory = categories.FirstOrDefault(c => c.categoriesID == productData.CategoryID);
                if (selectedCategory != null)
                {
                    cmbCategory.SelectedItem = selectedCategory;
                }

                // Выбор поставщика
                var selectedSupplier = suppliers.FirstOrDefault(s => s.SuppliersID == productData.SupplierID);
                if (selectedSupplier != null)
                {
                    cmbSupplier.SelectedItem = selectedSupplier;
                }

                // Загрузка изображения
                if (!string.IsNullOrEmpty(productData.ImagePath) && File.Exists(productData.ImagePath))
                {
                    try
                    {
                        using (Image img = Image.FromFile(productData.ImagePath))
                        {
                            pictureBox.Image = new Bitmap(img);
                        }
                        selectedImagePath = productData.ImagePath;
                        selectedImageFileName = Path.GetFileName(productData.ImagePath);
                        lblImagePath.Text = selectedImageFileName;
                        lblImagePath.ForeColor = System.Drawing.Color.Green;
                    }
                    catch
                    {
                        pictureBox.Image = CreateNoImageBitmap(200, 150);
                        lblImagePath.Text = "Ошибка загрузки изображения";
                        lblImagePath.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else
                {
                    pictureBox.Image = CreateNoImageBitmap(200, 150);
                    lblImagePath.Text = "Файл не выбран";
                    lblImagePath.ForeColor = System.Drawing.Color.Gray;
                }
            }
        }

        /// <summary>
        /// Проверка на существование товара с таким же названием
        /// </summary>
        private bool IsProductExists(string productName, int? excludeProductId = null)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(dbService.GetConnectionString()))
                {
                    string query = "SELECT COUNT(*) FROM product WHERE Name = @Name";

                    // Если редактируем товар, исключаем текущий товар из проверки
                    if (excludeProductId.HasValue)
                    {
                        query += " AND ProductID != @ProductID";
                    }

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", productName.Trim());

                        if (excludeProductId.HasValue)
                        {
                            command.Parameters.AddWithValue("@ProductID", excludeProductId.Value);
                        }

                        connection.Open();
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке дубликатов:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Проверка на существование товара с таким же названием у того же поставщика
        /// </summary>
        private bool IsProductExistsForSupplier(string productName, int supplierId, int? excludeProductId = null)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(dbService.GetConnectionString()))
                {
                    string query = "SELECT COUNT(*) FROM product WHERE Name = @Name AND SuppliersID = @SupplierID";

                    if (excludeProductId.HasValue)
                    {
                        query += " AND ProductID != @ProductID";
                    }

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", productName.Trim());
                        command.Parameters.AddWithValue("@SupplierID", supplierId);

                        if (excludeProductId.HasValue)
                        {
                            command.Parameters.AddWithValue("@ProductID", excludeProductId.Value);
                        }

                        connection.Open();
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке дубликатов:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Проверка на существование товара с таким же названием в той же категории
        /// </summary>
        private bool IsProductExistsForCategory(string productName, int categoryId, int? excludeProductId = null)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(dbService.GetConnectionString()))
                {
                    string query = "SELECT COUNT(*) FROM product WHERE Name = @Name AND categoriesID = @CategoryID";

                    if (excludeProductId.HasValue)
                    {
                        query += " AND ProductID != @ProductID";
                    }

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", productName.Trim());
                        command.Parameters.AddWithValue("@CategoryID", categoryId);

                        if (excludeProductId.HasValue)
                        {
                            command.Parameters.AddWithValue("@ProductID", excludeProductId.Value);
                        }

                        connection.Open();
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке дубликатов:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            // Визуальное предупреждение о возможном дубликате (не блокирующее)
            if (!string.IsNullOrWhiteSpace(txtName.Text) && txtName.Text.Length >= 3)
            {
                CheckForDuplicateWarning();
            }
        }

        private async void CheckForDuplicateWarning()
        {
            try
            {
                string productName = txtName.Text.Trim();
                if (string.IsNullOrWhiteSpace(productName)) return;

                int? excludeId = isEditMode ? productData?.ProductID : null;

                // Используем Task.Run для асинхронной проверки без блокировки UI
                bool exists = await System.Threading.Tasks.Task.Run(() =>
                    IsProductExists(productName, excludeId));

                if (exists)
                {
                    lblDuplicateWarning.Visible = true;
                    lblDuplicateWarning.Text = "⚠ Товар с таким названием уже существует!";
                    lblDuplicateWarning.ForeColor = System.Drawing.Color.Orange;
                }
                else
                {
                    lblDuplicateWarning.Visible = false;
                }
            }
            catch
            {
                // Игнорируем ошибки при предупредительной проверке
            }
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Файлы изображений|*.jpg;*.jpeg;*.png;*.gif;*.bmp|Все файлы|*.*";
                openFileDialog.Title = "Выберите изображение для товара";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Загружаем изображение
                        using (Image img = Image.FromFile(openFileDialog.FileName))
                        {
                            pictureBox.Image = new Bitmap(img);
                        }

                        // Сохраняем путь
                        selectedImagePath = openFileDialog.FileName;
                        selectedImageFileName = Path.GetFileName(openFileDialog.FileName);
                        lblImagePath.Text = selectedImageFileName;
                        lblImagePath.ForeColor = System.Drawing.Color.Green;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при загрузке изображения:\n{ex.Message}",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnClearImage_Click(object sender, EventArgs e)
        {
            pictureBox.Image = CreateNoImageBitmap(200, 150);
            selectedImagePath = "";
            selectedImageFileName = "";
            lblImagePath.Text = "Файл не выбран";
            lblImagePath.ForeColor = System.Drawing.Color.Gray;
        }

        private Bitmap CreateNoImageBitmap(int width, int height)
        {
            Bitmap noImage = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(noImage))
            {
                g.Clear(Color.LightGray);

                using (Pen pen = new Pen(Color.Gray, 2))
                {
                    g.DrawRectangle(pen, 0, 0, width - 1, height - 1);
                    g.DrawLine(pen, 10, 10, width - 10, height - 10);
                    g.DrawLine(pen, width - 10, 10, 10, height - 10);
                }

                string text = "Нет фото";
                using (Font font = new Font("Arial", 12, FontStyle.Bold))
                using (Brush brush = new SolidBrush(Color.White))
                {
                    SizeF textSize = g.MeasureString(text, font);
                    g.DrawString(text, font, brush,
                        (width - textSize.Width) / 2,
                        (height - textSize.Height) / 2);
                }
            }
            return noImage;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                // ПРОВЕРКА НА ДУБЛИКАТЫ ПЕРЕД СОХРАНЕНИЕМ
                string productName = txtName.Text.Trim();
                int? excludeId = isEditMode ? productData?.ProductID : null;

                // Проверка на точное совпадение названия
                if (IsProductExists(productName, excludeId))
                {
                    DialogResult result = MessageBox.Show(
                        "Товар с таким названием уже существует в базе данных.\n\n" +
                        "Вы действительно хотите создать дубликат?",
                        "Предупреждение о дубликате",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.No)
                    {
                        txtName.Focus();
                        txtName.SelectAll();
                        return;
                    }
                }

                // Дополнительная проверка: товар с таким названием у того же поставщика
                var selectedSupplier = cmbSupplier.SelectedItem as ProductsForm.Supplier;
                if (selectedSupplier != null && selectedSupplier.SuppliersID > 0)
                {
                    if (IsProductExistsForSupplier(productName, selectedSupplier.SuppliersID, excludeId))
                    {
                        DialogResult result = MessageBox.Show(
                            $"У поставщика \"{selectedSupplier.Name}\" уже есть товар с названием \"{productName}\".\n\n" +
                            "Вы действительно хотите создать дубликат?",
                            "Предупреждение о дубликате",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (result == DialogResult.No)
                        {
                            txtName.Focus();
                            txtName.SelectAll();
                            return;
                        }
                    }
                }

                // Дополнительная проверка: товар с таким названием в той же категории
                var selectedCategory = cmbCategory.SelectedItem as ProductsForm.Category;
                if (selectedCategory != null && selectedCategory.categoriesID > 0)
                {
                    if (IsProductExistsForCategory(productName, selectedCategory.categoriesID, excludeId))
                    {
                        DialogResult result = MessageBox.Show(
                            $"В категории \"{selectedCategory.categoriesName}\" уже есть товар с названием \"{productName}\".\n\n" +
                            "Вы действительно хотите создать дубликат?",
                            "Предупреждение о дубликате",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (result == DialogResult.No)
                        {
                            txtName.Focus();
                            txtName.SelectAll();
                            return;
                        }
                    }
                }

                // Сохраняем изображение в папку проекта
                string savedImagePath = SaveImageToProjectFolder();

                using (MySqlConnection connection = new MySqlConnection(dbService.GetConnectionString()))
                {
                    connection.Open();

                    if (isEditMode)
                    {
                        // Обновление существующего товара
                        string query = @"
                            UPDATE product 
                            SET Name = @Name,
                                Price = @Price,
                                categoriesID = @CategoryID,
                                Description = @Description,
                                SuppliersID = @SupplierID,
                                StockQualitu = @StockQuantity,
                                ImagePath = @ImagePath
                            WHERE ProductID = @ProductID";

                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ProductID", productData.ProductID);
                            SetCommandParameters(command, savedImagePath);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Товар успешно обновлен!", "Успех",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Не удалось обновить товар.", "Ошибка",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        // Добавление нового товара
                        string query = @"
                            INSERT INTO product (Name, Price, categoriesID, Description, SuppliersID, StockQualitu, ImagePath)
                            VALUES (@Name, @Price, @CategoryID, @Description, @SupplierID, @StockQuantity, @ImagePath)";

                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            SetCommandParameters(command, savedImagePath);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Товар успешно добавлен!", "Успех",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Не удалось добавить товар.", "Ошибка",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (MySqlException mysqlEx)
            {
                if (mysqlEx.Number == 1062) // Duplicate entry error
                {
                    MessageBox.Show("Товар с таким названием уже существует.\n\n" +
                                   "Пожалуйста, используйте другое название.",
                                   "Ошибка дубликата",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Ошибка MySQL: {mysqlEx.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении товара:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string SaveImageToProjectFolder()
        {
            if (string.IsNullOrEmpty(selectedImagePath) || !File.Exists(selectedImagePath))
                return null;

            try
            {
                // Создаем папку ProductImages если её нет
                string imageFolder = Path.Combine(Application.StartupPath, "ProductImages");
                if (!Directory.Exists(imageFolder))
                    Directory.CreateDirectory(imageFolder);

                // Генерируем уникальное имя файла
                string fileExtension = Path.GetExtension(selectedImagePath);
                string fileName = $"{Guid.NewGuid()}{fileExtension}";
                string destPath = Path.Combine(imageFolder, fileName);

                // Копируем файл
                File.Copy(selectedImagePath, destPath, true);

                // ВАЖНО: возвращаем ОТНОСИТЕЛЬНЫЙ путь
                return "ProductImages\\" + fileName;
            }
            catch
            {
                return null;
            }
        }

        private void SetCommandParameters(MySqlCommand command, string imagePath)
        {
            command.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            command.Parameters.AddWithValue("@Price", decimal.Parse(txtPrice.Text));

            var selectedCategory = cmbCategory.SelectedItem as ProductsForm.Category;
            command.Parameters.AddWithValue("@CategoryID", selectedCategory?.categoriesID ?? 0);

            command.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());

            var selectedSupplier = cmbSupplier.SelectedItem as ProductsForm.Supplier;
            command.Parameters.AddWithValue("@SupplierID", selectedSupplier?.SuppliersID ?? 0);

            command.Parameters.AddWithValue("@StockQuantity", int.Parse(txtStockQuantity.Text));

            command.Parameters.AddWithValue("@ImagePath",
                string.IsNullOrEmpty(imagePath) ? (object)DBNull.Value : imagePath);
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название товара", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену (больше 0)", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPrice.Focus();
                txtPrice.SelectAll();
                return false;
            }

            if (!int.TryParse(txtStockQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Введите корректное количество (0 или больше)", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtStockQuantity.Focus();
                txtStockQuantity.SelectAll();
                return false;
            }

            if (cmbCategory.SelectedItem == null)
            {
                MessageBox.Show("Выберите категорию", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbCategory.Focus();
                return false;
            }

            if (cmbSupplier.SelectedItem == null)
            {
                MessageBox.Show("Выберите поставщика", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbSupplier.Focus();
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }

            if ((e.KeyChar == ',') && ((sender as TextBox).Text.Contains(',')))
            {
                e.Handled = true;
            }
        }

        private void txtStockQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}