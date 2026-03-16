using System;
using System.Drawing;
using System.Windows.Forms;
using Kursych.Forms.Config;
using Kursych.Forms.Main;
using Kursych.Helpers;


namespace Kursych.Forms.Auth
{
    public partial class LoginForm : Form
    {
        private DatabaseService dbService;
        private MySQLHelper dbHelper;

        // Поля для CAPTCHA
        private int failedAttempts = 0;
        private string currentCaptcha = "";
        private int lockSecondsRemaining = 0;
        private bool isLocked = false;

        public LoginForm()
        {
            InitializeComponent();
            this.AcceptButton = btnLogin;
            dbHelper = new MySQLHelper();
            dbService = new DatabaseService();

            CheckConnectionOnLoad();

            // Инициализация таймера
            if (lockTimer == null)
            {
                lockTimer = new Timer();
            }
            lockTimer.Interval = 1000;
            lockTimer.Tick += LockTimer_Tick;
        }

        private void CheckConnectionOnLoad()
        {
            if (!dbHelper.TestConnection())
            {
                DialogResult result = MessageBox.Show(
                    "Не удалось подключиться к базе данных. Хотите настроить подключение?",
                    "Ошибка подключения",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    ShowConfigForm();
                }
            }
        }

        private void ShowConfigForm()
        {
            using (DatabaseConfigForm configForm = new DatabaseConfigForm())
            {
                if (configForm.ShowDialog() == DialogResult.OK)
                {
                    dbHelper = new MySQLHelper();
                    dbService = new DatabaseService();

                    if (dbHelper.TestConnection())
                    {
                        MessageBox.Show("Подключение настроено успешно!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void ShowCaptcha()
        {
            // Показываем элементы CAPTCHA
            lblCaptcha.Visible = true;
            pbCaptcha.Visible = true;
            txtCaptcha.Visible = true;
            btnRefreshCaptcha.Visible = true;

            // Сдвигаем кнопки вниз
            btnLogin.Location = new Point(100, 280);
            btnExit.Location = new Point(210, 280);
            btnConfig.Location = new Point(100, 320);

            // Генерируем новую CAPTCHA
            GenerateNewCaptcha();
        }

        private void HideCaptcha()
        {
            // Скрываем элементы CAPTCHA
            lblCaptcha.Visible = false;
            pbCaptcha.Visible = false;
            txtCaptcha.Visible = false;
            btnRefreshCaptcha.Visible = false;
            lblTimer.Visible = false;

            // Возвращаем кнопки на место
            btnLogin.Location = new Point(100, 180);
            btnExit.Location = new Point(210, 180);
            btnConfig.Location = new Point(100, 230);

            // Очищаем поле CAPTCHA
            txtCaptcha.Clear();
        }

        private void GenerateNewCaptcha()
        {
            try
            {
                // Генерируем текст
                currentCaptcha = CaptchaGenerator.Generate(4);

                // Создаем изображение
                Bitmap captchaImage = CaptchaRenderer.RenderCaptcha(currentCaptcha, 180, 40);

                // Отображаем в PictureBox
                if (pbCaptcha.Image != null)
                {
                    pbCaptcha.Image.Dispose();
                }
                pbCaptcha.Image = captchaImage;

                // Очищаем поле ввода
                txtCaptcha.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка генерации CAPTCHA: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LockSystem()
        {
            isLocked = true;
            lockSecondsRemaining = 10;

            // Блокируем элементы управления
            btnLogin.Enabled = false;
            btnRefreshCaptcha.Enabled = false;
            txtLogin.Enabled = false;
            txtPassword.Enabled = false;
            txtCaptcha.Enabled = false;
            btnConfig.Enabled = false;

            // Показываем таймер
            lblTimer.Visible = true;
            lblTimer.Text = $"Блокировка: {lockSecondsRemaining} сек";

            // Запускаем таймер
            if (lockTimer != null)
            {
                lockTimer.Start();
            }

            // Генерируем новую CAPTCHA
            GenerateNewCaptcha();
        }

        private void UnlockSystem()
        {
            isLocked = false;

            // Разблокируем элементы
            btnLogin.Enabled = true;
            btnRefreshCaptcha.Enabled = true;
            txtLogin.Enabled = true;
            txtPassword.Enabled = true;
            txtCaptcha.Enabled = true;
            btnConfig.Enabled = true;

            // Скрываем таймер
            lblTimer.Visible = false;

            // Останавливаем таймер
            if (lockTimer != null)
            {
                lockTimer.Stop();
            }
        }

        private void LockTimer_Tick(object sender, EventArgs e)
        {
            if (lockTimer == null) return;

            lockSecondsRemaining--;
            lblTimer.Text = $"Блокировка: {lockSecondsRemaining} сек";

            if (lockSecondsRemaining <= 0)
            {
                UnlockSystem();
            }
        }

        private void btnRefreshCaptcha_Click(object sender, EventArgs e)
        {
            if (!isLocked)
            {
                GenerateNewCaptcha();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Проверка на блокировку
            if (isLocked)
            {
                MessageBox.Show($"Система заблокирована. Подождите {lockSecondsRemaining} сек.",
                                "Блокировка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка заполнения полей
            if (string.IsNullOrEmpty(txtLogin.Text))
            {
                MessageBox.Show("Введите логин!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLogin.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Введите пароль!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            // Проверка CAPTCHA (если она активна)
            if (failedAttempts > 0)
            {
                if (string.IsNullOrEmpty(txtCaptcha.Text))
                {
                    MessageBox.Show("Введите код CAPTCHA!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCaptcha.Focus();
                    return;
                }

                if (!CaptchaGenerator.Validate(txtCaptcha.Text, currentCaptcha))
                {
                    failedAttempts++;
                    MessageBox.Show("Неверный код CAPTCHA!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (failedAttempts >= 2)
                    {
                        LockSystem();
                    }
                    else
                    {
                        GenerateNewCaptcha();
                    }
                    return;
                }
            }

            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;

            try
            {
                if (!dbHelper.TestConnection())
                {
                    DialogResult result = MessageBox.Show(
                        "Нет подключения к базе данных. Настроить подключение?",
                        "Ошибка подключения",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        ShowConfigForm();
                    }
                    return;
                }

                // Проверка admin из конфига
                if (ConfigHelper.ValidateAdminCredentials(login, password))
                {
                    // Создаем пользователя-администратора
                    User adminUser = new User
                    {
                        UserLogin = login,
                        UserName = "Администратор",
                        UserSurname = "Системный",
                        RoleID = 1,
                        RoleName = "Администратор",
                        IsActive = true
                    };

                    UserSession.CurrentUser = adminUser;
                    UserSession.IsMySQLConnected = true;

                    // Сбрасываем счетчик попыток
                    failedAttempts = 0;
                    HideCaptcha();

                    MessageBox.Show("Добро пожаловать, Администратор!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }

                // Обычная авторизация через БД
                User user = dbService.AuthenticateUser(login, password);

                if (user != null && user.IsActive)
                {
                    // Успешная авторизация
                    UserSession.CurrentUser = user;
                    UserSession.IsMySQLConnected = true;

                    // Сбрасываем счетчик попыток
                    failedAttempts = 0;
                    HideCaptcha();

                    string roleName = UserSession.GetRoleName(user.RoleID);
                    MessageBox.Show($"Добро пожаловать, {user.UserName}!\nРоль: {roleName}",
                        "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else if (user != null && !user.IsActive)
                {
                    MessageBox.Show("Ваша учетная запись деактивирована. Обратитесь к администратору.",
                        "Доступ запрещен", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    failedAttempts++;
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // После первой неудачи показываем CAPTCHA
                    if (failedAttempts >= 1 && !lblCaptcha.Visible)
                    {
                        ShowCaptcha();
                    }

                    txtPassword.Text = "";
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка авторизации: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            ShowConfigForm();
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }

        private void txtLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtPassword.Focus();
            }
        }

        private void txtCaptcha_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }

        // Освобождаем ресурсы при закрытии
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (pbCaptcha.Image != null)
            {
                pbCaptcha.Image.Dispose();
            }
            base.OnFormClosing(e);
        }
    }
}