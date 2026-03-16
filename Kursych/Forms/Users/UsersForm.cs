using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Kursych.Forms.Main;
using System.Data;

namespace Kursych.Forms.Users
{
    public partial class UsersForm : Form
    {
        private List<User> users = new List<User>();
        private DatabaseService dbService;

        public UsersForm()
        {
            InitializeComponent();
            dbService = new DatabaseService();

            // ЯВНО подключаем обработчики событий для кнопок
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);

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

                // Создаем колонки
                dataGridView.Columns.Add("UserID", "ID");
                dataGridView.Columns.Add("UserLogin", "Логин");
                dataGridView.Columns.Add("FullName", "ФИО");
                dataGridView.Columns.Add("Phone", "Телефон");
                dataGridView.Columns.Add("RoleName", "Роль");
                dataGridView.Columns.Add("Email", "Email");
                dataGridView.Columns.Add("CreatedDate", "Дата создания");

                if (dataGridView.Columns.Contains("UserID"))
                    dataGridView.Columns["UserID"].Visible = false;

                // Настройка ширины колонок
                if (dataGridView.Columns.Contains("UserLogin"))
                    dataGridView.Columns["UserLogin"].Width = 100;
                if (dataGridView.Columns.Contains("FullName"))
                    dataGridView.Columns["FullName"].Width = 200;
                if (dataGridView.Columns.Contains("Phone"))
                    dataGridView.Columns["Phone"].Width = 100;
                if (dataGridView.Columns.Contains("RoleName"))
                    dataGridView.Columns["RoleName"].Width = 100;
                if (dataGridView.Columns.Contains("Email"))
                    dataGridView.Columns["Email"].Width = 150;
                if (dataGridView.Columns.Contains("CreatedDate"))
                    dataGridView.Columns["CreatedDate"].Width = 120;

                foreach (DataRow row in usersTable.Rows)
                {
                    string fullName = $"{row["UserSurname"]} {row["UserName"]} {row["UserPatronymic"]}".Trim();

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
                               row["Email"].ToString() : ""
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

                    users.Add(user);
                    dataGridView.Rows.Add(
                        user.UserID,
                        user.UserLogin,
                        fullName,
                        user.Phone,
                        user.RoleName,
                        user.Email,
                        user.CreatedDate.ToString("dd.MM.yyyy HH:mm")
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке пользователей: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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

        // ЯВНЫЕ обработчики событий
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
                        CreatedDate = DateTime.Now
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