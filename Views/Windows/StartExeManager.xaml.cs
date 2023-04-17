
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using Wpf.Ui.Controls;

namespace UiDesktopApp1.Views.Windows
{
    /// <summary>
    /// StartExeManager.xaml 的交互逻辑
    /// </summary>
    public partial class StartExeManager : UiWindow
    {
        private StartExePath startExePath;
        public StartExeManager()
        {
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            if (!File.Exists("startExeAcc.json"))
            {
                StartExePath path = new StartExePath();
                path.PythonExePath = "..\\python\\python.exe";
                path.GitExePath = "..\\git\\bin\\git.exe";
                byte[] jsonbyte = JsonSerializer.SerializeToUtf8Bytes(path, jsonOptions);
                File.WriteAllBytes("startExeAcc.json", jsonbyte);
            }
            var jf = File.ReadAllText("startExeAcc.json");
            startExePath = JsonSerializer.Deserialize<StartExePath>(jf, jsonOptions);

            InitializeComponent();
        }

        public void OK_Click(object sender, RoutedEventArgs e)
        {
            StartExePath path = new StartExePath();
            path.PythonExePath = pythonExePath.Text;
            path.GitExePath = gitExePath.Text;

            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            byte[] jsonbyte = JsonSerializer.SerializeToUtf8Bytes(path, jsonOptions);
            File.WriteAllBytes("startExeAcc.json", jsonbyte);

            this.Close();
        }

        public void Cancle_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class StartExePath
    {
        public string PythonExePath { get; set; }
        public string GitExePath { get; set; }
    }
}
