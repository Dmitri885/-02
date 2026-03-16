using System;
using System.Runtime.InteropServices;
using System.Timers; // УБИРАЕМ эту строку
using System.Windows.Forms; // ОСТАВЛЯЕМ
using System.Configuration;

namespace Kursych
{
    public static class InactivityTracker
    {
        // Используем System.Windows.Forms.Timer (явно указываем)
        private static System.Windows.Forms.Timer inactivityTimer;
        private static int inactivityTimeoutSeconds = 30;
        private static bool isEnabled = true;
        private static Form mainForm;
        private static bool isLocked = false;
        private static bool isInitialized = false;

        public static event EventHandler InactivityLock;

        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [StructLayout(LayoutKind.Sequential)]
        private struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }

        public static void Initialize(Form form)
        {
            if (isInitialized) return;

            mainForm = form;
            LoadSettings();

            if (!isEnabled) return;

            // Создаем Windows Forms Timer (не System.Timers.Timer)
            inactivityTimer = new System.Windows.Forms.Timer();
            inactivityTimer.Interval = 1000; // 1 секунда
            inactivityTimer.Tick += CheckInactivity;
            inactivityTimer.Start();

            HookFormEvents(mainForm);
            isInitialized = true;

            System.Diagnostics.Debug.WriteLine($"InactivityTracker инициализирован. Таймаут: {inactivityTimeoutSeconds} сек.");
        }

        private static void LoadSettings()
        {
            try
            {
                string timeoutValue = ConfigurationManager.AppSettings["InactivityTimeoutSeconds"];
                if (!string.IsNullOrEmpty(timeoutValue) && int.TryParse(timeoutValue, out int timeout))
                {
                    inactivityTimeoutSeconds = timeout;
                }

                string enabledValue = ConfigurationManager.AppSettings["EnableInactivityLock"];
                if (!string.IsNullOrEmpty(enabledValue))
                {
                    isEnabled = enabledValue.ToLower() == "true";
                }
            }
            catch
            {
                // Используем значения по умолчанию
            }
        }

        private static void HookFormEvents(Form form)
        {
            form.MouseMove += (s, e) => ResetTimer();
            form.KeyPress += (s, e) => ResetTimer();
            form.KeyDown += (s, e) => ResetTimer();
            form.MouseClick += (s, e) => ResetTimer();

            foreach (Control control in GetAllControls(form))
            {
                control.MouseMove += (s, e) => ResetTimer();
                control.KeyPress += (s, e) => ResetTimer();
                control.KeyDown += (s, e) => ResetTimer();
                control.MouseClick += (s, e) => ResetTimer();
            }
        }

        private static System.Collections.Generic.IEnumerable<Control> GetAllControls(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                yield return control;
                foreach (Control child in GetAllControls(control))
                {
                    yield return child;
                }
            }
        }

        private static void ResetTimer(object sender = null, EventArgs e = null)
        {
            // При активности таймер сбрасывается автоматически
            // Так как мы используем Tick событие, а не Elapsed
        }

        private static void CheckInactivity(object sender, EventArgs e)
        {
            if (!isEnabled || isLocked || mainForm == null) return;

            uint idleTime = GetIdleTime();

            if (idleTime >= inactivityTimeoutSeconds)
            {
                LockSystem();
            }
        }

        private static uint GetIdleTime()
        {
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);

            if (!GetLastInputInfo(ref lastInputInfo))
            {
                return 0;
            }

            return ((uint)Environment.TickCount - lastInputInfo.dwTime) / 1000;
        }

        private static void LockSystem()
        {
            if (isLocked) return;

            isLocked = true;
            System.Diagnostics.Debug.WriteLine("Система заблокирована из-за бездействия");

            InactivityLock?.Invoke(null, EventArgs.Empty);
        }

        public static void UnlockSystem()
        {
            isLocked = false;
        }

        public static void Stop()
        {
            if (inactivityTimer != null)
            {
                inactivityTimer.Stop();
                inactivityTimer.Dispose();
                inactivityTimer = null;
            }
            isInitialized = false;
        }

        public static void UpdateTimeout(int seconds)
        {
            if (seconds > 0)
            {
                inactivityTimeoutSeconds = seconds;
            }
        }

        public static bool IsLocked
        {
            get { return isLocked; }
        }
    }
}