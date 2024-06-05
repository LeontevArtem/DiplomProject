using ScreenLocker.Common.Classes;
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
    /// Логика взаимодействия для Chat.xaml
    /// </summary>
    public partial class Chat : Window
    {
        MainWindow mainWindow;
        User Companion;
        ChatList parrentWindow;
        public Chat(MainWindow mainWindow,User Companion, ChatList parrentWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.parrentWindow = parrentWindow;
            this.Companion = Companion;
            LoadChat();
        }
        public void LoadChat()
        {

            parrent.Children.Clear();
            foreach (Common.Classes.Message message in MainWindow.messages.Where(x=>(x.From.id == Companion.id && x.To.id == MainWindow.CurrentSession.User.id) || (x.To.id == Companion.id && x.From.id == MainWindow.CurrentSession.User.id)))
            {
                UIElements.MessageItem messageElement = new UIElements.MessageItem(message);
                if (message.From.id == MainWindow.CurrentSession.User.id) messageElement.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                else messageElement.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                parrent.Children.Add(messageElement);
                this.ScrollViewerContainer.ScrollToEnd();
            }
        }

        private void Sent_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MsSQL.Query($"INSERT INTO [dbo].[Message]([FromID],[ToID],[MessageText],[Tag],[Date])VALUES('{MainWindow.CurrentSession.User.id}','{Companion.id}','{MessageText.GetText()}','Message','{DateTime.Now}')", MainWindow.ConnectionString);
            MainWindow.CurrentSession.WriteLogToDataBase($"Пользователем {MainWindow.CurrentSession.User.firstname} отправлено сообщение пользователю {Companion.firstname}",Session.LogTag.Message);
            mainWindow.LoadData();
            LoadChat();
        }

        private void Back_MouseDown(object sender, MouseButtonEventArgs e)
        {
            parrentWindow.Show();
            this.Close();
        }
    }
}
