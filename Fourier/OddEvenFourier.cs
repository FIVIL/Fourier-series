using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI
{
    class OddEvenFourier
    {
        private string function = string.Empty;
        private int n;
        public string Function { get => function; }
        private FunctionType type;
        public FunctionType Type { get => type; }
        private double period;
        public double Period { get => period; }
        public double a0 { get; private set; }
        public double[] an { get; private set; }
        public double[] bn { get; private set; }
        private double A0()
        {
            Dynamics.Integrate(function, -period / 2, period / 2, 0.001, out double res);
            if (Math.Abs(res) < 0.00001) res = 0;
            res = (1 / period) * res;
            if (double.IsNaN(res)) res = 0;
            return res;
        }
        private double[] An()
        {
            if (type == FunctionType.odd)
            {
                double[] temp = new double[n];
                for (int i = 0; i < n; i++)
                {
                    temp[i] = 0;
                }
                return temp;
            }
            var tempp = Dynamics.anEven(function, -period / 2, period / 2, n, period, 0.001);
            return AddDoubleArray(tempp, Dynamics.an(function, -period / 2, period / 2, n, period, 0.001));
        }
        private double[] Bn()
        {
            if (type == FunctionType.even)
            {
                double[] temp = new double[n];
                for (int i = 0; i < n; i++)
                {
                    temp[i] = 0;
                }
                return temp;
            }
            var tempp = Dynamics.bnOdd(function, -period / 2, 0, n, period, 0.001);
            return AddDoubleArray(tempp, Dynamics.bn(function, 0, period / 2, n, period, 0.001));
        }
        public OddEvenFourier(string function, double period, int n, FunctionType ft)
        {
            this.function = function;
            this.period = period * 2;
            type = ft;
            this.n = n;
            a0 = A0();
            an = An();
            bn = Bn();
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
        private static double[] AddDoubleArray(double[] a, double[] b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                a[i] += b[i];
            }
            return a;
        }
    }
}
