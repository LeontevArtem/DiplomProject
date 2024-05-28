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
    /// Логика взаимодействия для MessageItem.xaml
    /// </summary>
    public partial class MessageItem : System.Windows.Controls.UserControl
    {
        public MessageItem(Common.Classes.Message message)
        {
            InitializeComponent();
            this.SenterName.Content = message.From.firstname;
            this.MessageText.Text = message.MessageText;
        }
    }
}
