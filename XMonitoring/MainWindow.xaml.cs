using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using HDD;
using OSS;
using Seti;
using Komputer;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Management;
using System.Security.Principal;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Drawing;
using Image = System.Windows.Controls.Image;

namespace XMonitoring
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string ex;
        string l;
        string bufferpath = AppDomain.CurrentDomain.BaseDirectory + "\\LOG\\Буфер.txt";
        string OSBuffer = AppDomain.CurrentDomain.BaseDirectory + "\\LOG\\OSБуфер.txt";
        string DiskBuffer = AppDomain.CurrentDomain.BaseDirectory + "\\LOG\\DiskБуфер.txt";
        string SetiBuffer = AppDomain.CurrentDomain.BaseDirectory + "\\LOG\\SetiБуфер.txt";
        string KompBuffer = AppDomain.CurrentDomain.BaseDirectory + "\\LOG\\KompБуфер.txt";
        string buffer;
        private TaskbarIcon trayIcon;
        string m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11, m12, m13, m14, m15, m16, m17, m18, m19, m20, m21;
        public MainWindow()
        {
            InitializeComponent();
            if (!IsRunningAsAdmin())
            {
                Close();
                RunAsAdmin();
                return;
            }            
            trayIcon = new TaskbarIcon();
            trayIcon.Icon = Properties.Resources.Icon1;
            trayIcon.ToolTipText = "XMonitoring - сканирование системы";
            Hyperlink hyperlink = new Hyperlink();
            hyperlink.Inlines.Add("https://xfinity.kz/");
            hyperlink.NavigateUri = new System.Uri("https://xfinity.kz/");
            lab.Inlines.Add(hyperlink);
            labe.Content = "\u00A9 2023 XFinity Solutions LLP";
            trayIcon.TrayLeftMouseUp += TrayIcon_LeftMouseUp;
            trayIcon.TrayMouseDoubleClick += TrayIcon_MouseDoubleClick;
            label1.Content = Properties.Settings.Default.label1;
            label2.Content = Properties.Settings.Default.label2;
            label3.Content = Properties.Settings.Default.label3;
            label4.Content = Properties.Settings.Default.label4;
            TextBlock1.Content = Properties.Settings.Default.textblock1;
            TextBlock2.Content = Properties.Settings.Default.textblock2;
            TextBlock3.Content = Properties.Settings.Default.textblock3;
            TextBlock4.Content = Properties.Settings.Default.textblock4;
            TextBlock4.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom(Properties.Settings.Default.textblock4foreground));
            TextBlock3.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom(Properties.Settings.Default.textblock3foreground));
            TextBlock2.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom(Properties.Settings.Default.textblock2foreground));
            TextBlock1.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom(Properties.Settings.Default.textblock1foreground));
            TextBlock4.Margin = Properties.Settings.Default.textblock4margin;
            TextBlock3.Margin = Properties.Settings.Default.textblock3margin;
            TextBlock2.Margin = Properties.Settings.Default.textblock2margin;
            TextBlock1.Margin = Properties.Settings.Default.textblock1margin;
            labl1.Content = Properties.Settings.Default.labl1;
            lable1.Content = Properties.Settings.Default.lable1;
            labl2.Content = Properties.Settings.Default.labl2;
            lable2.Content = Properties.Settings.Default.lable2;
            checkbox.IsChecked = Properties.Settings.Default.checkbox;
            checkbox1.IsChecked = Properties.Settings.Default.checkbox1;
            labl3.Content = Properties.Settings.Default.labl3;
            labl4.Content = Properties.Settings.Default.labl4;
            SaveLabelText();
            OS os = new OS();
            os.Main();
            Avto();
            checkbox.Click += CheckBox_CheckedChanged;
            checkbox1.Click += CheckBox1_CheckedChanged;
        }

        static bool IsRunningAsAdmin()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        static void RunAsAdmin()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Verb = "runas";
            startInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;

            try
            {
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось запустить программу от имени администратора: " + ex.Message);
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkbox1.IsChecked == true)
            {
                Zapusk();
            }
            else
            {
                RemoveAppFromStartup();
            }
        }
        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (checkbox.IsChecked == true)
            {
                checkbox.IsChecked = true;
            }
            else
            {
                checkbox.IsChecked = false;
            }
        }
        private void TrayIcon_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
                    System.Windows.Application.Current.Shutdown();    
        }

        private void TrayIcon_LeftMouseUp(object sender, RoutedEventArgs e)
        {
            Show();
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            SaveLabelText();
            if (checkbox.IsChecked == true)
            {
                e.Cancel = true;
                this.Hide();
            }
            else
            {
                base.OnClosing(e);
            }
        }

        private void SaveLabelText()
        {
            Properties.Settings.Default.label1 = label1.Content.ToString();
            Properties.Settings.Default.label2 = label2.Content.ToString();
            Properties.Settings.Default.label3 = label3.Content.ToString();
            Properties.Settings.Default.label4 = label4.Content.ToString();
            Properties.Settings.Default.textblock1 = TextBlock1.Content.ToString();
            Properties.Settings.Default.textblock2 = TextBlock2.Content.ToString();
            Properties.Settings.Default.textblock3 = TextBlock3.Content.ToString();
            Properties.Settings.Default.textblock4 = TextBlock4.Content.ToString();
            Properties.Settings.Default.checkbox = (bool)checkbox.IsChecked;
            Properties.Settings.Default.checkbox1 = (bool)checkbox1.IsChecked;
            Properties.Settings.Default.textblock4foreground = TextBlock4.Foreground.ToString();
            Properties.Settings.Default.textblock3foreground = TextBlock3.Foreground.ToString();
            Properties.Settings.Default.textblock2foreground = TextBlock2.Foreground.ToString();
            Properties.Settings.Default.textblock1foreground = TextBlock1.Foreground.ToString();
            Properties.Settings.Default.textblock4margin = TextBlock4.Margin;
            Properties.Settings.Default.textblock3margin = TextBlock3.Margin;
            Properties.Settings.Default.textblock2margin = TextBlock2.Margin;
            Properties.Settings.Default.textblock1margin = TextBlock1.Margin;
            Properties.Settings.Default.lable1 = lable1.Content.ToString();
            Properties.Settings.Default.labl3 = labl3.Content.ToString();
            Properties.Settings.Default.lable2 = lable2.Content.ToString();
            Properties.Settings.Default.labl4 = labl4.Content.ToString();
            Properties.Settings.Default.labl1 = labl1.Content.ToString();
            Properties.Settings.Default.labl2 = labl2.Content.ToString();
            Properties.Settings.Default.Save();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
           NetworkSessionWatcher s = new NetworkSessionWatcher();
           (string[] internet, System.Windows.Media.Brush brush, Thickness thickness) = s.Internet();
            string[] returned = internet;
            TextBlock3.Content = returned[1];
            TextBlock3.Foreground = brush;
            TextBlock3.Margin = thickness;
            string[] adapt = s.Adapter();
            lable1.Content = adapt[0];
            TextBox2.Text = adapt[1] + "\r\n" + s.SessionWatcher() + "\r\n" + returned[0];
            if (adapt[2] == "Yes")
            {
                string newImagePath = "/image (6).png";
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(newImagePath, UriKind.RelativeOrAbsolute);
                bitmap.EndInit();
                myImage.Source = bitmap;
            }
            else
            {
                string newImagePath = "/image (9).png";
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(newImagePath, UriKind.RelativeOrAbsolute);
                bitmap.EndInit();
                myImage.Source = bitmap;
            }
            label3.Content = "Последняя проверка: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            labl3.Content = "Состояние на: " + DateTime.Now.ToString();
            File.WriteAllText(SetiBuffer, label3.Content + "\r\n" + TextBlock3.Content + "\r\n" + brush.ToString() + "\r\n" + $"{thickness.Left}, {thickness.Top}, {thickness.Right}, {thickness.Bottom}" + "\r\n" + lable1.Content + "\r\n" + TextBox2.Text);

            buffer = File.ReadAllText(OSBuffer) + "\r\n";
            buffer += File.ReadAllText(DiskBuffer) + "\r\n";
            buffer += File.ReadAllText(SetiBuffer) + "\r\n";
            buffer += File.ReadAllText(KompBuffer) + "\r\n";
            File.WriteAllText(bufferpath, buffer);
            m9 = (string)label3.Content;
            m10 = (string)TextBlock3.Content;
            m11 = brush.ToString();
            m12 = $"{thickness.Left}, {thickness.Top}, {thickness.Right}, {thickness.Bottom}";
            m13 = (string)lable1.Content;
            m14 = TextBox2.Text;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            IsFirstTimeRun();
            Komp k = new Komp();
            (string[] smart, System.Windows.Media.Brush brush, Thickness thickness) = k.SaveConfig();
            string[] returned = smart;
            TextBlock4.Content = returned[1];
            TextBlock4.Foreground = brush;
            TextBlock4.Margin = thickness;
            TextBox4.Text = returned[0];
            lable2.Content = returned[3];
            if (returned[4] == null)
                izm.Text = "Изменения комплектующих компьютера отсутствуют";
            else
                izm.Text = returned[4];
            label4.Content = "Последняя проверка: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            labl4.Content = "Состояние на: " + DateTime.Now.ToString();
            File.WriteAllText(KompBuffer, label4.Content + "\r\n" + TextBlock4.Content + "\r\n" + brush.ToString() + "\r\n" + $"{thickness.Left}, {thickness.Top}, {thickness.Right}, {thickness.Bottom}" + "\r\n" + lable2.Content + "\r\n" + TextBox4.Text + "\r\n" + izm.Text);

            buffer = File.ReadAllText(OSBuffer) + "\r\n";
            buffer += File.ReadAllText(DiskBuffer) + "\r\n";
            buffer += File.ReadAllText(SetiBuffer) + "\r\n";
            buffer += File.ReadAllText(KompBuffer) + "\r\n";
            File.WriteAllText(bufferpath, buffer);
            m15 = (string)label4.Content;
            m16 = (string)TextBlock4.Content;
            m17 = brush.ToString();
            m18 = $"{thickness.Left}, {thickness.Top}, {thickness.Right}, {thickness.Bottom}";
            m19 = (string)lable2.Content;
            m20 = TextBox4.Text;
            m21 = izm.Text;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OS os = new OS();
            (string[] smart, System.Windows.Media.Brush brush, Thickness thickness) = os.ChangeOS();
            string[] returned = smart;
            TextBox1.Text = returned[0];
            TextBlock1.Content = returned[1];
            TextBlock1.Foreground = brush;
            TextBlock1.Margin = thickness;
            if (returned[3] == "Yes")
            {
                string newImagePath = "/image (6).png";
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(newImagePath, UriKind.RelativeOrAbsolute);
                bitmap.EndInit();
                imageOS.Source = bitmap;
            }
            else
            {
                string newImagePath = "/image (9).png";
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(newImagePath, UriKind.RelativeOrAbsolute);
                bitmap.EndInit();
                imageOS.Source = bitmap;
            }
            label1.Content = "Последняя проверка: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"); 
            labl1.Content = "Состояние на: " + DateTime.Now.ToString();
            File.WriteAllText(OSBuffer, label1.Content + "\r\n" + TextBlock1.Content + "\r\n" + brush.ToString() + "\r\n" + $"{thickness.Left}, {thickness.Top}, {thickness.Right}, {thickness.Bottom}");

            buffer = File.ReadAllText(OSBuffer) + "\r\n";
            buffer += File.ReadAllText(DiskBuffer) + "\r\n";
            buffer += File.ReadAllText(SetiBuffer) + "\r\n";
            buffer += File.ReadAllText(KompBuffer) + "\r\n";
            File.WriteAllText(bufferpath, buffer);
            m1 = (string)label1.Content;
            m2 = (string)TextBlock1.Content;
            m3 = brush.ToString();
            m4 = $"{thickness.Left}, {thickness.Top}, {thickness.Right}, {thickness.Bottom}";
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            LoadDiskImages();
            HDD1 hdd = new HDD1();
            (string[] smart, System.Windows.Media.Brush brush, Thickness thickness) = hdd.SMART();
            string[] returned = smart;
            TextBlock2.Content = returned[1];
            TextBlock2.Foreground = brush;
            TextBox3.Text = returned[0];
            TextBlock2.Margin = thickness;
            label2.Content = "Последняя проверка: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            labl2.Content = "Состояние на: " + DateTime.Now.ToString();
            File.WriteAllText(DiskBuffer, label2.Content + "\r\n" + TextBlock2.Content + "\r\n" + brush.ToString() + "\r\n" + $"{thickness.Left}, {thickness.Top}, {thickness.Right}, {thickness.Bottom}");

            buffer = File.ReadAllText(OSBuffer) + "\r\n";
            buffer += File.ReadAllText(DiskBuffer) + "\r\n";
            buffer += File.ReadAllText(SetiBuffer) + "\r\n";
            buffer += File.ReadAllText(KompBuffer) + "\r\n";
            File.WriteAllText(bufferpath, buffer);
            m5 = (string)label2.Content;
            m6 = (string)TextBlock2.Content;
            m7 = brush.ToString();
            m8 = $"{thickness.Left}, {thickness.Top}, {thickness.Right}, {thickness.Bottom}";
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 1;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 2;
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 3;
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 4;
        }

        private void Button_OS(object sender, RoutedEventArgs e)
        {
            Process.Start(AppDomain.CurrentDomain.BaseDirectory + @"\LOG\OSLOG.txt");
        }

        private void Avto()
        {
            Button_Click(null, null);
            Button_Click_1(null, null);
            Button_Click_2(null, null);
            Button_Click_3(null, null);
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            Button_Click(sender, e);
            Button_Click_1(sender, e);
            Button_Click_2(sender, e);
            Button_Click_3(sender, e);
        }

        static void Zapusk()
        {
            if (!IsAppInStartup())
            { 
                AddAppToStartup();
            }
        }

        static bool IsAppInStartup()
        {
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                string value = (string)key.GetValue("XMonitoring");
                if (value != null && value.Equals(exePath, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        static void AddAppToStartup()
        {
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue("XMonitoring", exePath);
            }
        }

        public void IsFirstTimeRun()
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\first_run.flag";

            if (!File.Exists(filePath))
            {
                Komp k = new Komp();
                (string[] smart, System.Windows.Media.Brush brush, Thickness thickness) = k.SaveConfig();
                string[] returned = smart;
                string path = AppDomain.CurrentDomain.BaseDirectory + @"\LOG\Log.txt";
                File.WriteAllText(path, returned[3] + returned[0]);
                Button_Click_3(null, null);
                File.Create(filePath);
            }
        }

        static void RemoveAppFromStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.DeleteValue("XMonitoring", false);
            }
        }


        private void LoadDiskImages()
        {
            imageContainer.Children.Clear();
            DriveInfo[] drives = DriveInfo.GetDrives();
            string icon = "";
            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady)
                {
                    Image image = new Image();
                    image.Width = 146;
                    image.Height = 164;
                    ManagementScope scope = new ManagementScope(@"\\.\root\microsoft\windows\storage");
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM MSFT_PhysicalDisk");
                    scope.Connect();
                    searcher.Scope = scope;

                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        switch (Convert.ToInt16(queryObj["MediaType"]))
                        {
                            case 1:
                                icon = "image (1).png";
                                break;

                            case 3:
                                icon = "image (1).png";
                                break;

                            case 4:
                                icon = "image (7).png";
                                break;

                            case 5:
                                icon = "image (1).png";
                                break;

                            default:
                                icon = "image (1).png";
                                break;
                        }
                    }
                    searcher.Dispose();
                    BitmapImage bitmapImage = new BitmapImage(new Uri(icon, UriKind.RelativeOrAbsolute));
                    image.Source = bitmapImage;
                    imageContainer.Children.Add(image);
                }
            }
        }
    }
}

