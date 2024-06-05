using ScreenLocker.Common.Classes;
using ScreenLocker.Common.DataBase;
using ScreenLocker.Common.DataMonitoring;
using ScreenLocker.Common.Images;
using ScreenLocker.Common.LockFunctions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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

namespace ScreenLocker.Pages
{
    /// <summary>
    /// Логика взаимодействия для SessionData.xaml
    /// </summary>
    public partial class SessionData : Page
    {
        public SessionData()
        {
            InitializeComponent();
            Auditory.ItemsSource = MainWindow.AuditoriesList;
        }

        private void WorkStart_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Auditory.SelectedItem!= null && Accept.IsChecked == true)
            {
                MainWindow.UnlockComputer();
                MainWindow.CurrentSession.StartSession();
                //MainWindow.CurrentSession.AddObservation("Тестовое змечание");
                new Thread(new ThreadStart(DataMonitoring)).Start();
                new Thread(new ThreadStart(SetWorkAreaPreview)).Start();
                MainWindow.ni.Text += $" ({MainWindow.CurrentSession.User.firstname})";
            }
        }

        public void DataMonitoring()
        {
            List<ProcessWindow> processes;
            if (MainWindow.Runing)
            {
                while (MainWindow.Runing)
                {
                    processes = DataMonitor.GetAllProcesses();
                    MainWindow.CurrentSession.WriteLogToDataBase($"{JsonSerializer.Serialize(processes)}", Session.LogTag.Processes);
                    if (MainWindow.CurrentSession.CheckAccess())
                    {
                        MainWindow.LockComputer();
                    }
                    MainWindow.CurrentSession.CheckMessage();
                    Thread.Sleep(1000);
                }
            }
            
        }
        public void SetWorkAreaPreview()
        {

            while (true)
            {
                Common.DataBase.MsSQL.Query($"UPDATE [dbo].[Sessions] SET [WorkAreaPreview] = '{ImageProcessor.ConvertImageToString(ImageProcessor.TakeScreenShot())}' WHERE SessionID={MainWindow.CurrentSession.Id}", MainWindow.ConnectionString);
                Thread.Sleep(1000*60);
            }
        }
        


        private void Auditory_Selected(object sender, SelectionChangedEventArgs e)
        {
            MainWindow.CurrentSession.Auditory = (Auditory.SelectedItem as Common.Classes.Auditory);
        }

    }
}
