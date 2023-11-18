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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace 灵动窗Framework
{
    /// <summary>
    /// isLand.xaml 的交互逻辑
    /// </summary>
    public partial class isLand : Window
    {
        private DoubleAnimation opacityAnimation;
        private DoubleAnimation widthAnimation;

        public isLand()
        {
            InitializeComponent();
            InitializeAnimations();
            BorderWindow.MouseEnter += BorderWindow_MouseEnter;
            BorderWindow.MouseLeave += BorderWindow_MouseLeave;
            BorderWindow.MouseRightButtonUp += BorderWindow_MouseRightButtonUp;
        }
        double aawidth = 120;
        public void SetHeight(double height)
        {
            BorderWindow.Height = height;
        }
        public void SetWidth(double width)
        {
            BorderWindow.Width = width;
            aawidth = width;
        }
        public void SetRadius(double cornerRadius)
        {
            BorderWindow.CornerRadius = new CornerRadius(cornerRadius);
        }
        public void SetTop(double top)
        {
            this.Top = top;
        }
        private void InitializeAnimations()
        {
            opacityAnimation = new DoubleAnimation
            {
                Duration = TimeSpan.FromSeconds(0.2),
                From = 1.0,
                To = 0.3
            };

            widthAnimation = new DoubleAnimation
            {
                Duration = TimeSpan.FromSeconds(0.3),
                From = 100,
                To = 110
            };
        }

        private void BorderWindow_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            opacityAnimation.To = 0.3;
            opacityAnimation.From = 1.0;
            BorderWindow.BeginAnimation(Border.OpacityProperty, opacityAnimation);
        }

        private void BorderWindow_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            opacityAnimation.To = 1.0;
            opacityAnimation.From = 0.3;
            BorderWindow.BeginAnimation(Border.OpacityProperty, opacityAnimation);
        }

        private void BorderWindow_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (BorderWindow.Width == aawidth)
            {
                widthAnimation.From = aawidth;
                widthAnimation.To = 35;
            }
            else
            {
                widthAnimation.From = 35;
                widthAnimation.To = aawidth;
            }

            BorderWindow.BeginAnimation(Border.WidthProperty, widthAnimation);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Top = 20;
        }


    }
}
