using System.Drawing;
using System.Windows.Forms;

namespace Kursych.Forms.Config
{
    partial class SettingsForm
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
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "SettingsForm";

            this.Text = "Настройки системы";
            this.Size = new Size(400, 250);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(240, 242, 245);

            // Основная панель
            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = Color.White
            };

            // Заголовок
            var lblTitle = new Label
            {
                Text = "⚙️ Настройки безопасности",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(20, 20),
                AutoSize = true
            };

            // Блокировка бездействия
            var lblInactivity = new Label
            {
                Text = "Блокировка при бездействии:",
                Font = new Font("Segoe UI", 10F),
                Location = new Point(20, 70),
                AutoSize = true
            };

            chkEnableLock = new CheckBox
            {
                Text = "Включить",
                Font = new Font("Segoe UI", 10F),
                Location = new Point(220, 68),
                AutoSize = true
            };

            // Таймаут
            var lblTimeout = new Label
            {
                Text = "Таймаут (секунд):",
                Font = new Font("Segoe UI", 10F),
                Location = new Point(20, 110),
                AutoSize = true
            };

            nudTimeout = new NumericUpDown
            {
                Location = new Point(220, 108),
                Size = new Size(100, 25),
                Minimum = 10,
                Maximum = 300,
                Value = 30,
                Font = new Font("Segoe UI", 10F)
            };

            // Пояснение
            var lblHint = new Label
            {
                Text = "Система будет заблокирована через указанное количество секунд\nпосле последнего действия пользователя.",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = Color.Gray,
                Location = new Point(20, 150),
                AutoSize = true
            };

            // Кнопки
            btnSave = new Button
            {
                Text = "💾 Сохранить",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 134, 222),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(150, 200),
                Size = new Size(100, 30)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "✖ Отмена",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(260, 200),
                Size = new Size(100, 30)
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();

            mainPanel.Controls.AddRange(new Control[] {
                lblTitle, lblInactivity, chkEnableLock,
                lblTimeout, nudTimeout, lblHint,
                btnSave, btnCancel
            });

            this.Controls.Add(mainPanel);
        }

        #endregion
    }
}