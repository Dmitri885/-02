using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Text;
using MySql.Data.MySqlClient;
using Kursych.Forms.Main;

namespace Kursych.Forms.Reports
{
    public partial class ReportsForm : Form
    {
        private DatabaseService dbService;

        public ReportsForm()
        {
            InitializeComponent();
            dbService = new DatabaseService();

            // Подключаем обработчики событий
            this.Load += ReportsForm_Load;
            this.btnGenerate.Click += btnGenerate_Click;
            //this.btnExportExcel.Click += btnExportExcel_Click;
            this.btnExportWord.Click += btnExportWord_Click;

            // Подключаем обработчики для ограничения дат
            this.dtpFrom.ValueChanged += DtpFrom_ValueChanged;
            this.dtpTo.ValueChanged += DtpTo_ValueChanged;

            // СНАЧАЛА устанавливаем MinDate и MaxDate
            dtpFrom.MinDate = new DateTime(2025, 9, 9);
            dtpFrom.MaxDate = DateTime.Now;
            dtpTo.MinDate = new DateTime(2025, 9, 9);
            dtpTo.MaxDate = DateTime.Now;

            // БЕЗОПАСНАЯ установка значений
            try
            {
                dtpFrom.Value = DateTime.Now.AddMonths(-1);
            }
            catch
            {
                dtpFrom.Value = dtpFrom.MinDate;
            }

            try
            {
                dtpTo.Value = DateTime.Now;
            }
            catch
            {
                dtpTo.Value = dtpTo.MaxDate;
            }
        }

        private void ReportsForm_Load(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            LoadReport();
        }

        // ================ ОГРАНИЧЕНИЕ ДАТ ================

        private void DtpFrom_ValueChanged(object sender, EventArgs e)
        {
            // Дата "по" не может быть меньше даты "с"
            if (dtpTo.Value < dtpFrom.Value)
            {
                dtpTo.Value = dtpFrom.Value;
            }

            // Максимальная дата "по" - сегодня
            dtpTo.MaxDate = DateTime.Now;
        }

        private void DtpTo_ValueChanged(object sender, EventArgs e)
        {
            // Дата "с" не может быть больше даты "по"
            if (dtpFrom.Value > dtpTo.Value)
            {
                dtpFrom.Value = dtpTo.Value;
            }

            // Максимальная дата "с" - сегодня
            dtpFrom.MaxDate = DateTime.Now;
        }

        // ================ ВАЛИДАЦИЯ ДАТ ================

