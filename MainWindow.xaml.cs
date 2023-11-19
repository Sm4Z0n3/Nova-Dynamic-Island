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
using System.Windows.Forms;
using System.Drawing;
using System.Management;
using Brushes = System.Windows.Media.Brushes;

namespace 灵动窗Framework
{
    /// <summary>
    /// Interaction logic for MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        private isLand isLandWindow = new isLand();
        double widthA = 120;
        double heightA = 35;
        double radiusA = 15;
        double topA = 20;
        private NotifyIcon notifyIcon;

        public MainWindow()
        {
            InitializeComponent();
            isLandWindow.Show();
            CreateNotifyIcon();
            Closing += MainWindow_Closing;
            //Color backgroundColor = SystemParameters.WindowGlassColor;
            //this.Background = new SolidColorBrush(backgroundColor);
        }
        private void CreateNotifyIcon()
        {
            notifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application,
                Visible = true,
                Text = "Nova灵动窗 | 单击打开设置 | 双击关闭程序"
            };
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            notifyIcon.MouseClick += NotifyIcon_Click;
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
        private void Window_Width_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            widthA = this.Window_Width.Value;
            isLandWindow.SetWidth(widthA);
        }
        private void Window_Radius_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            radiusA = this.Window_Radius.Value;
            isLandWindow.SetRadius(radiusA);
        }
        private void Window_Height_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            heightA = this.Window_Height.Value;
            isLandWindow.SetHeight(heightA);
        }
        private void Window_Top_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            topA = this.Window_Top.Value;
            isLandWindow.SetTop(topA);
        }

        private async void Test_Content_Click(object sender, RoutedEventArgs e)
        {
           await isLandWindow.ShowContent(
               0.6,
               5,
               "你好世界",
               new System.Windows.Controls.Image(),
               Brushes.White
               );
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Window_Width.Value = 120;
            Window_Height.Value = 35;
            Window_Radius.Value = 15;
            Window_Top.Value = 20;
        }
    }


}
