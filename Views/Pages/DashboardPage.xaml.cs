using Wpf.Ui.Common.Interfaces;
using System.Management;
using System.Diagnostics;
using System.Windows;
using Wpf.Ui.Controls;

using NvAPIWrapper.GPU;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Windows.Controls;
using UiDesktopApp1.Views.Windows;
using System.Text.Json;
using System.IO;
using System;
using Microsoft.Extensions.Logging;
using System.Text;

namespace UiDesktopApp1.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : INavigableView<ViewModels.DashboardViewModel>
    {
        private StartSetting settings;
        private List<GPUItem> gpus = new();
        private string strStartExe1 = "python\\python.exe";

        public ViewModels.DashboardViewModel ViewModel
        {
            get;
        }

        public DashboardPage(ViewModels.DashboardViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

        private void UiPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!File.Exists("startConfig.json"))
            {
                var sw = File.CreateText("startConfig.json");
                sw.WriteLine("{}");
                sw.Close();
            }
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var jf = File.ReadAllText("startConfig.json");
            settings = JsonSerializer.Deserialize<StartSetting>(jf, jsonOptions);

            if (settings.grid1 == null)
            {
                settings.grid1 = new List<int>() { 0,0,0,0,0,0,0,0,0,0 }; 
            } else
            {
                var childred = g1.Children;
                for (int i = 0; i < childred.Count; i++)
                {
                    var child = (ToggleSwitch)childred[i];
                    if (settings.grid1[i] == 1)
                    {
                        child.IsChecked = true;
                    }
                    else
                    {
                        child.IsChecked = false;
                    }
                }
            }

            if (settings.grid2 == null)
            {
                settings.grid2 =  new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; ;
            }
            else
            {
                var childred = g2.Children;
                for (int i = 0; i < childred.Count; i++)
                {
                    var child = (ToggleSwitch)childred[i];
                    if (settings.grid2[i] == 1)
                    {
                        child.IsChecked = true;
                    }
                    else
                    {
                        child.IsChecked = false;
                    }
                }
            }

            if (settings.grid3 == null)
            {
                settings.grid3 =  new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; ;
            }
            else
            {
                var childred = g3.Children;
                for (int i = 0; i < childred.Count; i++)
                {
                    var child = (ToggleSwitch)childred[i];
                    if (settings.grid3[i] == 1)
                    {
                        child.IsChecked = true;
                    }
                    else
                    {
                        child.IsChecked = false;
                    }
                }
            }

            if (settings.grid4 == null)
            {
                settings.grid4=  new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; ;
            }
            else
            {
                var childred = g4.Children;
                for (int i = 0; i < childred.Count; i++)
                {
                    var child = (ToggleSwitch)childred[i];
                    if (settings.grid4[i] == 1)
                    {
                        child.IsChecked = true;
                    }
                    else
                    {
                        child.IsChecked = false;
                    }
                }
            }

            this.settings.xatten = 1;
            this.settings.port = 7980;

            this.host.Text = this.settings.listen;
            this.port.Text = this.settings.port.ToString();
            this.theme.SelectedIndex = this.settings.theme;
            this.xatten.SelectedIndex = this.settings.xatten;
            this.extParam.Text =  settings.ExtParam;

            gpus.Clear();
            ((ToggleSwitch)g3.Children[0]).IsChecked = true;
            this.GPUName.Items.Clear();
            this.GPUName.Items.Add("-1:CPU");

            for (int i = 0; i<PhysicalGPU.GetPhysicalGPUs().Length; i++)
            {
                GPUItem gpuItem = new GPUItem();
                gpuItem.Name = PhysicalGPU.GetPhysicalGPUs()[i].FullName;
                gpuItem.Memory = PhysicalGPU.GetPhysicalGPUs()[i].MemoryInformation.DedicatedVideoMemoryInkB / 1024 / 1024;
                gpus.Add(gpuItem);
                this.GPUName.Items.Add("" + i + ":" + gpuItem.Name + " " + gpuItem.Memory + "G");
            }
            this.GPUName.SelectedIndex = 1;

            if (gpus[0].Name.Contains("10"))
            {
                if (gpus[0].Memory <= 4)
                {
                    gpumem.SelectedIndex = 0;
                }
                else if (gpus[0].Memory > 4 && gpus[0].Memory <= 6)
                {
                    gpumem.SelectedIndex = 1;
                }
                else
                {
                    gpumem.SelectedIndex = 2;
                }
            }
            else
            {
                if (gpus[0].Memory <= 4)
                {
                    gpumem.SelectedIndex = 0;
                }
                else if (gpus[0].Memory > 4 && gpus[0].Memory <= 6)
                {
                    gpumem.SelectedIndex = 1;
                }
                else
                {
                    gpumem.SelectedIndex = 2;
                }
            }
        }
        private void GradioAcc_Click(object sender, RoutedEventArgs e)
        {
            GradioManager gra = new GradioManager();
            gra.Show();
        }
        private void ApiAcc_Click(object sender, RoutedEventArgs e)
        {
            ApiManager gra = new ApiManager();
            gra.Show();
        }
        private void CROS_Click(object sender, RoutedEventArgs e)
        {
            CROSManager gra = new CROSManager();
            gra.Show();
        }

        private void StartExe_Click(object sender, RoutedEventArgs e)
        {
            StartExeManager gra = new StartExeManager();
            gra.Show();
        }

        private void MainlandBoost_Click(object sender, RoutedEventArgs e)
        {
            MainlandBoost gra = new MainlandBoost();
            gra.Show();
        }
        private void TorchManager_Click(object sender, RoutedEventArgs e)
        {
            TorchManager gra = new TorchManager();
            gra.Show();
        }

        private void OneClickConsole_Click(object sender, RoutedEventArgs e)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            startInfo.Arguments = "-noexit -c \"$Env:GIT_PYTHON_GIT_EXECUTABLE='git\\bin\\git.exe' ";
            startInfo.WorkingDirectory = "..\\";
            process.StartInfo = startInfo;
            process.Start();
        }

        private void OneClickStart_Click(object sender, RoutedEventArgs e)
        {
            string params1 = "";
            var childred = g1.Children;
            for (int i = 0; i < childred.Count; i++)
            {
                var child = (ToggleSwitch)childred[i];
                if (child.IsChecked == true)
                {
                    params1 += child.Content.ToString().Split(" ")[0]+ " ";
                    settings.grid1[i] = 1;
                } else
                {
                    settings.grid1[i] = 0;
                }
            }

            childred = g2.Children;
            for (int i = 0; i < childred.Count; i++)
            {
                var child = (ToggleSwitch)childred[i];
                if (child.IsChecked == true)
                {
                    params1 += child.Content.ToString().Split(" ")[0]+ " ";
                    settings.grid2[i] = 1;
                }
                else
                {
                    settings.grid2[i] = 0;
                }
            }

            childred = g3.Children;
            for (int i = 0; i < childred.Count; i++)
            {
                var child = (ToggleSwitch)childred[i];
                if (child.IsChecked == true)
                {
                    params1 += child.Content.ToString().Split(" ")[0] + " ";
                    settings.grid3[i] = 1;
                }
                else
                {
                    settings.grid3[i] = 0;
                }
            }

            childred = g4.Children;
            for (int i = 0; i < childred.Count; i++)
            {
                var child = (ToggleSwitch)childred[i];
                if (child.IsChecked == true)
                {
                    params1 += child.Content.ToString().Split(" ")[0] + " ";
                    settings.grid4[i] = 1;
                }
                else
                {
                    settings.grid4[i] = 0;
                }
            }

            if (xatten.SelectedIndex != 0)
            {
                params1 += ((ComboBoxItem)xatten.Items[xatten.SelectedIndex]).Content.ToString().Split(" ")[0] + " ";
            }

            if (theme.SelectedIndex != 0)
            {
                if (theme.SelectedIndex == 1)
                {
                    params1 += "--theme light ";
                }
                else
                {
                    params1 += "--theme dark ";
                }
            }

            params1 += "--port " + port.Text.Trim() + " ";
            if (host.Text.Length > 0)
            {
                params1 += "--listen ";
            }

            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            if (File.Exists("gradioAcc.json") && share.IsChecked == true)
            {
                var jf = File.ReadAllText("gradioAcc.json");
                List<GradioAcc> gradioAcc = JsonSerializer.Deserialize<List<GradioAcc>>(jf, jsonOptions);

                params1 += " --gradio-auth ";
                for (int i = 0; i<gradioAcc.Count; i++)
                {
                    params1 += gradioAcc[i].Name + ":" + gradioAcc[i].Pass;
                    if (i != gradioAcc.Count-1)
                    {
                        params1 += ",";
                    }
                }
            }

            if (File.Exists("apiAcc.json") && api.IsChecked == true)
            {
                var jf = File.ReadAllText("apiAcc.json");
                List<GradioAcc> gradioAcc = JsonSerializer.Deserialize<List<GradioAcc>>(jf, jsonOptions);

                if (gradioAcc.Count > 0)
                {
                    params1 += " --api-auth ";
                    for (int i = 0; i<gradioAcc.Count; i++)
                    {
                        params1 += gradioAcc[i].Name + ":" + gradioAcc[i].Pass;
                        if (i != gradioAcc.Count-1)
                        {
                            params1 += ",";
                        }
                    }
                }
            }

            if (File.Exists("crosAcc.json") && api.IsChecked == true)
            {
                var jf = File.ReadAllText("crosAcc.json");
                List<CROS> crosAcc = JsonSerializer.Deserialize<List<CROS>>(jf, jsonOptions);
                if (crosAcc.Count > 0)
                {
                    params1 += " --cors-allow-origins ";
                    for (int i = 0; i<crosAcc.Count; i++)
                    {
                        params1 += crosAcc[i].Name;
                        if (i != crosAcc.Count-1)
                        {
                            params1 += ",";
                        }
                    }
                }
            }

            settings.GPUIdx = this.GPUName.SelectedIndex;
            settings.Memory = this.gpumem.SelectedIndex;
            settings.xatten = this.xatten.SelectedIndex;
            settings.theme  = this.theme.SelectedIndex;
            settings.listen = this.host.Text.Trim();
            settings.port = int.Parse(this.port.Text.Trim());
            if (((RadioButton)startExe.Children[0]).IsChecked == true)
            {
                settings.startExe = 0;
            } else
            {
                settings.startExe = 1;
            }
            
            settings.ExtParam = this.extParam.Text.Trim();

            byte[] jsonbyte = JsonSerializer.SerializeToUtf8Bytes(settings, jsonOptions);
            File.WriteAllBytes("startConfig.json", jsonbyte);

            string lanchFile = "launch.py";
            if (settings.startExe == 1)
            {
                lanchFile = "webui.py";
            }

            if (this.gpumem.SelectedIndex  == 0)
            {
                params1 += " --lowvram ";
            }
            else if (this.gpumem.SelectedIndex  == 1)
            {
                params1 += " --medvram ";
            }

            if (GPUName.SelectedIndex > 1)
            {
                params1 += " --device-id=" + (GPUName.SelectedIndex-1);
            }

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            startInfo.Arguments = "-noexit -c " + strStartExe1 + " "+lanchFile+" " + params1;
            startInfo.WorkingDirectory = "..\\";
            process.StartInfo = startInfo;
            process.Start();
        }

        public class GPUItem
        {
            public string Name { get; set; }
            public uint Memory { get; set; }
        }

        public class StartSetting
        {
            public int GPUIdx { get; set; }
            public int Memory { get; set; }
            public int xatten { get; set; }
            public int theme { get; set; }
            public string listen { get; set; }
            public int port { get; set; }

            public List<int> grid1 { get; set; }
            public List<int> grid2 { get; set; }
            public List<int> grid3 { get; set; }
            public List<int> grid4 { get; set; }
            public string ExtParam { get; set; }
            public int startExe { get; set; }
        }
    }
}