using System.Drawing;
using System.Windows.Forms;

namespace Kursych.Forms.Print
{
    partial class ReceiptForm
    {
        private System.ComponentModel.IContainer components = null;

        // Основные элементы
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelLine;

        // Информация о заказе
        private System.Windows.Forms.Label lblOrderNumber;
        private System.Windows.Forms.Label lblOrderNumberValue;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblDateValue;
        private System.Windows.Forms.Label lblCashier;
        private System.Windows.Forms.Label lblCashierValue;

        // Таблица товаров
        private System.Windows.Forms.DataGridView dataGridView;

        // Итоговые суммы
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblTotalValue;
        private System.Windows.Forms.Label lblDiscount;
        private System.Windows.Forms.Label lblDiscountValue;
        private System.Windows.Forms.Label lblFinalTotal;
        private System.Windows.Forms.Label lblFinalTotalValue;

        // Кнопки
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelLine = new System.Windows.Forms.Panel();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelBottom = new System.Windows.Forms.Panel();

            this.lblOrderNumber = new System.Windows.Forms.Label();
            this.lblOrderNumberValue = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblDateValue = new System.Windows.Forms.Label();
            this.lblCashier = new System.Windows.Forms.Label();
            this.lblCashierValue = new System.Windows.Forms.Label();

            this.dataGridView = new System.Windows.Forms.DataGridView();

            this.lblTotal = new System.Windows.Forms.Label();
            this.lblTotalValue = new System.Windows.Forms.Label();
            this.lblDiscount = new System.Windows.Forms.Label();
            this.lblDiscountValue = new System.Windows.Forms.Label();
            this.lblFinalTotal = new System.Windows.Forms.Label();
            this.lblFinalTotalValue = new System.Windows.Forms.Label();

            this.btnPrint = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();

            this.panelTop.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();

            // 
            // ReceiptForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.ClientSize = new System.Drawing.Size(500, 650);
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReceiptForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Чек";

            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.White;
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Controls.Add(this.panelLine);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(500, 80);
            this.panelTop.TabIndex = 0;

            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblTitle.Location = new System.Drawing.Point(30, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(400, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "🧾 Чек";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // 
            // panelLine
            // 
            this.panelLine.BackColor = System.Drawing.Color.FromArgb(46, 134, 222);
            this.panelLine.Location = new System.Drawing.Point(30, 65);
            this.panelLine.Name = "panelLine";
            this.panelLine.Size = new System.Drawing.Size(100, 3);
            this.panelLine.TabIndex = 1;

            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.White;
            this.panelContent.Controls.Add(this.lblOrderNumber);
            this.panelContent.Controls.Add(this.lblOrderNumberValue);
            this.panelContent.Controls.Add(this.lblDate);
            this.panelContent.Controls.Add(this.lblDateValue);
            this.panelContent.Controls.Add(this.lblCashier);
            this.panelContent.Controls.Add(this.lblCashierValue);
            this.panelContent.Controls.Add(this.dataGridView);
            this.panelContent.Controls.Add(this.lblTotal);
            this.panelContent.Controls.Add(this.lblTotalValue);
            this.panelContent.Controls.Add(this.lblDiscount);
            this.panelContent.Controls.Add(this.lblDiscountValue);
            this.panelContent.Controls.Add(this.lblFinalTotal);
            this.panelContent.Controls.Add(this.lblFinalTotalValue);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 80);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(500, 490);
            this.panelContent.TabIndex = 1;

            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.White;
            this.panelBottom.Controls.Add(this.btnPrint);
            this.panelBottom.Controls.Add(this.btnClose);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 570);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(500, 80);
            this.panelBottom.TabIndex = 2;

            // 
            // lblOrderNumber
            // 
            this.lblOrderNumber.AutoSize = true;
            this.lblOrderNumber.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblOrderNumber.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblOrderNumber.Location = new System.Drawing.Point(30, 20);
            this.lblOrderNumber.Name = "lblOrderNumber";
            this.lblOrderNumber.Size = new System.Drawing.Size(79, 19);
            this.lblOrderNumber.TabIndex = 0;
            this.lblOrderNumber.Text = "№ заказа:";

            // 
            // lblOrderNumberValue
            // 
            this.lblOrderNumberValue.AutoSize = true;
            this.lblOrderNumberValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblOrderNumberValue.ForeColor = System.Drawing.Color.FromArgb(46, 134, 222);
            this.lblOrderNumberValue.Location = new System.Drawing.Point(120, 20);
            this.lblOrderNumberValue.Name = "lblOrderNumberValue";
            this.lblOrderNumberValue.Size = new System.Drawing.Size(40, 19);
            this.lblOrderNumberValue.TabIndex = 1;
            this.lblOrderNumberValue.Text = "123";

            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblDate.Location = new System.Drawing.Point(30, 45);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(44, 19);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "Дата:";

            // 
            // lblDateValue
            // 
            this.lblDateValue.AutoSize = true;
            this.lblDateValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDateValue.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblDateValue.Location = new System.Drawing.Point(80, 45);
            this.lblDateValue.Name = "lblDateValue";
            this.lblDateValue.Size = new System.Drawing.Size(120, 19);
            this.lblDateValue.TabIndex = 3;
            this.lblDateValue.Text = "01.01.2024 15:30";

