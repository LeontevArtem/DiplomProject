using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для SidePanel1.xaml
    /// </summary>
    public partial class SidePanel1 : UserControl
    {
        public enum PanelOrientation
        {
            left,
            right
        }
        public enum PanelMode
        {
            open,
            close
        }
        public Color BackgroundColor = Color.FromRgb(255, 255, 255);
        public PanelOrientation orientation;
        PanelMode mode = PanelMode.close;
        int ControlPanelCount = 0;
        public SidePanel1(PanelOrientation orientation)
        {
            InitializeComponent();
            if (orientation == PanelOrientation.right)
            {
                menuicon.HorizontalAlignment = HorizontalAlignment.Right;
                ControlPanelBorder.HorizontalAlignment = HorizontalAlignment.Right;
                ControlPanelBorder.Margin = new Thickness(5, 5, -200, 5);
            }
            this.orientation = orientation;
        }
        public SidePanel1()
        {
            InitializeComponent();
            this.orientation = PanelOrientation.left;
            this.mode = PanelMode.close;
        }


        public void SetBackground(Color BackgroundColor)
        {
            SolidColorBrush BackgroundBrush = new SolidColorBrush(BackgroundColor);
            ControlPanelBorder.Background = BackgroundBrush;
            menuicon.Background = BackgroundBrush;
        }
        public void AddChildren(System.Windows.UIElement uIElement)
        {
            Children.Children.Add(uIElement);
        }
        private void menuicon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SwitchControlPanel();
        }
        public void SwitchImage(PanelMode mode)
        {
            if (mode == PanelMode.open)
            {
                DoubleAnimation opacityAnimation = new DoubleAnimation();
                opacityAnimation.From = 1;
                opacityAnimation.To = 0;
                opacityAnimation.Duration = TimeSpan.FromSeconds(0.1);
                opacityAnimation.Completed += delegate
                {
                    DoubleAnimation opacityAnimation2 = new DoubleAnimation();
                    opacityAnimation2.From = 0;
                    opacityAnimation2.To = 0.75;
                    opacityAnimation2.Duration = TimeSpan.FromSeconds(0.1);
                    menuicon.BeginAnimation(Frame.OpacityProperty, opacityAnimation2);
                    IconImg.Source = new BitmapImage(new Uri($"{Directory.GetCurrentDirectory()}\\..\\..\\WpfControlLibrary2\\Images\\cross.png"));
                };
                menuicon.BeginAnimation(Frame.OpacityProperty, opacityAnimation);
                
            }
            else
            {
                DoubleAnimation opacityAnimation = new DoubleAnimation();
                opacityAnimation.From = 1;
                opacityAnimation.To = 0;
                opacityAnimation.Duration = TimeSpan.FromSeconds(0.1);
                opacityAnimation.Completed += delegate
                {
                    DoubleAnimation opacityAnimation2 = new DoubleAnimation();
                    opacityAnimation2.From = 0;
                    opacityAnimation2.To = 1;
                    opacityAnimation2.Duration = TimeSpan.FromSeconds(0.1);
                    menuicon.BeginAnimation(Frame.OpacityProperty, opacityAnimation2);
                    IconImg.Source = new BitmapImage(new Uri($"{Directory.GetCurrentDirectory()}\\..\\..\\WpfControlLibrary2\\Images\\menuicon.png"));
                };
                menuicon.BeginAnimation(Frame.OpacityProperty, opacityAnimation);
                
            }
        }
        public void SwitchControlPanel()
        {
            ThicknessAnimation OpenAnimationLeft = new ThicknessAnimation();
            OpenAnimationLeft.From = new Thickness(ControlPanelBorder.Margin.Left, ControlPanelBorder.Margin.Top, ControlPanelBorder.Margin.Right, ControlPanelBorder.Margin.Bottom);
            OpenAnimationLeft.To = new Thickness(5, ControlPanelBorder.Margin.Top, ControlPanelBorder.Margin.Right, ControlPanelBorder.Margin.Bottom);
            OpenAnimationLeft.Duration = TimeSpan.FromSeconds(0.3);

            ThicknessAnimation CloseAnimationLeft = new ThicknessAnimation();
            CloseAnimationLeft.From = new Thickness(ControlPanelBorder.Margin.Left, ControlPanelBorder.Margin.Top, ControlPanelBorder.Margin.Right, ControlPanelBorder.Margin.Bottom);
            CloseAnimationLeft.To = new Thickness(-200, ControlPanelBorder.Margin.Top, ControlPanelBorder.Margin.Right, ControlPanelBorder.Margin.Bottom);
            CloseAnimationLeft.Duration = TimeSpan.FromSeconds(0.3);
            SwitchImage(mode);
            if (mode == PanelMode.open)
            {
                if (orientation == PanelOrientation.right)
                {
                    
                }
                else
                {
                    OpenAnimationLeft.BeginAnimation(MarginProperty, OpenAnimationLeft);
                    ControlPanelBorder.BeginAnimation(MarginProperty, OpenAnimationLeft);
                    mode = PanelMode.close;
                }
            }
            else if (mode == PanelMode.close)
            {
                if (orientation == PanelOrientation.right)
                {

                }
                else
                {
                    CloseAnimationLeft.BeginAnimation(MarginProperty, OpenAnimationLeft);
                    ControlPanelBorder.BeginAnimation(MarginProperty, CloseAnimationLeft);
                    mode = PanelMode.open;
                }
            }
        }
    }
}
