using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace UiDesktopApp1.Views.Windows
{
    /// <summary>
    /// MainlandBoost.xaml 的交互逻辑
    /// </summary>
    public partial class MainlandBoost : UiWindow
    {
        //private MainlandSource mainlandSource;
        public MainlandBoost()
        {
            //var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            //if (!File.Exists("mainlandSource.json"))
            //{
            //    MainlandSource path = new MainlandSource();
            //    path.pipsource = "http://mirrors.cloud.tencent.com/pypi/simple";
            //    path.pipdomain = "mirrors.cloud.tencent.com";
            //    path.githubdomain = "http://mirrors.cloud.tencent.com/pypi/simple";
            //    byte[] jsonbyte = JsonSerializer.SerializeToUtf8Bytes(path, jsonOptions);
            //    File.WriteAllBytes("mainlandSource.json", jsonbyte);
            //}
            //var jf = File.ReadAllText("mainlandSource.json");
            //mainlandSource = JsonSerializer.Deserialize<MainlandSource>(jf, jsonOptions);

            InitializeComponent();
        }

        public void OK_Click(object sender, RoutedEventArgs e)
        {
            //MainlandSource path = new MainlandSource();
            //path.pipsource = "http://mirrors.cloud.tencent.com/pypi/simple";
            //path.pipdomain = "mirrors.cloud.tencent.com";
            //path.githubdomain = "http://mirrors.cloud.tencent.com/pypi/simple";

            //var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            //byte[] jsonbyte = JsonSerializer.SerializeToUtf8Bytes(path, jsonOptions);
            //File.WriteAllBytes("startExeAcc.json", jsonbyte);

            this.Close();
        }

        public void Cancle_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ApplyPIP_Click(object sender, RoutedEventArgs e)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            startInfo.Arguments = " -c \"python\\python.exe -m pip config set global.index-url "+ pip1.Text.Trim() +" ; python\\python.exe -m pip config set global.trusted-host "+ pip2.Text.Trim()+"\"";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = false;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = false;
            startInfo.WorkingDirectory = "..\\";

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

        }

        private void ApplyGithub_Click(object sender, RoutedEventArgs e)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            startInfo.Arguments = " -c \"git\\bin\\git.exe config --global url.\""+github.Text.Trim()+"\".insteadOf \"https://github.com\" ";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = false;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = false;
            startInfo.WorkingDirectory = "..\\";

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }

        private void RestorePIP_Click(object sender, RoutedEventArgs e)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            startInfo.Arguments = " -c \"python\\python.exe -m pip config unset global.index-url ; python\\python.exe -m pip config unset global.trusted-host \"";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = false;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = false;
            startInfo.WorkingDirectory = "..\\";

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }

        private void RestoreGithub_Click(object sender, RoutedEventArgs e)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            startInfo.Arguments = " -c \"git\\bin\\git.exe config --global --unset url."+github.Text.Trim()+".insteadOf\"";
            Debug.WriteLine(startInfo.Arguments);
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = false;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = false;
            startInfo.WorkingDirectory = "..\\";

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }
    }

    //public class MainlandSource
    //{
    //    public string pipsource;
    //    public string pipdomain;
    //    public string githubdomain;
    //}
}
