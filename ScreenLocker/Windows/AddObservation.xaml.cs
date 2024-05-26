using ScreenLocker.Common.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScreenLocker.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddObservation.xaml
    /// </summary>
    public partial class AddObservation : Window
    {
        MainWindow mainWindow;
        public AddObservation(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void Save_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Data.Text!=String.Empty)
            {
                MainWindow.CurrentSession.AddObservation(Data.Text);
                //MsSQL.WriteLogToDataBase(MainWindow.CurrentUser.SessionID, $"Сессия {MainWindow.CurrentUser.SessionID}. Добавлено замечание: \"{Data.Text}\"");
            }
            this.Close();
        }
    }
}
