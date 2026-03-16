using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Kursych.Forms.Main;
using System.Data;
using System.Drawing;

namespace Kursych.Forms.Users
{
    public partial class UsersForm : Form
    {
        private List<User> users = new List<User>();
        private DatabaseService dbService;
        private bool showPersonalData = false; // Флаг для отображения ПД

        public UsersForm()
        {
            InitializeComponent();
            dbService = new DatabaseService();

            // ЯВНО подключаем обработчики событий для кнопок
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);
            this.btnViewDetails.Click += new EventHandler(this.btnViewDetails_Click);
            this.dataGridView.CellDoubleClick += new DataGridViewCellEventHandler(this.dataGridView_CellDoubleClick);

            // Загружаем данные
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                DataTable usersTable = dbService.GetUsers();
                users.Clear();
                dataGridView.Rows.Clear();
                dataGridView.Columns.Clear();

                // Создаем колонки (основные - всегда видны)
                dataGridView.Columns.Add("UserID", "ID");
                dataGridView.Columns.Add("UserLogin", "Логин");
                dataGridView.Columns.Add("FullName", "ФИО (сокр.)");
                dataGridView.Columns.Add("RoleName", "Роль");
                dataGridView.Columns.Add("CreatedDate", "Дата создания");

                // Персональные данные (будут скрыты по умолчанию)
                dataGridView.Columns.Add("FullNameFull", "ФИО полное");
                dataGridView.Columns.Add("Phone", "Телефон");
                dataGridView.Columns.Add("Email", "Email");
                dataGridView.Columns.Add("Address", "Адрес");
                dataGridView.Columns.Add("BirthDate", "Дата рождения");

                if (dataGridView.Columns.Contains("UserID"))
                    dataGridView.Columns["UserID"].Visible = false;

                // Настройка видимости персональных данных
                ConfigurePersonalDataVisibility();

                // Настройка ширины колонок
                ConfigureColumnWidth();

                foreach (DataRow row in usersTable.Rows)
                {
                    string fullName = $"{row["UserSurname"]} {row["UserName"]} {row["UserPatronymic"]}".Trim();
                    string shortName = GetShortName(row["UserSurname"].ToString(), row["UserName"].ToString());

                    // Определяем роль по RoleID
                    string roleName = GetRoleNameByID(Convert.ToInt32(row["RoleID"]));

                    User user = new User
                    {
                        UserID = Convert.ToInt32(row["UserID"]),
                        UserLogin = row["UserLogin"].ToString(),
                        UserName = row["UserName"].ToString(),
                        UserSurname = row["UserSurname"].ToString(),
                        UserPatronymic = row["UserPatronymic"].ToString(),
                        UserPassword = row["UserPassword"].ToString(),
                        RoleID = Convert.ToInt32(row["RoleID"]),
                        Phone = row.Table.Columns.Contains("Phone") && !row.IsNull("Phone") ?
                                row["Phone"].ToString() : "",
                        Email = row.Table.Columns.Contains("Email") && !row.IsNull("Email") ?
                               row["Email"].ToString() : "",
                        Address = row.Table.Columns.Contains("Address") && !row.IsNull("Address") ?
                                 row["Address"].ToString() : "",
                        BirthDate = row.Table.Columns.Contains("BirthDate") && !row.IsNull("BirthDate") ?
                                   Convert.ToDateTime(row["BirthDate"]) : (DateTime?)null
                    };

                    if (row.Table.Columns.Contains("CreatedDate") && !row.IsNull("CreatedDate"))
                    {
                        user.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                    }
                    else
                    {
                        user.CreatedDate = DateTime.Now;
                    }

                    user.RoleName = roleName;
                    user.IsActive = true; // По умолчанию активен

                    users.Add(user);

                    // Добавляем строку с данными
                    int rowIndex = dataGridView.Rows.Add(
                        user.UserID,
                        user.UserLogin,
                        shortName,
                        user.RoleName,
                        user.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
                        fullName,  // полное ФИО (скрыто)
                        MaskPhone(user.Phone),  // маскированный телефон
                        MaskEmail(user.Email),  // маскированный email
                        MaskAddress(user.Address),  // маскированный адрес
                        user.BirthDate?.ToString("dd.MM.yyyy") ?? "**.**.****"  // дата рождения
                    );

                    // Сохраняем полный объект пользователя в теге строки
                    dataGridView.Rows[rowIndex].Tag = user;
                }

                // Применяем условное форматирование
                ApplyConditionalFormatting();

