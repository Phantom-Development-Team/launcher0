using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;
using UiDesktopApp1.Models;
using Wpf.Ui.Common.Interfaces;

namespace UiDesktopApp1.ViewModels
{
    public partial class DataViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private IEnumerable _commitItem;

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {
        }

        private void InitializeViewModel()
        {
            //Process process = new Process();
            //ProcessStartInfo startInfo = new ProcessStartInfo();
            //startInfo.FileName = @"powershell.exe";
            //startInfo.Arguments = " -c \"git\\bin\\git.exe log --pretty='%h _ %s _ %cd' --date=\"short\" -n 20\"";
            //startInfo.UseShellExecute = false;
            //startInfo.RedirectStandardOutput = true;
            //startInfo.CreateNoWindow = true;
            //startInfo.WorkingDirectory = "";

            //process.StartInfo = startInfo;
            //process.Start();

            //process.WaitForExit();

            //_commitItem = new List<CommitItem>();
            //List<CommitItem> _commitItem1 = (List<CommitItem>)_commitItem;


            //string[] item = process.StandardOutput.ReadToEnd().Split("\n");

            //for (int i = 0; i < item.Length; i++)
            //{
                
            //    CommitItem item1 = new CommitItem();
            //    item1.Hash = item[i].Split(" _ ")[0];
            //    _commitItem1.Add(new CommitItem());
            //    Debug.WriteLine(item1.Hash);
            //}

            _isInitialized = true;
        }
    }
}
