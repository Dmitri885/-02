using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Kursych.Forms.Print
{
    public partial class ReceiptForm : Form
    {
        private DataTable orderItems;
        private int orderId;
        private string cashierName;
        private DateTime orderDate;
        private decimal totalAmount;
        private decimal discountAmount;
        private decimal finalAmount;

        public ReceiptForm()
        {
            InitializeComponent();

            // Подключаем обработчики событий
            this.btnPrint.Click += btnPrint_Click;
            this.btnClose.Click += btnClose_Click;
        }

        public void SetOrderData(int id, string cashier, DateTime date, DataTable items,
                                 decimal total, decimal discount, decimal final)
        {
            orderId = id;
            cashierName = cashier;
            orderDate = date;
            orderItems = items;
            totalAmount = total;
            discountAmount = discount;
            finalAmount = final;

            // Заполняем данные на форме
            lblOrderNumberValue.Text = orderId > 0 ? orderId.ToString() : "Предварительный";
            lblDateValue.Text = orderDate.ToString("dd.MM.yyyy HH:mm");
            lblCashierValue.Text = cashierName;

            // Заполняем таблицу товаров
            dataGridView.Rows.Clear();

            if (orderItems != null)
            {
                foreach (DataRow row in orderItems.Rows)
                {
                    string productName = row["Name"].ToString();
                    int quantity = Convert.ToInt32(row["Quantity"]);
                    decimal price = Convert.ToDecimal(row["Price"]);
                    decimal sum = price * quantity;

                    dataGridView.Rows.Add(productName, quantity, price, sum);
                }
            }

            // Обновляем суммы
            lblTotalValue.Text = totalAmount.ToString("N2");
            lblDiscountValue.Text = discountAmount.ToString("N2");
            lblFinalTotalValue.Text = finalAmount.ToString("N2");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            // Создаем диалог печати
            PrintDialog printDialog = new PrintDialog();
            PrintDocument printDocument = new PrintDocument();

            printDocument.PrintPage += PrintDocument_PrintPage;
            printDialog.Document = printDocument;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    printDocument.Print();
                    MessageBox.Show("Чек отправлен на печать", "Печать",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка печати: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Рисуем чек
            float yPos = 10;
            float lineHeight = 20;
            Font font = new Font("Courier New", 10);
            Font boldFont = new Font("Courier New", 10, FontStyle.Bold);

            // Заголовок
            e.Graphics.DrawString("      СтройБаза", boldFont, Brushes.Black, 10, yPos);
            yPos += lineHeight;
            e.Graphics.DrawString("      Чек продажи", font, Brushes.Black, 10, yPos);
            yPos += lineHeight * 2;

            // Информация о заказе
            string orderText = orderId > 0 ? $"№ заказа: {orderId}" : "Предварительный чек";
            e.Graphics.DrawString(orderText, font, Brushes.Black, 10, yPos);
            yPos += lineHeight;
            e.Graphics.DrawString($"Дата: {orderDate:dd.MM.yyyy HH:mm}", font, Brushes.Black, 10, yPos);
            yPos += lineHeight;
            e.Graphics.DrawString($"Кассир: {cashierName}", font, Brushes.Black, 10, yPos);
            yPos += lineHeight * 2;

            // Разделитель
            e.Graphics.DrawString("----------------------------", font, Brushes.Black, 10, yPos);
            yPos += lineHeight;

            // Заголовки таблицы
            e.Graphics.DrawString("Товар", boldFont, Brushes.Black, 10, yPos);
            e.Graphics.DrawString("Кол", boldFont, Brushes.Black, 150, yPos);
            e.Graphics.DrawString("Цена", boldFont, Brushes.Black, 200, yPos);
            e.Graphics.DrawString("Сумма", boldFont, Brushes.Black, 280, yPos);
            yPos += lineHeight;

            // Товары
            if (orderItems != null)
            {
                foreach (DataRow row in orderItems.Rows)
                {
                    string name = row["Name"].ToString();
                    if (name.Length > 15) name = name.Substring(0, 15) + "...";

                    int quantity = Convert.ToInt32(row["Quantity"]);
                    decimal price = Convert.ToDecimal(row["Price"]);
                    decimal sum = price * quantity;

                    e.Graphics.DrawString(name, font, Brushes.Black, 10, yPos);
                    e.Graphics.DrawString(quantity.ToString(), font, Brushes.Black, 150, yPos);
                    e.Graphics.DrawString(price.ToString("N2"), font, Brushes.Black, 200, yPos);
                    e.Graphics.DrawString(sum.ToString("N2"), font, Brushes.Black, 280, yPos);

                    yPos += lineHeight;
                }
            }

            // Разделитель
            e.Graphics.DrawString("----------------------------", font, Brushes.Black, 10, yPos);
            yPos += lineHeight * 2;

            // Итоги
            e.Graphics.DrawString($"Сумма: {totalAmount:N2}", font, Brushes.Black, 180, yPos);
            yPos += lineHeight;
            e.Graphics.DrawString($"Скидка: {discountAmount:N2}", font, Brushes.Black, 180, yPos);
            yPos += lineHeight;
            e.Graphics.DrawString($"ИТОГО: {finalAmount:N2}", boldFont, Brushes.Black, 180, yPos);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}