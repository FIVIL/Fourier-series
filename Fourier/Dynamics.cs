using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace UI
{
    static class Dynamics
    {
        private static string SimpsonCode(string lambda, string a, string b, string desiredRelativeError, string path = "\"res.txt\"")
        {
            return @"using System;
using System.IO;
namespace MohandesiProject
{
    public static class Program
    {
        static public double Integrate(Func<double, double> f, double a, double b, double desiredRelativeError)
        {
            int log2MaxFunctionEvals = 20;
            int functionEvalsUsed;
            double estimatedError;

            return Integrate(f, a, b, desiredRelativeError, log2MaxFunctionEvals, out functionEvalsUsed, out estimatedError);
        }
        static public double Integrate(Func<double, double> f, double a, double b, double relativeErrorTolerance, int log2MaxFunctionEvals, out int functionEvalsUsed, out double estimatedError)
        {
            double integral = 0.0;
            double mostRecentContribution = 0.0;
            double previousContribution = 0.0;
            double previousIntegral = 0.0;
            double sum = 0.0;

            functionEvalsUsed = 0;
            estimatedError = double.MaxValue;

            for (int stage = 0; stage <= log2MaxFunctionEvals; stage++)
            {
                if (stage == 0)
                {
                    sum = f(a) + f(b);
                    functionEvalsUsed = 2;
                    integral = sum * 0.5 * (b - a);
                }
                else
                {
                    // Pattern of Simpson's rule coefficients:
                    //
                    // 1               1
                    // 1       4       1
                    // 1   4   2   4   1
                    // 1 4 2 4 2 4 2 4 1
                    // ...
                    //
                    // Each row multiplies new function evaluations by 4, and the evalutations from the previous step by 2.

                    int numNewPts = 1 << (stage - 1);
                    mostRecentContribution = 0.0;
                    double h = (b - a) / numNewPts;
                    double x = a + 0.5 * h;
                    for (int i = 0; i < numNewPts; i++)
                        mostRecentContribution += f(x + i * h);
                    functionEvalsUsed += numNewPts;
                    mostRecentContribution *= 4.0;
                    sum += mostRecentContribution - 0.5 * previousContribution;

                    integral = sum * (b - a) / ((1 << stage) * 3.0);

                    // Require at least five stages to reduce the risk of incorrectly declaring convergence too soon.
                    // Note that you can specify fewer stages, but the early termination rule below will not be used.
                    if (stage >= 5)
                    {
                        estimatedError = Math.Abs(integral - previousIntegral); // conservative
                        if (estimatedError <= relativeErrorTolerance * Math.Abs(previousIntegral))
                            return integral;
                    }
                    previousContribution = mostRecentContribution;
                    previousIntegral = integral;
                }
            }
            return integral;
        }
        static void Main(string[] args)
        {
            var res = Integrate(" + lambda + "," + a + "," + b + "," + desiredRelativeError + @" );
            File.WriteAllText(" + path + @",res.ToString());
        }
    }
}";
        }
        private static string anCode(string lambda, string a, string b, string n,string period, string desiredRelativeError, string path = "\"res.txt\"")
        {
            string punch = "\";\"";
            return @"using System;
using System.IO;
namespace MohandesiProject
{
    public static class Program
    {
        static public double Integrate(Func<double, double> f, double a, double b, double desiredRelativeError)
        {
            int log2MaxFunctionEvals = 20;
            int functionEvalsUsed;
            double estimatedError;

            return Integrate(f, a, b, desiredRelativeError, log2MaxFunctionEvals, out functionEvalsUsed, out estimatedError);
        }
        static public double Integrate(Func<double, double> f, double a, double b, double relativeErrorTolerance, int log2MaxFunctionEvals, out int functionEvalsUsed, out double estimatedError)
        {
            double integral = 0.0;
            double mostRecentContribution = 0.0;
            double previousContribution = 0.0;
            double previousIntegral = 0.0;
            double sum = 0.0;

            functionEvalsUsed = 0;
            estimatedError = double.MaxValue;

            for (int stage = 0; stage <= log2MaxFunctionEvals; stage++)
            {
                if (stage == 0)
                {
                    sum = f(a) + f(b);
                    functionEvalsUsed = 2;
                    integral = sum * 0.5 * (b - a);
                }
                else
                {
                    // Pattern of Simpson's rule coefficients:
                    //
                    // 1               1
                    // 1       4       1
                    // 1   4   2   4   1
                    // 1 4 2 4 2 4 2 4 1
                    // ...
                    //
                    // Each row multiplies new function evaluations by 4, and the evalutations from the previous step by 2.

                    int numNewPts = 1 << (stage - 1);
                    mostRecentContribution = 0.0;
                    double h = (b - a) / numNewPts;
                    double x = a + 0.5 * h;
                    for (int i = 0; i < numNewPts; i++)
                        mostRecentContribution += f(x + i * h);
                    functionEvalsUsed += numNewPts;
                    mostRecentContribution *= 4.0;
                    sum += mostRecentContribution - 0.5 * previousContribution;

                    integral = sum * (b - a) / ((1 << stage) * 3.0);

                    // Require at least five stages to reduce the risk of incorrectly declaring convergence too soon.
                    // Note that you can specify fewer stages, but the early termination rule below will not be used.
                    if (stage >= 5)
                    {
                        estimatedError = Math.Abs(integral - previousIntegral); // conservative
                        if (estimatedError <= relativeErrorTolerance * Math.Abs(previousIntegral))
                            return integral;
                    }
                    previousContribution = mostRecentContribution;
                    previousIntegral = integral;
                }
            }
            return integral;
        }
        static void Main(string[] args)
        {
            string Res = string.Empty;
            for (int i = 1; i <= "+n+@"; i++)
            {
                var res = Integrate(" + lambda + "*Math.Cos((2*Math.PI/" + period + ")*i*x)" + ", " + a + ", " + b + ", " + desiredRelativeError + @");
                Res += (res +" + punch + @");
            }
        File.WriteAllText(" + path + @", Res);
        }
    }
}";
        }
        private static string anCodeEven(string lambda, string a, string b, string n, string period, string desiredRelativeError, string path = "\"res.txt\"")
        {
            string punch = "\";\"";
            return @"using System;
using System.IO;
namespace MohandesiProject
{
    public static class Program
    {
        static public double Integrate(Func<double, double> f, double a, double b, double desiredRelativeError)
        {
            int log2MaxFunctionEvals = 20;
            int functionEvalsUsed;
            double estimatedError;

            return Integrate(f, a, b, desiredRelativeError, log2MaxFunctionEvals, out functionEvalsUsed, out estimatedError);
        }
        static public double Integrate(Func<double, double> f, double a, double b, double relativeErrorTolerance, int log2MaxFunctionEvals, out int functionEvalsUsed, out double estimatedError)
        {
            double integral = 0.0;
            double mostRecentContribution = 0.0;
            double previousContribution = 0.0;
            double previousIntegral = 0.0;
            double sum = 0.0;

            functionEvalsUsed = 0;
            estimatedError = double.MaxValue;

            for (int stage = 0; stage <= log2MaxFunctionEvals; stage++)
            {
                if (stage == 0)
                {
                    sum = f(a) + f(b);
                    functionEvalsUsed = 2;
                    integral = sum * 0.5 * (b - a);
                }
                else
                {
                    // Pattern of Simpson's rule coefficients:
                    //
                    // 1               1
                    // 1       4       1
                    // 1   4   2   4   1
                    // 1 4 2 4 2 4 2 4 1
                    // ...
                    //
                    // Each row multiplies new function evaluations by 4, and the evalutations from the previous step by 2.

                    int numNewPts = 1 << (stage - 1);
                    mostRecentContribution = 0.0;
                    double h = (b - a) / numNewPts;
                    double x = a + 0.5 * h;
                    for (int i = 0; i < numNewPts; i++)
                        mostRecentContribution += f(x + i * h);
                    functionEvalsUsed += numNewPts;
                    mostRecentContribution *= 4.0;
                    sum += mostRecentContribution - 0.5 * previousContribution;

                    integral = sum * (b - a) / ((1 << stage) * 3.0);

                    // Require at least five stages to reduce the risk of incorrectly declaring convergence too soon.
                    // Note that you can specify fewer stages, but the early termination rule below will not be used.
                    if (stage >= 5)
                    {
                        estimatedError = Math.Abs(integral - previousIntegral); // conservative
                        if (estimatedError <= relativeErrorTolerance * Math.Abs(previousIntegral))
                            return integral;
                    }
                    previousContribution = mostRecentContribution;
                    previousIntegral = integral;
                }
            }
            return integral;
        }
        private static Func<double, double> Even(Func<double, double> f)
        {
            return x => { return f(Math.Abs(x)); };
        }
        static void Main(string[] args)
        {
            string Res = string.Empty;
            for (int i = 1; i <= " + n + @"; i++)
            {
                var res = Integrate(Even(" + lambda + "*Math.Cos((2*Math.PI/" + period + ")*i*x)" + "), " + a + ", " + b + ", " + desiredRelativeError + @");
                Res += (res +" + punch + @");
            }
        File.WriteAllText(" + path + @", Res);
        }
    }
}";
        }
        private static string bnCode(string lambda, string a, string b, string n, string period, string desiredRelativeError, string path = "\"res.txt\"")
        {
            string punch = "\";\"";
            return @"using System;
using System.IO;
namespace MohandesiProject
{
    public static class Program
    {
        static public double Integrate(Func<double, double> f, double a, double b, double desiredRelativeError)
        {
            int log2MaxFunctionEvals = 20;
            int functionEvalsUsed;
            double estimatedError;

            return Integrate(f, a, b, desiredRelativeError, log2MaxFunctionEvals, out functionEvalsUsed, out estimatedError);
        }
        static public double Integrate(Func<double, double> f, double a, double b, double relativeErrorTolerance, int log2MaxFunctionEvals, out int functionEvalsUsed, out double estimatedError)
        {
            double integral = 0.0;
            double mostRecentContribution = 0.0;
            double previousContribution = 0.0;
            double previousIntegral = 0.0;
            double sum = 0.0;

            functionEvalsUsed = 0;
            estimatedError = double.MaxValue;

            for (int stage = 0; stage <= log2MaxFunctionEvals; stage++)
            {
                if (stage == 0)
                {
                    sum = f(a) + f(b);
                    functionEvalsUsed = 2;
                    integral = sum * 0.5 * (b - a);
                }
                else
                {
                    // Pattern of Simpson's rule coefficients:
                    //
                    // 1               1
                    // 1       4       1
                    // 1   4   2   4   1
                    // 1 4 2 4 2 4 2 4 1
                    // ...
                    //
                    // Each row multiplies new function evaluations by 4, and the evalutations from the previous step by 2.

                    int numNewPts = 1 << (stage - 1);
                    mostRecentContribution = 0.0;
                    double h = (b - a) / numNewPts;
                    double x = a + 0.5 * h;
                    for (int i = 0; i < numNewPts; i++)
                        mostRecentContribution += f(x + i * h);
                    functionEvalsUsed += numNewPts;
                    mostRecentContribution *= 4.0;
                    sum += mostRecentContribution - 0.5 * previousContribution;

                    integral = sum * (b - a) / ((1 << stage) * 3.0);

                    // Require at least five stages to reduce the risk of incorrectly declaring convergence too soon.
                    // Note that you can specify fewer stages, but the early termination rule below will not be used.
                    if (stage >= 5)
                    {
                        estimatedError = Math.Abs(integral - previousIntegral); // conservative
                        if (estimatedError <= relativeErrorTolerance * Math.Abs(previousIntegral))
                            return integral;
                    }
                    previousContribution = mostRecentContribution;
                    previousIntegral = integral;
                }
            }
            return integral;
        }
        static void Main(string[] args)
        {
            string Res = string.Empty;
            for (int i = 1; i <= " + n + @"; i++)
            {
                var res = Integrate(" + lambda + "*Math.Sin((2*Math.PI/" + period + ")*i*x)" + ", " + a + ", " + b + ", " + desiredRelativeError + @");
                Res += (res +" + punch + @");
            }
        File.WriteAllText(" + path + @", Res);
        }
    }
}";
        }
        private static string bnCodeOdd(string lambda, string a, string b, string n, string period, string desiredRelativeError, string path = "\"res.txt\"")
        {
            string punch = "\";\"";
            return @"using System;
using System.IO;
namespace MohandesiProject
{
    public static class Program
    {
        static public double Integrate(Func<double, double> f, double a, double b, double desiredRelativeError)
        {
            int log2MaxFunctionEvals = 20;
            int functionEvalsUsed;
            double estimatedError;

            return Integrate(f, a, b, desiredRelativeError, log2MaxFunctionEvals, out functionEvalsUsed, out estimatedError);
        }
        static public double Integrate(Func<double, double> f, double a, double b, double relativeErrorTolerance, int log2MaxFunctionEvals, out int functionEvalsUsed, out double estimatedError)
        {
            double integral = 0.0;
            double mostRecentContribution = 0.0;
            double previousContribution = 0.0;
            double previousIntegral = 0.0;
            double sum = 0.0;

            functionEvalsUsed = 0;
            estimatedError = double.MaxValue;

            for (int stage = 0; stage <= log2MaxFunctionEvals; stage++)
            {
                if (stage == 0)
                {
                    sum = f(a) + f(b);
                    functionEvalsUsed = 2;
                    integral = sum * 0.5 * (b - a);
                }
                else
                {
                    // Pattern of Simpson's rule coefficients:
                    //
                    // 1               1
                    // 1       4       1
                    // 1   4   2   4   1
                    // 1 4 2 4 2 4 2 4 1
                    // ...
                    //
                    // Each row multiplies new function evaluations by 4, and the evalutations from the previous step by 2.

                    int numNewPts = 1 << (stage - 1);
                    mostRecentContribution = 0.0;
                    double h = (b - a) / numNewPts;
                    double x = a + 0.5 * h;
                    for (int i = 0; i < numNewPts; i++)
                        mostRecentContribution += f(x + i * h);
                    functionEvalsUsed += numNewPts;
                    mostRecentContribution *= 4.0;
                    sum += mostRecentContribution - 0.5 * previousContribution;

                    integral = sum * (b - a) / ((1 << stage) * 3.0);

                    // Require at least five stages to reduce the risk of incorrectly declaring convergence too soon.
                    // Note that you can specify fewer stages, but the early termination rule below will not be used.
                    if (stage >= 5)
                    {
                        estimatedError = Math.Abs(integral - previousIntegral); // conservative
                        if (estimatedError <= relativeErrorTolerance * Math.Abs(previousIntegral))
                            return integral;
                    }
                    previousContribution = mostRecentContribution;
                    previousIntegral = integral;
                }
            }
            return integral;
        }
        private static Func<double, double> Odd(Func<double, double> f)
        {
            return x =>
            {
                if (x < 0) return -f(Math.Abs(x));
                else return f(x);
            };
        }
        static void Main(string[] args)
        {
            string Res = string.Empty;
            for (int i = 1; i <= " + n + @"; i++)
            {
                var res = Integrate(Odd(" + lambda + "*Math.Sin((2*Math.PI/" + period + ")*i*x)" + "), " + a + ", " + b + ", " + desiredRelativeError + @");
                Res += (res +" + punch + @");
            }
        File.WriteAllText(" + path + @", Res);
        }
    }
}";
        }
        private static string OddOrEvenEvaluateCode(string lambda, string a, string b, string path = "\"res.txt\"")
        {
            return @"using System;
using System.IO;
namespace MohandesiProject
{
    class Program
    {
        static int Worker(Func<double,double> f,double a,double b)
        {
            //odd
            bool odd = true;
            for (double i = a; i <= b; i+=0.001)
            {
                if(f(i)!=-f(-i))
                {
                    odd = false;
                    break;
                }
            }
            if (odd) return 0;
            //even
            bool even = true;
            for (double i = a; i <= b; i += 0.001)
            {
                if (f(i) != f(-i))
                {
                    even = false;
                    break;
                }
            }
            if (even) return 1;
            //none
            return (2);
        }
        static void Main(string[] args)
        {
            File.WriteAllText(" + path + "," + " Worker(" + lambda + "," + a + "," + b + @").ToString());
        }
    }
}
";
        }
        private static string GetValuesCode(string lambda, string a, string b, string step, string path = "\"res.txt\"")
        {
            string punch = "\";\"";
            return @"using System;
using System.IO;
namespace MohandesiProject
{
    class Program
    {
        static string Worker(Func<double,double> f,double a,double b,double step)
        {
            string res = string.Empty;
            for (double i = a; i < b; i+=step)
            {
                res += f(i).ToString() + " + punch + @";
            }
        res += f(b).ToString();
            return res;
        }
    static void Main(string[] args)
    {
        File.WriteAllText(" + path + "," + " Worker(" + lambda + "," + a + "," + b + "," + step + @"));
        //Console.WriteLine(d);
        //Console.ReadKey();
    }
}
}
";
        }
        private static bool CompileExecutable(String sourceName)
        {
            FileInfo sourceFile = new FileInfo(sourceName);
            CodeDomProvider provider = null;
            bool compileOk = false;

            // Select the code provider based on the input file extension.
            if (sourceFile.Extension.ToUpper(CultureInfo.InvariantCulture) == ".CS")
            {
                provider = CodeDomProvider.CreateProvider("CSharp");
            }
            else if (sourceFile.Extension.ToUpper(CultureInfo.InvariantCulture) == ".VB")
            {
                provider = CodeDomProvider.CreateProvider("VisualBasic");
            }
            else
            {
                Console.WriteLine("Source file must have a .cs or .vb extension");
            }

            if (provider != null)
            {

                // Format the executable file name.
                // Build the output assembly path using the current directory
                // and <source>_cs.exe or <source>_vb.exe.

                String exeName = String.Format(@"{0}\{1}.exe",
                    System.Environment.CurrentDirectory,
                    sourceFile.Name.Replace(".", "_"));

                CompilerParameters cp = new CompilerParameters();

                // Generate an executable instead of 
                // a class library.
                cp.GenerateExecutable = true;

                // Specify the assembly file name to generate.
                cp.OutputAssembly = exeName;

                // Save the assembly as a physical file.
                cp.GenerateInMemory = false;

                // Set whether to treat all warnings as errors.
                cp.TreatWarningsAsErrors = false;

                // Invoke compilation of the source file.
                CompilerResults cr = provider.CompileAssemblyFromFile(cp,
                    sourceName);

                if (cr.Errors.Count > 0)
                {
                    // Display compilation errors.
                    Console.WriteLine("Errors building {0} into {1}",
                        sourceName, cr.PathToAssembly);
                    foreach (CompilerError ce in cr.Errors)
                    {
                        Console.WriteLine("  {0}", ce.ToString());
                        Console.WriteLine();
                    }
                }
                else
                {
                    // Display a successful compilation message.
                    //Console.WriteLine("Source {0} built into {1} successfully.",
                    //    sourceName, cr.PathToAssembly);
                }

                // Return the results of the compilation.
                if (cr.Errors.Count > 0)
                {
                    compileOk = false;
                }
                else
                {
                    compileOk = true;
                }
            }
            return compileOk;
        }
        /// <summary>
        /// Simplest interface, returning no diagnostic data
        /// </summary>
        /// <param name="lambda">integrand</param>
        /// <param name="a">left limit of integration</param>
        /// <param name="b">right limit of integration</param>
        /// <param name="desiredRelativeError">relative error tolerance</param>
        /// <param name="result">Integral value</param>
        /// <returns>Boolean value</returns>
        public static bool Integrate(string lambda, double a, double b, double desiredRelativeError, out double result)
        {
            File.WriteAllText("code.cs", SimpsonCode(lambda, a.ToString(), b.ToString(), desiredRelativeError.ToString()));
            var code = CompileExecutable("code.cs");
            if (code == true)
            {
                File.Delete("code.cs");
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.FileName = "code_cs.exe";
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.WaitForExit();
                File.Delete("code_cs.exe");
                var res = File.ReadAllText("res.txt");
                var temp = double.TryParse(res, out result);
                File.Delete("res.txt");
                return temp;
            }
            else
            {
                result = 0;
                return false;
            }
        }
        public static double[] an(string lambda, double a, double b, int n, double period, double desiredRelativeError)
        {
            File.WriteAllText("code.cs", anCode(lambda, a.ToString(), b.ToString(),n.ToString(),period.ToString(), desiredRelativeError.ToString()));
            var code = CompileExecutable("code.cs");
            if (code == true)
            {
                File.Delete("code.cs");
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.FileName = "code_cs.exe";
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.WaitForExit();
                File.Delete("code_cs.exe");
                var tempp = File.ReadAllText("res.txt");
                var res = tempp.Split(';');
                File.Delete("res.txt");
                double[] rets = new double[n];
                int index = 0;
                foreach (var item in res)
                {
                    try
                    {
                        var temp = double.Parse(item);
                        temp = (2 / period) * temp;
                        if (Math.Abs(temp) < 0.00001) temp = 0;
                        rets[index++] = temp;
                    }
                    catch { }
                }
                return rets;
            }
            return null;
        }
        public static double[] anEven(string lambda, double a, double b, int n, double period, double desiredRelativeError)
        {
            File.WriteAllText("code.cs", anCodeEven(lambda, a.ToString(), b.ToString(), n.ToString(), period.ToString(), desiredRelativeError.ToString()));
            var code = CompileExecutable("code.cs");
            if (code == true)
            {
                File.Delete("code.cs");
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.FileName = "code_cs.exe";
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.WaitForExit();
                File.Delete("code_cs.exe");
                var tempp = File.ReadAllText("res.txt");
                var res = tempp.Split(';');
                File.Delete("res.txt");
                double[] rets = new double[n];
                int index = 0;
                foreach (var item in res)
                {
                    try
                    {
                        var temp = double.Parse(item);
                        temp = (2 / period) * temp;
                        if (Math.Abs(temp) < 0.00001) temp = 0;
                        rets[index++] = temp;
                    }
                    catch { }
                }
                return rets;
            }
            return null;
        }
        public static double[] bn(string lambda, double a, double b, int n, double period, double desiredRelativeError)
        {
            File.WriteAllText("code.cs", bnCode(lambda, a.ToString(), b.ToString(), n.ToString(), period.ToString(), desiredRelativeError.ToString()));
            var code = CompileExecutable("code.cs");
            if (code == true)
            {
                File.Delete("code.cs");
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.FileName = "code_cs.exe";
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.WaitForExit();
                File.Delete("code_cs.exe");
                var tempp = File.ReadAllText("res.txt");
                var res = tempp.Split(';');
                File.Delete("res.txt");
                double[] rets = new double[n];
                int index = 0;
                foreach (var item in res)
                {
                    try
                    {
                        var temp = double.Parse(item);
                        temp = (2 / period) * temp;
                        if (Math.Abs(temp) < 0.00001) temp = 0;
                        rets[index++] = temp;
                    }
                    catch { }
                }
                return rets;
            }
            return null;
        }
        public static double[] bnOdd(string lambda, double a, double b, int n, double period, double desiredRelativeError)
        {
            File.WriteAllText("code.cs", bnCodeOdd(lambda, a.ToString(), b.ToString(), n.ToString(), period.ToString(), desiredRelativeError.ToString()));
            var code = CompileExecutable("code.cs");
            if (code == true)
            {
                File.Delete("code.cs");
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.FileName = "code_cs.exe";
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.WaitForExit();
                File.Delete("code_cs.exe");
                var tempp = File.ReadAllText("res.txt");
                var res = tempp.Split(';');
                File.Delete("res.txt");
                double[] rets = new double[n];
                int index = 0;
                foreach (var item in res)
                {
                    try
                    {
                        var temp = double.Parse(item);
                        temp = (2 / period) * temp;
                        if (Math.Abs(temp) < 0.00001) temp = 0;
                        rets[index++] = temp;
                    }
                    catch { }
                }
                return rets;
            }
            return null;
        }
        public static bool EvaluateOddOrEven(string lambda, double a, double b, out FunctionType ft)
        {
            File.WriteAllText("code.cs", OddOrEvenEvaluateCode(lambda, a.ToString(), b.ToString()));
            var code = CompileExecutable("code.cs");
            if (code == true)
            {
                File.Delete("code.cs");
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.FileName = "code_cs.exe";
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.WaitForExit();
                File.Delete("code_cs.exe");
                var res = File.ReadAllText("res.txt");
                int result = -1;
                var temp = int.TryParse(res, out result);
                File.Delete("res.txt");
                if (result == 0) ft = FunctionType.odd;
                else if (result == 1) ft = FunctionType.even;
                else ft = FunctionType.normal;
                return temp;
            }
            else
            {
                ft = FunctionType.normal;
                return false;
            }
        }
        public static bool GetValues(string lambda, double a, double b, double step, out KeyValuePair<double, double>[] result)
        {
            File.WriteAllText("code.cs", GetValuesCode(lambda, a.ToString(), b.ToString(), step.ToString()));
            var code = CompileExecutable("code.cs");
            if (code == true)
            {
                File.Delete("code.cs");
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.FileName = "code_cs.exe";
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.WaitForExit();
                File.Delete("code_cs.exe");
                var res = File.ReadAllText("res.txt");
                var temp = res.Split(';');
                File.Delete("res.txt");
                result = new KeyValuePair<double, double>[temp.Length];
                double init = a;
                int index = 0;
                foreach (var item in temp)
                {
                    try
                    {
                        var temp1 = double.Parse(item);
                        result[index] = new KeyValuePair<double, double>(init, temp1);
                        index++;
                        init += step;
                    }
                    catch { }
                }
                return true;
            }
            else
            {
                result = new KeyValuePair<double, double>[1];
                return false;
            }
        }
    }
}
