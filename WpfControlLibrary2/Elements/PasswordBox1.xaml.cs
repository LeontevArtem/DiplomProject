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

namespace WpfControlLibrary2.Elements
{
    /// <summary>
    /// Логика взаимодействия для PasswordBox1.xaml
    /// </summary>
    public partial class PasswordBox1 : UserControl
    {
        public PasswordBox1()
        {
            InitializeComponent();
        }
        private Color BackgroundColor = WpfControlLibrary2.Resources.BackgroundColor;
        private Color OnMouseEnterColor = WpfControlLibrary2.Resources.OnMouseEnterColor;
        public string GetText()
        {
            return Text.Password;
        }
        public async Task ShowError(int Duration)
        {
            Border.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            OnMouseEnterColor = Color.FromRgb(255, 0, 0);
            BackgroundColor = Color.FromRgb(255, 0, 0);
            await Task.Delay(Duration * 1000);
            Border.BorderBrush = new SolidColorBrush(BackgroundColor);
            BackgroundColor = WpfControlLibrary2.Resources.BackgroundColor;
            OnMouseEnterColor = WpfControlLibrary2.Resources.OnMouseEnterColor;
            Border.BorderBrush = new SolidColorBrush(BackgroundColor);
        }
        public void BackgroundOnEnter()
        {
            SolidColorBrush BackgroundBrush = new SolidColorBrush(OnMouseEnterColor);
            Border.BorderBrush = BackgroundBrush;
            Border.BorderThickness = new Thickness(2.5, 2.5, 2.5, 2.5);
        }
        public void BackgroundOnLeave()
        {
            SolidColorBrush BackgroundBrush = new SolidColorBrush(BackgroundColor);
            Border.BorderBrush = BackgroundBrush;
            Border.BorderThickness = new Thickness(2, 2, 2, 2);
        }
        private void Border_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            BackgroundOnEnter();
        }
        private void Border_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            BackgroundOnLeave();
        }
        private void Text_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Text.Foreground == Brushes.Gray)
            {
                Text.Foreground = Brushes.Black;
            }
            BackgroundOnEnter();
        }
        private void Text_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Text.Password.Trim().Equals(""))
            {
                Text.Foreground = Brushes.Gray;
            }
            BackgroundOnLeave();
        }
    }
}
