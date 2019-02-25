using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Plot.xaml
    /// </summary>
    public partial class Plot : UserControl
    {
        public Plot()
        {
            InitializeComponent();
        }
        public ObservableCollection<ViewModel.Point> Points;
        public static List<ObservableCollection<ViewModel.Point>> Pointses;
        static Plot()
        {
            Pointses = new List<ObservableCollection<ViewModel.Point>>();
        }
        public void Draw(Func<double, double> f, double minx, double maxx, double step)
        {
            ViewModel vm = new ViewModel();
            double maxY = 0;
            for (double i = minx; i <= maxx; i += step)
            {
                var temp = f(i);
                if (temp > maxY) maxY = temp;
                vm.Points.Add(new ViewModel.Point
                {
                    X = i,
                    Y = temp
                });
            }
            Points = vm.Points;
            Pointses.Add(Points);
            YColumn.Maximum = maxY * 1.5;
            YColumn.Minimum = maxY * -1.5;
            Chart.DataContext = vm;
        }
        private Syncfusion.UI.Xaml.Charts.FastLineSeries newseries(int n, bool setorclear, ViewModel vm)
        {
            Syncfusion.UI.Xaml.Charts.FastLineSeries retvalue = new Syncfusion.UI.Xaml.Charts.FastLineSeries()
            {
                DataContext = vm,
                ShowTooltip = true,
                EnableAnimation = true,
                YBindingPath = "Y",
                XBindingPath = "X"
            };
            if (n == 1)
            {
                retvalue.ItemsSource = new Binding("Points");
            }
            else if (n == 2)
            {
                retvalue.ItemsSource = new Binding("Points2");
            }
            else if (n == 3)
            {
                retvalue.ItemsSource = new Binding("Points3");
            }
            else if (n == 4)
            {
                retvalue.ItemsSource = new Binding("Points4");
            }
            else if (n == 5)
            {
                retvalue.ItemsSource = new Binding("Points5");
            }
            return retvalue;
        }
        public void Cleare(int n)
        {
            ViewModel vm = (Chart.DataContext as ViewModel);
            if (n == 1) vm.Points.Clear();
            if (n == 2) vm.Points2.Clear();
            if (n == 3) vm.Points3.Clear();
            if (n == 4) vm.Points4.Clear();
            if (n == 5) vm.Points5.Clear();
            Chart.DataContext = vm;
        }
        public void Draw(Func<double, double> f, double minx, double maxx, double step, int n)
        {
            ViewModel vm = (Chart.DataContext as ViewModel);
            if (n == 1) vm.Points.Clear();
            if (n == 2) vm.Points2.Clear();
            if (n == 3) vm.Points3.Clear();
            if (n == 4) vm.Points4.Clear();
            if (n == 5) vm.Points5.Clear();
            double maxY = 0;
            for (double i = minx; i <= maxx; i += step)
            {
                var temp = f(i);
                if (temp > maxY) maxY = temp;
                if (n == 1)
                {
                    vm.Points.Add(new ViewModel.Point
                    {
                        X = i,
                        Y = temp
                    });
                }
                else if (n == 2)
                {
                    vm.Points2.Add(new ViewModel.Point
                    {
                        X = i,
                        Y = temp
                    });
                }
                else if (n == 3)
                {
                    vm.Points3.Add(new ViewModel.Point
                    {
                        X = i,
                        Y = temp
                    });

                }
                else if (n == 4)
                {
                    vm.Points4.Add(new ViewModel.Point
                    {
                        X = i,
                        Y = temp
                    });
                }
                else if (n == 5)
                {
                    vm.Points5.Add(new ViewModel.Point
                    {
                        X = i,
                        Y = temp
                    });
                }
            }
            Points = vm.Points;
            Pointses.Add(Points);
            YColumn.Maximum = maxY * 1.5;
            YColumn.Minimum = maxY * -1.5;
            Chart.DataContext = vm;
        }
        public void Draw(KeyValuePair<double, double>[] values)
        {
            ViewModel vm = new ViewModel();
            double maxY = 0;
            foreach (var item in values)
            {
                if (item.Value > maxY) maxY = item.Value;
                vm.Points.Add(new ViewModel.Point()
                {
                    X = item.Key,
                    Y = item.Value
                });
            }
            YColumn.Maximum = maxY * 1.5;
            YColumn.Minimum = maxY * -1.5;
            Chart.DataContext = vm;
        }
        public void Draw(ObservableCollection<ViewModel.Point> p)
        {
            ViewModel vm = new ViewModel();
            double maxY = 0;
            foreach (var item in p)
            {
                if (item.Y > maxY) maxY = item.Y;
            }
            YColumn.Maximum = maxY * 1.5;
            YColumn.Minimum = maxY * -1.5;
            Chart.DataContext = vm;
        }
        public class ViewModel
        {
            public ViewModel()
            {
                Points = new ObservableCollection<Point>();
                Points2 = new ObservableCollection<Point>();
                Points3 = new ObservableCollection<Point>();
                Points4 = new ObservableCollection<Point>();
                Points5 = new ObservableCollection<Point>();
            }
            public ObservableCollection<Point> Points { get; set; }
            public ObservableCollection<Point> Points2 { get; set; }
            public ObservableCollection<Point> Points3 { get; set; }
            public ObservableCollection<Point> Points4 { get; set; }
            public ObservableCollection<Point> Points5 { get; set; }

            public class Point
            {
                public double X { get; set; }
                public double Y { get; set; }
            }
        }
    }

}
