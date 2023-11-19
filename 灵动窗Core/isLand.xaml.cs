using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace 灵动窗Core
{
    /// <summary>
    /// isLand.xaml 的交互逻辑
    /// </summary>
    public partial class isLand : Window
    {
        private DoubleAnimation opacityAnimation;
        private DoubleAnimation widthAnimation;

        bool broder_s = false;
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

        bool isLabelVisible = false;
        public async Task ShowContent(double AnimationTime, int ShowTime, string body, Image icon, Brush color)
        {
            double oldhei = BorderWindow.Height;
            double oldwid = BorderWindow.Width;
            ElasticEase elasticEase = new ElasticEase
            {
                Oscillations = 1, // Number of oscillations
                Springiness = 10 // The "springiness" of the animation
            };
            DoubleAnimation ContantHeight = new DoubleAnimation
            {
                Duration = TimeSpan.FromSeconds(AnimationTime),
                From = oldhei,
                To = 0.3,
                EasingFunction = elasticEase // Apply the ElasticEase
            };
            DoubleAnimation ContantWidth = new DoubleAnimation
            {
                Duration = TimeSpan.FromSeconds(AnimationTime),
                From = oldwid,
                To = 0.3,
                EasingFunction = elasticEase // Apply the ElasticEase
            };

            ContantHeight.To = 110;
            ContantWidth.To = 500;
            BorderWindow.BeginAnimation(Border.HeightProperty, ContantHeight);
            BorderWindow.BeginAnimation(Border.WidthProperty, ContantWidth);
            isLabelVisible = true;
            Label newLabel = new Label
            {
                Content = body,
                FontSize = 16,
                Margin = new Thickness(20),
                Foreground = color
            };

            BorderBody.Children.Add(newLabel);

            async Task DelayedAnimation()
            {
                if (isLabelVisible)
                {
                    await Task.Delay(ShowTime * 1000);
                    ContantHeight.To = oldhei;
                    ContantWidth.To = oldwid;
                    ContantHeight.From = 110;
                    ContantWidth.From = 500;
                    BorderWindow.BeginAnimation(Border.HeightProperty, ContantHeight);
                    BorderWindow.BeginAnimation(Border.WidthProperty, ContantWidth);
                    BorderBody.Children.Remove(newLabel);
                    isLabelVisible = false;
                }
            }

            var delayedTask = DelayedAnimation();

            BorderWindow.MouseDown += async (sender, e) =>
            {
                if (isLabelVisible && e.ChangedButton == MouseButton.Left)
                {
                    ContantHeight.To = oldhei;
                    ContantWidth.To = oldwid;
                    ContantHeight.From = 110;
                    ContantWidth.From = 500;
                    isLabelVisible = false;
                    BorderWindow.BeginAnimation(Border.HeightProperty, ContantHeight);
                    BorderWindow.BeginAnimation(Border.WidthProperty, ContantWidth);
                    BorderBody.Children.Remove(newLabel);
                    await Task.Delay(800); // 等待0.8秒，与之前回弹动画的时间相匹配
                    await delayedTask; // 等待原本的延迟动画完成
                }
            };
            await delayedTask;
        }


        private void InitializeAnimations()
        {
            opacityAnimation = new DoubleAnimation
            {
                Duration = TimeSpan.FromSeconds(0.2),
                From = 1.0,
                To = 0.5
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
            opacityAnimation.To = 0.5;
            opacityAnimation.From = 1.0;
            BorderWindow.BeginAnimation(Border.OpacityProperty, opacityAnimation);
        }
        private void BorderWindow_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            opacityAnimation.To = 1.0;
            opacityAnimation.From = 0.5;
            BorderWindow.BeginAnimation(Border.OpacityProperty, opacityAnimation);
        }

        private void BorderWindow_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!isLabelVisible)
            {
                if (BorderWindow.Width == aawidth)
                {
                    widthAnimation.From = aawidth;
                    widthAnimation.To = 35;
                    broder_s = false;
                }
                else
                {
                    widthAnimation.From = 35;
                    widthAnimation.To = aawidth;
                    broder_s = true;
                }
                BorderWindow.BeginAnimation(Border.WidthProperty, widthAnimation);
            }


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Top = 20;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            //禁止关闭
        }
    }
}
