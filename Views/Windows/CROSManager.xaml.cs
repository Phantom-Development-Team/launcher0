using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using Wpf.Ui.Controls;
using Button = System.Windows.Controls.Button;

namespace UiDesktopApp1.Views.Windows
{
    public class CROS
    {
        public int Index { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// GradioManager.xaml 的交互逻辑
    /// </summary>
    public partial class CROSManager : UiWindow
    {
        public List<CROS> apiAcc;
        public ObservableCollection<CROS> apiCollection = new();
        public CROSManager()
        {
            InitializeComponent();

            if (!File.Exists("crosAcc.json"))
            {
                var sw = File.CreateText("crosAcc.json");
                sw.WriteLine("[]");
                sw.Close();
            }
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var jf = File.ReadAllText("crosAcc.json");
            apiAcc = JsonSerializer.Deserialize<List<CROS>>(jf, jsonOptions);

            for (int i = 0; i < apiAcc.Count(); i++)
            {
                apiAcc[i].Index = i;
                apiCollection.Add(apiAcc[i]);
            }

            accs.ItemsSource = apiCollection;
        }

        public void addAcc_Click(object sender, RoutedEventArgs e)
        {
            CROS acc = new CROS();
            acc.Name = username.Text; 
            apiAcc.Add(acc);

            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            byte[] jsonbyte = JsonSerializer.SerializeToUtf8Bytes(apiAcc, jsonOptions);
            File.WriteAllBytes("crosAcc.json", jsonbyte);

            apiCollection.Clear();
            var jf = File.ReadAllText("crosAcc.json");
            apiAcc = JsonSerializer.Deserialize<List<CROS>>(jf, jsonOptions);

            for (int i = 0; i < apiAcc.Count(); i++)
            {
                apiAcc[i].Index = i;
                apiCollection.Add(apiAcc[i]);
            }

            accs.ItemsSource = apiCollection;
        }

        void delAcc_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int idx = (int)btn.Tag;

            apiAcc.RemoveAt(idx);
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            byte[] jsonbyte = JsonSerializer.SerializeToUtf8Bytes(apiAcc, jsonOptions);
            File.WriteAllBytes("crosAcc.json", jsonbyte);

            apiCollection.Clear();
            var jf = File.ReadAllText("crosAcc.json");
            apiAcc = JsonSerializer.Deserialize<List<CROS>>(jf, jsonOptions);

            for (int i = 0; i < apiAcc.Count(); i++)
            {
                apiAcc[i].Index = i;
                apiCollection.Add(apiAcc[i]);
            }

            accs.ItemsSource = apiCollection;
        }
    }
}
