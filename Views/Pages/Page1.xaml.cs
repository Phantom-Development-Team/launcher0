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

using UiDesktopApp1.Views.Windows;

namespace UiDesktopApp1.Views.Pages
{
    /// <summary>
    /// Page1.xaml 的交互逻辑
    /// </summary>
    public partial class Page1
    {
        private BackgroundWorker bgWorker = new BackgroundWorker();

        public List<ExtItem> commits;
        public ObservableCollection<ExtItem> ExtCollection = new();
        public Page1()
        {
            List<string> extsDir = new List<string>(Directory.EnumerateDirectories("..\\extensions"));
            for (int i = 0; i<extsDir.Count(); i++)
            {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
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
                ExtCollection.Add(item1);

                //bgWorker.ReportProgress(i, "Working");
                //System.Threading.Thread.Sleep(10);
            }

            InitializeComponent();
            //InitializeBackgroundWorker();

            //if (bgWorker.IsBusy)
            //    return;

            //bgWorker.RunWorkerAsync("hello");
            exts.ItemsSource = ExtCollection;

        }
        private void InitializeBackgroundWorker()
        {
            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgessChanged);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_WorkerCompleted);
        }

        public void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
                if (bgWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    List<string> extsDir = new List<string>(Directory.EnumerateDirectories("..\\extensions"));
                    for (int i = 0; i<extsDir.Count(); i++)
                    {
                        Process process = new Process();
                        ProcessStartInfo startInfo = new ProcessStartInfo();
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
                        ExtCollection.Add(item1);

                        bgWorker.ReportProgress(i, "Working");
                        System.Threading.Thread.Sleep(10);
                    }
                    
            }
        }

        public void bgWorker_ProgessChanged(object sender, ProgressChangedEventArgs e)
        {
            //string state = (string)e.UserState;//接收ReportProgress方法传递过来的userState
            //this.progressBar1.Value = e.ProgressPercentage;
            //this.label1.Text = "处理进度:" + Convert.ToString(e.ProgressPercentage) + "%";
        }

        public void bgWorker_WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error!=null)
            {
                MessageBox.Show(e.Error.ToString());
                return;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            bgWorker.CancelAsync();
        }
    

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("Explorer.exe", AppDomain.CurrentDomain.BaseDirectory+"..\\extensions");
        }
        private void CheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            ExtCollection = new ObservableCollection<ExtItem>();
            var extsDir = Directory.EnumerateDirectories("..\\extensions");
            foreach (string ext in extsDir)
            {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = @"git.exe";
                startInfo.Arguments = " remote -v ";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.CreateNoWindow = true;
                startInfo.WorkingDirectory = ext;

                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                string msg2 = process.StandardOutput.ReadToEnd();
                ExtItem item1 = new ExtItem();
                item1.Name = ext;
                item1.GitUrl = msg2.Split("\\n")[0].Split(" ")[0].Substring(7);

                process = new Process();
                startInfo = new ProcessStartInfo();
                startInfo.FileName = @"git.exe";
                startInfo.Arguments = " remote update ; status -uno";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.CreateNoWindow = true;
                startInfo.WorkingDirectory = ext;

                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                string msg = process.StandardOutput.ReadToEnd();
                Debug.WriteLine(msg);
                if (msg.Contains("Your branch is behind"))
                {
                    item1.hasUpdate = false;
                }
                else
                {
                    item1.hasUpdate = true;
                }

                ExtCollection.Add(item1);

                exts.ItemsSource = ExtCollection;
            }
        }

        private void checkUpdateExt_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"git.exe";
            startInfo.Arguments = " pull";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = (string)btn.Tag;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string msg = process.StandardOutput.ReadToEnd();
            Debug.WriteLine(msg);

            if (msg.Contains("Already up to date."))
            {
                MessageBox.Show("代码已经是最新的了！");
            }
            else
            {
                MessageBox.Show("更新成功！");
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
           
            VerManager ma = new VerManager(ExtCollection[idx].GitUrl, ExtCollection[idx].Path);
            ma.Show();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            ExtCollection.Clear();
            List<string> extsDir = new List<string>(Directory.EnumerateDirectories("..\\extensions"));
            for (int i = 0; i<extsDir.Count(); i++)
            {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
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
                ExtCollection.Add(item1);
            }

            exts.ItemsSource = ExtCollection;
        }
    }
}
