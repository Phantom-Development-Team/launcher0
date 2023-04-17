using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using Wpf.Ui.Controls;


namespace UiDesktopApp1.Views.Windows
{
    public class ApiAcc
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
    }

    /// <summary>
    /// GradioManager.xaml 的交互逻辑
    /// </summary>
    public partial class ApiManager : UiWindow
    {
        public List<ApiAcc> apiAcc;
        public ObservableCollection<ApiAcc> apiCollection = new();
        public ApiManager()
        {
            InitializeComponent();

            if (!File.Exists("apiAcc.json"))
            {
                var sw = File.CreateText("apiAcc.json");
                sw.WriteLine("[]");
                sw.Close();
            }
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var jf = File.ReadAllText("apiAcc.json");
            apiAcc = JsonSerializer.Deserialize<List<ApiAcc>>(jf, jsonOptions);

            for (int i = 0; i < apiAcc.Count(); i++)
            {
                apiAcc[i].Index = i;
                apiCollection.Add(apiAcc[i]);
            }

            accs.ItemsSource = apiCollection;
        }

        public void addAcc_Click(object sender, RoutedEventArgs e)
        {
            ApiAcc acc = new ApiAcc();
            acc.Name = username.Text; 
            acc.Pass = pass.Text;
            apiAcc.Add(acc);

            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            byte[] jsonbyte = JsonSerializer.SerializeToUtf8Bytes(apiAcc, jsonOptions);
            File.WriteAllBytes("apiAcc.json", jsonbyte);

            apiCollection.Clear();
            var jf = File.ReadAllText("apiAcc.json");
            apiAcc = JsonSerializer.Deserialize<List<ApiAcc>>(jf, jsonOptions);

            for (int i = 0; i < apiAcc.Count(); i++)
            {
                apiAcc[i].Index = i;
                apiCollection.Add(apiAcc[i]);
            }

            accs.ItemsSource = apiCollection;
        }

        void delAcc_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = (System.Windows.Controls.Button)sender;
            int idx = (int)btn.Tag;

            apiAcc.RemoveAt(idx);
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            byte[] jsonbyte = JsonSerializer.SerializeToUtf8Bytes(apiAcc, jsonOptions);
            File.WriteAllBytes("apiAcc.json", jsonbyte);

            apiCollection.Clear();
            var jf = File.ReadAllText("apiAcc.json");
                apiAcc = JsonSerializer.Deserialize<List<ApiAcc>>(jf, jsonOptions);

            for (int i = 0; i < apiAcc.Count(); i++)
            {
                    apiAcc[i].Index = i;
                    apiCollection.Add(apiAcc[i]);
            }

            accs.ItemsSource = apiCollection;
        }
    }
}
