using System;
using System.Windows.Forms;
using Kursych.Forms.Print;
using System.Data;
using MySql.Data.MySqlClient;
using Kursych.Forms.Main;
using System.Collections.Generic;
using System.Drawing;

namespace Kursych.Forms.Orders
{
    public partial class OrderCreateForm : Form
    {
        private DatabaseService dbService;
        private DataTable allProducts;
        private int currentOrderId = 0;

        // Добавляем константы для текстов placeholder
        private const string SEARCH_PLACEHOLDER = "Введите название товара";
        private bool isSearchPlaceholderActive = true;

        // Добавляем NumericUpDown для выбора количества
        private NumericUpDown numQuantity;

        public OrderCreateForm()
        {
            InitializeComponent();
            dbService = new DatabaseService();

            // Создаем NumericUpDown
            InitializeNumericUpDown();

            // Устанавливаем placeholder для поля поиска
            SetupPlaceholder();

            // Подключаем обработчики событий
            AttachEventHandlers();
        }

        private void InitializeNumericUpDown()
        {
            numQuantity = new NumericUpDown();
            numQuantity.Location = new System.Drawing.Point(100, 365);
            numQuantity.Size = new System.Drawing.Size(80, 25);
            numQuantity.Minimum = 1;
            numQuantity.Maximum = 1000;
            numQuantity.Value = 1;
            panelLeft.Controls.Add(numQuantity);
            numQuantity.ValueChanged += NumQuantity_ValueChanged;
        }

        private void NumQuantity_ValueChanged(object sender, EventArgs e)
        {
            // Обновляем информацию при изменении количества
            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridViewProducts.SelectedRows[0];
                if (selectedRow.Cells["StockQuantity"].Value != null)
                {
                    int stockQuantity = Convert.ToInt32(selectedRow.Cells["StockQuantity"].Value);
                    if (numQuantity.Value > stockQuantity)
                    {
                        MessageBox.Show($"Недостаточно товара на складе. Максимум: {stockQuantity}",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        numQuantity.Value = Math.Max(1, stockQuantity);
                    }
                }
            }
        }

        private void SetupPlaceholder()
        {
            // Устанавливаем текст placeholder
            txtSearchName.Text = SEARCH_PLACEHOLDER;
            txtSearchName.ForeColor = Color.Gray;
            isSearchPlaceholderActive = true;
        }

        private void AttachEventHandlers()
        {
            // Подключаем обработчики событий
            this.Load += OrderCreateForm_Load;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnAddToOrder.Click += btnAddToOrder_Click;
            this.btnRemoveFromOrder.Click += btnRemoveFromOrder_Click;
            this.btnCreateOrder.Click += btnCreateOrder_Click;
            this.btnCancel.Click += btnCancel_Click;

            // Обработчики для поля поиска
            this.txtSearchName.Enter += TxtSearchName_Enter;
            this.txtSearchName.Leave += TxtSearchName_Leave;
            this.txtSearchName.KeyPress += TxtSearchName_KeyPress;

            // Обработчики для фильтров
            this.comboBoxSuppliers.SelectedIndexChanged += ComboBoxSuppliers_SelectedIndexChanged;
            this.comboBoxCategories.SelectedIndexChanged += ComboBoxCategories_SelectedIndexChanged;

            // Обработчики для DataGridView
            this.dataGridViewProducts.DataError += dataGridView_DataError;
            this.dataGridViewOrder.DataError += dataGridView_DataError;
            this.dataGridViewOrder.CellValueChanged += DataGridViewOrder_CellValueChanged;
            this.dataGridViewOrder.CellValidating += DataGridViewOrder_CellValidating;

            // Обработчик выбора товара
            this.dataGridViewProducts.SelectionChanged += DataGridViewProducts_SelectionChanged;
        }

        private void DataGridViewProducts_SelectionChanged(object sender, EventArgs e)
        {
            // Обновляем максимальное значение NumericUpDown при выборе товара
            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridViewProducts.SelectedRows[0];
                if (selectedRow.Cells["StockQuantity"].Value != null)
                {
                    int stockQuantity = Convert.ToInt32(selectedRow.Cells["StockQuantity"].Value);
                    numQuantity.Maximum = stockQuantity;
                    numQuantity.Value = 1;
                }
            }
        }

