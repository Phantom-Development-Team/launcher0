using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UiDesktopApp1.Models;
using UiDesktopApp1.Services;
using UiDesktopApp1.Views.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace UiDesktopApp1.Views.Pages
{
    /// <summary>
    /// Page1.xaml 的交互逻辑
    /// </summary>
    public partial class Page1
    {
        public Page1()
        {
           InitializeComponent();

           exts.ItemsSource = Store.extLocal;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("Explorer.exe", AppDomain.CurrentDomain.BaseDirectory+"..\\extensions");
        }

        private void checkUpdateExt_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"git.exe";
            startInfo.Arguments = " reset --hard";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = (string)btn.Tag;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = @"git.exe";
            startInfo.Arguments = " pull origin master";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = (string)btn.Tag;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string msg = process.StandardOutput.ReadToEnd();

            if (msg.Length <= 0 )
            {
                process = new Process();
                startInfo = new ProcessStartInfo();
                startInfo.FileName = @"git.exe";
                startInfo.Arguments = " pull origin main";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.CreateNoWindow = true;
                startInfo.WorkingDirectory = (string)btn.Tag;

                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                string msg2 = process.StandardOutput.ReadToEnd();
                if (msg2.Contains("Already up to date."))
                {
                    MessageBox.Show("代码已经是最新的了！");
                }
                else
                {
                    MessageBox.Show("更新成功！");
                }
            } else
            {
                if (msg.Contains("Already up to date."))
                {
                    MessageBox.Show("代码已经是最新的了！");
                }
                else
                {
                    MessageBox.Show("更新成功！");
                }
            }

           
        }
        private void openExt_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string ext = btn.Tag.ToString();
            Process.Start("Explorer.exe", AppDomain.CurrentDomain.BaseDirectory+ext);
        }
        private void Setup_Click(object sender, RoutedEventArgs e)
        {
            UiDesktopApp1.Views.Windows.ExtManager ext = new UiDesktopApp1.Views.Windows.ExtManager();
            ext.Show();
        }
        private void verManager_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int idx = int.Parse(btn.Tag.ToString());
           
            VerManager ma = new VerManager(Store.extLocal[idx].GitUrl, Store.extLocal[idx].Path);
            ma.Show();
        }
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Init.InitExtData();
            exts.ItemsSource = Store.extLocal;
        }
    }
    public class ExtRemote
    {
        public string name { get; set; }
        public string hash { get; set; }
        public string date { get; set; }
        public string url { get; set; }
    }
}
