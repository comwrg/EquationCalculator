using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EquationCalculator
{
    public class CalcPostfix
    {
        public static DollarEquation Calc(List<string> list)
        {
            var stack = new Stack<DollarEquation>();
            foreach (string s in list)
            {
                if (int.TryParse(s, out int i))
                {
                    DollarEquation o = new DollarEquation {C = i};
                    stack.Push(o);
                    
                }
                else if (s.Contains("x"))
                {
                    DollarEquation o;
                    if (s.Contains("x*x") || s.Contains("x^2"))
                    {
                        o = new DollarEquation {A = int.Parse(s.Replace("x*x", "").Replace("x^2", ""))};
                    }
                    else
                    {
                        o = new DollarEquation { B = int.Parse(s.Replace("x", "")) };
                    }
                    stack.Push(o);
                }
                else
                {
                    switch (s)
                    {
                        case "+":
                            stack.Push(stack.Pop() + stack.Pop());
                            break;
                        case "-":
                            stack.Push(stack.Pop() - stack.Pop());
                            break;
                        case "*":
                            stack.Push(stack.Pop() * stack.Pop());
                            break;
                        case "/":
                            stack.Push(stack.Pop() / stack.Pop());
                            break;
                        case "%":
                            stack.Push(stack.Pop() % stack.Pop());
                            break;
                    }
                }
            }
            return stack.Pop();
        }
    }
}
