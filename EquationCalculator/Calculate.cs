using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;

/*
 * 也叫方程解算机 只对整数 X 加减乘除符号 （ +  -  * / ） 模运算符  %  还有 ^2 就是平方 只对以上这些起作用。
 * 要解决一元一次方程和一元二次方程。注意程序必须遵守数学原则：运算符优先级 乘除还有%模运算必须在加减之前计算 2 运算符计算时必须从左到右 有括号先算括号，
 * +和-可以写在开头或结尾（例如：+3x+15=0）
只需要测试整数，不需要测试小数和虚数。
整数有范围 如果计算结果不是整数，就打印出“超出整数范围”   如果除数为0 就报错“除数不能为0” 多个加号（减号） 自动认为一个加号（减号） 
作业要通过类似类型的测试 
x+1=2
2x-5=5
x=5*x-5*3
5(3)+2x=25
x/3=7
x^2+7*x+6*2=0
(x-9)(x-1)=0
2(3x)+3x=81
2(x-1)+8=4*x-20
4x^2-11*2=x^2+5
*/
namespace EquationCalculator
{
    class Calculate
    {
        public string Calc(string exp)
        {
            exp = exp.Replace(" ", "");
            exp = FillOmitted(exp);
            exp = Simplification(exp);
            Log(exp);
            Log(CalcOneDollarEquation(exp).ToString(CultureInfo.InvariantCulture));
            return "";
        }

        private double CalcOneDollarEquation(string exp)
        {
            // \dx, \d
            // ax + C = 0
            // x = -C/B
            double coefficientOfX = CalcCoefficent(exp, @"([+\-]\d+)x");
            double constant = CalcCoefficent(exp, @"([+\-]\d+)(?!x)");
            Log($"coefficient of x: {coefficientOfX}, constant: {constant}");
            return -constant / coefficientOfX;
        }

        private string FillOmitted(string notation)
        {
            #region add plus

            // 1. x+1=2 -> +x+1=+2
            string[] arr = SplitExp(notation);
            for (int i = 0; i < 2; i++)
            {
                string s = arr[i];
                if (!(s.StartsWith("+") || s.StartsWith("-")))
                {
                    arr[i] = "+" + arr[i];
                }
            }
            notation = $"{arr[0]}={arr[1]}";

            // 2. (3x) -> (+3x)
            var mc = Regex.Matches(notation, @"\((?!\+|-).");
            foreach (Match m in mc)
            {
                string s = m.Groups[0].Value;
                notation = notation.Replace(s, s.Replace("(", "(+"));
            }

            #endregion

            #region fill coefficient

            notation = notation.Replace("+x", "+1x");
            notation = notation.Replace("-x", "-1x");

            #endregion

            return notation;
        }

        private string[] SplitExp(string exp)
        {
            string[] arr =  exp.Split('=');
            if (arr.Length != 2)
            {
                throw new Exception("expression error");
            }
            return arr;
        }

        private int CalcCoefficent(string exp, string pattern)
        {
            string[] arr = SplitExp(exp);
            int sum = 0;
            for (int i = 0; i < 2; i++)
            {
                MatchCollection mc = Regex.Matches(arr[i], pattern);
                foreach (Match m in mc)
                {
                    int coefficent = Convert.ToInt32(m.Groups[1].Value);
                    if (i == 0)
                    {
                        sum += coefficent;
                    }
                    else if (i == 1)
                    {
                        sum -= coefficent;
                    }
                }
            }
            
            return sum;
        }

        private void Log(string s)
        {
            Console.WriteLine(s);
        }

        private string Simplification(string exp)
        {
            // 5*x -> 5x
            var mc = Regex.Matches(exp, @"\d+\*x");
            foreach (Match m in mc)
            {
                exp = exp.Replace(m.Groups[0].Value, m.Groups[0].Value.Replace("*", ""));
            }
            

            // 5(3) -> 5*3
            mc = Regex.Matches(exp, @"\d+\(\d+\)");
            foreach (Match m in mc)
            {
                string s = m.Groups[0].Value.Replace("(", "*").Replace(")", "");
                exp = exp.Replace(m.Groups[0].Value, s);
            }

            // 5[+-*/]3 -> 15
            mc = Regex.Matches(exp, @"(\d+)([+\-*/])(\d+)");
            foreach (Match m in mc)
            {
                int i = 0;
                int a = Convert.ToInt32(m.Groups[1].Value);
                int b = Convert.ToInt32(m.Groups[3].Value);
                switch (m.Groups[2].Value)
                {
                    case "+":
                        i = a + b;
                        break;
                    case "-":
                        i = a - b;
                        break;
                    case "*":
                        i = a * b;
                        break;
                    case "/":
                        i = a / b;
                        break;
                }
                exp = exp.Replace(m.Groups[0].Value, i.ToString());
            }

            return exp;

            

        }
    }
}
