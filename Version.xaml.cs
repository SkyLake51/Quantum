using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KMCCC.Launcher;
using KMCCC.Authentication;
using System.IO;
using Path = System.IO.Path;
using MessageBox = System.Windows.MessageBox;

namespace Quantum
{
    /// <summary>
    /// Offline.xaml 的交互逻辑
    /// </summary>
    public partial class Version : Page
    {
        public static LauncherCore Core = LauncherCore.Create();
        public Version()
        {
            InitializeComponent();
            var versions = Core.GetVersions().ToArray();
            versionCombo.ItemsSource = versions;
            if (versionCombo.Items.Count != 0)
                versionCombo.SelectedItem = versionCombo.Items[0];

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string WantedPath = System.Windows.Forms.Application.StartupPath.Substring(0, System.Windows.Forms.Application.StartupPath.LastIndexOf(@"\"));
            string path2 = System.IO.Path.GetDirectoryName(WantedPath);
            //用户选择目录
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.ShowDialog();
            var s = fbd.SelectedPath;
            if (fbd.SelectedPath != string.Empty)
            {
                try
                {
                    FileUtility.CopyFolder(fbd.SelectedPath , path2 + @"\.minecraft\");
                    MessageBox.Show("导入完成");
                }
                catch (Exception)
                {
                    MessageBox.Show("导入出错");
                }
            }
        }
        public static class FileUtility
        {
            /// <summary>
            /// 复制文件夹及文件
            /// </summary>
            /// <param name="sourceFolder">原文件路径</param>
            /// <param name="destFolder">目标文件路径</param>
            /// <returns></returns>
            public static void CopyFolder(string sourceFolder, string destFolder)
            {
                try
                {
                    //如果目标路径不存在,则创建目标路径
                    if (!Directory.Exists(destFolder))
                    {
                        Directory.CreateDirectory(destFolder);
                    }
                    //得到原文件根目录下的所有文件
                    string[] files = Directory.GetFiles(sourceFolder);
                    foreach (string file in files)
                    {
                        string name = Path.GetFileName(file);
                        string dest = Path.Combine(destFolder, name);
                        // 复制文件
                        File.Copy(file, dest);
                    }
                    //得到原文件根目录下的所有文件夹
                    string[] folders = Directory.GetDirectories(sourceFolder);
                    foreach (string folder in folders)
                    {
                        string dirName = folder.Split('\\')[folder.Split('\\').Length - 1];
                        string destfolder = Path.Combine(destFolder, dirName);
                        // 递归调用
                        CopyFolder(folder, destfolder);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"copy file Error:{ex.Message}\r\n source:{ex.StackTrace}");
                }
            }


           
        }
    }
}
