using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControlLibrary2.Elements
{
    /// <summary>
    /// Логика взаимодействия для TextBox1.xaml
    /// </summary>
    public partial class TextBox1 : UserControl
    {
        public string PlaceHolder = "PlaceHolder";
        public string XAMLPlaceholder { set { PlaceHolder = value; } }
        public Color BackgroundColor = WpfControlLibrary2.Resources.BackgroundColor;
        public Color OnMouseEnterColor = WpfControlLibrary2.Resources.OnMouseEnterColor;
        public TextBox1()
        {
            InitializeComponent();
        }
        public string GetText()
        {
            return Text.Text;
        }
        public void SetText(string Text)
        {
            this.Text.Text = Text;
        }
        public async Task ShowError(int Duration)
        {
            Border.BorderBrush = new SolidColorBrush(Color.FromRgb(255,0,0));
            OnMouseEnterColor = Color.FromRgb(255, 0, 0);
            BackgroundColor = Color.FromRgb(255, 0, 0);
            await Task.Delay(Duration*1000);
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
                Text.Text = "";
                Text.Foreground = Brushes.Black;
            }
            BackgroundOnEnter();
        }
        private void Text_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Text.Text.Trim().Equals(""))
            {
                Text.Foreground = Brushes.Gray;
                Text.Text = PlaceHolder;
            }
            BackgroundOnLeave();
        }
    }
}
