using ScreenLocker.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScreenLocker.UIElements
{
    /// <summary>
    /// Логика взаимодействия для ChatUserItem.xaml
    /// </summary>
    public partial class ChatUserItem : System.Windows.Controls.UserControl
    {
        MainWindow mainWindow;
        Common.Classes.User currentUser;
        ChatList parrentWindow;
        public ChatUserItem(MainWindow mainWindow,Common.Classes.User currentUser, ChatList parrentWindow)
        {
            InitializeComponent();
            this.currentUser = currentUser;
            this.mainWindow = mainWindow;
            this.parrentWindow = parrentWindow;
            this.SenterName.Content = currentUser.firstname;
            this.UnreadCounter.Content = MainWindow.messages.Where(x=>x.To.id==MainWindow.CurrentSession.User.id&&x.From.id==currentUser.id&&!x.IsRead).Count().ToString();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.chat = new Windows.Chat(mainWindow,currentUser, parrentWindow);
            MainWindow.chat.Show();
            parrentWindow.Hide();
        }
    }
}
