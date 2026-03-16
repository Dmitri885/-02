namespace Kursych.Forms.Reports
{
    partial class ReportsForm
    {
        private System.ComponentModel.IContainer components = null;

        // Основные элементы - ОБЪЯВЛЯЕМ ТОЛЬКО ОДИН РАЗ!
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelLine;
        private System.Windows.Forms.DataGridView dataGridView;

        // Элементы фильтров
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnGenerate;

        // Элементы итогов
        private System.Windows.Forms.Label lblTotalIncome;
        private System.Windows.Forms.TextBox txtTotalIncome;
        private System.Windows.Forms.Label lblTotalOrders;
        private System.Windows.Forms.TextBox txtTotalOrders;
        private System.Windows.Forms.Label lblTotalItems;
        private System.Windows.Forms.TextBox txtTotalItems;

        // Кнопки экспорта
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnExportWord;

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
            this.panelFilters = new System.Windows.Forms.Panel();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblTotalIncome = new System.Windows.Forms.Label();
            this.txtTotalIncome = new System.Windows.Forms.TextBox();
            this.lblTotalOrders = new System.Windows.Forms.Label();
            this.txtTotalOrders = new System.Windows.Forms.TextBox();
            this.lblTotalItems = new System.Windows.Forms.Label();
            this.txtTotalItems = new System.Windows.Forms.TextBox();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnExportWord = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();

            this.panelTop.SuspendLayout();
            this.panelFilters.SuspendLayout();
            this.panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();

            // 
            // ReportsForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReportsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Отчеты по продажам";

            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.White;
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Controls.Add(this.panelLine);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1100, 80);
            this.panelTop.TabIndex = 0;

            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblTitle.Location = new System.Drawing.Point(30, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(300, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "📊 Отчеты";
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
            // panelFilters
            // 
            this.panelFilters.BackColor = System.Drawing.Color.White;
            this.panelFilters.Controls.Add(this.lblFrom);
            this.panelFilters.Controls.Add(this.dtpFrom);
            this.panelFilters.Controls.Add(this.lblTo);
            this.panelFilters.Controls.Add(this.dtpTo);
            this.panelFilters.Controls.Add(this.btnGenerate);
            this.panelFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilters.Location = new System.Drawing.Point(0, 80);
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Size = new System.Drawing.Size(1100, 70);
            this.panelFilters.TabIndex = 1;

            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblFrom.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblFrom.Location = new System.Drawing.Point(30, 25);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(67, 19);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "Период с:";

            // 
            // dtpFrom
            // 
            this.dtpFrom.CalendarMonthBackground = System.Drawing.Color.FromArgb(240, 242, 245);
            this.dtpFrom.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(100, 22);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(130, 25);
            this.dtpFrom.TabIndex = 1;

            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTo.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblTo.Location = new System.Drawing.Point(250, 25);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(27, 19);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "по:";

            // 
            // dtpTo
            // 
            this.dtpTo.CalendarMonthBackground = System.Drawing.Color.FromArgb(240, 242, 245);
            this.dtpTo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(280, 22);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(130, 25);
            this.dtpTo.TabIndex = 3;

            // 
            // btnGenerate
            // 
            this.btnGenerate.BackColor = System.Drawing.Color.FromArgb(46, 134, 222);
            this.btnGenerate.FlatAppearance.BorderSize = 0;
            this.btnGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGenerate.ForeColor = System.Drawing.Color.White;
            this.btnGenerate.Location = new System.Drawing.Point(440, 18);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(150, 33);
            this.btnGenerate.TabIndex = 4;
            this.btnGenerate.Text = "📈 Сформировать";
            this.btnGenerate.UseVisualStyleBackColor = false;

            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.White;
            this.panelBottom.Controls.Add(this.lblTotalIncome);
            this.panelBottom.Controls.Add(this.txtTotalIncome);
            this.panelBottom.Controls.Add(this.lblTotalOrders);
            this.panelBottom.Controls.Add(this.txtTotalOrders);
            this.panelBottom.Controls.Add(this.lblTotalItems);
            this.panelBottom.Controls.Add(this.txtTotalItems);
            this.panelBottom.Controls.Add(this.btnExportExcel);
            this.panelBottom.Controls.Add(this.btnExportWord);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 600);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1100, 100);
            this.panelBottom.TabIndex = 3;

            // 
            // lblTotalIncome
            // 
            this.lblTotalIncome.AutoSize = true;
            this.lblTotalIncome.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalIncome.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblTotalIncome.Location = new System.Drawing.Point(30, 40);
            this.lblTotalIncome.Name = "lblTotalIncome";
            this.lblTotalIncome.Size = new System.Drawing.Size(59, 19);
            this.lblTotalIncome.TabIndex = 0;
            this.lblTotalIncome.Text = "Доход:";

            // 
            // txtTotalIncome
            // 
            this.txtTotalIncome.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.txtTotalIncome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotalIncome.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.txtTotalIncome.ForeColor = System.Drawing.Color.FromArgb(46, 134, 222);
            this.txtTotalIncome.Location = new System.Drawing.Point(100, 37);
            this.txtTotalIncome.Name = "txtTotalIncome";
            this.txtTotalIncome.ReadOnly = true;
            this.txtTotalIncome.Size = new System.Drawing.Size(130, 25);
            this.txtTotalIncome.TabIndex = 1;
            this.txtTotalIncome.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;

            // 
            // lblTotalOrders
            // 
            this.lblTotalOrders.AutoSize = true;
            this.lblTotalOrders.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalOrders.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblTotalOrders.Location = new System.Drawing.Point(260, 40);
            this.lblTotalOrders.Name = "lblTotalOrders";
            this.lblTotalOrders.Size = new System.Drawing.Size(102, 19);
            this.lblTotalOrders.TabIndex = 2;
            this.lblTotalOrders.Text = "Всего заказов:";

            // 
            // txtTotalOrders
            // 
            this.txtTotalOrders.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.txtTotalOrders.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotalOrders.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.txtTotalOrders.ForeColor = System.Drawing.Color.FromArgb(46, 134, 222);
            this.txtTotalOrders.Location = new System.Drawing.Point(370, 37);
            this.txtTotalOrders.Name = "txtTotalOrders";
            this.txtTotalOrders.ReadOnly = true;
            this.txtTotalOrders.Size = new System.Drawing.Size(80, 25);
            this.txtTotalOrders.TabIndex = 3;
            this.txtTotalOrders.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;

            // 
            // lblTotalItems
            // 
            this.lblTotalItems.AutoSize = true;
            this.lblTotalItems.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalItems.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblTotalItems.Location = new System.Drawing.Point(480, 40);
            this.lblTotalItems.Name = "lblTotalItems";
            this.lblTotalItems.Size = new System.Drawing.Size(120, 19);
            this.lblTotalItems.TabIndex = 4;
            this.lblTotalItems.Text = "Продано единиц:";

            // 
            // txtTotalItems
            // 
            this.txtTotalItems.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.txtTotalItems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotalItems.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.txtTotalItems.ForeColor = System.Drawing.Color.FromArgb(46, 134, 222);
            this.txtTotalItems.Location = new System.Drawing.Point(610, 37);
            this.txtTotalItems.Name = "txtTotalItems";
            this.txtTotalItems.ReadOnly = true;
            this.txtTotalItems.Size = new System.Drawing.Size(80, 25);
            this.txtTotalItems.TabIndex = 5;
            this.txtTotalItems.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;

            // 
            // btnExportExcel
            // 
            //this.btnExportExcel.BackColor = System.Drawing.Color.FromArgb(39, 174, 96);
            //this.btnExportExcel.FlatAppearance.BorderSize = 0;
            //this.btnExportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.btnExportExcel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            //this.btnExportExcel.ForeColor = System.Drawing.Color.White;
            //this.btnExportExcel.Location = new System.Drawing.Point(780, 33);
            //this.btnExportExcel.Name = "btnExportExcel";
            //this.btnExportExcel.Size = new System.Drawing.Size(130, 35);
            //this.btnExportExcel.TabIndex = 6;
            //this.btnExportExcel.Text = "📊 Excel";
            //this.btnExportExcel.UseVisualStyleBackColor = false;

            // 
            // btnExportWord
            // 
            this.btnExportWord.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.btnExportWord.FlatAppearance.BorderSize = 0;
            this.btnExportWord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportWord.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnExportWord.ForeColor = System.Drawing.Color.White;
            this.btnExportWord.Location = new System.Drawing.Point(930, 33);
            this.btnExportWord.Name = "btnExportWord";
            this.btnExportWord.Size = new System.Drawing.Size(130, 35);
            this.btnExportWord.TabIndex = 7;
            this.btnExportWord.Text = "📝 Word";
            this.btnExportWord.UseVisualStyleBackColor = false;

            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 150);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(1100, 450);
            this.dataGridView.TabIndex = 2;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            // 
            // ReportsForm
            // 
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.panelFilters);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);

            this.panelTop.ResumeLayout(false);
            this.panelFilters.ResumeLayout(false);
            this.panelFilters.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
        }
    }
}