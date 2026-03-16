using System.Drawing;
using System.Windows.Forms;

namespace Kursych.Forms.Orders
{
    partial class OrderCreateForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Основные элементы
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelLine;

        // Таблицы
        private System.Windows.Forms.DataGridView dataGridViewProducts;
        private System.Windows.Forms.DataGridView dataGridViewOrder;

        // Заголовки таблиц
        private System.Windows.Forms.Label lblProducts;
        private System.Windows.Forms.Label lblOrder;

        // Элементы поиска и фильтров
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Label lblSearchName;
        private System.Windows.Forms.TextBox txtSearchName;
        private System.Windows.Forms.Label lblSupplier;
        private System.Windows.Forms.ComboBox comboBoxSuppliers;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox comboBoxCategories;
        private System.Windows.Forms.Button btnSearch;

        // Элементы количества
        private System.Windows.Forms.Label lblQuantity;

        // Кнопки действий с заказом
        private System.Windows.Forms.Button btnAddToOrder;
        private System.Windows.Forms.Button btnRemoveFromOrder;

        // Итоговые суммы
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.Label lblDiscount;
        private System.Windows.Forms.TextBox txtDiscount;
        private System.Windows.Forms.Label lblFinalTotal;
        private System.Windows.Forms.TextBox txtFinalTotal;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.TextBox txtOrderDate;

        // Кнопки управления
        private System.Windows.Forms.Button btnCreateOrder;
        private System.Windows.Forms.Button btnCancel;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelLine = new System.Windows.Forms.Panel();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.lblSearchName = new System.Windows.Forms.Label();
            this.txtSearchName = new System.Windows.Forms.TextBox();
            this.lblSupplier = new System.Windows.Forms.Label();
            this.comboBoxSuppliers = new System.Windows.Forms.ComboBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.comboBoxCategories = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.lblProducts = new System.Windows.Forms.Label();
            this.dataGridViewProducts = new System.Windows.Forms.DataGridView();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.btnAddToOrder = new System.Windows.Forms.Button();
            this.panelRight = new System.Windows.Forms.Panel();
            this.lblOrder = new System.Windows.Forms.Label();
            this.dataGridViewOrder = new System.Windows.Forms.DataGridView();
            this.lblTotal = new System.Windows.Forms.Label();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.lblDiscount = new System.Windows.Forms.Label();
            this.txtDiscount = new System.Windows.Forms.TextBox();
            this.lblFinalTotal = new System.Windows.Forms.Label();
            this.txtFinalTotal = new System.Windows.Forms.TextBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.txtOrderDate = new System.Windows.Forms.TextBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.btnCreateOrder = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRemoveFromOrder = new System.Windows.Forms.Button();

