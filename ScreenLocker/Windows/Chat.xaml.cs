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
        public Chat(MainWindow mainWindow,User Companion)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.Companion = Companion;
            LoadChat();
        }
        public void LoadChat()
        {
            
            
            foreach (Common.Classes.Message message in MainWindow.messages.Where(x=>x.From.id == Companion.id))
            {
                UIElements.MessageItem messageElement = new UIElements.MessageItem(message);
                if (message.From.id == MainWindow.CurrentSession.User.id) messageElement.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                else messageElement.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                parrent.Children.Add(messageElement);
            }
        }

        private void Sent_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MsSQL.Query($"INSERT INTO [dbo].[Message]([FromID],[ToID],[MessageText],[Tag],[Date])VALUES('{MainWindow.CurrentSession.User.id}','{Companion.id}','{MessageText.GetText()}','Message','{DateTime.Now}')", MainWindow.ConnectionString);
            MainWindow.CurrentSession.WriteLogToDataBase($"Пользователем {MainWindow.CurrentSession.User.firstname} отправлено сообщение пользователю {Companion.firstname}",Session.LogTag.Message);
        }
    }
}
