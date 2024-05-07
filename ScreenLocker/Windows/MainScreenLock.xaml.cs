using ScreenLocker.Common.Classes;
using ScreenLocker.Common.DataBase;
using ScreenLocker.Common.Moodle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScreenLocker.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainScreenLock.xaml
    /// </summary>
    public partial class MainScreenLock : Window
    {
        MainWindow mainWindow;
        public MainScreenLock(System.Windows.Forms.Screen curScreen, MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.SourceInitialized += Window1_SourceInitialized;// Для перехвата события перетаскивания
            MainScreen.Navigate(new Pages.Login(mainWindow,this));
        }

        

        /// <summary>
        /// Обработчик нормального закрытия
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Runing = false;
            Common.LockFunctions.Lock.SetTaskManager(true);//Отменить блокировку сочетания клавиш
            Common.LockFunctions.Lock.ShowStartMenu();// Отменить блокировку меню Win
            System.Windows.Application.Current.Shutdown();
        }

        /// <summary>
        /// Перехват драгндропа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window1_SourceInitialized(object sender, EventArgs e)
        {

            WindowInteropHelper helper = new WindowInteropHelper(this);
            HwndSource source = HwndSource.FromHwnd(helper.Handle);
            source.AddHook(Common.LockFunctions.Lock.WndProc);

        }
        /// <summary>
        /// Запрет закрытия окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) { if (MainWindow.Blocking) e.Cancel = true; }

        /// <summary>
        /// Действия при открытии
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var helper = new WindowInteropHelper(this).Handle;
            Common.LockFunctions.Lock.SetWindowLong(helper, Common.LockFunctions.Lock.GWL_EX_STYLE, (Common.LockFunctions.Lock.GetWindowLong(helper, Common.LockFunctions.Lock.GWL_EX_STYLE) | Common.LockFunctions.Lock.WS_EX_TOOLWINDOW) & ~Common.LockFunctions.Lock.WS_EX_APPWINDOW);

        }
    }

}