            this.panelTop.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).BeginInit();
            this.panelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOrder)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();

            // 
            // OrderCreateForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrderCreateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Оформление заказа";

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
            this.lblTitle.Text = "🛒 Оформление заказа";
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
            // panelSearch
            // 
            this.panelSearch.BackColor = System.Drawing.Color.White;
            this.panelSearch.Controls.Add(this.lblSearchName);
            this.panelSearch.Controls.Add(this.txtSearchName);
            this.panelSearch.Controls.Add(this.lblSupplier);
            this.panelSearch.Controls.Add(this.comboBoxSuppliers);
            this.panelSearch.Controls.Add(this.lblCategory);
            this.panelSearch.Controls.Add(this.comboBoxCategories);
            this.panelSearch.Controls.Add(this.btnSearch);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(0, 80);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(1200, 60);
            this.panelSearch.TabIndex = 1;

            // 
            // lblSearchName
            // 
            this.lblSearchName.AutoSize = true;
            this.lblSearchName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSearchName.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblSearchName.Location = new System.Drawing.Point(30, 20);
            this.lblSearchName.Name = "lblSearchName";
            this.lblSearchName.Size = new System.Drawing.Size(57, 19);
            this.lblSearchName.TabIndex = 0;
            this.lblSearchName.Text = "Поиск:";

            // 
            // txtSearchName
            // 
            this.txtSearchName.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.txtSearchName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearchName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearchName.Location = new System.Drawing.Point(90, 18);
            this.txtSearchName.Name = "txtSearchName";
            this.txtSearchName.Size = new System.Drawing.Size(200, 25);
            this.txtSearchName.TabIndex = 1;

            // 
            // lblSupplier
            // 
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSupplier.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblSupplier.Location = new System.Drawing.Point(310, 20);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(92, 19);
            this.lblSupplier.TabIndex = 2;
            this.lblSupplier.Text = "Поставщик:";

            // 
            // comboBoxSuppliers
            // 
            this.comboBoxSuppliers.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.comboBoxSuppliers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSuppliers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxSuppliers.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.comboBoxSuppliers.Location = new System.Drawing.Point(390, 18);
            this.comboBoxSuppliers.Name = "comboBoxSuppliers";
            this.comboBoxSuppliers.Size = new System.Drawing.Size(180, 25);
            this.comboBoxSuppliers.TabIndex = 3;

            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCategory.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblCategory.Location = new System.Drawing.Point(590, 20);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(85, 19);
            this.lblCategory.TabIndex = 4;
            this.lblCategory.Text = "Категория:";

            // 
            // comboBoxCategories
            // 
            this.comboBoxCategories.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.comboBoxCategories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCategories.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxCategories.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.comboBoxCategories.Location = new System.Drawing.Point(670, 18);
            this.comboBoxCategories.Name = "comboBoxCategories";
            this.comboBoxCategories.Size = new System.Drawing.Size(180, 25);
            this.comboBoxCategories.TabIndex = 5;

            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(46, 134, 222);
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(880, 15);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 30);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "🔍 Поиск";
            this.btnSearch.UseVisualStyleBackColor = false;

            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.White;
            this.panelLeft.Controls.Add(this.lblProducts);
            this.panelLeft.Controls.Add(this.dataGridViewProducts);
            this.panelLeft.Controls.Add(this.lblQuantity);
            this.panelLeft.Controls.Add(this.btnAddToOrder);
            this.panelLeft.Location = new System.Drawing.Point(12, 150);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(580, 450);
            this.panelLeft.TabIndex = 2;

            // 
            // lblProducts
            // 
            this.lblProducts.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblProducts.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblProducts.Location = new System.Drawing.Point(15, 15);
            this.lblProducts.Name = "lblProducts";
            this.lblProducts.Size = new System.Drawing.Size(200, 25);
            this.lblProducts.TabIndex = 0;
            this.lblProducts.Text = "📋 Товары";

            // 
            // dataGridViewProducts
            // 
            this.dataGridViewProducts.AllowUserToAddRows = false;
            this.dataGridViewProducts.AllowUserToDeleteRows = false;
            this.dataGridViewProducts.BackgroundColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.dataGridViewProducts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProducts.Location = new System.Drawing.Point(15, 45);
            this.dataGridViewProducts.Name = "dataGridViewProducts";
            this.dataGridViewProducts.ReadOnly = true;
            this.dataGridViewProducts.RowHeadersVisible = false;
            this.dataGridViewProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewProducts.Size = new System.Drawing.Size(550, 300);
            this.dataGridViewProducts.TabIndex = 1;

            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblQuantity.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblQuantity.Location = new System.Drawing.Point(11, 365);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(94, 19);
            this.lblQuantity.TabIndex = 2;
            this.lblQuantity.Text = "Количество:";

            // 
            // btnAddToOrder
            // 
            this.btnAddToOrder.BackColor = System.Drawing.Color.FromArgb(46, 134, 222);
            this.btnAddToOrder.FlatAppearance.BorderSize = 0;
            this.btnAddToOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddToOrder.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddToOrder.ForeColor = System.Drawing.Color.White;
            this.btnAddToOrder.Location = new System.Drawing.Point(200, 365);
            this.btnAddToOrder.Name = "btnAddToOrder";
            this.btnAddToOrder.Size = new System.Drawing.Size(150, 30);
            this.btnAddToOrder.TabIndex = 4;
            this.btnAddToOrder.Text = "➕ Добавить в заказ";
            this.btnAddToOrder.UseVisualStyleBackColor = false;

            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.Color.White;
            this.panelRight.Controls.Add(this.lblOrder);
            this.panelRight.Controls.Add(this.dataGridViewOrder);
            this.panelRight.Controls.Add(this.lblTotal);
            this.panelRight.Controls.Add(this.txtTotal);
            this.panelRight.Controls.Add(this.lblDiscount);
            this.panelRight.Controls.Add(this.txtDiscount);
            this.panelRight.Controls.Add(this.lblFinalTotal);
            this.panelRight.Controls.Add(this.txtFinalTotal);
            this.panelRight.Controls.Add(this.lblDate);
            this.panelRight.Controls.Add(this.txtOrderDate);
            this.panelRight.Location = new System.Drawing.Point(608, 150);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(580, 450);
            this.panelRight.TabIndex = 3;

            // 
            // lblOrder
            // 
            this.lblOrder.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblOrder.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblOrder.Location = new System.Drawing.Point(15, 15);
            this.lblOrder.Name = "lblOrder";
            this.lblOrder.Size = new System.Drawing.Size(200, 25);
            this.lblOrder.TabIndex = 0;
            this.lblOrder.Text = "🛒 Текущий заказ";

            // 
            // dataGridViewOrder
            // 
            this.dataGridViewOrder.AllowUserToAddRows = false;
            this.dataGridViewOrder.AllowUserToDeleteRows = false;
            this.dataGridViewOrder.BackgroundColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.dataGridViewOrder.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOrder.Location = new System.Drawing.Point(15, 45);
            this.dataGridViewOrder.Name = "dataGridViewOrder";
            this.dataGridViewOrder.RowHeadersVisible = false;
            this.dataGridViewOrder.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewOrder.Size = new System.Drawing.Size(550, 260);
            this.dataGridViewOrder.TabIndex = 1;

            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblTotal.Location = new System.Drawing.Point(15, 320);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(60, 19);
            this.lblTotal.TabIndex = 2;
            this.lblTotal.Text = "Сумма:";

            // 
            // txtTotal
            // 
            this.txtTotal.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.txtTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.txtTotal.ForeColor = System.Drawing.Color.FromArgb(46, 134, 222);
            this.txtTotal.Location = new System.Drawing.Point(80, 317);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.ReadOnly = true;
            this.txtTotal.Size = new System.Drawing.Size(120, 25);
            this.txtTotal.TabIndex = 3;
            this.txtTotal.Text = "0.00";
            this.txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;

            // 
            // lblDiscount
            // 
            this.lblDiscount.AutoSize = true;
            this.lblDiscount.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDiscount.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblDiscount.Location = new System.Drawing.Point(220, 320);
            this.lblDiscount.Name = "lblDiscount";
            this.lblDiscount.Size = new System.Drawing.Size(64, 19);
            this.lblDiscount.TabIndex = 4;
            this.lblDiscount.Text = "Скидка:";

            // 
            // txtDiscount
            // 
            this.txtDiscount.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.txtDiscount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDiscount.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.txtDiscount.ForeColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.txtDiscount.Location = new System.Drawing.Point(290, 317);
            this.txtDiscount.Name = "txtDiscount";
            this.txtDiscount.ReadOnly = true;
            this.txtDiscount.Size = new System.Drawing.Size(100, 25);
            this.txtDiscount.TabIndex = 5;
            this.txtDiscount.Text = "0.00";
            this.txtDiscount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;

            // 
            // lblFinalTotal
            // 
            this.lblFinalTotal.AutoSize = true;
            this.lblFinalTotal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblFinalTotal.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblFinalTotal.Location = new System.Drawing.Point(410, 320);
            this.lblFinalTotal.Name = "lblFinalTotal";
            this.lblFinalTotal.Size = new System.Drawing.Size(54, 19);
            this.lblFinalTotal.TabIndex = 6;
            this.lblFinalTotal.Text = "Итого:";

            // 
            // txtFinalTotal
            // 
            this.txtFinalTotal.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.txtFinalTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFinalTotal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.txtFinalTotal.ForeColor = System.Drawing.Color.FromArgb(39, 174, 96);
            this.txtFinalTotal.Location = new System.Drawing.Point(465, 317);
            this.txtFinalTotal.Name = "txtFinalTotal";
            this.txtFinalTotal.ReadOnly = true;
            this.txtFinalTotal.Size = new System.Drawing.Size(100, 25);
            this.txtFinalTotal.TabIndex = 7;
            this.txtFinalTotal.Text = "0.00";
            this.txtFinalTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;

            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblDate.Location = new System.Drawing.Point(15, 360);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(46, 19);
            this.lblDate.TabIndex = 8;
            this.lblDate.Text = "Дата:";

            // 
            // txtOrderDate
            // 
            this.txtOrderDate.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.txtOrderDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOrderDate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtOrderDate.Location = new System.Drawing.Point(65, 357);
            this.txtOrderDate.Name = "txtOrderDate";
            this.txtOrderDate.ReadOnly = true;
            this.txtOrderDate.Size = new System.Drawing.Size(150, 25);
            this.txtOrderDate.TabIndex = 9;

            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.White;
            this.panelBottom.Controls.Add(this.btnCreateOrder);
            this.panelBottom.Controls.Add(this.btnCancel);
            this.panelBottom.Controls.Add(this.btnRemoveFromOrder);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 610);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1200, 90);
            this.panelBottom.TabIndex = 4;

            // 
            // btnCreateOrder
            // 
            this.btnCreateOrder.BackColor = System.Drawing.Color.FromArgb(39, 174, 96);
            this.btnCreateOrder.FlatAppearance.BorderSize = 0;
            this.btnCreateOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateOrder.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCreateOrder.ForeColor = System.Drawing.Color.White;
            this.btnCreateOrder.Location = new System.Drawing.Point(758, 30);
            this.btnCreateOrder.Name = "btnCreateOrder";
            this.btnCreateOrder.Size = new System.Drawing.Size(140, 35);
            this.btnCreateOrder.TabIndex = 1;
            this.btnCreateOrder.Text = "✅ Создать заказ";
            this.btnCreateOrder.UseVisualStyleBackColor = false;

            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(149, 165, 166);
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(908, 30);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(140, 35);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "✖ Отмена";
            this.btnCancel.UseVisualStyleBackColor = false;

            // 
            // btnRemoveFromOrder
            // 
            this.btnRemoveFromOrder.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.btnRemoveFromOrder.FlatAppearance.BorderSize = 0;
            this.btnRemoveFromOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveFromOrder.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRemoveFromOrder.ForeColor = System.Drawing.Color.White;
            this.btnRemoveFromOrder.Location = new System.Drawing.Point(608, 30);
            this.btnRemoveFromOrder.Name = "btnRemoveFromOrder";
            this.btnRemoveFromOrder.Size = new System.Drawing.Size(140, 35);
            this.btnRemoveFromOrder.TabIndex = 0;
            this.btnRemoveFromOrder.Text = "❌ Убрать из заказа";
            this.btnRemoveFromOrder.UseVisualStyleBackColor = false;

            // Добавляем элементы на форму
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.panelTop);

            this.panelTop.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            this.panelLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).EndInit();
            this.panelRight.ResumeLayout(false);
            this.panelRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOrder)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}