using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
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

        public isLand()
        {
            InitializeComponent();
            InitializeAnimations();
            BorderWindow.MouseEnter += BorderWindow_MouseEnter;
            BorderWindow.MouseLeave += BorderWindow_MouseLeave;
            BorderWindow.MouseRightButtonUp += BorderWindow_MouseRightButtonUp;
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await StartServerAsync();
        }

        #region 供调用的函数
        double aawidth = 20;
        double aaheight = 20;
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
        public void SetHeight(double height)
        {
            BorderWindow.Height = height;
            aaheight = height;
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
        public async Task ShowContent(double AnimationTime, int ShowTime, string body, BitmapImage icon, Brush color,int FSize)
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
                FontSize = FSize,
                Margin = new Thickness(20),
                Foreground = color,
                Height = 110
            };

            Image newImage = new Image
            {
                Source = icon,
                Width = 88,
                VerticalAlignment = VerticalAlignment.Center
            };

            StackPanel stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(20)
            };

            stackPanel.Children.Add(newImage);
            stackPanel.Children.Add(newLabel);
            BorderBody.Children.Add(stackPanel);

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
                    BorderBody.Children.Remove(stackPanel);
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
        #endregion

        #region 事件

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
                }
                else
                {
                    widthAnimation.From = 35;
                    widthAnimation.To = aawidth;
                }
                // 创建一个 BounceEase 对象
                BounceEase bounce = new BounceEase();
                // 设置缓动模式为 EaseOut，表示动画在结束时回弹
                bounce.EasingMode = EasingMode.EaseOut;
                // 设置回弹次数为 3
                bounce.Bounces = 3;
                // 设置回弹强度为 2，值越大，回弹越小
                bounce.Bounciness = 5;
                // 将 BounceEase 对象赋值给动画的 EasingFunction 属性
                widthAnimation.EasingFunction = bounce;
                BorderWindow.BeginAnimation(Border.WidthProperty, widthAnimation);
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Top = 14;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            //禁止关闭
        }
        #endregion

        //http://localhost:24305/
        #region API接口
        public async Task StartServerAsync()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:24305/");
            listener.Start();

            Console.WriteLine("Server listening on port 24305...");

            while (true)
            {
                HttpListenerContext context = await listener.GetContextAsync();
                ProcessRequest(context);
            }
        }

        public async void ProcessRequest(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;

            try
            {
                if (request.HttpMethod == "POST")
                {
                    using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        string requestBody = await reader.ReadToEndAsync();
                        Console.WriteLine("Received POST data: " + requestBody);

                        var postParams = ParseQueryString(requestBody);

                        if (postParams.Get("ShowTime") != null &&
                            postParams.Get("AnimationTime") != null &&
                            postParams.Get("body") != null &&
                            postParams.Get("icon") != null &&
                            postParams.Get("color") != null &&
                            postParams.Get("fsize") != null)
                        {
                            int showTime = int.Parse(postParams.Get("ShowTime"));
                            int FSize = int.Parse(postParams.Get("fsize"));
                            double animationTime = double.Parse(postParams.Get("AnimationTime"));
                            string base64String = postParams.Get("icon").Split(',')[1];

                            BitmapImage bitmapImage = new BitmapImage();
                            byte[] bytes = Convert.FromBase64String(base64String);
                            using (MemoryStream stream = new MemoryStream(bytes))
                            {
                                bitmapImage.BeginInit();
                                bitmapImage.StreamSource = stream;
                                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                                bitmapImage.EndInit();
                            }

                            Color color = (Color)ColorConverter.ConvertFromString(postParams.Get("color"));
                            SolidColorBrush brush = new SolidColorBrush(color);

                            await ShowContent(animationTime, showTime, postParams.Get("body"), bitmapImage, brush, FSize);

                            HttpListenerResponse response = context.Response;
                            string responseString = "200";
                            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                            response.ContentLength64 = buffer.Length;
                            response.OutputStream.Write(buffer, 0, buffer.Length);
                            response.OutputStream.Close();
                        }
                    }
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public NameValueCollection ParseQueryString(string queryString)
        {
            NameValueCollection queryParameters = new NameValueCollection();

            foreach (var kvp in queryString.Split('&'))
            {
                var keyValue = kvp.Split('=');
                if (keyValue.Length == 2)
                {
                    queryParameters[keyValue[0]] = System.Net.WebUtility.UrlDecode(keyValue[1]);
                }
            }

            return queryParameters;
        }
        #endregion
    }
}
