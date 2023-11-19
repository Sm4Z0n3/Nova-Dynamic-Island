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
using System.Drawing;
using Brushes = System.Windows.Media.Brushes;
using System.Runtime.CompilerServices;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
//using ModernFlyouts.Core;

namespace 灵动窗Core
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private isLand isLandWindow = new isLand();
        double widthA = 120;
        double heightA = 35;
        double radiusA = 15;
        double topA = 20;
        private TaskbarIcon notifyIcon;

        public MainWindow()
        {
            InitializeComponent();
            isLandWindow.Show();
            CreateNotifyIcon();
            Closing += MainWindow_Closing;
            StartUP.IsChecked = IsStartupEnabled();
        }
        private void CreateNotifyIcon()
        {
            notifyIcon = new TaskbarIcon
            {
                Icon = System.Drawing.SystemIcons.Application,
                ToolTipText = "Nova灵动窗 | 单击打开设置 | 双击关闭程序\nhttp://*:24305/ API监听中..."
            };
            notifyIcon.TrayMouseDoubleClick += NotifyIcon_DoubleClick;
            notifyIcon.TrayLeftMouseUp += NotifyIcon_Click;
        }
        #region 事件
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
                new BitmapImage(),
                Brushes.White,
                16
                );
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Window_Width.Value = 120;
            Window_Height.Value = 35;
            Window_Radius.Value = 15;
            Window_Top.Value = 20;
        }
        #endregion
        static void SetStartup(bool addToStartup)
        {
            try
            {
                string appName = Process.GetCurrentProcess().ProcessName;
                string appPath = Process.GetCurrentProcess().MainModule.FileName;

                RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                if (addToStartup)
                {
                    rk.SetValue(appName, appPath);
                    Console.WriteLine("Application added to startup.");
                }
                else
                {
                    rk.DeleteValue(appName, false);
                    Console.WriteLine("Application removed from startup.");
                }

                rk.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error setting startup registry: " + ex.Message);
            }
        }
        static bool IsStartupEnabled()
        {
            string appName = Process.GetCurrentProcess().ProcessName;
           // MessageBox.Show(Directory.GetCurrentDirectory() + "\\"+ appName + ".exe");
            try
            {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                object value = rk.GetValue(appName);

                rk.Close();

                return value != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error checking startup registry: " + ex.Message);
                return false;
            }
        }

        private void StartUP_Checked(object sender, RoutedEventArgs e)
        {
            SetStartup((bool)StartUP.IsChecked);
        }
    }


}
