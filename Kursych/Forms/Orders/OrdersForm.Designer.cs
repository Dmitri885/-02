namespace Kursych.Forms.Orders
{
    partial class OrdersForm
    {
        private System.ComponentModel.IContainer components = null;

        // Основные элементы
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelLine;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button btnCreateOrder;
        private System.Windows.Forms.Button btnViewDetails;
        private System.Windows.Forms.Button btnCancelOrder;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnPrintReceipt;

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
            this.panelBottom = new System.Windows.Forms.Panel();
            this.btnCreateOrder = new System.Windows.Forms.Button();
            this.btnViewDetails = new System.Windows.Forms.Button();
            this.btnCancelOrder = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.panelTop.SuspendLayout();
            this.panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();

            // 
            // OrdersForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.ClientSize = new System.Drawing.Size(1200, 600);
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrdersForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Управление заказами";


           

            // В InitializeComponent() после btnRefresh добавьте:
            this.btnPrintReceipt = new System.Windows.Forms.Button();
            this.btnPrintReceipt.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnPrintReceipt.FlatAppearance.BorderSize = 0;
            this.btnPrintReceipt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
this.btnPrintReceipt.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
this.btnPrintReceipt.ForeColor = System.Drawing.Color.White;
this.btnPrintReceipt.Location = new System.Drawing.Point(880, 25);
this.btnPrintReceipt.Name = "btnPrintReceipt";
this.btnPrintReceipt.Size = new System.Drawing.Size(140, 35);
this.btnPrintReceipt.TabIndex = 4;
this.btnPrintReceipt.Text = "🖨️ Печать чека";
this.btnPrintReceipt.UseVisualStyleBackColor = false;

// Добавьте в panelBottom.Controls:
this.panelBottom.Controls.Add(this.btnPrintReceipt);
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.White;
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Controls.Add(this.panelLine);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1200, 80);
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
            this.lblTitle.Text = "🛒 Заказы";
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
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.White;
            this.panelBottom.Controls.Add(this.btnCreateOrder);
            this.panelBottom.Controls.Add(this.btnViewDetails);
            this.panelBottom.Controls.Add(this.btnCancelOrder);
            this.panelBottom.Controls.Add(this.btnRefresh);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 520);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1200, 80);
            this.panelBottom.TabIndex = 2;

            // 
            // btnCreateOrder
            // 
            this.btnCreateOrder.BackColor = System.Drawing.Color.FromArgb(46, 134, 222);
            this.btnCreateOrder.FlatAppearance.BorderSize = 0;
            this.btnCreateOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateOrder.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCreateOrder.ForeColor = System.Drawing.Color.White;
            this.btnCreateOrder.Location = new System.Drawing.Point(30, 25);
            this.btnCreateOrder.Name = "btnCreateOrder";
            this.btnCreateOrder.Size = new System.Drawing.Size(140, 35);
            this.btnCreateOrder.TabIndex = 0;
            this.btnCreateOrder.Text = "➕ Создать заказ";
            this.btnCreateOrder.UseVisualStyleBackColor = false;

            // 
            // btnViewDetails
            // 
            this.btnViewDetails.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnViewDetails.FlatAppearance.BorderSize = 0;
            this.btnViewDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewDetails.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnViewDetails.ForeColor = System.Drawing.Color.White;
            this.btnViewDetails.Location = new System.Drawing.Point(180, 25);
            this.btnViewDetails.Name = "btnViewDetails";
            this.btnViewDetails.Size = new System.Drawing.Size(140, 35);
            this.btnViewDetails.TabIndex = 1;
            this.btnViewDetails.Text = "🔍 Просмотр";
            this.btnViewDetails.UseVisualStyleBackColor = false;

            // 
            // btnCancelOrder
            // 
            this.btnCancelOrder.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.btnCancelOrder.FlatAppearance.BorderSize = 0;
            this.btnCancelOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelOrder.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancelOrder.ForeColor = System.Drawing.Color.White;
            this.btnCancelOrder.Location = new System.Drawing.Point(330, 25);
            this.btnCancelOrder.Name = "btnCancelOrder";
            this.btnCancelOrder.Size = new System.Drawing.Size(140, 35);
            this.btnCancelOrder.TabIndex = 2;
            this.btnCancelOrder.Text = "❌ Отменить заказ";
            this.btnCancelOrder.UseVisualStyleBackColor = false;

            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(149, 165, 166);
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(1030, 25);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(140, 35);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "🔄 Обновить";
            this.btnRefresh.UseVisualStyleBackColor = false;

            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 80);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(1200, 440);
            this.dataGridView.TabIndex = 1;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            // 
            // OrdersForm
            // 
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.panelTop.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
        }
    }
}