        private bool ValidateDates()
        {
            // Проверка на корректность дат
            if (dtpFrom.Value > dtpTo.Value)
            {
                MessageBox.Show("Дата 'по' не может быть меньше даты 'с'", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Проверка на будущие даты
            if (dtpFrom.Value > DateTime.Now || dtpTo.Value > DateTime.Now)
            {
                MessageBox.Show("Нельзя выбрать будущую дату", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Проверка на слишком большой период (например, больше 1 года)
            if ((dtpTo.Value - dtpFrom.Value).TotalDays > 365)
            {
                DialogResult result = MessageBox.Show(
                    "Выбран период больше 1 года. Это может замедлить формирование отчета.\n\nПродолжить?",
                    "Предупреждение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                return result == DialogResult.Yes;
            }

            return true;
        }

        // ================ ЗАГРУЗКА ОТЧЕТА ================

        private void LoadReport()
        {
            // Валидация дат перед загрузкой
            if (!ValidateDates())
                return;

            try
            {
                DateTime fromDate = dtpFrom.Value.Date;
                DateTime toDate = dtpTo.Value.Date.AddDays(1).AddSeconds(-1);

                DataTable reportData = GetSalesReport(fromDate, toDate);

                dataGridView.DataSource = reportData;

                // Настройка колонок
                if (dataGridView.Columns.Count > 0)
                {
                    if (dataGridView.Columns.Contains("OrderID"))
                    {
                        dataGridView.Columns["OrderID"].HeaderText = "№ заказа";
                        dataGridView.Columns["OrderID"].Width = 80;
                    }
                    if (dataGridView.Columns.Contains("OrderDate"))
                    {
                        dataGridView.Columns["OrderDate"].HeaderText = "Дата";
                        dataGridView.Columns["OrderDate"].Width = 120;
                    }
                    if (dataGridView.Columns.Contains("Customer"))
                    {
                        dataGridView.Columns["Customer"].HeaderText = "Клиент";
                        dataGridView.Columns["Customer"].Width = 150;
                    }
                    if (dataGridView.Columns.Contains("Products"))
                    {
                        dataGridView.Columns["Products"].HeaderText = "Товары";
                        dataGridView.Columns["Products"].Width = 250;
                        dataGridView.Columns["Products"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    }
                    if (dataGridView.Columns.Contains("TotalUnits"))
                    {
                        dataGridView.Columns["TotalUnits"].HeaderText = "Кол-во";
                        dataGridView.Columns["TotalUnits"].Width = 80;
                        dataGridView.Columns["TotalUnits"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    if (dataGridView.Columns.Contains("FinalAmount"))
                    {
                        dataGridView.Columns["FinalAmount"].HeaderText = "Сумма (руб.)";
                        dataGridView.Columns["FinalAmount"].Width = 100;
                        dataGridView.Columns["FinalAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridView.Columns["FinalAmount"].DefaultCellStyle.Format = "N2";
                    }

                    // Автоматическая высота строк для переноса текста
                    dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                }

                CalculateTotals(fromDate, toDate);

                // Обновляем заголовок формы
                this.Text = $"Отчет по продажам: {dtpFrom.Value:dd.MM.yyyy} - {dtpTo.Value:dd.MM.yyyy}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке отчета: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable GetSalesReport(DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable();

            string query = @"
                SELECT 
                    o.OrderID,
                    DATE_FORMAT(o.OrderDate, '%d.%m.%Y %H:%i') as OrderDate,
                    COALESCE(CONCAT(u.UserSurname, ' ', u.UserName), 'Неизвестный клиент') as Customer,
                    GROUP_CONCAT(
                        CONCAT(p.Name, ' (', oi.Quantity, ' шт.)')
                        SEPARATOR '; '
                    ) as Products,
                    SUM(oi.Quantity) as TotalUnits,
                    o.FinalAmount
                FROM orders o
                LEFT JOIN users u ON o.UserID = u.UserID
                LEFT JOIN order_items oi ON o.OrderID = oi.OrderID
                LEFT JOIN product p ON oi.ProductID = p.ProductID
                WHERE o.OrderDate BETWEEN @FromDate AND @ToDate
                    AND o.Status != 'Отменен'
                GROUP BY o.OrderID
                ORDER BY o.OrderDate DESC";

            using (MySqlConnection connection = new MySqlConnection(dbService.GetConnectionString()))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }

        private void CalculateTotals(DateTime fromDate, DateTime toDate)
        {
            string query = @"
                SELECT 
                    COALESCE(SUM(o.FinalAmount), 0) as TotalIncome,
                    COUNT(DISTINCT o.OrderID) as TotalOrders,
                    COALESCE(SUM(oi.Quantity), 0) as TotalItems
                FROM orders o
                LEFT JOIN order_items oi ON o.OrderID = oi.OrderID
                WHERE o.OrderDate BETWEEN @FromDate AND @ToDate
                    AND o.Status != 'Отменен'";

            using (MySqlConnection connection = new MySqlConnection(dbService.GetConnectionString()))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);

                    connection.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtTotalIncome.Text = Convert.ToDecimal(reader["TotalIncome"]).ToString("N2") + " ₽";
                            txtTotalOrders.Text = Convert.ToInt32(reader["TotalOrders"]).ToString();
                            txtTotalItems.Text = Convert.ToInt32(reader["TotalItems"]).ToString();
                        }
                    }
                }
            }
        }

        // ================ ЭКСПОРТ ================

        //private void btnExportExcel_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (dataGridView.Rows.Count == 0)
        //        {
        //            MessageBox.Show("Нет данных для экспорта", "Информация",
        //                MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            return;
        //        }

        //        SaveFileDialog saveDialog = new SaveFileDialog();
        //        saveDialog.Filter = "Excel файлы (*.csv)|*.csv|Все файлы (*.*)|*.*";
        //        saveDialog.FileName = $"Отчет_продажи_{dtpFrom.Value:ddMMyyyy}_{dtpTo.Value:ddMMyyyy}.csv";
        //        saveDialog.DefaultExt = "csv";
        //        saveDialog.Title = "Сохранить отчет в Excel";

        //        if (saveDialog.ShowDialog() == DialogResult.OK)
        //        {
        //            using (StreamWriter sw = new StreamWriter(saveDialog.FileName, false, Encoding.UTF8))
        //            {
        //                // Заголовки
        //                sw.WriteLine("№ заказа;Дата;Клиент;Товары;Кол-во;Сумма");

        //                // Данные
        //                foreach (DataGridViewRow row in dataGridView.Rows)
        //                {
        //                    if (!row.IsNewRow)
        //                    {
        //                        string orderId = row.Cells["OrderID"].Value?.ToString() ?? "";
        //                        string date = row.Cells["OrderDate"].Value?.ToString() ?? "";
        //                        string customer = row.Cells["Customer"].Value?.ToString() ?? "";
        //                        string products = row.Cells["Products"].Value?.ToString() ?? "";
        //                        string units = row.Cells["TotalUnits"].Value?.ToString() ?? "0";
        //                        string amount = row.Cells["FinalAmount"].Value != null ?
        //                            Convert.ToDecimal(row.Cells["FinalAmount"].Value).ToString("F2") : "0.00";

        //                        sw.WriteLine($"{orderId};{date};{customer};{products};{units};{amount}");
        //                    }
        //                }

        //                // Итоги
        //                sw.WriteLine($";;;;{txtTotalItems.Text};{txtTotalIncome.Text.Replace(" ₽", "").Replace(",", ".")}");
        //            }

        //            MessageBox.Show($"Отчет успешно сохранен!\n{saveDialog.FileName}", "Успех",
        //                MessageBoxButtons.OK, MessageBoxIcon.Information);

        //            // Спрашиваем, открыть ли файл
        //            if (MessageBox.Show("Открыть файл?", "Вопрос",
        //                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //            {
        //                System.Diagnostics.Process.Start(saveDialog.FileName);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка",
        //            MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        private void btnExportWord_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView.Rows.Count == 0)
                {
                    MessageBox.Show("Нет данных для экспорта", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Word файлы (*.doc)|*.doc|Все файлы (*.*)|*.*";
                saveDialog.FileName = $"Отчет_продажи_{dtpFrom.Value:ddMMyyyy}_{dtpTo.Value:ddMMyyyy}.doc";
                saveDialog.DefaultExt = "doc";
                saveDialog.Title = "Сохранить отчет в Word";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder html = new StringBuilder();

                    // Начало HTML документа
                    html.AppendLine("<html>");
                    html.AppendLine("<head>");
                    html.AppendLine("<meta charset='UTF-8'>");
                    html.AppendLine("<style>");
                    html.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
                    html.AppendLine("h1 { color: #333; font-size: 18px; text-align: center; }");
                    html.AppendLine("table { border-collapse: collapse; width: 100%; margin-top: 20px; }");
                    html.AppendLine("th { background-color: #4CAF50; color: white; padding: 10px; }");
                    html.AppendLine("td { border: 1px solid #ddd; padding: 8px; }");
                    html.AppendLine("tr:nth-child(even) { background-color: #f2f2f2; }");
                    html.AppendLine(".total { font-weight: bold; background-color: #e0e0e0; }");
                    html.AppendLine(".right { text-align: right; }");
                    html.AppendLine("</style>");
                    html.AppendLine("</head>");
                    html.AppendLine("<body>");

                    // Заголовок
                    html.AppendLine($"<h1>Отчет по продажам за период: {dtpFrom.Value:dd.MM.yyyy} - {dtpTo.Value:dd.MM.yyyy}</h1>");

                    // Таблица
                    html.AppendLine("<table>");
                    html.AppendLine("<tr>");
                    html.AppendLine("<th>№ заказа</th>");
                    html.AppendLine("<th>Дата</th>");
                    html.AppendLine("<th>Клиент</th>");
                    html.AppendLine("<th>Товары</th>");
                    html.AppendLine("<th>Кол-во</th>");
                    html.AppendLine("<th>Сумма (руб.)</th>");
                    html.AppendLine("</tr>");

                    // Данные
                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            html.AppendLine("<tr>");
                            html.AppendLine($"<td>{row.Cells["OrderID"].Value}</td>");
                            html.AppendLine($"<td>{row.Cells["OrderDate"].Value}</td>");
                            html.AppendLine($"<td>{row.Cells["Customer"].Value}</td>");
                            html.AppendLine($"<td>{row.Cells["Products"].Value}</td>");
                            html.AppendLine($"<td class='right'>{row.Cells["TotalUnits"].Value}</td>");
                            html.AppendLine($"<td class='right'>{Convert.ToDecimal(row.Cells["FinalAmount"].Value):F2}</td>");
                            html.AppendLine("</tr>");
                        }
                    }

                    // Итоги
                    html.AppendLine("<tr class='total'>");
                    html.AppendLine("<td colspan='4' style='text-align: right;'><strong>ИТОГО:</strong></td>");
                    html.AppendLine($"<td class='right'><strong>{txtTotalItems.Text}</strong></td>");
                    html.AppendLine($"<td class='right'><strong>{txtTotalIncome.Text.Replace(" ₽", "").Replace(",", ".")}</strong></td>");
                    html.AppendLine("</tr>");

                    html.AppendLine("</table>");

                    // Дополнительная информация
                    html.AppendLine("<br><hr><br>");
                    html.AppendLine("<p><strong>Сводка:</strong></p>");
                    html.AppendLine($"<p>Всего заказов: {txtTotalOrders.Text}</p>");
                    html.AppendLine($"<p>Продано единиц товара: {txtTotalItems.Text}</p>");
                    html.AppendLine($"<p>Общий доход: {txtTotalIncome.Text}</p>");
                    html.AppendLine($"<p>Дата формирования отчета: {DateTime.Now:dd.MM.yyyy HH:mm}</p>");

                    html.AppendLine("</body>");
                    html.AppendLine("</html>");

                    // Сохраняем как HTML, но с расширением .doc - Word откроет
                    File.WriteAllText(saveDialog.FileName, html.ToString(), Encoding.UTF8);

                    MessageBox.Show($"Отчет успешно сохранен!\n{saveDialog.FileName}", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Спрашиваем, открыть ли файл
                    if (MessageBox.Show("Открыть файл?", "Вопрос",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(saveDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}