using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI
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

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //var ffff = new SimpleFourier("x=>x", Math.PI, 25);
            //var p = new Plot();
            //p.Background = Brushes.White;
            //p.Draw(ffff.FourierSeries(20), -50, 50, 0.01);
            //Grid.SetRow(p, 1);
            //MainGrid.Children.Add(p);
        }
        bool Expanded = true;
        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            switch (Expanded)
            {
                case true:
                    ThicknessAnimation th = new ThicknessAnimation()
                    {
                        Duration = TimeSpan.FromMilliseconds(500),
                        From = new Thickness(-170, 0, 860, 0),
                        To = new Thickness(0, 0, 690, 0)
                    };
                    xpander.BeginAnimation(Grid.MarginProperty, th);
                    Expanded = false;
                    break;
                case false:
                    Expanded = true;
                    ThicknessAnimation thh = new ThicknessAnimation()
                    {
                        Duration = TimeSpan.FromMilliseconds(500),
                        To = new Thickness(-170, 0, 860, 0),
                        From = new Thickness(0, 0, 690, 0)
                    };
                    xpander.BeginAnimation(Grid.MarginProperty, thh);
                    break;
                default:
                    break;
            }
        }


        private void NewFunction_Click(object sender, RoutedEventArgs e)
        {
            var gg = new Grid()
            {
                Background = Brushes.SkyBlue,
                Width = 300,
                Height = 200,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            gg.Children.Add(new TextBlock
            {
                Text = "please wait",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            });
            Grid.SetRow(gg, 1);
            MainGrid.Children.Add(gg);
            anbn.Children.Clear();
            var f = new NewFunction();
            var p =
            new PopUp()
            {
                Content = f,
                Owner = this
            };
            p.Width = 730;
            p.Height = 340;
            p.ShowDialog();
            if (UI.NewFunction.Piese == false)
            {
                if (UI.NewFunction.Type == FunctionType.normal)
                {
                    var sf = new SimpleFourier(UI.NewFunction.Func, UI.NewFunction.Period, 40);
                    ffff = sf;
                    Fourier.Draw(sf.FourierSeries(30), -10, 10, 0.01);
                    anbn.Children.Add(new TextBlock
                    {
                        Text = "a0: " + sf.a0.ToString(),
                        VerticalAlignment = VerticalAlignment.Stretch,
                        TextAlignment = TextAlignment.Center
                    });
                    for (int i = 0; i < sf.an.Length; i++)
                    {
                        anbn.Children.Add(new TextBlock
                        {
                            Text = "a" + (i + 1).ToString() + ": " + sf.an[i].ToString() +
                            "    b" + (i + 1).ToString() + ": " + sf.bn[i].ToString(),
                            VerticalAlignment = VerticalAlignment.Stretch,
                            TextAlignment = TextAlignment.Center
                        });
                    }
                    Dynamics.GetValues(sf.Function, -sf.Period, sf.Period, 0.001, out KeyValuePair<double, double>[] values);
                    Function.Draw(values);
                }
                else
                {
                    var sf = new OddEvenFourier(UI.NewFunction.Func, UI.NewFunction.Period, 40, UI.NewFunction.Type);
                    ffff = sf;
                    Fourier.Draw(sf.FourierSeries(30), -10, 10, 0.01);
                    anbn.Children.Add(new TextBlock
                    {
                        Text = "a0: " + sf.a0.ToString(),
                        VerticalAlignment = VerticalAlignment.Stretch,
                        TextAlignment = TextAlignment.Center
                    });
                    for (int i = 0; i < sf.an.Length; i++)
                    {
                        anbn.Children.Add(new TextBlock
                        {
                            Text = "a" + (i + 1).ToString() + ": " + sf.an[i].ToString() +
                            "    b" + (i + 1).ToString() + ": " + sf.bn[i].ToString(),
                            VerticalAlignment = VerticalAlignment.Stretch,
                            TextAlignment = TextAlignment.Left
                        });
                    }
                    Dynamics.GetValues(sf.Function, 0, sf.Period, 0.001, out KeyValuePair<double, double>[] values);
                    var othvals = new List<KeyValuePair<double, double>>();
                    if (UI.NewFunction.Type == FunctionType.even)
                    {
                        foreach (var item in values)
                        {
                            //othvals.Add(new KeyValuePair<double, double>(item.Key, item.Value));
                            othvals.Add(new KeyValuePair<double, double>(-item.Key, item.Value));
                        }
                    }
                    else if (UI.NewFunction.Type == FunctionType.odd)
                    {
                        foreach (var item in values)
                        {
                            //othvals.Add(new KeyValuePair<double, double>(item.Key, item.Value));
                            othvals.Add(new KeyValuePair<double, double>(-item.Key, -item.Value));
                        }
                    }
                    foreach (var item in values)
                    {
                        othvals.Add(new KeyValuePair<double, double>(item.Key, item.Value));
                    }
                    Function.Draw(othvals.ToArray());
                }
            }
            else
            {
                var sf = new PiecewiseFourier(40);
                for (int i = 0; i < UI.NewFunction.Pieses.Count; i++)
                {
                    sf.AddCriterion(UI.NewFunction.Pieses[i], UI.NewFunction.Froms[i], UI.NewFunction.Tos[i]);
                }
                sf.Criterioned();
                ffff = sf;
                var vs = new List<KeyValuePair<double, double>>();
                foreach (var item in sf.Values)
                {
                    vs.Add(new KeyValuePair<double, double>(item.Key, item.Value));
                }
                Function.Draw(vs.ToArray());
                Fourier.Draw(sf.FourierSeries(30), -10, 10, 0.01);
                anbn.Children.Add(new TextBlock
                {
                    Text = "a0: " + sf.a0.ToString(),
                    VerticalAlignment = VerticalAlignment.Stretch,
                    TextAlignment = TextAlignment.Left
                });
                for (int i = 0; i < sf.an.Length; i++)
                {
                    anbn.Children.Add(new TextBlock
                    {
                        Text = "a" + (i + 1).ToString() + ": " + sf.an[i].ToString() +
                        "    b" + (i + 1).ToString() + ": " + sf.bn[i].ToString(),
                        VerticalAlignment = VerticalAlignment.Stretch,
                        TextAlignment = TextAlignment.Left
                    });
                }
            }
            MainGrid.Children.Remove(gg);
        }
        dynamic ffff = null;
        private void Fourier_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                if (ffff != null)
                {
                    var ns = new Ns(ffff);
                    var p = new PopUp()
                    {
                        Height = 425,
                        Width = 805,
                        WindowStyle = WindowStyle.SingleBorderWindow,
                        Title = "Fouriers",
                        Owner = this,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        AllowsTransparency = false
                    };
                    ns.VerticalAlignment = VerticalAlignment.Center;
                    ns.HorizontalAlignment = HorizontalAlignment.Center;
                    p.Content = ns;
                    p.ShowDialog();
                }
            }
        }

        private void n1_KeyDown(object sender, KeyEventArgs e)
        {
            var temp = (sender as TextBox);
            if (e.Key == Key.Enter)
            {
                if (temp.Text == string.Empty) Fourier.Cleare(1);
                else
                {
                    int.TryParse(temp.Text, out int v);
                    Fourier.Draw(ffff.FourierSeries(v), -10, 10, 0.01, 1);
                }
                
            }
        }

        private void n2_KeyDown(object sender, KeyEventArgs e)
        {
            var temp = (sender as TextBox);
            if (e.Key == Key.Enter)
            {
                if (temp.Text == string.Empty) Fourier.Cleare(2);
                else
                {
                    int.TryParse(temp.Text, out int v);
                    Fourier.Draw(ffff.FourierSeries(v), -10, 10, 0.01, 2);
                }
                
            }
        }

        private void n3_KeyDown(object sender, KeyEventArgs e)
        {
            var temp = (sender as TextBox);
            if (e.Key == Key.Enter)
            {
                if (temp.Text == string.Empty) Fourier.Cleare(3);
                else
                {
                    int.TryParse(temp.Text, out int v);
                    Fourier.Draw(ffff.FourierSeries(v), -10, 10, 0.01, 3);
                }
                
            }
        }

        private void n4_KeyDown(object sender, KeyEventArgs e)
        {
            var temp = (sender as TextBox);
            if (e.Key == Key.Enter)
            {
                if (temp.Text == string.Empty) Fourier.Cleare(4);
                else
                {
                    int.TryParse(temp.Text, out int v);
                    Fourier.Draw(ffff.FourierSeries(v), -10, 10, 0.01, 4);
                }

            }
        }

        private void n5_KeyDown(object sender, KeyEventArgs e)
        {
            var temp = (sender as TextBox);
            if (e.Key == Key.Enter)
            {
                if (temp.Text == string.Empty) Fourier.Cleare(5);
                else
                {
                    int.TryParse(temp.Text, out int v);
                    Fourier.Draw(ffff.FourierSeries(v), -10, 10, 0.01, 5);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var file = string.Empty;
            foreach (var item in anbn.Children)
            {
                if (item.GetType() == typeof(TextBlock))
                {
                    file += ((item as TextBlock).Text + "\r\n");
                }
            }
            var sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == true)
            {
                System.IO.File.WriteAllText(sfd.FileName+".txt", file);
            }
        }
    }
}
