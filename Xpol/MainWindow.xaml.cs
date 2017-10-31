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

namespace XPol
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double LIN3;
        public MainWindow()
        {
            InitializeComponent();
        }

        void setLin3(double lin)
        {
            LIN3 = lin;
        }

        private void Calculate_click(object sender, RoutedEventArgs e)
        {

            double ampX = double.Parse(Amp1.Text);
            double ampY= double.Parse(Amp2.Text);
            double phsX = double.Parse(Phase1.Text);
            double phsY = double.Parse(Phase2.Text);

            double dB = ampY - ampX;
            double degrees = phsY - phsX;
            double linear = Math.Pow(10, (dB / 20));
            double radians = degrees * Math.PI / 180;
            double lin1 = (1 + Math.Pow(linear, 2.0)) / (2 * linear * Math.Sin(radians));
            double lin2 = 1 + Math.Sqrt(1 - 1 / Math.Pow(lin1, 2.0));
            double lin3 = lin1 * lin2;

            setLin3(lin3);

            if (lin3 > 0)
            {
                CPD.Text = "RHCP";
            }

            else
            {
                CPD.Text = "LHCP";
            }

            double Axial = Math.Round(20 * Math.Log10(Math.Abs(lin3)), 2);
            AR_dB.Text = Axial.ToString();

            double cp = Math.Round(10 * Math.Log10(1 - 0.5 * (1 + Math.Sin(2 * Math.Atan(Math.Abs(LIN3))))),2);
            XP.Text = cp.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