        private void TxtSearchName_Enter(object sender, EventArgs e)
        {
            // Когда пользователь кликает на поле
            if (isSearchPlaceholderActive)
            {
                txtSearchName.Text = "";
                txtSearchName.ForeColor = Color.Black;
                isSearchPlaceholderActive = false;
            }
        }

        private void TxtSearchName_Leave(object sender, EventArgs e)
        {
            // Когда пользователь уходит из поля
            if (string.IsNullOrWhiteSpace(txtSearchName.Text))
            {
                SetupPlaceholder();
            }
        }

        private void TxtSearchName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Убираем фокус с поля поиска чтобы обработать placeholder
                btnSearch.Focus();

                // Выполняем поиск
                PerformSearch();
                e.Handled = true;
            }
        }

        private void OrderCreateForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Установка текущей даты
                this.txtOrderDate.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

                // Загружаем данные для поиска
                LoadSearchData();

                // Загружаем все товары
                LoadAllProducts();

                // Инициализация DataGridView товаров
                InitializeProductsGrid();

                // Инициализация DataGridView заказа
                InitializeOrderGrid();

                // Обновление сумм
                UpdateTotals();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке формы: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSearchData()
        {
            try
            {
                // Загружаем поставщиков
                DataTable suppliers = dbService.GetSuppliers();
                comboBoxSuppliers.Items.Clear();
                comboBoxSuppliers.Items.Add("Все поставщики");

                foreach (DataRow row in suppliers.Rows)
                {
                    if (row["Name"] != DBNull.Value)
                        comboBoxSuppliers.Items.Add(row["Name"].ToString());
                }
                comboBoxSuppliers.SelectedIndex = 0;

                // Загружаем категории
                DataTable categories = dbService.GetCategories();
                comboBoxCategories.Items.Clear();
                comboBoxCategories.Items.Add("Все категории");

                foreach (DataRow row in categories.Rows)
                {
                    if (row["categoriesName"] != DBNull.Value)
                        comboBoxCategories.Items.Add(row["categoriesName"].ToString());
                }
                comboBoxCategories.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных для поиска: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAllProducts()
        {
            try
            {
                allProducts = dbService.GetProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                allProducts = new DataTable();
            }
        }

        private void InitializeProductsGrid()
        {
            try
            {
                // Получаем реальный текст поиска (без placeholder)
                string searchText = GetRealSearchText();

                // Используем все товары или отфильтрованные
                DataTable products = FilterProducts(searchText);

                dataGridViewProducts.Columns.Clear();
                dataGridViewProducts.Rows.Clear();

                // Отключаем автоматическое создание колонок
                dataGridViewProducts.AutoGenerateColumns = false;

                // Создаем колонки
                DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn();
                idColumn.Name = "ProductID";
                idColumn.HeaderText = "ID";
                idColumn.DataPropertyName = "ProductID";
                idColumn.ReadOnly = true;
                idColumn.Width = 50;
                dataGridViewProducts.Columns.Add(idColumn);

                DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn();
                nameColumn.Name = "Name";
                nameColumn.HeaderText = "Название";
                nameColumn.DataPropertyName = "Name";
                nameColumn.ReadOnly = true;
                nameColumn.Width = 180;
                dataGridViewProducts.Columns.Add(nameColumn);

                DataGridViewTextBoxColumn priceColumn = new DataGridViewTextBoxColumn();
                priceColumn.Name = "Price";
                priceColumn.HeaderText = "Цена, руб.";
                priceColumn.DataPropertyName = "Price";
                priceColumn.ReadOnly = true;
                priceColumn.DefaultCellStyle.Format = "N2";
                priceColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                priceColumn.Width = 80;
                dataGridViewProducts.Columns.Add(priceColumn);

                DataGridViewTextBoxColumn categoryColumn = new DataGridViewTextBoxColumn();
                categoryColumn.Name = "Category";
                categoryColumn.HeaderText = "Категория";
                categoryColumn.DataPropertyName = "categoriesName";
                categoryColumn.ReadOnly = true;
                categoryColumn.Width = 120;
                dataGridViewProducts.Columns.Add(categoryColumn);

                DataGridViewTextBoxColumn supplierColumn = new DataGridViewTextBoxColumn();
                supplierColumn.Name = "Supplier";
                supplierColumn.HeaderText = "Поставщик";
                supplierColumn.DataPropertyName = "SupplierName";
                supplierColumn.ReadOnly = true;
                supplierColumn.Width = 120;
                dataGridViewProducts.Columns.Add(supplierColumn);

                DataGridViewTextBoxColumn stockColumn = new DataGridViewTextBoxColumn();
                stockColumn.Name = "StockQuantity";
                stockColumn.HeaderText = "На складе";
                stockColumn.DataPropertyName = "StockQualitu";
                stockColumn.ReadOnly = true;
                stockColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                stockColumn.Width = 80;
                dataGridViewProducts.Columns.Add(stockColumn);

                // Заполняем данные
                foreach (DataRow row in products.Rows)
                {
                    object[] rowData = new object[6];

                    // ID
                    rowData[0] = row["ProductID"];

                    // Название
                    rowData[1] = row["Name"];

                    // Цена
                    rowData[2] = row["Price"] != DBNull.Value ? row["Price"] : 0m;

                    // Категория
                    rowData[3] = row["categoriesName"] != DBNull.Value ? row["categoriesName"].ToString() : "";

                    // Поставщик
                    rowData[4] = row["SupplierName"] != DBNull.Value ? row["SupplierName"].ToString() : "";

                    // Количество на складе
                    rowData[5] = row["StockQualitu"] != DBNull.Value ? row["StockQualitu"] : 0;

                    dataGridViewProducts.Rows.Add(rowData);
                }

                // Обновляем количество найденных товаров
                UpdateProductCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetRealSearchText()
        {
            if (isSearchPlaceholderActive || txtSearchName.Text == SEARCH_PLACEHOLDER)
            {
                return "";
            }
            return txtSearchName.Text.Trim();
        }

        private DataTable FilterProducts(string searchText = null)
        {
            if (allProducts == null || allProducts.Rows.Count == 0)
                return allProducts?.Clone() ?? new DataTable();

            // Если searchText не передан, получаем его из поля
            if (searchText == null)
            {
                searchText = GetRealSearchText();
            }

            DataTable filtered = allProducts.Clone();

            foreach (DataRow row in allProducts.Rows)
            {
                bool include = true;

                // Фильтр по названию
                if (!string.IsNullOrEmpty(searchText))
                {
                    string productName = row["Name"]?.ToString()?.ToLower() ?? "";
                    if (!productName.Contains(searchText.ToLower()))
                        include = false;
                }

                // Фильтр по поставщику
                if (include && comboBoxSuppliers.SelectedIndex > 0)
                {
                    string selectedSupplier = comboBoxSuppliers.SelectedItem.ToString();
                    string productSupplier = row["SupplierName"]?.ToString() ?? "";
                    if (productSupplier != selectedSupplier)
                        include = false;
                }

                // Фильтр по категории
                if (include && comboBoxCategories.SelectedIndex > 0)
                {
                    string selectedCategory = comboBoxCategories.SelectedItem.ToString();
                    string productCategory = row["categoriesName"]?.ToString() ?? "";
                    if (productCategory != selectedCategory)
                        include = false;
                }

                if (include)
                {
                    filtered.ImportRow(row);
                }
            }

            return filtered;
        }

        private void UpdateProductCount()
        {
            int productCount = dataGridViewProducts.Rows.Count;
            if (dataGridViewProducts.AllowUserToAddRows)
                productCount--;

            lblProducts.Text = $"Товары ({productCount}):";
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void ComboBoxSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void ComboBoxCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            try
            {
                InitializeProductsGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeOrderGrid()
        {
            dataGridViewOrder.Columns.Clear();

            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn();
            idColumn.Name = "ProductID";
            idColumn.HeaderText = "ID";
            idColumn.ReadOnly = true;
            idColumn.Width = 50;
            dataGridViewOrder.Columns.Add(idColumn);

            DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.Name = "Name";
            nameColumn.HeaderText = "Название";
            nameColumn.ReadOnly = true;
            nameColumn.Width = 180;
            dataGridViewOrder.Columns.Add(nameColumn);

            DataGridViewTextBoxColumn priceColumn = new DataGridViewTextBoxColumn();
            priceColumn.Name = "Price";
            priceColumn.HeaderText = "Цена, руб.";
            priceColumn.ReadOnly = true;
            priceColumn.DefaultCellStyle.Format = "N2";
            priceColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            priceColumn.Width = 80;
            dataGridViewOrder.Columns.Add(priceColumn);

            DataGridViewTextBoxColumn quantityColumn = new DataGridViewTextBoxColumn();
            quantityColumn.Name = "Quantity";
            quantityColumn.HeaderText = "Количество";
            quantityColumn.ValueType = typeof(int);
            quantityColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            quantityColumn.Width = 80;
            dataGridViewOrder.Columns.Add(quantityColumn);

            DataGridViewTextBoxColumn sumColumn = new DataGridViewTextBoxColumn();
            sumColumn.Name = "Sum";
            sumColumn.HeaderText = "Сумма, руб.";
            sumColumn.ReadOnly = true;
            sumColumn.DefaultCellStyle.Format = "N2";
            sumColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            sumColumn.Width = 90;
            dataGridViewOrder.Columns.Add(sumColumn);
        }

        private void DataGridViewOrder_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Пересчитываем сумму при изменении количества
            if (e.ColumnIndex == dataGridViewOrder.Columns["Quantity"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewOrder.Rows[e.RowIndex];

                if (row.Cells["Price"].Value != null && row.Cells["Quantity"].Value != null)
                {
                    try
                    {
                        decimal price = Convert.ToDecimal(row.Cells["Price"].Value);
                        int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                        row.Cells["Sum"].Value = price * quantity;

                        UpdateTotals();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при расчете суммы: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void DataGridViewOrder_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == dataGridViewOrder.Columns["Quantity"].Index && e.RowIndex >= 0)
            {
                // Проверяем, что введено положительное число
                if (!int.TryParse(e.FormattedValue.ToString(), out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("Введите положительное число для количества", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                    return;
                }

                // Проверяем наличие на складе
                int productId = Convert.ToInt32(dataGridViewOrder.Rows[e.RowIndex].Cells["ProductID"].Value);
                string productName = dataGridViewOrder.Rows[e.RowIndex].Cells["Name"].Value.ToString();

                // Ищем товар в основном списке
                int stockQuantity = 0;
                foreach (DataGridViewRow productRow in dataGridViewProducts.Rows)
                {
                    if (productRow.Cells["ProductID"].Value != null &&
                        Convert.ToInt32(productRow.Cells["ProductID"].Value) == productId)
                    {
                        stockQuantity = Convert.ToInt32(productRow.Cells["StockQuantity"].Value);
                        break;
                    }
                }

                if (quantity > stockQuantity)
                {
                    MessageBox.Show($"Недостаточно товара '{productName}' на складе. Максимум: {stockQuantity}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
            }
        }

        private void UpdateTotals()
        {
            try
            {
                decimal total = 0;
                int totalItems = 0;

                foreach (DataGridViewRow row in dataGridViewOrder.Rows)
                {
                    if (row.Cells["Sum"].Value != null && row.Cells["Sum"].Value != DBNull.Value)
                    {
                        if (decimal.TryParse(row.Cells["Sum"].Value.ToString(), out decimal sum))
                        {
                            total += sum;
                        }
                    }

                    if (row.Cells["Quantity"].Value != null && row.Cells["Quantity"].Value != DBNull.Value)
                    {
                        totalItems += Convert.ToInt32(row.Cells["Quantity"].Value);
                    }
                }

                this.txtTotal.Text = total.ToString("N2");

                // Рассчитываем скидку (10% при сумме > 1000)
                decimal discount = 0;
                if (total > 1000)
                {
                    discount = total * 0.1m;
                }
                this.txtDiscount.Text = discount.ToString("N2");

                this.txtFinalTotal.Text = (total - discount).ToString("N2");

                // Обновляем заголовок заказа
                lblOrder.Text = $"Заказ ({totalItems} товаров):";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении сумм: {ex.Message}");
            }
        }

        private void btnAddToOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewProducts.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridViewProducts.SelectedRows[0];

                    if (selectedRow.Cells["ProductID"].Value == null)
                    {
                        MessageBox.Show("Не выбран товар", "Информация",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Получаем данные товара
                    int productId = Convert.ToInt32(selectedRow.Cells["ProductID"].Value);
                    string name = selectedRow.Cells["Name"].Value?.ToString() ?? "Неизвестный товар";
                    decimal price = selectedRow.Cells["Price"].Value != null ?
                        Convert.ToDecimal(selectedRow.Cells["Price"].Value) : 0;

                    int stockQuantity = selectedRow.Cells["StockQuantity"].Value != null ?
                        Convert.ToInt32(selectedRow.Cells["StockQuantity"].Value) : 0;

                    // Получаем выбранное количество
                    int quantity = (int)numQuantity.Value;

                    // Проверяем наличие на складе
                    if (stockQuantity <= 0)
                    {
                        MessageBox.Show($"Товар '{name}' отсутствует на складе", "Информация",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (quantity > stockQuantity)
                    {
                        MessageBox.Show($"Недостаточно товара '{name}' на складе. Максимум: {stockQuantity}",
                            "Информация", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Проверяем, есть ли уже такой товар в заказе
                    bool exists = false;
                    foreach (DataGridViewRow orderRow in dataGridViewOrder.Rows)
                    {
                        if (orderRow.Cells["ProductID"].Value != null &&
                            orderRow.Cells["ProductID"].Value.ToString() == productId.ToString())
                        {
                            // Получаем текущее количество
                            int currentQuantity = orderRow.Cells["Quantity"].Value != null ?
                                Convert.ToInt32(orderRow.Cells["Quantity"].Value) : 0;

                            // Проверяем, можно ли добавить еще
                            int newQuantity = currentQuantity + quantity;
                            if (newQuantity > stockQuantity)
                            {
                                MessageBox.Show($"Недостаточно товара '{name}' на складе. Максимум доступно: {stockQuantity - currentQuantity}",
                                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            // Увеличиваем количество
                            orderRow.Cells["Quantity"].Value = newQuantity;

                            exists = true;
                            break;
                        }
                    }

                    if (!exists)
                    {
                        // Добавляем новый товар
                        dataGridViewOrder.Rows.Add(productId, name, price, quantity, price * quantity);
                    }

                    UpdateTotals();

                    // Сбрасываем количество
                    numQuantity.Value = 1;
                }
                else
                {
                    MessageBox.Show("Выберите товар для добавления в заказ", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении товара: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemoveFromOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewOrder.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridViewOrder.SelectedRows[0];

                    // Подтверждение удаления
                    string productName = selectedRow.Cells["Name"].Value?.ToString() ?? "товар";
                    int quantity = selectedRow.Cells["Quantity"].Value != null ?
                        Convert.ToInt32(selectedRow.Cells["Quantity"].Value) : 1;

                    DialogResult result = MessageBox.Show(
                        $"Удалить '{productName}' ({quantity} шт.) из заказа?",
                        "Подтверждение удаления",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        dataGridViewOrder.Rows.Remove(selectedRow);
                        UpdateTotals();

                        MessageBox.Show($"Товар '{productName}' удален из заказа", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Выберите товар для удаления из заказа", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении товара: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCreateOrder_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrder.Rows.Count == 0)
            {
                MessageBox.Show("Добавьте товары в заказ", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Получаем ID текущего пользователя
            int currentUserId = GetCurrentUserId();
            if (currentUserId <= 0)
            {
                MessageBox.Show("Не удалось определить пользователя", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Рассчитываем итоговые суммы
            decimal total = decimal.Parse(txtTotal.Text);
            decimal discount = decimal.Parse(txtDiscount.Text);
            decimal finalTotal = decimal.Parse(txtFinalTotal.Text);

            DialogResult result = MessageBox.Show(
                $"Подтверждаете создание заказа?\n\n" +
                $"Товаров: {dataGridViewOrder.Rows.Count} наименований\n" +
                $"Общая сумма: {total:N2} руб.\n" +
                $"Скидка: {discount:N2} руб.\n" +
                $"Итого к оплате: {finalTotal:N2} руб.",
                "Подтверждение создания заказа",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    bool success = CreateOrderWithItems(currentUserId, total, discount, finalTotal);

                    if (success)
                    {
                        MessageBox.Show($"Заказ №{currentOrderId} успешно создан!",
                            "Успех",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось создать заказ", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при создании заказа: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool CreateOrderWithItems(int userId, decimal totalAmount, decimal discountAmount, decimal finalAmount)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(dbService.GetConnectionString()))
                {
                    connection.Open();

                    using (MySqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // 1. Создаем заказ в таблице orders
                            string createOrderQuery = @"
                                INSERT INTO orders (UserID, OrderDate, Status, TotalAmount, DiscountAmount, FinalAmount) 
                                VALUES (@UserID, @OrderDate, @Status, @TotalAmount, @DiscountAmount, @FinalAmount);
                                SELECT LAST_INSERT_ID();";

                            using (MySqlCommand cmd = new MySqlCommand(createOrderQuery, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@UserID", userId);
                                cmd.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                                cmd.Parameters.AddWithValue("@Status", "Новый");
                                cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                                cmd.Parameters.AddWithValue("@DiscountAmount", discountAmount);
                                cmd.Parameters.AddWithValue("@FinalAmount", finalAmount);

                                currentOrderId = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            // 2. Добавляем все товары в order_items
                            string insertOrderItemQuery = @"
                                INSERT INTO order_items (OrderID, ProductID, Quantity, Price, TotalPrice) 
                                VALUES (@OrderID, @ProductID, @Quantity, @Price, @TotalPrice)";

                            int itemsAdded = 0;
                            foreach (DataGridViewRow row in dataGridViewOrder.Rows)
                            {
                                if (row.IsNewRow) continue;

                                if (row.Cells["ProductID"].Value != null &&
                                    row.Cells["Quantity"].Value != null &&
                                    row.Cells["Price"].Value != null)
                                {
                                    int productId = Convert.ToInt32(row.Cells["ProductID"].Value);
                                    int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                                    decimal price = Convert.ToDecimal(row.Cells["Price"].Value);
                                    decimal totalPrice = price * quantity;

                                    using (MySqlCommand cmd = new MySqlCommand(insertOrderItemQuery, connection, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@OrderID", currentOrderId);
                                        cmd.Parameters.AddWithValue("@ProductID", productId);
                                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                                        cmd.Parameters.AddWithValue("@Price", price);
                                        cmd.Parameters.AddWithValue("@TotalPrice", totalPrice);

                                        cmd.ExecuteNonQuery();
                                        itemsAdded++;
                                    }
                                }
                            }

                            // 3. Обновляем количество на складе
                            string updateStockQuery = @"
                                UPDATE product 
                                SET StockQualitu = StockQualitu - @Quantity 
                                WHERE ProductID = @ProductID AND StockQualitu >= @Quantity";

                            foreach (DataGridViewRow row in dataGridViewOrder.Rows)
                            {
                                if (row.IsNewRow) continue;

                                if (row.Cells["ProductID"].Value != null && row.Cells["Quantity"].Value != null)
                                {
                                    int productId = Convert.ToInt32(row.Cells["ProductID"].Value);
                                    int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);

                                    using (MySqlCommand cmd = new MySqlCommand(updateStockQuery, connection, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@ProductID", productId);
                                        cmd.Parameters.AddWithValue("@Quantity", quantity);

                                        int updated = cmd.ExecuteNonQuery();
                                        if (updated == 0)
                                        {
                                            throw new Exception($"Недостаточно товара на складе. Товар ID: {productId}");
                                        }
                                    }
                                }
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании заказа: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
            e.Cancel = true;
        }

        private int GetCurrentUserId()
        {
            if (UserSession.CurrentUser != null)
                return UserSession.CurrentUser.UserID;
            return 3; // ID по умолчанию
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Вы уверены, что хотите отменить оформление заказа? Все добавленные товары будут удалены.",
                "Подтверждение отмены",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}