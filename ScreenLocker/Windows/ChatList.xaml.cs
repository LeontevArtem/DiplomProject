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
    /// Логика взаимодействия для ChatList.xaml
    /// </summary>
    public partial class ChatList : Window
    {
        MainWindow mainWindow;
        public ChatList(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            LoadData();
        }
        public void LoadData()
        {
            var GroupedByUser = MainWindow.messages.Where(x => x.To.id == MainWindow.CurrentSession.User.id&&x.From.id!=MainWindow.CurrentSession.User.id).GroupBy(x=>x.From);
            foreach (var User in GroupedByUser)
            {
                parrent.Children.Add(new UIElements.ChatUserItem(mainWindow,User.Key,this));
            }
        }
    }
}