            // 
            // lblCashier
            // 
            this.lblCashier.AutoSize = true;
            this.lblCashier.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCashier.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblCashier.Location = new System.Drawing.Point(30, 70);
            this.lblCashier.Name = "lblCashier";
            this.lblCashier.Size = new System.Drawing.Size(67, 19);
            this.lblCashier.TabIndex = 4;
            this.lblCashier.Text = "Кассир:";

            // 
            // lblCashierValue
            // 
            this.lblCashierValue.AutoSize = true;
            this.lblCashierValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCashierValue.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblCashierValue.Location = new System.Drawing.Point(100, 70);
            this.lblCashierValue.Name = "lblCashierValue";
            this.lblCashierValue.Size = new System.Drawing.Size(98, 19);
            this.lblCashierValue.TabIndex = 5;
            this.lblCashierValue.Text = "Иванов И.И.";

            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.BackgroundColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(30, 100);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.Size = new System.Drawing.Size(440, 250);
            this.dataGridView.TabIndex = 6;

            // Настройка колонок
            this.dataGridView.Columns.Add("Product", "Товар");
            this.dataGridView.Columns.Add("Quantity", "Кол-во");
            this.dataGridView.Columns.Add("Price", "Цена");
            this.dataGridView.Columns.Add("Sum", "Сумма");

            this.dataGridView.Columns["Product"].Width = 200;
            this.dataGridView.Columns["Quantity"].Width = 60;
            this.dataGridView.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView.Columns["Price"].Width = 80;
            this.dataGridView.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView.Columns["Price"].DefaultCellStyle.Format = "N2";
            this.dataGridView.Columns["Sum"].Width = 80;
            this.dataGridView.Columns["Sum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView.Columns["Sum"].DefaultCellStyle.Format = "N2";

            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblTotal.Location = new System.Drawing.Point(250, 365);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(59, 19);
            this.lblTotal.TabIndex = 7;
            this.lblTotal.Text = "Сумма:";

            // 
            // lblTotalValue
            // 
            this.lblTotalValue.AutoSize = true;
            this.lblTotalValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalValue.ForeColor = System.Drawing.Color.FromArgb(46, 134, 222);
            this.lblTotalValue.Location = new System.Drawing.Point(350, 365);
            this.lblTotalValue.Name = "lblTotalValue";
            this.lblTotalValue.Size = new System.Drawing.Size(60, 19);
            this.lblTotalValue.TabIndex = 8;
            this.lblTotalValue.Text = "1000.00";

            // 
            // lblDiscount
            // 
            this.lblDiscount.AutoSize = true;
            this.lblDiscount.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDiscount.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblDiscount.Location = new System.Drawing.Point(250, 390);
            this.lblDiscount.Name = "lblDiscount";
            this.lblDiscount.Size = new System.Drawing.Size(63, 19);
            this.lblDiscount.TabIndex = 9;
            this.lblDiscount.Text = "Скидка:";

            // 
            // lblDiscountValue
            // 
            this.lblDiscountValue.AutoSize = true;
            this.lblDiscountValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDiscountValue.ForeColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.lblDiscountValue.Location = new System.Drawing.Point(350, 390);
            this.lblDiscountValue.Name = "lblDiscountValue";
            this.lblDiscountValue.Size = new System.Drawing.Size(44, 19);
            this.lblDiscountValue.TabIndex = 10;
            this.lblDiscountValue.Text = "0.00";

            // 
            // lblFinalTotal
            // 
            this.lblFinalTotal.AutoSize = true;
            this.lblFinalTotal.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblFinalTotal.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblFinalTotal.Location = new System.Drawing.Point(250, 420);
            this.lblFinalTotal.Name = "lblFinalTotal";
            this.lblFinalTotal.Size = new System.Drawing.Size(56, 21);
            this.lblFinalTotal.TabIndex = 11;
            this.lblFinalTotal.Text = "ИТОГО:";

            // 
            // lblFinalTotalValue
            // 
            this.lblFinalTotalValue.AutoSize = true;
            this.lblFinalTotalValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblFinalTotalValue.ForeColor = System.Drawing.Color.FromArgb(39, 174, 96);
            this.lblFinalTotalValue.Location = new System.Drawing.Point(350, 420);
            this.lblFinalTotalValue.Name = "lblFinalTotalValue";
            this.lblFinalTotalValue.Size = new System.Drawing.Size(72, 21);
            this.lblFinalTotalValue.TabIndex = 12;
            this.lblFinalTotalValue.Text = "1000.00";

            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(46, 134, 222);
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(150, 25);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(120, 35);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "🖨️ Печать";
            this.btnPrint.UseVisualStyleBackColor = false;

            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(149, 165, 166);
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(280, 25);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 35);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "✖ Закрыть";
            this.btnClose.UseVisualStyleBackColor = false;

            // Добавляем элементы на форму
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);

            this.panelTop.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.panelContent.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
        }
    }
}