using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing;

namespace Kursych.Forms.Users
{
    public partial class UserEditForm : Form
    {
        private bool isEditMode = false;
        private int userId = 0;
        private ErrorProvider errorProvider;

        public UserEditForm()
        {
            InitializeComponent();

            // Создаем ErrorProvider для отображения ошибок
            errorProvider = new ErrorProvider();
            errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;

            // Подключаем дополнительные обработчики
            this.txtPhone.KeyPress += txtPhone_KeyPress;
            this.txtPhone.TextChanged += txtPhone_TextChanged;
            this.txtPhone.Leave += txtPhone_Leave;

            this.txtLogin.KeyPress += txtLogin_KeyPress;
            this.txtLogin.Leave += txtLogin_Leave;

            this.txtFullName.KeyPress += txtFullName_KeyPress;
            this.txtFullName.Leave += txtFullName_Leave;
            this.txtFullName.TextChanged += txtFullName_TextChanged;

            this.txtEmail.Leave += txtEmail_Leave;

            this.FormClosing += UserEditForm_FormClosing;

            // Заполняем список ролей
            LoadRoles();

            // Устанавливаем подсказки
            SetPlaceholders();
        }

        private void LoadRoles()
        {
            // Заполняем список ролей согласно вашей базе данных
            cmbRole.Items.Clear();
            cmbRole.Items.Add("Администратор");
            cmbRole.Items.Add("Менеджер");
            cmbRole.Items.Add("Кассир");

            if (cmbRole.Items.Count > 0 && cmbRole.SelectedIndex == -1)
                cmbRole.SelectedIndex = 1; // По умолчанию Менеджер
        }

        private void SetPlaceholders()
        {
            // Можно добавить подсказки в поля
            txtPhone.Tag = "Формат: 79123456789";
            txtEmail.Tag = "example@mail.ru";
        }

        // ================ ПУБЛИЧНЫЕ МЕТОДЫ ДЛЯ UsersForm ================

        public void SetEditMode(int id, string login, string fullName, string phone, string role, string email)
        {
            isEditMode = true;
            userId = id;
            txtLogin.Text = login;
            txtFullName.Text = fullName;
            txtPhone.Text = phone;
            txtEmail.Text = email;

            // В режиме редактирования не показываем пароль
            txtPassword.Visible = false;
            lblPassword.Visible = false;

            // Увеличиваем высоту формы (компенсируем скрытие поля пароля)
            this.Height -= 50;

            // Устанавливаем роль - перебираем все элементы
            if (cmbRole.Items.Count > 0)
            {
                for (int i = 0; i < cmbRole.Items.Count; i++)
                {
                    if (cmbRole.Items[i].ToString() == role)
                    {
                        cmbRole.SelectedIndex = i;
                        break;
                    }
                }
            }

            this.Text = "Редактирование пользователя";
            if (lblTitle != null)
                lblTitle.Text = "👤 Редактирование пользователя";
        }

        public void SetAddMode()
        {
            isEditMode = false;
            txtPassword.Visible = true;
            lblPassword.Visible = true;
            txtLogin.Text = "";
            txtFullName.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";

            // Сбрасываем выбор роли
            if (cmbRole.Items.Count > 0)
                cmbRole.SelectedIndex = 1; // По умолчанию Менеджер

            this.Text = "Добавление пользователя";
            if (lblTitle != null)
                lblTitle.Text = "👤 Добавление пользователя";
        }

        public string GetLogin()
        {
            return txtLogin.Text.Trim();
        }

        public string GetPassword()
        {
            return txtPassword.Text;
        }

        public string GetFullName()
        {
            return txtFullName.Text.Trim();
        }

        public string GetPhone()
        {
            // Убираем все не-цифры перед сохранением
            return Regex.Replace(txtPhone.Text, @"[^\d]", "");
        }

        public string GetEmail()
        {
            return txtEmail.Text.Trim();
        }

        public string GetRole()
        {
            return cmbRole.SelectedItem?.ToString() ?? "";
        }

        // ================ МЕТОДЫ ВАЛИДАЦИИ ФИО ================

        /// <summary>
        /// Проверка, содержит ли строка только русские буквы, пробелы и дефисы
        /// </summary>
        private bool IsValidRussianName(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            // Регулярное выражение: только русские буквы, пробелы и дефисы
            // ^ - начало строки
            // [а-яА-ЯёЁ\s-] - русские буквы (включая ё), пробел, дефис
            // + - один или более символов
            // $ - конец строки
            Regex regex = new Regex(@"^[а-яА-ЯёЁ\s-]+$");
            return regex.IsMatch(input);
        }

