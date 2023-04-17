using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
using UiDesktopApp1.Views.Pages;
using Wpf.Ui.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace UiDesktopApp1.Views.Windows
{
    public class ExtTagItem
    {
        public string Ext { get; set; }
        public string Hash { get; set; }
        public string Message { get; set; }
        public string Date { get; set; }
        public string Tag { get; set; }
    }
    /// <summary>
    /// VerManager.xaml 的交互逻辑
    /// </summary>
    public partial class VerManager : UiWindow
    {
        private string currExt;
        private string currHash;
        private List<ExtTagItem> tags;
        public List<CommitItem> commits;
        public ObservableCollection<CommitItem> CommiteCollection = new();
        public ObservableCollection<CommitItem> CommiteTagCollection = new();
        private string giturl;
        public VerManager()
        {
            InitializeComponent();
        }

        public VerManager(string giturl, string ext)
        {
            this.currExt = ext;
            this.giturl = giturl;

            InitializeComponent();

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            startInfo.Arguments = " -c \"..\\..\\git\\bin\\git.exe log --oneline --pretty='%h^^%s^^%cd' --date=\"short\" -n 1\"";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = ext;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string msg = process.StandardOutput.ReadToEnd();

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            startInfo.Arguments = " -c \"..\\..\\git\\bin\\git.exe remote -v";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = ext;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string msg2 = process.StandardOutput.ReadToEnd();
            Debug.WriteLine(giturl);

            lblCurrHash.Content = msg.Split("^^")[0];
            lblCurrDate.Content = msg.Split("^^")[2];
            lblCurrMessage.Content = msg.Split("^^")[1];
            lblCurrGit.Content = msg2.Split("\\n")[0].Split(" ")[0];
            currHash = (string)lblCurrHash.Content;

            InitializeData(ext);

            for (int i = 0; i < tags.Count; i++)
            {
                if (currExt != tags[i].Ext) continue;
                CommitItem item1 = new CommitItem();
                item1.Hash = tags[i].Hash;
                item1.Message = tags[i].Message;
                item1.Date = tags[i].Date;
                item1.Tag = tags[i].Tag;
                item1.Id = i;
                item1.Enable = true;
                item1.Checked = false;
                CommiteTagCollection.Add(item1);
            }

            commit.ItemsSource  = CommiteCollection;
            commit2.ItemsSource = CommiteTagCollection;
        }

        private void InitializeData(string ext)
        {
            if (!File.Exists("exttag.json"))
            {
                var sw = File.CreateText("exttag.json");
                sw.WriteLine("[]");
                sw.Close();
            }
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var jf = File.ReadAllText("exttag.json");
            tags = JsonSerializer.Deserialize<List<ExtTagItem>>(jf, jsonOptions);

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            startInfo.Arguments = " -c \"..\\..\\git\\bin\\git.exe --no-pager log main --pretty='%h^^%s^^%cd' --date=\"short\" -n 150\"";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = ext;

            process.StartInfo = startInfo;

            int idx = 0;
            commits = new List<CommitItem>();

            process.ErrorDataReceived += new DataReceivedEventHandler(delegate (object sender, DataReceivedEventArgs e)
            {

            });
            process.OutputDataReceived += new DataReceivedEventHandler(delegate (object sender, DataReceivedEventArgs e)
            {
                if (e.Data == null) return;
                
                CommitItem item1 = new CommitItem();
                string[] itemarr = e.Data.Split("^^");
                if (itemarr.Length < 3)
                {
                    return;
                }
                
                item1.Hash = itemarr[0];
                item1.Message = itemarr[1];
                item1.Date = itemarr[2];
                item1.Id = idx++;
                item1.Enable = true;
                item1.Checked = false;

                for (int j = 0; j < tags.Count(); j++)
                {
                    if (currExt != tags[j].Ext) continue;
                    if (item1.Hash == tags[j].Hash)
                    {
                        item1.Tag = tags[j].Tag;
                    }
                }

                if (currHash == item1.Hash)
                {
                    item1.Enable = false;
                    item1.Checked = true;
                }

                commits.Add(item1);
            });

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            process.WaitForExit();

            if (commits.Count <= 0)
            {
                Debug.WriteLine("1111");
                process = new Process();
                startInfo = new ProcessStartInfo();
                startInfo.FileName = @"powershell.exe";
                startInfo.Arguments = " -c \"..\\..\\git\\bin\\git.exe --no-pager log master --pretty='%h^^%s^^%cd' --date=\"short\" -n 150\"";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.CreateNoWindow = true;
                startInfo.WorkingDirectory = ext;

                process.StartInfo = startInfo;

                idx = 0;
                commits = new List<CommitItem>();

                process.ErrorDataReceived += new DataReceivedEventHandler(delegate (object sender, DataReceivedEventArgs e)
                {

                });
                process.OutputDataReceived += new DataReceivedEventHandler(delegate (object sender, DataReceivedEventArgs e)
                {
                    if (e.Data == null) return;

                    CommitItem item1 = new CommitItem();
                    string[] itemarr = e.Data.Split("^^");
                    if (itemarr.Length < 3)
                    {
                        return;
                    }

                    item1.Hash = itemarr[0];
                    item1.Message = itemarr[1];
                    item1.Date = itemarr[2];
                    item1.Id = idx++;
                    item1.Enable = true;
                    item1.Checked = false;

                    for (int j = 0; j < tags.Count(); j++)
                    {
                        if (currExt != tags[j].Ext) continue;
                        if (item1.Hash == tags[j].Hash)
                        {
                            item1.Tag = tags[j].Tag;
                        }
                    }

                    if (currHash == item1.Hash)
                    {
                        item1.Enable = false;
                        item1.Checked = true;
                    }

                    commits.Add(item1);
                });

                process.Start();
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
                process.WaitForExit();


                for (int i = 0; i < commits.Count(); i++)
                {
                    CommiteCollection.Add(commits[i]);
                }
            } else
            {
                for (int i = 0; i < commits.Count(); i++)
                {
                    CommiteCollection.Add(commits[i]);
                }
            }
        }

        private void setup_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = (System.Windows.Controls.Button)sender;
            string hash = btn.Tag.ToString();

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            startInfo.Arguments = " -c \"..\\..\\git\\bin\\git.exe checkout " + hash;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = currExt;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();


            currHash = hash;

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            startInfo.Arguments = " -c \"..\\..\\git\\bin\\git.exe log --oneline --pretty='%h^^%s^^%cd' --date=\"short\" -n 1\"";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = currExt;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string msg = process.StandardOutput.ReadToEnd();

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            startInfo.Arguments = " -c \"..\\..\\git\\bin\\git.exe remote -v";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = currExt;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string msg2 = process.StandardOutput.ReadToEnd();

            lblCurrHash.Content = msg.Split("^^")[0];
            lblCurrDate.Content = msg.Split("^^")[2];
            lblCurrMessage.Content = msg.Split("^^")[1];
            lblCurrGit.Content = msg2.Split("\\n")[0].Split(" ")[0];
            currHash = (string)lblCurrHash.Content;

            CommiteCollection.Clear();
            CommiteTagCollection.Clear();

            InitializeData(currExt);

            for (int i = 0; i < tags.Count; i++)
            {
                if (currExt != tags[i].Ext) continue;
                CommitItem item1 = new CommitItem();
                item1.Hash = tags[i].Hash;
                item1.Message = tags[i].Message;
                item1.Date = tags[i].Date;
                item1.Tag = tags[i].Tag;
                item1.Id = i;
                item1.Enable = true;
                item1.Checked = false;
                CommiteTagCollection.Add(item1);
            }

            commit.ItemsSource  = CommiteCollection;
            commit2.ItemsSource = CommiteTagCollection;
        }

        private void tag_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = (System.Windows.Controls.Button)sender;
            int Id = int.Parse(btn.Tag.ToString());

            TagInput tag1 = new TagInput();
            bool? dialogResult = tag1.ShowDialog();

            switch (dialogResult)
            {
                case true:
                    var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
                    byte[] jsonbyte;
                    for (int i = 0; i<tags.Count(); i++)
                    {
                        if (tags[i].Hash == CommiteCollection[Id].Hash && tags[i].Ext == currExt)
                        {
                            tags[i].Tag = tag1.tag;
                            tags[i].Hash  = CommiteCollection[Id].Hash;
                            tags[i].Message = CommiteCollection[Id].Message;
                            tags[i].Date = CommiteCollection[Id].Date;

                            jsonbyte = JsonSerializer.SerializeToUtf8Bytes(tags, jsonOptions);
                            File.WriteAllBytes("exttag.json", jsonbyte);

                            CommiteCollection.Clear();
                            CommiteTagCollection.Clear();

                            InitializeData(currExt);

                            for (int j = 0; j < tags.Count; j++)
                            {
                                if (currExt != tags[j].Ext) continue;
                                CommitItem item1 = new CommitItem();
                                item1.Hash = tags[j].Hash;
                                item1.Message = tags[j].Message;
                                item1.Date = tags[j].Date;
                                item1.Tag = tags[j].Tag;
                                item1.Id = j;
                                item1.Enable = true;
                                item1.Checked = false;
                                CommiteTagCollection.Add(item1);
                            }

                            commit.ItemsSource  = CommiteCollection;
                            commit2.ItemsSource = CommiteTagCollection;
                            return;
                        }
                    }

                    ExtTagItem ti = new ExtTagItem();
                    ti.Ext = currExt;
                    ti.Tag = tag1.tag;
                    ti.Hash    = CommiteCollection[Id].Hash;
                    ti.Message = CommiteCollection[Id].Message;
                    ti.Date = CommiteCollection[Id].Date;

                    tags.Add(ti);
                    jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
                    jsonbyte = JsonSerializer.SerializeToUtf8Bytes(tags, jsonOptions);
                    File.WriteAllBytes("exttag.json", jsonbyte);

                    CommiteCollection.Clear();
                    CommiteTagCollection.Clear();

                    InitializeData(currExt);

                    for (int i = 0; i < tags.Count; i++)
                    {
                        if (currExt != tags[i].Ext) continue;
                        CommitItem item1 = new CommitItem();
                        item1.Hash = tags[i].Hash;
                        item1.Message = tags[i].Message;
                        item1.Date = tags[i].Date;
                        item1.Tag = tags[i].Tag;
                        item1.Id = i;
                        item1.Enable = true;
                        item1.Checked = false;
                        CommiteTagCollection.Add(item1);
                    }

                    commit.ItemsSource  = CommiteCollection;
                    commit2.ItemsSource = CommiteTagCollection;
                    break;
                case false:
                    // User canceled dialog box
                    break;
                default:
                    // Indeterminate
                    break;
            }
        }
    }
}
