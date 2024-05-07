using ScreenLocker.Common.Classes;
using ScreenLocker.Common.DataBase;
using ScreenLocker.Common.LockFunctions;
using ScreenLocker.Common.Moodle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScreenLocker.Pages
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        MainWindow mainWindow;
        Pages.MainScreenLock parrentPage;
        public Login(MainWindow mainWindow, Pages.MainScreenLock parrentPage)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.parrentPage = parrentPage;
        }
        private async void LogInClick(object sender, RoutedEventArgs e)
        {
            User curUser = Moodle.ConvertJsonToUser(await Moodle.Authenticate(LoginBox.GetText(), PasswordBox.GetText()));
            if (curUser.Validate())
            {
                MainWindow.CurrentSession.User = curUser;
                MainWindow.CurrentSession.User.SaveToDatabase();
                parrentPage.MainScreen.Navigate(new Pages.SessionData());
                //new Windows.AddObservation(mainWindow).Show();
            }
            else
            {
                LoginBox.SetText("Неверный логин или пароль");
                Task LoginError = LoginBox.ShowError(5);
                Task PasswordError = PasswordBox.ShowError(5);
                await LoginError;
                await PasswordError;
                LoginBox.SetText("");
            }

        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Runing = false;
            Hooks.Unhook();
            Common.LockFunctions.Lock.SetTaskManager(true);//Отменить блокировку сочетания клавиш
            Common.LockFunctions.Lock.ShowStartMenu();// Отменить блокировку меню Win
            System.Windows.Application.Current.Shutdown();
        }
    }
}
