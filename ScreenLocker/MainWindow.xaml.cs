using Microsoft.VisualBasic.Logging;
using ScreenLocker.Common.Classes;
using ScreenLocker.Common.DataBase;
using ScreenLocker.Common.DataMonitoring;
using ScreenLocker.Common.LockFunctions;
using ScreenLocker.Pages;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ScreenLocker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public static string ConnectionString = "server = DESKTOP-OGA8BNV; Trusted_Connection = No; DataBase = Diplom; User = sa; PWD = sa";

        public static bool Runing;//Флаг, что программа запущена
        public static bool Blocking;// Флаг что надо блокировать все что только можно

        public static Session CurrentSession;
        public static List<System.Windows.Window> screenLocks = new List<System.Windows.Window>();
        public static List<User> users = new List<User>();
        public static List<Auditory> AuditoriesList = new List<Auditory>();
        public static List<Common.Classes.Message> messages = new List<Common.Classes.Message>();
        public static Windows.Chat chat;


        public static MainWindow mainWindow;

        public static System.Windows.Forms.NotifyIcon ni;

        public MainWindow()
        {
            InitializeComponent();
            CreateTrayIcon();
            Runing = true;
            mainWindow = this;
            MainWindow.CurrentSession = new Session();
            LoadData();
            CreateLockScreens();
            if (CheckDB())
            {
                //Lock.SetAutorunValue(true); // - Ля какая опасная штука
                LockComputer();
            }
            else System.Windows.MessageBox.Show("Не удалось подключиться к базе данных");

        }
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }
        public void CreateTrayIcon()
        {
            ni = new System.Windows.Forms.NotifyIcon();
            ni.Icon = new System.Drawing.Icon("Main.ico");
            ni.Visible = true;
            ni.Text = "ScreenLocker";
            ToolStripMenuItem addObservationMenuItem = new ToolStripMenuItem("Добавить замечание");
            addObservationMenuItem.Click += delegate (object sender, EventArgs e) { new Windows.AddObservation(this).Show(); };
            addObservationMenuItem.Image = new Bitmap("addIcon.png");
            ToolStripMenuItem Chat = new ToolStripMenuItem("Открыть чат");
            Chat.Click += delegate (object sender, EventArgs e) { new Windows.ChatList(this).Show(); };
            Chat.Image = new Bitmap("chat.png");
            ni.ContextMenuStrip = new ContextMenuStrip();
            ni.ContextMenuStrip.Items.Add(addObservationMenuItem);
            ni.ContextMenuStrip.Items.Add(Chat);
            ni.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                };
        }
        public static void LockComputer()
        {
            if (!Blocking)
            {
                mainWindow.Dispatcher.Invoke(() =>
                {
                    Blocking = true;
                    Hooks.SetHook();// - ЛЯЯЯЯЯЯЯЯЯЯЯЯЯЯ ОПАСНОСТЬ
                    Lock.SetTaskManager(false);// Отключение диспетчера задач
                    ShowLockScreens(); // Блокировка
                });
            }
            
            
        }
        public static void UnlockComputer()
        {
            if (Blocking)
            {
                Blocking = false;
                foreach (System.Windows.Window screenLock in MainWindow.screenLocks)
                {
                    screenLock.Hide();
                }
                Hooks.Unhook();
                MainWindow.Blocking = false;
                Lock.ShowStartMenu();
            }
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
            users = new List<User>();
            System.Data.DataTable UserQuery = MsSQL.Query($"SELECT * FROM [dbo].[Users]", ConnectionString);
            for (int i = 0; i < UserQuery.Rows.Count; i++)
            {
                User NewUser = new User();
                NewUser.id = Convert.ToString(UserQuery.Rows[i][0]);
                NewUser.username = Convert.ToString(UserQuery.Rows[i][1]);
                NewUser.password = Convert.ToString(UserQuery.Rows[i][2]);
                NewUser.firstname = Convert.ToString(UserQuery.Rows[i][3]);
                NewUser.lastname = Convert.ToString(UserQuery.Rows[i][4]);
                NewUser.email = Convert.ToString(UserQuery.Rows[i][5]);
                NewUser.cohort = Convert.ToString(UserQuery.Rows[i][6]);
                if (!users.Exists(x => x.id == NewUser.id)) users.Add(NewUser);

            }
            System.Data.DataTable MessagesQuery = MsSQL.Query($"SELECT * FROM [dbo].[Message]", MainWindow.ConnectionString);
            for (int i = 0; i < MessagesQuery.Rows.Count; i++)
            {
                Common.Classes.Message NewMessage = new Common.Classes.Message();
                NewMessage.ID = Convert.ToInt32(MessagesQuery.Rows[i][0]);
                NewMessage.From = MainWindow.users.Find(x => x.id == Convert.ToString(MessagesQuery.Rows[i][1]));
                NewMessage.To = MainWindow.users.Find(x => x.id == Convert.ToString(MessagesQuery.Rows[i][2]));
                NewMessage.MessageText = Convert.ToString(MessagesQuery.Rows[i][3]);
                NewMessage.IsRead = Convert.ToInt32(MessagesQuery.Rows[i][4]) == 1 ? true : false;
                NewMessage.Tag = Convert.ToString(MessagesQuery.Rows[i][5]);
                if (!messages.Exists(x => x.ID == NewMessage.ID)) messages.Add(NewMessage);
            }
        }
        /// <summary>
        /// Отобразить незакрываемые экраны блокировки
        /// </summary>
        public static void CreateLockScreens()
        {
            for (int i = 0; i < System.Windows.Forms.Screen.AllScreens.Length; i++)//Разворачивание экранов
            {
                if (System.Windows.Forms.Screen.AllScreens[i].Primary)
                {
                    var WindowMain = new MainScreenLock(System.Windows.Forms.Screen.AllScreens[i], mainWindow);
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
                    var Window = new ScreenLock(System.Windows.Forms.Screen.AllScreens[i], mainWindow);
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
        public static void ShowLockScreens()
        {
            foreach (System.Windows.Window screenLock in MainWindow.screenLocks)
            {
                screenLock.Show();
                if (screenLock is MainScreenLock) (screenLock as MainScreenLock).MainScreen.Navigate(new Pages.Login(mainWindow, (screenLock as MainScreenLock)));
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
            Environment.Exit(0);
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