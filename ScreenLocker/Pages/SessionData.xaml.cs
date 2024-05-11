using ScreenLocker.Common.Classes;
using ScreenLocker.Common.DataBase;
using ScreenLocker.Common.DataMonitoring;
using ScreenLocker.Common.LockFunctions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            if (Auditory.SelectedItem!= null)
            {
                foreach (Window screenLock in MainWindow.screenLocks)
                {
                    screenLock.Hide();
                }
                Hooks.Unhook();
                MainWindow.Blocking = false;
                Common.LockFunctions.Lock.ShowStartMenu();
                MainWindow.CurrentSession.StartSession();
                MainWindow.CurrentSession.AddObservation("Тестовое змечание");
                new Thread(new ThreadStart(DataMonitoring)).Start();
            }
        }

        public void DataMonitoring()
        {
            List<string> processes;
            while (MainWindow.Runing)
            {
                processes = DataMonitor.GetAllProcesses();
                MainWindow.CurrentSession.WriteLogToDataBase($"Список процессов: {JsonSerializer.Serialize(processes)}", Session.LogTag.Processes);
                Thread.Sleep(1000*1);
            }
        }
        
        


        private void Auditory_Selected(object sender, SelectionChangedEventArgs e)
        {
            MainWindow.CurrentSession.Auditory = (Auditory.SelectedItem as Common.Classes.Auditory);
        }

    }
}
