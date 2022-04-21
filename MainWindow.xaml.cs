using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using KMCCC.Launcher;
using KMCCC.Authentication;


namespace Quantum
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Version version = new Version();
        LoginUI.Offline offline = new LoginUI.Offline();
        Setting setting = new Setting();
        LoginUI.Online online = new LoginUI.Online();
        About about = new About();
        public int launchMode = 1;
        public static LauncherCore Core = LauncherCore.Create();


        public MainWindow()
        {
            InitializeComponent();
            ServicePointManager.DefaultConnectionLimit = 512;
        }

        public void GameStart()
        {
            LaunchOptions launchOptions = new LaunchOptions();
            switch (launchMode)
            {
                case 1:
                    launchOptions.Authenticator = new OfflineAuthenticator(offline.ID.Text);
                    break;
                case 2:
                    launchOptions.Authenticator = new YggdrasilLogin(online.Email.Text, online.Password.Password, false);
                    break;

            }


            if (version.versionCombo.Text != string.Empty &&
                setting.SelectJava.Text != string.Empty && setting.RAM.Text != string.Empty)
            {
                try
                {
                    if (launchMode != 3)
                    {
                        Core.JavaPath = setting.SelectJava.Text;
                        var ver = (KMCCC.Launcher.Version)version.versionCombo.SelectedItem;
                        launchOptions.Version = ver;
                        launchOptions.MaxMemory = Convert.ToInt32(setting.RAM.Text);
                        var result = Core.Launch(launchOptions);
                        if (!result.Success)
                        {
                            switch (result.ErrorType)
                            {
                                case ErrorType.NoJAVA:
                                    MessageBox.Show("选择的Java无效", "启动失败");
                                    break;
                                case ErrorType.AuthenticationFailed:
                                    MessageBox.Show("请检查您的帐号和密码", "登陆失败");
                                    break;
                                case ErrorType.UncompressingFailed:
                                    MessageBox.Show("错误的文件结构", "启动失败");
                                    break;
                                default:
                                    MessageBox.Show("0x00003c", "错误");
                                    break;
                            }
                        }
                    }
                    else
                    {
                       MessageBox.Show("0x00007f", "错误");
                    }


                }
                catch
                {
                    MessageBox.Show("0x00008e", "错误");
                }
            }
            else
            {
                MessageBox.Show("不完整的启动信息", "无效启动");
            }
        
    }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new Frame
            {
                Content = version
            };
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new Frame
            {
                Content = online
            };
            launchMode = 2;

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new Frame
            {
                 Content = offline
            };
            launchMode = 1; 
         
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new Frame
            {
                Content = setting
            };
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            GameStart();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Dark dark = new Dark();
            dark.Show();
            this.Close();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new Frame
            {
                Content = about
            };
        }
    }
}
