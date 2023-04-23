using NvAPIWrapper.GPU;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
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
using UiDesktopApp1.Models;
using UiDesktopApp1.Services;
using UiDesktopApp1.Views.Pages;
using Wpf.Ui.Controls;

namespace UiDesktopApp1.Views.Windows
{
    /// <summary>
    /// Init.xaml 的交互逻辑
    /// </summary>
    public partial class Init : UiWindow
    {
        public Init()
        {
            InitializeComponent();
        }

        public void InitData()
        {
            this.Show();

            info.Content = "检测显卡。。。";
            try
            {
                PhysicalGPU.GetPhysicalGPUs();
            } catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }

            info.Content = "检测扩展。。。";
            Store.extLocal = new System.Collections.ObjectModel.ObservableCollection<ExtItem>();

            Process process;
            ProcessStartInfo startInfo;

            var httpClient = new HttpClient();
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            using (var request = new HttpRequestMessage(HttpMethod.Get,
                "https://gitee.com/nightaway/automatic111-webui-exts/raw/master/exts_ver.json"))
            {
                var response = httpClient.Send(request);
                response.EnsureSuccessStatusCode();
                using var stream = response.Content.ReadAsStream();
                Store.extRemote = JsonSerializer.Deserialize<List<ExtRemote>>(stream, jsonOptions);
            }
            List<string> extsDir = new List<string>(Directory.EnumerateDirectories("..\\extensions"));
            for (int i = 0; i<extsDir.Count(); i++)
            {
                process = new Process();
                startInfo = new ProcessStartInfo();
                startInfo.FileName = @"git.exe";
                startInfo.Arguments = " remote -v ";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.CreateNoWindow = true;
                startInfo.WorkingDirectory = extsDir[i];

                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                string msg = process.StandardOutput.ReadToEnd();

                ExtItem item1 = new ExtItem();
                item1.Index = i;
                item1.Name = extsDir[i].Split("\\")[2];
                item1.Path =  extsDir[i];
                item1.GitUrl = msg.Split("\\n")[0].Split(" ")[0].Substring(7);

                process = new Process();
                startInfo = new ProcessStartInfo();
                startInfo.FileName = @"git.exe";
                startInfo.Arguments = "  log --oneline --pretty=\"%h^^%s^^%cd\" --date=\"short\" -n 1";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = false;
                startInfo.CreateNoWindow = true;
                startInfo.WorkingDirectory =  extsDir[i];

                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                msg = process.StandardOutput.ReadToEnd();
                string[] data = msg.Split("^^");
                item1.Hash = data[0];
                item1.Date = data[2];

                process = new Process();
                startInfo = new ProcessStartInfo();
                startInfo.FileName = @"git.exe";
                startInfo.Arguments = " remote set-url origin "+ "https://gitcode.net/nightaway/"+item1.GitUrl.Split("//")[item1.GitUrl.Split("//").Length-1].Split("/")[2];
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.CreateNoWindow = true;
                startInfo.WorkingDirectory = extsDir[i]; ;

                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                for (int j = 0; j< Store.extRemote.Count; j++)
                {
                    if (Store.extRemote[j].url.Split("//").Length > 1)
                    {
                        if (item1.GitUrl.Split("//")[item1.GitUrl.Split("//").Length-1].Split("/")[2] == Store.extRemote[j].url.Split("//")[1].Split("/")[2])
                        {
                            if (item1.Hash == Store.extRemote[j].hash)
                            {
                                item1.hasUpdate = false;
                            }
                            else
                            {
                                item1.hasUpdate = true;
                            }
                        }
                    }
                }

                Store.extLocal.Add(item1);
            }

            info.Content = "检测代码仓库。。。";

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = @"git.exe";
            startInfo.Arguments = " remote set-url origin "+ "https://ghproxy.com/https://github.com/AUTOMATIC1111/stable-diffusion-webui.git";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = "";

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            info.Content = "启动 PIP 加速。。。";
            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            startInfo.Arguments = " -c \"python\\python.exe -m pip config set global.index-url https://mirrors.cloud.tencent.com/pypi/simple ; python\\python.exe -m pip config set global.trusted-host mirrors.cloud.tencent.com\"";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = false;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = "..\\";

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            this.Close();
        }
    }
}
