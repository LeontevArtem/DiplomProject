using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace WpfControlLibrary2.Elements
{
    /// <summary>
    /// Логика взаимодействия для Switch1.xaml
    /// </summary>
    public partial class Switch1 : UserControl
    {
        public Switch1()
        {
            InitializeComponent();
            StartPos = SwitchBox.Margin.Left;
            EndPos = this.Width - SwitchBox.Width - 5;
        }
        public double StartPos;
        public double EndPos;
        public int count = 0;
        public bool state = false;
        public void Switch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            EndPos = this.Width - SwitchBox.Width - 5;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(0.1);
            timer.Tick += delegate { MoveSwitch(); };
            timer.Start();
            if (count % 2 == 1)
            {
                ColorAnimation animation;
                animation = new ColorAnimation();
                animation.To = Color.FromRgb(25, 183, 25);
                animation.Duration = new Duration(TimeSpan.FromSeconds(0.2));
                this.BackgroundBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                state = true;
            }
            else if (count % 2 == 0)
            {
                ColorAnimation animation;
                animation = new ColorAnimation();
                animation.To = Color.FromRgb(183, 25, 25);
                animation.Duration = new Duration(TimeSpan.FromSeconds(0.2));
                this.BackgroundBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                state = false;
            }
            count++;
        }
        public void SetBackground(Color BackgroundColor)
        {
            SolidColorBrush BackgroundBrush = new SolidColorBrush(BackgroundColor);
            BackgroundBorder.Background = BackgroundBrush;
        }
        public void MoveSwitch()
        {
            if (count % 2 == 0)
            {
                if (SwitchBox.Margin.Left < EndPos)
                {
                    SwitchBox.Margin = new Thickness(SwitchBox.Margin.Left + 0.5, SwitchBox.Margin.Top, SwitchBox.Margin.Right, SwitchBox.Margin.Bottom);
                }
            }
            else if (count % 2 == 1)
            {
                if (SwitchBox.Margin.Left > StartPos)
                {
                    SwitchBox.Margin = new Thickness(SwitchBox.Margin.Left - 0.5, SwitchBox.Margin.Top, SwitchBox.Margin.Right, SwitchBox.Margin.Bottom);
                }
            }
        }
    }
}
