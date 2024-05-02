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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfControlLibrary2.Elements
{
    /// <summary>
    /// Логика взаимодействия для Button1.xaml
    /// </summary>
    public partial class Button1 : UserControl
    {
        //public SolidColorBrush TextColor { get { return TextColor; } set { Text.Foreground = value; } }
        public Color BackgroundColor = WpfControlLibrary2.Resources.BackgroundColor;
        public Color XAMLBackgroundColor {  set { border.Background = new SolidColorBrush(value); BackgroundColor = value; } }
        public Color OnMouseEnterColor = WpfControlLibrary2.Resources.OnMouseEnterColor;
        public Color XAMLOnMouseEnterColor { set { OnMouseEnterColor = value; } }
        public Color XAMLTextColor { set { Text.Foreground = new SolidColorBrush(value); } }
        
        public string XAMLText { set { Text.Text = value; } }
        public Button1()
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
        public void SetBackgroundColor(Color Color)
        {
            border.Background = new SolidColorBrush(Color);
        }
        private void border_MouseEnter(object sender, MouseEventArgs e)
        {
            ColorAnimation animation;
            animation = new ColorAnimation();
            animation.From = BackgroundColor;
            animation.To = OnMouseEnterColor;
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.2));
            this.border.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }
        private void border_MouseLeave(object sender, MouseEventArgs e)
        {
            ColorAnimation animation;
            animation = new ColorAnimation();
            animation.From = OnMouseEnterColor;
            animation.To = BackgroundColor;
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.2));
            this.border.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }
    }
}