                // Обновляем информацию о количестве записей
                UpdateCounters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке пользователей: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Настройка видимости персональных данных
        private void ConfigurePersonalDataVisibility()
        {
            bool show = showPersonalData && (UserSession.IsAdmin || UserSession.IsManager);

            if (dataGridView.Columns.Contains("FullNameFull"))
                dataGridView.Columns["FullNameFull"].Visible = show;
            if (dataGridView.Columns.Contains("Phone"))
                dataGridView.Columns["Phone"].Visible = show;
            if (dataGridView.Columns.Contains("Email"))
                dataGridView.Columns["Email"].Visible = show;
            if (dataGridView.Columns.Contains("Address"))
                dataGridView.Columns["Address"].Visible = show;
            if (dataGridView.Columns.Contains("BirthDate"))
                dataGridView.Columns["BirthDate"].Visible = show;

            // Обновляем текст кнопки
            if (btnTogglePersonalData != null)
            {
                btnTogglePersonalData.Text = show ? "🔒 Скрыть ПД" : "👁️ Показать ПД";
            }
        }

        private void ConfigureColumnWidth()
        {
            if (dataGridView.Columns.Contains("UserLogin"))
                dataGridView.Columns["UserLogin"].Width = 100;
            if (dataGridView.Columns.Contains("FullName"))
                dataGridView.Columns["FullName"].Width = 150;
            if (dataGridView.Columns.Contains("RoleName"))
                dataGridView.Columns["RoleName"].Width = 100;
            if (dataGridView.Columns.Contains("CreatedDate"))
                dataGridView.Columns["CreatedDate"].Width = 120;

            // Ширина для персональных данных
            if (dataGridView.Columns.Contains("FullNameFull"))
                dataGridView.Columns["FullNameFull"].Width = 200;
            if (dataGridView.Columns.Contains("Phone"))
                dataGridView.Columns["Phone"].Width = 120;
            if (dataGridView.Columns.Contains("Email"))
                dataGridView.Columns["Email"].Width = 150;
            if (dataGridView.Columns.Contains("Address"))
                dataGridView.Columns["Address"].Width = 200;
            if (dataGridView.Columns.Contains("BirthDate"))
                dataGridView.Columns["BirthDate"].Width = 100;
        }

        // Методы маскировки персональных данных
        private string GetShortName(string surname, string name)
        {
            if (string.IsNullOrEmpty(surname)) return "";
            if (string.IsNullOrEmpty(name)) return surname;
            return $"{surname} {name[0]}.";
        }

        private string MaskPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return "";
            if (phone.Length < 7) return "***";

            // Показываем первые 4 и последние 2 цифры
            if (phone.Length >= 11)
            {
                return phone.Substring(0, 4) + "***" + phone.Substring(phone.Length - 2);
            }
            return phone.Substring(0, Math.Min(4, phone.Length)) + "***";
        }

        private string MaskEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@")) return "***";

            string[] parts = email.Split('@');
            if (parts[0].Length <= 2) return "***@" + parts[1];

