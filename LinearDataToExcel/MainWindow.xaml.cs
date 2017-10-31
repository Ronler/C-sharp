using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace SpinLinearDataToExcel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public string data1;
        public string data2;
        public StreamReader reader;

        private void LoadData(object sender, RoutedEventArgs e)
        {
            LoadNewFile();
        }

        private void LoadNewFile()
        {
            OpenFileDialog pattern = new OpenFileDialog();
            pattern.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";

            bool? userClickedOK = pattern.ShowDialog();

            if (userClickedOK == true)
            {
                Data.Text = pattern.FileName;
            }

            Show.IsEnabled = true;
        }

        private void Convertdata(object sender, RoutedEventArgs e)
        {
            Addtobox();
        }

        private void Addtobox()
        {
            if (reader == null)
            {
                throw new Exception("you need data to convert. load data first.");
            }

            else
            {
                using (reader = new StreamReader(Data.Text))
                {
                    while (true)
                    {
                        var lineRead = reader.ReadLine();

                        if (lineRead.Length > 10)
                            if (lineRead.ToLower().Trim().Substring(0, 10) == "data_start")
                            {
                                data2 += "DATA_START,\n";
                                break;
                            }

                        data2 += lineRead.Replace('\t', ',') + "\n";

                        if (reader.EndOfStream)
                            throw new Exception("The start of the data should be labeled with \"DATA_START\"");
                    }

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        if (line.Length > 30)
                        {
                            float theta = float.Parse(line.Split(new char[] { ',', '\t' })[0]);
                            float amp = float.Parse(line.Split(new char[] { ',', '\t' })[1]);
                            float phase = float.Parse(line.Split(new char[] { ',', 't' })[2]);

                            data2 += String.Concat(theta, ",", amp, ",", phase, "\n");
                        }

                        if (line.Length <= 30)
                        {
                            data2 += line.Replace('\t', ',') + "\n";
                        }

                        Points.Text = data2;

                    }                  
                }
                Save.IsEnabled = true;
            }
        }

        private void SaveFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";

            bool? userClickedOK = savefile.ShowDialog();

            if (userClickedOK == true)
            {
                using (Stream s = File.Open(savefile.FileName, FileMode.CreateNew))
                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.Write(data2);
                }
            }
        }

        private void clear(object sender, RoutedEventArgs e)
        {
            Data.Text = null;
            Header.Text = null;
            Points.Text = null;
            Save.IsEnabled = false;
            Show.IsEnabled = false;
            Convert.IsEnabled = false;
        }

        private void Showdata(object sender, RoutedEventArgs e)
        {
            using (reader = new StreamReader(Data.Text))
            {
                data1 = reader.ReadToEnd();
                Header.Text = data1;
            }

            Convert.IsEnabled = true;
        }
    }
}
