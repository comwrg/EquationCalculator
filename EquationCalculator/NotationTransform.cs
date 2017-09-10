using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace EquationCalculator
{
    public class NotationTransform
    {
        public static List<string> Infix2Postfix(string notation)
        {
            notation = notation.Replace(" ", "");
            if (notation.Contains("="))
                notation = Equation2Notation(notation);
            var oper = new Stack<string>();
            var output = new List<string>();
            foreach (var s in GetItem(Simplification(notation)))
            {
                if (int.TryParse(s, out var i) || s.Contains("x"))
                {
                    output.Add(s);
                }
                else if (s == ")")
                {
                    while (true)
                    {
                        var o = oper.Pop();
                        if (o != "(")
                            output.Add(o);
                        else
                            break;
                    }
                }
                else if (oper.Count == 0 || IsHigher(oper.Peek(), s) < 0)
                {
                    oper.Push(s);
                }
                else
                {
                    while (oper.Count != 0 && IsHigher(s, oper.Peek()) <= 0 && oper.Peek() != "(")
                        output.Add(oper.Pop());
                    oper.Push(s);
                }
//                Console.WriteLine(
//                    $"oper: {string.Join(" ", oper.ToArray())},, output: {string.Join(" ", output.ToArray())}");
            }
            while (oper.Count != 0)
                output.Add(oper.Pop());
            return output;
        }

        public static List<string> GetItem(string notation)
        {
            var rlen = 0;
            var isSymbol = false;
            var list = new List<string>();
            while (notation.Length != 0)
            {
                var m = Regex.Match(notation, isSymbol ? @"^\-?\d+x?(?:\^\d)?" : @"^\d+x?(?:\^\d)?");
                if (m.Success)
                {
                    list.Add(m.Value);
                    rlen = m.Length;
                    isSymbol = false;
                }
                else
                {
                    list.Add(notation.Substring(0, 1));
                    rlen = 1;
                    isSymbol = true;
                }
                notation = notation.Remove(0, rlen);
            }
            return list;
        }

        public static int IsHigher(string a, string b)
        {
            var dic =
                new Dictionary<string, int> {{"+", 0}, {"-", 0}, {"*", 1}, {"/", 1}, {"%", 1}, {"(", 2}, {")", 2}};
            if (int.TryParse(a, out var i))
                return -1;
            if (a == "(")
                return -1;
            if (dic[a] > dic[b])
                return 1;
            if (dic[a] < dic[b])
                return -1;
            if (dic[a] == dic[b])
                return 0;
            return 0;
        }

        private static string Simplification(string notation)
        {
            // x -> 1x
            notation = Regex.Replace(notation, @"(?<!\d)x", "1x");
            // ) -> )* 
            notation = Regex.Replace(notation, @"[^\+\-\*/=\(]\(", DealBrackets);
            notation = Regex.Replace(notation, @"\)[^\+\-\*/=\)]", DealBrackets);
            // delete extra symbols
            notation = Regex.Replace(notation, @"^[\+\-]", "");
            notation = Regex.Replace(notation, @"\([\+\-]", "(");
            // +++++ -> +
            foreach (char c in "+-*/%")
            {
                notation = Regex.Replace(notation, Regex.Escape(c.ToString()) + "{2,}", DealMultipleSymbols);
            }
            // Console.WriteLine($"Simplification: {notation}");
            return notation;
        }

        private static string DealBrackets(Match m)
        {
            if (m.Value.Contains("("))
            {
                return m.Value.Replace("(", "*(");
            }
            if (m.Value.Contains(")"))
            {
                return m.Value.Replace(")", ")*");
            }
            return "";
        }

        private static string DealMultipleSymbols(Match m)
        {
            return m.Value.Substring(0, 1);
        }

        private static string Equation2Notation(string e)
        {
            string[] arr = e.Split('=');
            return $"{arr[0]}-({arr[1]})";
        }
    }
}