            // Показываем первые 2 символа и домен
            return parts[0].Substring(0, 2) + "***@" + parts[1];
        }

        private string MaskAddress(string address)
        {
            if (string.IsNullOrEmpty(address)) return "";
            if (address.Length <= 5) return "***";

            // Показываем только первые 5 символов
            return address.Substring(0, 5) + "...";
        }

        // Условное форматирование строк
        private void ApplyConditionalFormatting()
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Tag is User user)
                {
                    // Подсветка администраторов
                    if (user.RoleID == 1)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(230, 242, 255); // Светло-голубой
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(0, 51, 102);
                    }

                    // Подсветка неактивных пользователей (если есть поле IsActive)
                    if (!user.IsActive)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 220, 220); // Светло-красный
                        row.DefaultCellStyle.ForeColor = Color.DarkRed;
                    }

                    // Подсветка пользователей без телефона
                    if (string.IsNullOrEmpty(user.Phone))
                    {
                        row.Cells["Phone"].Style.BackColor = Color.FromArgb(255, 240, 240);
                        row.Cells["Phone"].Style.ForeColor = Color.DarkRed;
                    }
                }
            }
        }

        // Обновление счетчиков
        private void UpdateCounters()
        {
            int total = users.Count;
            int showing = dataGridView.Rows.Count;

            if (lblCounter != null)
            {
                lblCounter.Text = $"Показано: {showing} из {total}";
            }
        }

        // Просмотр детальной информации (с полными ПД)
        private void ShowUserDetails(User user)
        {
            UserDetailForm detailForm = new UserDetailForm(user);
            detailForm.ShowDialog();
        }

        // Обработчик для кнопки просмотра деталей
        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для просмотра", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dataGridView.SelectedRows[0].Tag is User user)
            {
                ShowUserDetails(user);
            }
        }

        // Обработчик двойного клика по строке
        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView.Rows[e.RowIndex].Tag is User user)
            {
                ShowUserDetails(user);
            }
        }

        // Обработчик для переключения отображения ПД
        private void btnTogglePersonalData_Click(object sender, EventArgs e)
        {
            // Только администраторы и менеджеры могут переключать режим
            if (!UserSession.IsAdmin && !UserSession.IsManager)
            {
                MessageBox.Show("У вас нет прав для просмотра персональных данных",
                    "Доступ запрещен", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            showPersonalData = !showPersonalData;
            ConfigurePersonalDataVisibility();
        }

        // Остальные существующие методы (GetRoleNameByID, GetRoleIDByName, 
        // IsDuplicateFullName, IsDuplicatePhone, IsDuplicateEmail,
        // btnAdd_Click, btnEdit_Click, btnDelete_Click, btnRefresh_Click)
        // остаются без изменений

        private string GetRoleNameByID(int roleID)
        {
            switch (roleID)
            {
                case 1: return "Администратор";
                case 2: return "Менеджер";
                case 3: return "Кассир";
                default: return "Неизвестная роль";
            }
        }

        private int GetRoleIDByName(string roleName)
        {
            switch (roleName)
            {
                case "Администратор": return 1;
                case "Менеджер": return 2;
                case "Кассир": return 3;
                default: return 2;
            }
        }

        // Проверка на дублирование ФИО
        private bool IsDuplicateFullName(string fullName, int excludeUserId = 0)
        {
            foreach (User user in users)
            {
                if (user.UserID == excludeUserId) continue;

                string existingFullName = $"{user.UserSurname} {user.UserName} {user.UserPatronymic}".Trim();
                if (string.Equals(existingFullName, fullName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        // Проверка на дублирование телефона
        private bool IsDuplicatePhone(string phone, int excludeUserId = 0)
        {
            if (string.IsNullOrEmpty(phone)) return false;

            foreach (User user in users)
            {
                if (user.UserID == excludeUserId) continue;

                if (!string.IsNullOrEmpty(user.Phone) &&
                    string.Equals(user.Phone, phone, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        // Проверка на дублирование email
        private bool IsDuplicateEmail(string email, int excludeUserId = 0)
        {
            if (string.IsNullOrEmpty(email)) return false;

            foreach (User user in users)
            {
                if (user.UserID == excludeUserId) continue;

                if (!string.IsNullOrEmpty(user.Email) &&
                    string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                UserEditForm editForm = new UserEditForm();
                editForm.SetAddMode();

                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    string login = editForm.GetLogin();
                    string password = editForm.GetPassword();
                    string fullName = editForm.GetFullName();
                    string phone = editForm.GetPhone();
                    string roleName = editForm.GetRole();
                    string email = editForm.GetEmail();

                    string[] nameParts = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string surname = nameParts.Length > 0 ? nameParts[0] : "";
                    string name = nameParts.Length > 1 ? nameParts[1] : "";
                    string patronymic = nameParts.Length > 2 ? nameParts[2] : "";

                    // Проверка уникальности логина
                    if (!dbService.IsLoginUnique(login))
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Проверка на дублирование ФИО
                    if (IsDuplicateFullName(fullName))
                    {
                        DialogResult result = MessageBox.Show(
                            "Пользователь с таким ФИО уже существует. Всё равно добавить?",
                            "Возможное дублирование",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.No)
                        {
                            return;
                        }
                    }

                    // Проверка на дублирование телефона
                    if (!string.IsNullOrEmpty(phone) && IsDuplicatePhone(phone))
                    {
                        DialogResult result = MessageBox.Show(
                            "Пользователь с таким телефоном уже существует. Всё равно добавить?",
                            "Возможное дублирование",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.No)
                        {
                            return;
                        }
                    }

                    // Проверка на дублирование email
                    if (!string.IsNullOrEmpty(email) && IsDuplicateEmail(email))
                    {
                        DialogResult result = MessageBox.Show(
                            "Пользователь с таким email уже существует. Всё равно добавить?",
                            "Возможное дублирование",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.No)
                        {
                            return;
                        }
                    }

                    User newUser = new User
                    {
                        UserLogin = login,
                        UserPassword = password,
                        UserName = name,
                        UserSurname = surname,
                        UserPatronymic = patronymic,
                        Phone = phone,
                        Email = email,
                        RoleID = GetRoleIDByName(roleName),
                        RoleName = roleName,
                        CreatedDate = DateTime.Now,
                        IsActive = true
                    };

                    if (dbService.AddUser(newUser))
                    {
                        LoadUsers();
                        MessageBox.Show("Пользователь успешно добавлен", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при сохранении пользователя", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении пользователя: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для редактирования", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow selectedRow = dataGridView.SelectedRows[0];
            int userId = Convert.ToInt32(selectedRow.Cells["UserID"].Value);

            User userToEdit = users.Find(u => u.UserID == userId);
            if (userToEdit == null)
            {
                MessageBox.Show("Пользователь не найден", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                UserEditForm editForm = new UserEditForm();
                string fullName = $"{userToEdit.UserSurname} {userToEdit.UserName} {userToEdit.UserPatronymic}".Trim();
                editForm.SetEditMode(
                    userToEdit.UserID,
                    userToEdit.UserLogin,
                    fullName,
                    userToEdit.Phone,
                    userToEdit.RoleName,
                    userToEdit.Email
                );

                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    string login = editForm.GetLogin();
                    string fullNameNew = editForm.GetFullName();
                    string phone = editForm.GetPhone();
                    string roleName = editForm.GetRole();
                    string email = editForm.GetEmail();

                    if (!dbService.IsLoginUnique(login, userId))
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Проверка на дублирование ФИО (исключая текущего пользователя)
                    if (IsDuplicateFullName(fullNameNew, userId))
                    {
                        DialogResult result = MessageBox.Show(
                            "Пользователь с таким ФИО уже существует. Всё равно обновить?",
                            "Возможное дублирование",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.No)
                        {
                            return;
                        }
                    }

                    // Проверка на дублирование телефона (исключая текущего пользователя)
                    if (!string.IsNullOrEmpty(phone) && IsDuplicatePhone(phone, userId))
                    {
                        DialogResult result = MessageBox.Show(
                            "Пользователь с таким телефоном уже существует. Всё равно обновить?",
                            "Возможное дублирование",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.No)
                        {
                            return;
                        }
                    }

                    // Проверка на дублирование email (исключая текущего пользователя)
                    if (!string.IsNullOrEmpty(email) && IsDuplicateEmail(email, userId))
                    {
                        DialogResult result = MessageBox.Show(
                            "Пользователь с таким email уже существует. Всё равно обновить?",
                            "Возможное дублирование",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.No)
                        {
                            return;
                        }
                    }

                    string[] nameParts = fullNameNew.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string surname = nameParts.Length > 0 ? nameParts[0] : "";
                    string name = nameParts.Length > 1 ? nameParts[1] : "";
                    string patronymic = nameParts.Length > 2 ? nameParts[2] : "";

                    userToEdit.UserLogin = login;
                    userToEdit.UserName = name;
                    userToEdit.UserSurname = surname;
                    userToEdit.UserPatronymic = patronymic;
                    userToEdit.Phone = phone;
                    userToEdit.Email = email;
                    userToEdit.RoleID = GetRoleIDByName(roleName);
                    userToEdit.RoleName = roleName;

                    if (dbService.UpdateUser(userToEdit))
                    {
                        LoadUsers();
                        MessageBox.Show("Пользователь успешно обновлен", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при обновлении пользователя", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении пользователя: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для удаления", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow selectedRow = dataGridView.SelectedRows[0];
            int userId = Convert.ToInt32(selectedRow.Cells["UserID"].Value);
            string login = selectedRow.Cells["UserLogin"].Value.ToString();
            string fullName = selectedRow.Cells["FullName"].Value.ToString();

            if (UserSession.CurrentUser != null && UserSession.CurrentUser.UserID == userId)
            {
                MessageBox.Show("Нельзя удалить самого себя", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string role = selectedRow.Cells["RoleName"].Value.ToString();
            if (role == "Администратор")
            {
                int adminCount = 0;
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.Cells["RoleName"].Value?.ToString() == "Администратор")
                        adminCount++;
                }

                if (adminCount <= 1)
                {
                    MessageBox.Show("Нельзя удалить последнего администратора", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            DialogResult result = MessageBox.Show(
                $"Вы уверены, что хотите удалить пользователя:\n\n" +
                $"Логин: {login}\n" +
                $"ФИО: {fullName}\n\n" +
                $"Это действие нельзя отменить.",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    if (dbService.DeleteUser(userId))
                    {
                        users.RemoveAll(u => u.UserID == userId);
                        LoadUsers();
                        MessageBox.Show("Пользователь успешно удален", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении пользователя", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }
    }
}