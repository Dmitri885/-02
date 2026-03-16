namespace Kursych.Forms.Products
{
    partial class ProductEditForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
        /// 
        private System.Windows.Forms.Label lblDuplicateWarning;
        private void InitializeComponent()
        {
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtStockQuantity = new System.Windows.Forms.TextBox();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.cmbSupplier = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblSupplier = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblStockQuantity = new System.Windows.Forms.Label();
            this.lblDuplicateWarning = new System.Windows.Forms.Label();

            // Элементы для изображения
            this.gbImage = new System.Windows.Forms.GroupBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btnSelectImage = new System.Windows.Forms.Button();
            this.btnClearImage = new System.Windows.Forms.Button();
            this.lblImagePath = new System.Windows.Forms.Label();

            this.gbImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();

            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(20, 25);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(83, 20);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Название:";

            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(120, 22);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(250, 26);
            this.txtName.TabIndex = 1;

            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new System.Drawing.Point(20, 65);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(52, 20);
            this.lblPrice.TabIndex = 2;
            this.lblPrice.Text = "Цена:";

            // 
            // txtPrice
            // 
            this.txtPrice.Location = new System.Drawing.Point(120, 62);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(250, 26);
            this.txtPrice.TabIndex = 3;

            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(20, 105);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(83, 20);
            this.lblCategory.TabIndex = 4;
            this.lblCategory.Text = "Категория:";

            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(120, 102);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(250, 28);
            this.cmbCategory.TabIndex = 5;

            // 
            // lblSupplier
            // 
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Location = new System.Drawing.Point(20, 145);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(94, 20);
            this.lblSupplier.TabIndex = 6;
            this.lblSupplier.Text = "Поставщик:";

            // 
            // cmbSupplier
            // 
            this.cmbSupplier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSupplier.FormattingEnabled = true;
            this.cmbSupplier.Location = new System.Drawing.Point(120, 142);
            this.cmbSupplier.Name = "cmbSupplier";
            this.cmbSupplier.Size = new System.Drawing.Size(250, 28);
            this.cmbSupplier.TabIndex = 7;

            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(20, 185);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(83, 20);
            this.lblDescription.TabIndex = 8;
            this.lblDescription.Text = "Описание:";

            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(120, 182);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(250, 80);
            this.txtDescription.TabIndex = 9;

            // 
            // lblStockQuantity
            // 
            this.lblStockQuantity.AutoSize = true;
            this.lblStockQuantity.Location = new System.Drawing.Point(20, 280);
            this.lblStockQuantity.Name = "lblStockQuantity";
            this.lblStockQuantity.Size = new System.Drawing.Size(94, 20);
            this.lblStockQuantity.TabIndex = 10;
            this.lblStockQuantity.Text = "Количество:";

            // 
            // txtStockQuantity
            // 
            this.txtStockQuantity.Location = new System.Drawing.Point(120, 277);
            this.txtStockQuantity.Name = "txtStockQuantity";
            this.txtStockQuantity.Size = new System.Drawing.Size(250, 26);
            this.txtStockQuantity.TabIndex = 11;
            this.txtStockQuantity.Text = "0";
            this.txtStockQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;

            // 
            // groupBox для изображения
            // 
            this.gbImage = new System.Windows.Forms.GroupBox();
            this.gbImage.Text = "Изображение товара";
            this.gbImage.Location = new System.Drawing.Point(400, 22);
            this.gbImage.Name = "gbImage";
            this.gbImage.Size = new System.Drawing.Size(250, 280);
            this.gbImage.TabIndex = 12;
            this.gbImage.TabStop = false;

            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(25, 30);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(200, 150);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;

            // 
            // btnSelectImage
            // 
            this.btnSelectImage.Location = new System.Drawing.Point(25, 195);
            this.btnSelectImage.Name = "btnSelectImage";
            this.btnSelectImage.Size = new System.Drawing.Size(90, 30);
            this.btnSelectImage.TabIndex = 1;
            this.btnSelectImage.Text = "Выбрать...";
            this.btnSelectImage.UseVisualStyleBackColor = true;

            // 
            // btnClearImage
            // 
            this.btnClearImage.Location = new System.Drawing.Point(135, 195);
            this.btnClearImage.Name = "btnClearImage";
            this.btnClearImage.Size = new System.Drawing.Size(90, 30);
            this.btnClearImage.TabIndex = 2;
            this.btnClearImage.Text = "Очистить";
            this.btnClearImage.UseVisualStyleBackColor = true;

            // 
            // lblImagePath
            // 
            this.lblImagePath.AutoSize = true;
            this.lblImagePath.Location = new System.Drawing.Point(25, 240);
            this.lblImagePath.Name = "lblImagePath";
            this.lblImagePath.Size = new System.Drawing.Size(147, 20);
            this.lblImagePath.TabIndex = 3;
            this.lblImagePath.Text = "Файл не выбран";
            this.lblImagePath.ForeColor = System.Drawing.Color.Gray;

            // Добавляем элементы в GroupBox
            this.gbImage.Controls.Add(this.pictureBox);
            this.gbImage.Controls.Add(this.btnSelectImage);
            this.gbImage.Controls.Add(this.btnClearImage);
            this.gbImage.Controls.Add(this.lblImagePath);

            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(120, 330);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 35);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;

            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(250, 330);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 35);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;

            // 
            // ProductEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 390);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbImage);
            this.Controls.Add(this.txtStockQuantity);
            this.Controls.Add(this.lblStockQuantity);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.cmbSupplier);
            this.Controls.Add(this.lblSupplier);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProductEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Добавление товара";
            this.gbImage.ResumeLayout(false);
            this.gbImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            // lblDuplicateWarning
            this.lblDuplicateWarning.AutoSize = true;
            this.lblDuplicateWarning.Location = new System.Drawing.Point(120, 40); // Настройте позицию
            this.lblDuplicateWarning.Name = "lblDuplicateWarning";
            this.lblDuplicateWarning.Size = new System.Drawing.Size(200, 13);
            this.lblDuplicateWarning.TabIndex = 0;
            this.lblDuplicateWarning.Text = "";
            this.lblDuplicateWarning.Visible = false;
            this.lblDuplicateWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);

            // Добавьте lblDuplicateWarning на форму
            this.Controls.Add(this.lblDuplicateWarning);

        }
        
        #endregion

        // Объявление всех элементов управления
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtStockQuantity;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.ComboBox cmbSupplier;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblSupplier;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblStockQuantity;

        // Элементы для изображения
        private System.Windows.Forms.GroupBox gbImage;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button btnSelectImage;
        private System.Windows.Forms.Button btnClearImage;
        private System.Windows.Forms.Label lblImagePath;
    }
}