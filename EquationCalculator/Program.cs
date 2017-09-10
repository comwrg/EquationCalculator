using System;
using System.Collections.Generic;
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
                Console.WriteLine("请输入方程后按回车：");
                string s = Console.ReadLine();
                var a = NotationTransform.Infix2Postfix(s);
                Console.WriteLine($"{string.Join(" ", a.ToArray())}");
                var b = CalcPostfix.Calc(a);
                Console.WriteLine($"{b.A}x^2+{b.B}x+{b.C}=0");
                foreach (double d in b.GetResult())
                {
                    Console.WriteLine($"答案：{d}");
                }
            }
        }
    }
}