        /// <summary>
        /// Форматирование ФИО (первая буква заглавная, остальные строчные)
        /// </summary>
        private string FormatRussianName(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            // Разделяем на части по пробелам
            string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < parts.Length; i++)
            {
                if (!string.IsNullOrEmpty(parts[i]))
                {
                    // Учитываем дефисы (например, Анна-Мария)
                    string[] subParts = parts[i].Split('-');
                    for (int j = 0; j < subParts.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(subParts[j]))
                        {
                            // Первая буква заглавная, остальные строчные
                            subParts[j] = char.ToUpper(subParts[j][0]) +
                                          (subParts[j].Length > 1 ? subParts[j].Substring(1).ToLower() : "");
                        }
                    }
                    parts[i] = string.Join("-", subParts);
                }
            }

            return string.Join(" ", parts);
        }

        /// <summary>
        /// Валидация отдельной части ФИО (фамилия, имя, отчество)
        /// </summary>
        private bool ValidateNamePart(string part, string fieldName, out string errorMessage)
        {
            errorMessage = "";

            if (string.IsNullOrWhiteSpace(part))
            {
                errorMessage = $"{fieldName} не может быть пустым";
                return false;
            }

            if (part.Length < 2)
            {
                errorMessage = $"{fieldName} слишком короткое (минимум 2 символа)";
                return false;
            }

            if (part.Length > 50)
            {
                errorMessage = $"{fieldName} слишком длинное (максимум 50 символов)";
                return false;
            }

            // Для отдельной части не должно быть пробелов, только буквы и дефис
            Regex regex = new Regex(@"^[а-яА-ЯёЁ-]+$");
            if (!regex.IsMatch(part))
            {
                errorMessage = $"{fieldName} должно содержать только русские буквы и дефис";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Валидация полного ФИО
        /// </summary>
        private bool ValidateFullName(string fullName, out string errorMessage)
        {
            errorMessage = "";

            if (string.IsNullOrWhiteSpace(fullName))
            {
                errorMessage = "ФИО не может быть пустым";
                return false;
            }

            // Проверка минимальной длины
            if (fullName.Length < 5)
            {
                errorMessage = "ФИО слишком короткое";
                return false;
            }

            // Проверка максимальной длины
            if (fullName.Length > 100)
            {
                errorMessage = "ФИО слишком длинное (максимум 100 символов)";
                return false;
            }

            // Проверка на допустимые символы
            if (!IsValidRussianName(fullName))
            {
                errorMessage = "ФИО должно содержать только русские буквы, пробелы и дефисы";
                return false;
            }

            // Проверка на наличие хотя бы двух слов (фамилия и имя)
            string[] parts = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
            {
                errorMessage = "Введите фамилию и имя (например: Иванов Иван)";
                return false;
            }

            if (parts.Length > 3)
            {
                errorMessage = "ФИО содержит больше 3 частей. Введите Фамилию, Имя и Отчество (если есть)";
                return false;
            }

            // Проверка каждой части
            if (!ValidateNamePart(parts[0], "Фамилия", out string surnameError))
            {
                errorMessage = surnameError;
                return false;
            }

            if (!ValidateNamePart(parts[1], "Имя", out string nameError))
            {
                errorMessage = nameError;
                return false;
            }

            // Отчество может быть не указано, но если указано - проверяем
            if (parts.Length == 3 && !string.IsNullOrWhiteSpace(parts[2]))
            {
                if (!ValidateNamePart(parts[2], "Отчество", out string patronymicError))
                {
                    errorMessage = patronymicError;
                    return false;
                }
            }

            return true;
        }

        // ================ ВАЛИДАЦИЯ ДРУГИХ ПОЛЕЙ ================

        /// <summary>
        /// Валидация логина
        /// </summary>
        private bool ValidateLogin(string login, out string errorMessage)
        {
            errorMessage = "";

            if (string.IsNullOrWhiteSpace(login))
            {
                errorMessage = "Введите логин";
                return false;
            }

            if (login.Length < 3)
            {
                errorMessage = "Логин должен содержать не менее 3 символов";
                return false;
            }

            if (login.Length > 50)
            {
                errorMessage = "Логин слишком длинный (максимум 50 символов)";
                return false;
            }

            // Разрешаем буквы (русские и английские), цифры, точку, подчеркивание
            Regex regex = new Regex(@"^[a-zA-Zа-яА-ЯёЁ0-9._]+$");
            if (!regex.IsMatch(login))
            {
                errorMessage = "Логин может содержать только буквы, цифры, точку и подчеркивание";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Валидация пароля
        /// </summary>
        private bool ValidatePassword(string password, out string errorMessage)
        {
            errorMessage = "";

            if (!isEditMode && string.IsNullOrWhiteSpace(password))
            {
                errorMessage = "Введите пароль";
                return false;
            }

            if (!isEditMode && password.Length < 4)
            {
                errorMessage = "Пароль должен содержать не менее 4 символов";
                return false;
            }

            if (!isEditMode && password.Length > 50)
            {
                errorMessage = "Пароль слишком длинный (максимум 50 символов)";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Валидация телефона
        /// </summary>
        private bool ValidatePhone(string phone, out string errorMessage)
        {
            errorMessage = "";

            if (string.IsNullOrWhiteSpace(phone))
            {
                // Телефон может быть необязательным
                return true;
            }

            // Удаляем все не-цифры для проверки
            string digitsOnly = Regex.Replace(phone, @"[^\d]", "");

            if (digitsOnly.Length != 11)
            {
                errorMessage = "Телефон должен содержать 11 цифр (например: 79123456789)";
                return false;
            }

            // Проверка, что номер начинается с 7 или 8
            if (!digitsOnly.StartsWith("7") && !digitsOnly.StartsWith("8"))
            {
                errorMessage = "Телефон должен начинаться с 7 или 8";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Валидация email
        /// </summary>
        private bool ValidateEmail(string email, out string errorMessage)
        {
            errorMessage = "";

            if (string.IsNullOrWhiteSpace(email))
            {
                // Email может быть необязательным
                return true;
            }

            if (email.Length > 100)
            {
                errorMessage = "Email слишком длинный (максимум 100 символов)";
                return false;
            }

            // Простая проверка email
            if (!email.Contains("@") || !email.Contains("."))
            {
                errorMessage = "Введите корректный email адрес (пример: name@domain.ru)";
                return false;
            }

            // Более строгая проверка email
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address != email)
                {
                    errorMessage = "Введите корректный email адрес";
                    return false;
                }
            }
            catch
            {
                errorMessage = "Введите корректный email адрес";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Валидация роли
        /// </summary>
        private bool ValidateRole(out string errorMessage)
        {
            errorMessage = "";

            if (cmbRole.SelectedIndex == -1)
            {
                errorMessage = "Выберите роль пользователя";
                return false;
            }

            return true;
        }

        // ================ ПОЛНАЯ ВАЛИДАЦИЯ ФОРМЫ ================

        private bool ValidateForm()
        {
            bool isValid = true;

            // Сброс подсветки ошибок
            ResetFieldColors();

            // Валидация логина
            if (!ValidateLogin(txtLogin.Text, out string loginError))
            {
                SetFieldError(txtLogin, loginError);
                isValid = false;
            }

            // Валидация пароля (только при добавлении)
            if (!isEditMode && !ValidatePassword(txtPassword.Text, out string passwordError))
            {
                SetFieldError(txtPassword, passwordError);
                isValid = false;
            }

            // Валидация ФИО
            if (!ValidateFullName(txtFullName.Text, out string fullNameError))
            {
                SetFieldError(txtFullName, fullNameError);
                isValid = false;
            }
            else
            {
                // Автоматически форматируем ФИО при успешной валидации
                txtFullName.Text = FormatRussianName(txtFullName.Text);
            }

            // Валидация телефона
            if (!ValidatePhone(txtPhone.Text, out string phoneError))
            {
                SetFieldError(txtPhone, phoneError);
                isValid = false;
            }

            // Валидация email
            if (!ValidateEmail(txtEmail.Text, out string emailError))
            {
                SetFieldError(txtEmail, emailError);
                isValid = false;
            }

            // Валидация роли
            if (!ValidateRole(out string roleError))
            {
                SetFieldError(cmbRole, roleError);
                isValid = false;
            }

            if (!isValid)
            {
                MessageBox.Show("Пожалуйста, исправьте ошибки в форме",
                    "Ошибка валидации",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            return isValid;
        }

        // ================ ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ================

        private void ResetFieldColors()
        {
            txtLogin.BackColor = SystemColors.Window;
            txtPassword.BackColor = SystemColors.Window;
            txtFullName.BackColor = SystemColors.Window;
            txtPhone.BackColor = SystemColors.Window;
            txtEmail.BackColor = SystemColors.Window;
            cmbRole.BackColor = SystemColors.Window;

            errorProvider.Clear();
        }

        private void SetFieldError(Control control, string errorMessage)
        {
            control.BackColor = Color.LightPink;
            errorProvider.SetError(control, errorMessage);
        }

        // ================ ОБРАБОТЧИКИ СОБЫТИЙ ================

        private void UserEditForm_Load(object sender, EventArgs e)
        {
            // Уже загружено в конструкторе
        }

        private void UserEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Очищаем ресурсы
            errorProvider?.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // ================ ОБРАБОТЧИКИ ДЛЯ ПОЛЯ ФИО ================

        private void txtFullName_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем: русские буквы, пробел, дефис, Backspace
            char c = e.KeyChar;

            if (char.IsControl(c)) // Разрешаем управляющие символы (Backspace, Enter)
                return;

            // Проверяем, является ли символ русской буквой
            bool isRussianLetter = (c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я') || c == 'ё' || c == 'Ё';

            // Разрешаем пробел и дефис
            if (c == ' ' || c == '-')
            {
                TextBox textBox = sender as TextBox;

                // Предотвращаем множественные пробелы подряд
                if (c == ' ' && !string.IsNullOrEmpty(textBox.Text) && textBox.Text.EndsWith(" "))
                {
                    e.Handled = true;
                }
                // Предотвращаем множественные дефисы подряд
                else if (c == '-' && !string.IsNullOrEmpty(textBox.Text) && textBox.Text.EndsWith("-"))
                {
                    e.Handled = true;
                }
                // Запрещаем пробел в начале
                else if (c == ' ' && string.IsNullOrEmpty(textBox.Text))
                {
                    e.Handled = true;
                }
                // Запрещаем дефис в начале
                else if (c == '-' && string.IsNullOrEmpty(textBox.Text))
                {
                    e.Handled = true;
                }
                else
                {
                    return;
                }
            }

            if (!isRussianLetter)
            {
                e.Handled = true; // Блокируем ввод
            }
        }

        private void txtFullName_TextChanged(object sender, EventArgs e)
        {
            // Сбрасываем подсветку ошибки при изменении текста
            txtFullName.BackColor = SystemColors.Window;
            errorProvider.SetError(txtFullName, "");
        }

        private void txtFullName_Leave(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();

            if (!string.IsNullOrEmpty(fullName))
            {
                // Форматируем ФИО
                string formatted = FormatRussianName(fullName);
                txtFullName.Text = formatted;

                // Проверяем валидность
                if (!ValidateFullName(formatted, out string errorMessage))
                {
                    SetFieldError(txtFullName, errorMessage);
                }
            }
        }

        // ================ ОБРАБОТЧИКИ ДЛЯ ПОЛЯ ТЕЛЕФОН ================

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем только цифры и управляющие клавиши
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            // Удаляем форматирование при вводе
            // Оставляем только цифры
            string digitsOnly = Regex.Replace(txtPhone.Text, @"[^\d]", "");

            if (digitsOnly.Length > 11)
            {
                digitsOnly = digitsOnly.Substring(0, 11);
            }

            // Обновляем текст только если он изменился
            if (txtPhone.Text != digitsOnly)
            {
                int cursorPosition = txtPhone.SelectionStart;
                int oldLength = txtPhone.Text.Length;

                txtPhone.Text = digitsOnly;

                // Корректируем позицию курсора
                int newLength = txtPhone.Text.Length;
                if (cursorPosition <= newLength)
                    txtPhone.SelectionStart = cursorPosition;
                else
                    txtPhone.SelectionStart = newLength;
            }

            // Сбрасываем подсветку ошибки
            txtPhone.BackColor = SystemColors.Window;
            errorProvider.SetError(txtPhone, "");
        }

        private void txtPhone_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                if (!ValidatePhone(txtPhone.Text, out string errorMessage))
                {
                    SetFieldError(txtPhone, errorMessage);
                }
                else
                {
                    // Форматируем телефон для отображения (опционально)
                    string digitsOnly = Regex.Replace(txtPhone.Text, @"[^\d]", "");
                    if (digitsOnly.Length == 11)
                    {
                        // Формат: +7 (XXX) XXX-XX-XX
                        txtPhone.Text = $"+7 ({digitsOnly.Substring(1, 3)}) {digitsOnly.Substring(4, 3)}-{digitsOnly.Substring(7, 2)}-{digitsOnly.Substring(9, 2)}";
                    }
                }
            }
        }

        // ================ ОБРАБОТЧИКИ ДЛЯ ПОЛЯ ЛОГИН ================

        private void txtLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем буквы (русские и английские), цифры, точку, подчеркивание, backspace
            if (!char.IsControl(e.KeyChar))
            {
                bool isValidChar = char.IsLetterOrDigit(e.KeyChar) ||
                                   e.KeyChar == '.' ||
                                   e.KeyChar == '_';

                if (!isValidChar)
                {
                    e.Handled = true;
                }
            }
        }

        private void txtLogin_Leave(object sender, EventArgs e)
        {
            if (!ValidateLogin(txtLogin.Text, out string errorMessage))
            {
                SetFieldError(txtLogin, errorMessage);
            }
        }

        // ================ ОБРАБОТЧИКИ ДЛЯ ПОЛЯ EMAIL ================

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                if (!ValidateEmail(txtEmail.Text, out string errorMessage))
                {
                    SetFieldError(txtEmail, errorMessage);
                }
            }
        }
    }
}