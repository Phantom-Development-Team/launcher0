﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using UiDesktopApp1.Models;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace UiDesktopApp1.Views.Windows
{
    /// <summary>
    /// ExtManager.xaml 的交互逻辑
    /// </summary>
    public partial class ExtManager : UiWindow
    {
        ExtObj exts;
        public ObservableCollection<Extension2> ExtCollection = new();
        public ExtManager()
        {
            var httpClient = new HttpClient();
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

            using (var request = new HttpRequestMessage(HttpMethod.Get,
                "https://gitee.com/nightaway/automatic111-webui-exts/raw/master/Extensions-index.md"))
            {
                var response = httpClient.Send(request);

                response.EnsureSuccessStatusCode();
                using var stream = response.Content.ReadAsStream();
                exts = JsonSerializer.Deserialize<ExtObj>(stream, jsonOptions);
                var extsDir1 = Directory.EnumerateDirectories("..\\extensions");

                for (var i = 0; i<exts.extensions.Count(); i++)
                {
                    Extension2 ext2 = new Extension2();
                    ext2.Name = exts.extensions[i].name;
                    ext2.URL = exts.extensions[i].url;
                    ext2.Desc = exts.extensions[i].description;

                    foreach (string dir in extsDir1)
                    {
                        if (dir.ToLower().Contains(ext2.Name.Replace(" ", "-").ToLower()))
                        {
                            ext2.Setup = true;
                            break;
                        }
                    }
                    ExtCollection.Add(ext2);
                }

                InitializeComponent();

                extDirs.ItemsSource = ExtCollection;
            }
        }

        private void Setup_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = (System.Windows.Controls.Button)sender;

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"git.exe";
            startInfo.Arguments = " clone " + btn.Tag;
            startInfo.UseShellExecute = true;
            startInfo.RedirectStandardOutput = false;
            startInfo.CreateNoWindow = false;
            startInfo.WorkingDirectory = "..\\extensions";

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            ExtCollection.Clear();
            var extsDir1 = Directory.EnumerateDirectories("..\\extensions");
            for (var i = 0; i<exts.extensions.Count(); i++)
            {
                Extension2 ext2 = new Extension2();
                ext2.Name = exts.extensions[i].name;
                ext2.URL = exts.extensions[i].url;
                ext2.Desc = exts.extensions[i].description;

                foreach (string dir in extsDir1)
                {
                    if (dir.ToLower().Contains(ext2.Name.Replace(" ", "-").ToLower()))
                    {
                        ext2.Setup = true;
                        break;
                    }
                }
                ExtCollection.Add(ext2);
            }
            extDirs.ItemsSource = ExtCollection;
        }
    }

    public class ExtObj
    {
        public Extension[] extensions { get; set; }
    }

    public class Extension
    {
        public string name { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public string added { get; set; }
        public string[] tags { get; set; }
    }

    public class Extension2
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public string Desc { get; set; }
        public string Addad { get; set; }
        public bool Setup { get; set; }
        public string[] Tags { get; set; }
    }
}
