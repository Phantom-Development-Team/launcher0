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
    /// TorchManager.xaml 的交互逻辑
    /// </summary>
    public partial class TorchManager : UiWindow
    {
        private string[] torchVersion;
        private StartExePath strStartExe1;
        public TorchManager()
        {
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            if (!File.Exists("startExeAcc.json"))
            {
                StartExePath path = new StartExePath();
                path.PythonExePath = "python\\python.exe";
                path.GitExePath = "git\\bin\\git.exe";
                byte[] jsonbyte = JsonSerializer.SerializeToUtf8Bytes(path, jsonOptions);
                File.WriteAllBytes("startExeAcc.json", jsonbyte);
            }
            var jf = File.ReadAllText("startExeAcc.json");
            strStartExe1 = JsonSerializer.Deserialize<StartExePath>(jf, jsonOptions);
            torchVersion = new string[]
            {
                "torch==1.12.1+cu113 torchvision==0.13.1+cu113 torchaudio==0.12.1 --extra-index-url https://download.pytorch.org/whl/cu113",
                "torch==1.13.1+cu117 torchvision==0.14.1+cu117 torchaudio==0.13.1 --extra-index-url https://download.pytorch.org/whl/cu117",
                "torch==2.0.0 torchvision torchaudio --index-url https://download.pytorch.org/whl/cu118",
            };

            InitializeComponent();
        }

        private void reinstall_click(object sender, RoutedEventArgs e)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            startInfo.Arguments = " -c \""+strStartExe1.PythonExePath+" -m pip uninstall -y torch torchvision torchaudio xformers";
            startInfo.WorkingDirectory = "..\\";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            startInfo.Arguments = " -c \" "+strStartExe1.PythonExePath+" -m pip install " + torchVersion[torchv.SelectedIndex];
            startInfo.WorkingDirectory = "..\\";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            if (torchv.SelectedIndex == 0)
            {
                process = new Process();
                startInfo = new ProcessStartInfo();
                startInfo.FileName = @"powershell.exe";
                startInfo.Arguments = " -c \" "+strStartExe1.PythonExePath+" -m pip install launcher\\xformers-0.0.14.dev0-cp310-cp310-win_amd64.whl";
                startInfo.WorkingDirectory = "..\\";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            } else if (torchv.SelectedIndex == 1)
            {
                process = new Process();
                startInfo = new ProcessStartInfo();
                startInfo.FileName = @"powershell.exe";
                startInfo.Arguments = " -c \" "+strStartExe1.PythonExePath+" -m pip install  pip install xformers==0.0.16rc425";
                startInfo.WorkingDirectory = "..\\";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            } else if (torchv.SelectedIndex == 2)
            {
                process = new Process();
                startInfo = new ProcessStartInfo();
                startInfo.FileName = @"powershell.exe";
                startInfo.Arguments = " -c \" "+strStartExe1.PythonExePath+" -m pip install  pip install xformers==0.0.17";
                startInfo.WorkingDirectory = "..\\";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            }

            //process = new Process();
            //startInfo = new ProcessStartInfo();
            //startInfo.FileName = @"powershell.exe";
            //startInfo.Arguments = " -noexit -c \" "+strStartExe1.PythonExePath+" -m pip install -r requirements.txt";
            //startInfo.WorkingDirectory = "..\\";
            //process.StartInfo = startInfo;
            //process.Start();
            //process.WaitForExit();
        }

        private void cancle_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
