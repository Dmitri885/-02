using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursych.Forms.Config
{
    public partial class SettingsForm : Form
    {
        private NumericUpDown nudTimeout;
        private CheckBox chkEnableLock;
        private Button btnSave;
        private Button btnCancel;

        public SettingsForm()
        {
            InitializeComponent();
            LoadSettings();
        }

       

        private void LoadSettings()
        {
            try
            {
                string timeoutValue = ConfigurationManager.AppSettings["InactivityTimeoutSeconds"];
                if (!string.IsNullOrEmpty(timeoutValue) && int.TryParse(timeoutValue, out int timeout))
                {
                    nudTimeout.Value = timeout;
                }

                string enabledValue = ConfigurationManager.AppSettings["EnableInactivityLock"];
                chkEnableLock.Checked = enabledValue?.ToLower() == "true";
            }
            catch
            {
                // Используем значения по умолчанию
                nudTimeout.Value = 30;
                chkEnableLock.Checked = true;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Сохраняем настройки в конфигурацию
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                config.AppSettings.Settings["InactivityTimeoutSeconds"].Value = nudTimeout.Value.ToString();
                config.AppSettings.Settings["EnableInactivityLock"].Value = chkEnableLock.Checked.ToString();

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

                // Обновляем таймер в трекере
                InactivityTracker.UpdateTimeout((int)nudTimeout.Value);

                MessageBox.Show("Настройки сохранены", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}