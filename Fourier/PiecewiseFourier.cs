using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI
{
    class PiecewiseFourier
    {
        private int n;
        private double period;
        public Dictionary<double,double> Values;
        public double Period { get => period; }
        public double a0 { get; private set; }
        public double[] an { get; private set; }
        public double[] bn { get; private set; }
        private List<Criterion> Criterions;
        public PiecewiseFourier(int n)
        {
            Values = new Dictionary<double, double>();
            Criterions = new List<Criterion>();
            this.n = n;
        }
        public void AddCriterion(string State, double low, double up)
        {
            Criterions.Add(new Criterion()
            {
                Statement = State,
                UpperBound = up,
                LowerBound = low
            });
        }
        public void Criterioned()
        {
            period = Math.Abs(Criterions[0].LowerBound) + Math.Abs(Criterions.Last().UpperBound);
            a0 = A0();
            an = An();
            bn = Bn();
            Evaluate();
        }
        private void Evaluate()
        {
            foreach (var item in Criterions)
            {
                Dynamics.GetValues(item.Statement, item.LowerBound - 0.001, item.UpperBound, 0.001, out KeyValuePair<double, double>[] res);
                foreach (var item2 in res)
                {
                    Values.Add(item2.Key, item2.Value);
                }
            }
        }
        private double A0()
        {
            double result = 0;
            foreach (var item in Criterions)
            {
                Dynamics.Integrate(item.Statement, item.LowerBound, item.UpperBound, 0.001, out double res);
                if (Math.Abs(res) < 0.00001) res = 0;
                res = (1 / period) * res;
                result += res;
            }
            return result;
        }
        private double[] An()
        {
            var ret = new double[n];
            for (int i = 0; i < n; i++)
            {
                ret[i] = 0;
            }
            foreach (var item in Criterions)
            {
                ret = AddDoubleArray(ret,
                    Dynamics.an(item.Statement, item.LowerBound, item.UpperBound, n, period, 0.001));
            }
            return ret;
        }
        private double[] Bn()
        {
            var ret = new double[n];
            for (int i = 0; i < n; i++)
            {
                ret[i] = 0;
            }
            foreach (var item in Criterions)
            {
                ret = AddDoubleArray(ret,
                    Dynamics.bn(item.Statement, item.LowerBound, item.UpperBound, n, period, 0.001));
            }
            return ret;
        }
        private static double[] AddDoubleArray(double[] a,double[] b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                a[i] += b[i];
            }
            return a;
        }
        private Func<double, double> InnerFourier(int n)
        {
            return x => (an[n - 1] * Math.Cos(n * x) + bn[n - 1] * Math.Sin(n * x));
        }
        public Func<double, double> FourierSeries(int m)
        {
            return (x) =>
            {
                double retvalue = a0;
                for (int i = 1; i <= m; i++)
                {
                    retvalue += InnerFourier(i)(x);
                }
                if (Math.Abs(retvalue) < 0.00001) retvalue = 0;
                return retvalue;
            };
        }
    }
    class Criterion
    {
        public string Statement { get; set; }
        public double LowerBound { get; set; }
        public double UpperBound { get; set; }
    }
}
