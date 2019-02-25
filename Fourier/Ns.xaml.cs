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

namespace UI
{
    /// <summary>
    /// Interaction logic for Ns.xaml
    /// </summary>
    public partial class Ns : Page
    {
        public Ns(dynamic sf)
        {
            InitializeComponent();
            _1.Draw(sf.FourierSeries(1), -10, 10, 0.01);
            _5.Draw(sf.FourierSeries(2), -10, 10, 0.01);
            _10.Draw(sf.FourierSeries(3), -10, 10, 0.01);
            _20.Draw(sf.FourierSeries(4), -10, 10, 0.01);
            _30.Draw(sf.FourierSeries(5), -10, 10, 0.01);
            _25.Draw(sf.FourierSeries(25), -10, 10, 0.01);
            _35.Draw(sf.FourierSeries(int.Parse(value.Text)), -10, 10, 0.01);
            sff = sf;
        }
        dynamic sff;

        private void value_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _35.Draw(sff.FourierSeries(int.Parse(value.Text)), -10, 10, 0.01);
            }
        }
    }
}
