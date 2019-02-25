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
    /// Interaction logic for NewFunction.xaml
    /// </summary>
    public partial class NewFunction : Page
    {
        public NewFunction()
        {
            InitializeComponent();
        }
        public static string Func;
        public static FunctionType Type;
        public static double Period;
        public static bool Piese;
        public static List<string> Pieses;
        public static List<double> Froms;
        public static List<double> Tos;
        private static string MakePretty(string s)
        {
            //sin
            s = s.Replace("Sin", "sin");
            s = s.Replace("SIN", "sin");
            s = s.Replace("Asin", "aSin");
            s = s.Replace("Arcsin", "aSin");
            s = s.Replace("ARCsin", "aSin");
            s = s.Replace("sinh", "Sinh");
            s = s.Replace("sinH", "Sinh");
            s = s.Replace("sin", "Math.Sin");
            s = s.Replace("aSin", "Math.Asin");
            s = s.Replace("Sinh", "Math.Sinh");
            //cos
            s = s.Replace("Cos", "cos");
            s = s.Replace("COS", "cos");
            s = s.Replace("Acos", "aCos");
            s = s.Replace("Arccos", "aCos");
            s = s.Replace("ARCcos", "aCos");
            s = s.Replace("cosh", "Cosh");
            s = s.Replace("cosH", "Cosh");
            s = s.Replace("cos", "Math.Cos");
            s = s.Replace("aCos", "Math.Acos");
            s = s.Replace("Cosh", "Math.Cosh");
            //tan
            s = s.Replace("Tan", "tan");
            s = s.Replace("TAN", "tan");
            s = s.Replace("Atan", "aTan");
            s = s.Replace("Arctan", "aTan");
            s = s.Replace("ARCtan", "aTan");
            s = s.Replace("tanh", "Tanh");
            s = s.Replace("tanH", "Tanh");
            s = s.Replace("tan", "Math.Tan");
            s = s.Replace("aTan", "Math.Atan");
            s = s.Replace("Tanh", "Math.Tanh");
            //cot
            s = s.Replace("Cot", "cot");
            s = s.Replace("COT", "cot");
            s = s.Replace("cot", "1/Math.Tan");
            //e
            s = s.Replace("E", "Math.E");
            s = s.Replace("e", "Math.E");
            //log
            s = s.Replace("log", "Math.Log");
            s = s.Replace("Log", "Math.Log");
            //sqrt
            s = s.Replace("sqrt", "Sqrt");
            s = s.Replace("SQRT", "Sqrt");
            s = s.Replace("Sqrt", "Math.Sqrt");
            //pi
            s = s.Replace("PI", "Math.PI");
            s = s.Replace("pi", "Math.PI");
            //abs
            s = s.Replace("abs", "Math.Abs");
            s = s.Replace("ABS", "Math.Abs");
            int i = s.IndexOf("|", 0);
            if (i != -1)
            {
                int j = s.IndexOf("|", i + 1);
                s = s.Remove(j, 1);
                s = s.Insert(j, ")");
                s = s.Remove(i, 1);
                s = s.Insert(i, "Math.Abs(");
            }
            //pow
            int start = 0;
            int p = s.IndexOf("^", start);
            start = p;
            while (p != -1)
            {
                s = s.Remove(p, 1);
                s = s.Insert(p, ",");
                int para = 0;
                int iter = p - 1;
                while (true)
                {
                    Char[] sa = s.ToCharArray();
                    if (iter == 0 || (para == 0 && (sa[iter] == '*' || sa[iter] == '/' || sa[iter] == '+' || sa[iter] == '-' || sa[iter] == '(')))
                    {
                        if (iter != 0)
                            s = s.Insert(iter + 1, "Math.Pow(");
                        else
                            s = s.Insert(iter, "Math.Pow(");
                        break;
                    }
                    if (sa[iter] == ')')
                    {
                        para++;
                    }
                    if (para > 0 && sa[iter] == '(')
                    {
                        para--;
                    }
                    iter--;
                }
                para = 0;
                iter = p + 10;
                while (true)
                {
                    Char[] sa = s.ToCharArray();
                    if (iter == sa.Length || (para == 0 && (sa[iter] == '*' || sa[iter] == '/' || sa[iter] == '+' || sa[iter] == '-' || sa[iter] == ')')))
                    {
                        s = s.Insert(iter, ")");
                        break;
                    }
                    if (sa[iter] == '(')
                    {
                        para++;
                    }
                    if (para > 0 && sa[iter] == ')')
                    {


                        para--;
                    }
                    iter++;
                }
                p = s.IndexOf("^", start + 1);
            }
            return s;
        }
        private double bouand(string s)
        {
            s = s.ToUpper();
            double ret;
            if (s.Contains("PI"))
            {
                if (s == "PI")
                    ret = Math.PI;
                else
                {
                    var tempp = s.Replace("PI", "");
                    if (tempp == "-") ret = -Math.PI;
                    else ret = Math.PI * double.Parse(tempp);
                }
            }
            else ret = double.Parse(s);
            return ret;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MainTab.SelectedIndex == 0)
            {
                Period = bouand(period.Text);
                if (Half_range.SelectedIndex == 0) Type = FunctionType.normal;
                else if (Half_range.SelectedIndex == 1) Type = FunctionType.odd;
                else Type = FunctionType.even;
                var temp = "(" + MakePretty(Rule.Text) + ")";
                Func = Value.Text + "=>" + temp;
                Piese = false;
            }
            else
            {
                Pieses = new List<string>();
                Froms = new List<double>();
                Tos = new List<double>();
                Piese = true;
                foreach (var item in MainStack.Children)
                {
                    var temp = (item as function);
                    var ruletemp = "(" + MakePretty(temp.Rule.Text) + ")";
                    Pieses.Add("x=>" + ruletemp);
                    Tos.Add(bouand(temp.To.Text));
                    Froms.Add(bouand(temp.From.Text));
                }
            }
           (this.Parent as Window).Close();
        }
    }
}
