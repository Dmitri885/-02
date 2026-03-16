using System;
using System.Windows.Forms;
using System.Data;
using Kursych.Forms.Main;
using Kursych.Forms.Print;
using System.IO;

namespace Kursych.Forms.Orders
{
    public partial class OrdersForm : Form
    {
        private DatabaseService dbService;

        public OrdersForm()
        {
            InitializeComponent();
            dbService = new DatabaseService();

            // Подключаем обработчики событий
            this.Load += OrdersForm_Load;
            this.btnCreateOrder.Click += btnCreateOrder_Click;
            this.btnCancelOrder.Click += btnCancelOrder_Click;
            this.btnViewDetails.Click += btnViewDetails_Click;
            this.btnPrintReceipt.Click += btnPrintReceipt_Click; // Добавляем обработчик
            this.btnRefresh.Click += btnRefresh_Click;
        }

        private void OrdersForm_Load(object sender, EventArgs e)
        {
            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                DataTable orders = dbService.GetOrders();

                dataGridView.Columns.Clear();

                // Создаем колонки
                dataGridView.Columns.Add("OrderID", "ID заказа");
                dataGridView.Columns["OrderID"].Width = 20;

                dataGridView.Columns.Add("Customer", "Клиент");
                dataGridView.Columns["Customer"].Width = 150;

                dataGridView.Columns.Add("Products", "Товары");
                dataGridView.Columns["Products"].Width = 250;

                dataGridView.Columns.Add("ItemsCount", "Позиц");
                dataGridView.Columns["ItemsCount"].Width = 50;
                dataGridView.Columns["ItemsCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                dataGridView.Columns.Add("TotalUnits", "Всего шт.");
                dataGridView.Columns["TotalUnits"].Width = 70;
                dataGridView.Columns["TotalUnits"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                dataGridView.Columns.Add("OrderDate", "Дата заказа");
                dataGridView.Columns["OrderDate"].Width = 120;

                dataGridView.Columns.Add("Status", "Статус");
                dataGridView.Columns["Status"].Width = 80;

                dataGridView.Columns.Add("TotalAmount", "Сумма");
                dataGridView.Columns["TotalAmount"].Width = 90;
                dataGridView.Columns["TotalAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView.Columns["TotalAmount"].DefaultCellStyle.Format = "N2";

                dataGridView.Columns.Add("Discount", "Скидка");
                dataGridView.Columns["Discount"].Width = 70;
                dataGridView.Columns["Discount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView.Columns["Discount"].DefaultCellStyle.Format = "N2";

                dataGridView.Columns.Add("FinalAmount", "Итого");
                dataGridView.Columns["FinalAmount"].Width = 90;
                dataGridView.Columns["FinalAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView.Columns["FinalAmount"].DefaultCellStyle.Format = "N2";

                dataGridView.Rows.Clear();

                foreach (DataRow row in orders.Rows)
                {
                    int rowIndex = dataGridView.Rows.Add();
                    DataGridViewRow dgvRow = dataGridView.Rows[rowIndex];

                    dgvRow.Cells["OrderID"].Value = row["ID заказа"];
                    dgvRow.Cells["Customer"].Value = row["Клиент"];
                    dgvRow.Cells["Products"].Value = row["Товары"];
                    dgvRow.Cells["ItemsCount"].Value = row["Кол-во позиций"];
                    dgvRow.Cells["TotalUnits"].Value = row["Всего единиц"];
                    dgvRow.Cells["OrderDate"].Value = row["Дата заказа"];
                    dgvRow.Cells["Status"].Value = row["Статус"];
                    dgvRow.Cells["TotalAmount"].Value = row["Общая стоимость"];
                    dgvRow.Cells["Discount"].Value = row["Скидка"];
                    dgvRow.Cells["FinalAmount"].Value = row["Итого"];
                }

                // Обновляем заголовок
                this.Text = $"Заказы (всего: {dataGridView.Rows.Count})";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заказов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCreateOrder_Click(object sender, EventArgs e)
        {
            var createForm = new OrderCreateForm();
            if (createForm.ShowDialog() == DialogResult.OK)
            {
                LoadOrders();
            }
        }

        private void btnPrintReceipt_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите заказ для печати чека", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                DataGridViewRow selectedRow = dataGridView.SelectedRows[0];
                int orderId = Convert.ToInt32(selectedRow.Cells["OrderID"].Value);
                string customer = selectedRow.Cells["Customer"].Value?.ToString() ?? "";
                string date = selectedRow.Cells["OrderDate"].Value?.ToString() ?? "";
                string products = selectedRow.Cells["Products"].Value?.ToString() ?? "";
                decimal total = Convert.ToDecimal(selectedRow.Cells["TotalAmount"].Value);
                decimal discount = Convert.ToDecimal(selectedRow.Cells["Discount"].Value);
                decimal final = Convert.ToDecimal(selectedRow.Cells["FinalAmount"].Value);

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*";
                saveDialog.FileName = $"Чек_заказ_{orderId}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                saveDialog.DefaultExt = "csv";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(saveDialog.FileName, false, System.Text.Encoding.UTF8))
                    {
                        sw.WriteLine("СТРОЙБАЗА");
                        sw.WriteLine($"ЧЕК №{orderId}");
                        sw.WriteLine($"Дата: {date}");
                        sw.WriteLine($"Клиент: {customer}");
                        sw.WriteLine("----------------------------------------");
                        sw.WriteLine("№;Товар;Кол-во;Цена;Сумма");

                        string[] productList = products.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < productList.Length; i++)
                        {
                            sw.WriteLine($"{i + 1};{productList[i].Trim()};;;");
                        }

                        sw.WriteLine("----------------------------------------");
                        sw.WriteLine($"ИТОГО:;;;{total:F2}");
                        sw.WriteLine($"Скидка:;;;{discount:F2}");
                        sw.WriteLine($"ИТОГО К ОПЛАТЕ:;;;{final:F2}");
                        sw.WriteLine("----------------------------------------");
                        sw.WriteLine("Спасибо за покупку!");
                    }

                    MessageBox.Show($"Чек успешно сохранен!\n{saveDialog.FileName}", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (MessageBox.Show("Открыть файл?", "Вопрос",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(saveDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании чека: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите заказ для отмены", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow selectedRow = dataGridView.SelectedRows[0];

            if (selectedRow.Cells["OrderID"].Value == null)
                return;

            int orderId = Convert.ToInt32(selectedRow.Cells["OrderID"].Value);
            string status = selectedRow.Cells["Status"].Value?.ToString() ?? "";

            if (status == "Отменен" || status == "Доставлен" || status == "Завершен")
            {
                MessageBox.Show($"Заказ со статусом '{status}' нельзя отменить", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Вы уверены, что хотите отменить заказ #{orderId}?",
                "Подтверждение отмены заказа",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    bool success = dbService.UpdateOrderStatus(orderId, "Отменен");

                    if (success)
                    {
                        LoadOrders();
                        MessageBox.Show($"Заказ #{orderId} успешно отменен", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось обновить статус заказа", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при отмене заказа: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите заказ для просмотра", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow selectedRow = dataGridView.SelectedRows[0];

            if (selectedRow.Cells["OrderID"].Value == null)
                return;

            int orderId = Convert.ToInt32(selectedRow.Cells["OrderID"].Value);
            string customer = selectedRow.Cells["Customer"].Value?.ToString() ?? "Неизвестный клиент";
            string products = selectedRow.Cells["Products"].Value?.ToString() ?? "Нет товаров";
            string itemsCount = selectedRow.Cells["ItemsCount"].Value?.ToString() ?? "0";
            string totalUnits = selectedRow.Cells["TotalUnits"].Value?.ToString() ?? "0";
            string date = selectedRow.Cells["OrderDate"].Value?.ToString() ?? "";
            string status = selectedRow.Cells["Status"].Value?.ToString() ?? "";
            string totalAmount = selectedRow.Cells["TotalAmount"].Value != null ?
                Convert.ToDecimal(selectedRow.Cells["TotalAmount"].Value).ToString("N2") : "0,00";
            string discount = selectedRow.Cells["Discount"].Value != null ?
                Convert.ToDecimal(selectedRow.Cells["Discount"].Value).ToString("N2") : "0,00";
            string finalAmount = selectedRow.Cells["FinalAmount"].Value != null ?
                Convert.ToDecimal(selectedRow.Cells["FinalAmount"].Value).ToString("N2") : "0,00";

            string details = $"Детали заказа #{orderId}\n" +
                           $"{new string('=', 40)}\n" +
                           $"Дата: {date}\n" +
                           $"Клиент: {customer}\n" +
                           $"Статус: {status}\n" +
                           $"{new string('-', 40)}\n" +
                           $"Товары:\n{products}\n" +
                           $"{new string('-', 40)}\n" +
                           $"Количество позиций: {itemsCount}\n" +
                           $"Всего единиц товара: {totalUnits} шт.\n" +
                           $"{new string('-', 40)}\n" +
                           $"Общая сумма: {totalAmount} руб.\n" +
                           $"Скидка: {discount} руб.\n" +
                           $"ИТОГО к оплате: {finalAmount} руб.";

            MessageBox.Show(details, $"Просмотр заказа #{orderId}",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadOrders();
        }
    }
}