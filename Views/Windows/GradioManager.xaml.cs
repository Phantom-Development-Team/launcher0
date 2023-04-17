using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.IO;
using Wpf.Ui.Controls;
using Button = System.Windows.Controls.Button;

namespace UiDesktopApp1.Views.Windows
{
    public class GradioAcc
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
    }
    /// <summary>   
    /// GradioManager.xaml 的交互逻辑
    /// </summary>
    public partial class GradioManager : UiWindow
    {
        public List<GradioAcc> gradioAcc;
        public ObservableCollection<GradioAcc> gradioCollection = new();
        public GradioManager()
        {
            InitializeComponent();

            if (!File.Exists("gradioAcc.json"))
            {
                var sw = File.CreateText("gradioAcc.json");
                sw.WriteLine("[]");
                sw.Close();
            }
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var jf = File.ReadAllText("gradioAcc.json");
            gradioAcc = JsonSerializer.Deserialize<List<GradioAcc>>(jf, jsonOptions);

            for (int i = 0; i < gradioAcc.Count(); i++)
            {
                gradioAcc[i].Index = i;
                gradioCollection.Add(gradioAcc[i]);
            }

            accs.ItemsSource = gradioCollection;
        }

        public void addAcc_Click(object sender, RoutedEventArgs e)
        {
            GradioAcc acc = new GradioAcc();
            acc.Name = username.Text; 
            acc.Pass = pass.Text;
            gradioAcc.Add(acc);

            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            byte[] jsonbyte = JsonSerializer.SerializeToUtf8Bytes(gradioAcc, jsonOptions);
            File.WriteAllBytes("gradioAcc.json", jsonbyte);

            gradioCollection.Clear();
            var jf = File.ReadAllText("gradioAcc.json");
            gradioAcc = JsonSerializer.Deserialize<List<GradioAcc>>(jf, jsonOptions);

            for (int i = 0; i < gradioAcc.Count(); i++)
            {
                gradioAcc[i].Index = i;
                gradioCollection.Add(gradioAcc[i]);
            }

            accs.ItemsSource = gradioCollection;
        }

        public void delAcc_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int idx = (int)btn.Tag;

            gradioAcc.RemoveAt(idx);
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            byte[] jsonbyte = JsonSerializer.SerializeToUtf8Bytes(gradioAcc, jsonOptions);
            File.WriteAllBytes("gradioAcc.json", jsonbyte);

            gradioCollection.Clear();
            var jf = File.ReadAllText("gradioAcc.json");
            gradioAcc = JsonSerializer.Deserialize<List<GradioAcc>>(jf, jsonOptions);

            for (int i = 0; i < gradioAcc.Count(); i++)
            {
                gradioAcc[i].Index = i;
                gradioCollection.Add(gradioAcc[i]);
            }

            accs.ItemsSource = gradioCollection;
        }
    }
}
