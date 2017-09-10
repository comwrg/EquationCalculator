using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EquationCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("calc：");
                string s = Console.ReadLine();
                var a = NotationTransform.Infix2Postfix(s);
                // Console.WriteLine($"{string.Join(" ", a.ToArray())}");
                try
                {
                    var b = CalcPostfix.Calc(a);
                    Console.Write("x=");
                    foreach (double d in b.GetResult())
                    {
                        Console.Write($"{d}");
                        if (!int.TryParse(d.ToString(CultureInfo.InvariantCulture), out int i))
                        {
                            Console.Write("(超出整数范围)");
                        }
                        Console.Write(",");
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
                finally
                {
                    Console.WriteLine();
                }
                
                // Console.WriteLine($"{b.A}x^2+{b.B}x+{b.C}=0");

            }
        }
    }
}
