﻿using Microsoft.VisualBasic.Logging;
using ScreenLocker.Common.Classes;
using ScreenLocker.Common.DataMonitoring;
using ScreenLocker.Common.LockFunctions;
using ScreenLocker.Pages;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ScreenLocker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string ConnectionString = "server = DESKTOP-ARTEM; Trusted_Connection = No; DataBase = Diplom; User = sa; PWD = sa";

        public static bool Runing;//Флаг, что программа запущена
        public static bool Blocking;// Флаг что надо блокировать все что только можно

        public static Session CurrentSession;
        public static List<Window> screenLocks = new List<Window>();
        public static List<User> users = new List<User>();
        public static List<Auditory> AuditoriesList = new List<Auditory>();
        

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            CurrentSession = new Session();
            if (CheckDB())
            {
                Lock.SetAutorunValue(true); // - Ля какая опасная штука
                Hooks.SetHook();// - ЛЯЯЯЯЯЯЯЯЯЯЯЯЯЯ ОПАСНОСТЬ
                Runing = true;
                Blocking = true;
                
                ShowLockScreens(); // Блокировка
                Lock.SetTaskManager(false);// Отключение диспетчера задач
            }
            else System.Windows.MessageBox.Show("Не удалось подключиться к базе данных");

        }

        public bool CheckDB()
        {
            System.Data.DataTable Check = Common.DataBase.MsSQL.Query($"SELECT GETDATE()", MainWindow.ConnectionString);
            return (DateTime.Parse(Check.Rows[0][0].ToString().Split(" ")[0]) == DateTime.Now.Date);
        }

        public void LoadData()
        {
            System.Data.DataTable Auditories = Common.DataBase.MsSQL.Query($"SELECT * FROM [dbo].[Auditory]", MainWindow.ConnectionString);
            for (int i=0;i< Auditories.Rows.Count;i++)
            {
                Auditory auditory = new Auditory();
                auditory.ID = Convert.ToInt32(Auditories.Rows[i][0]);
                auditory.Name = Convert.ToString(Auditories.Rows[i][1]);
                AuditoriesList.Add(auditory);
            }
        }

        /// <summary>
        /// Сбор данных по процессам
        /// </summary>
        
        /// <summary>
        /// Отобразить незакрываемые экраны блокировки
        /// </summary>
        public void ShowLockScreens()
        {
            for (int i = 0; i < System.Windows.Forms.Screen.AllScreens.Length; i++)//Разворачивание экранов
            {
                if (i == 0)
                {
                    var WindowMain = new MainScreenLock(System.Windows.Forms.Screen.AllScreens[i], this);
                    var WorkingAreaMain = System.Windows.Forms.Screen.AllScreens[i].WorkingArea;
                    WindowMain.Top = WorkingAreaMain.Top;
                    WindowMain.Left = WorkingAreaMain.Left;
                    WindowMain.Width = WorkingAreaMain.Width;
                    WindowMain.Height = WorkingAreaMain.Height;
                    WindowMain.Topmost = true;
                    WindowMain.ShowInTaskbar = false;
                    WindowMain.Show();
                    if (WindowMain.IsLoaded) WindowMain.WindowState = WindowState.Maximized;// растянуть на весь экран
                    Common.LockFunctions.Lock.KillStartMenu();// запретить панель задач
                    screenLocks.Add(WindowMain);
                }
                else
                {
                    var Window = new ScreenLock(System.Windows.Forms.Screen.AllScreens[i], this);
                    var WorkingArea = System.Windows.Forms.Screen.AllScreens[i].WorkingArea;
                    Window.Top = WorkingArea.Top;
                    Window.Left = WorkingArea.Left;
                    Window.Width = WorkingArea.Width;
                    Window.Height = WorkingArea.Height;
                    Window.Topmost = true;
                    Window.ShowInTaskbar = false;
                    Window.Show();
                    if (Window.IsLoaded) Window.WindowState = WindowState.Maximized;// растянуть на весь экран
                    Common.LockFunctions.Lock.KillStartMenu();// запретить панель задач
                    screenLocks.Add(Window);
                }
                //this.Hide();
            }
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Runing = false;
            MainWindow.Blocking = false;
            MainWindow.CurrentSession.EndSession();
            Hooks.Unhook();
            Common.LockFunctions.Lock.SetTaskManager(true);//Отменить блокировку сочетания клавиш
            Common.LockFunctions.Lock.ShowStartMenu();// Отменить блокировку меню Win
            System.Windows.Application.Current.Shutdown();
        }

        private void ReOpen_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Blocking = true;
            Common.LockFunctions.Lock.KillStartMenu();// запретить панель задач
            ShowLockScreens();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) { if (MainWindow.Runing) e.Cancel = true; }

        
    }
}