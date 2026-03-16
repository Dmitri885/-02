using System;

namespace Kursych
{
    public static class UserSession
    {
        private static User _currentUser;
        private static bool _isMySQLConnected;

        public static User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }

        public static bool IsMySQLConnected
        {
            get { return _isMySQLConnected; }
            set { _isMySQLConnected = value; }
        }

        public static bool IsLoggedIn
        {
            get { return _currentUser != null; }
        }

        public static bool IsAdmin
        {
            get { return IsLoggedIn && _currentUser.RoleID == 1; }
        }

        public static bool IsManager
        {
            get { return IsLoggedIn && _currentUser.RoleID == 2; }
        }

        public static bool IsCashier
        {
            get { return IsLoggedIn && _currentUser.RoleID == 3; }
        }

        public static void Clear()
        {
            _currentUser = null;
            _isMySQLConnected = false;
        }

        public static string GetRoleName(int roleId)
        {
            switch (roleId)
            {
                case 1:
                    return "Администратор";
                case 2:
                    return "Менеджер";
                case 3:
                    return "Кассир";
                default:
                    return "Неизвестная роль";
            }
        }
    }

    public class User
    {
        public int UserID { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserPatronymic { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; } = true;

        public string FullName
        {
            get { return $"{UserSurname} {UserName} {UserPatronymic}".Trim(); }
        }

        public string ShortName
        {
            get { return $"{UserSurname} {UserName[0]}."; }
        }
    }
}