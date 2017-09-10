using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EquationCalculator;
using static EquationCalculator.CalcPostfix;

namespace TestEquationCalculator
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void Main()
        {
            /**
             * x+1=2
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
             Dictionary<string, List<double>>dic = new Dictionary<string, List<double>>
             {
                 {"x+1=2",new List<double>{1}},
                 {"2x-5=5",new List<double>{5}},
                 {"x=5*x-5*3",new List<double>{(double)15/4}},
                 {"5(3)+2x=25",new List<double>{5}},
                 {"x/3=7",new List<double>{21}},
                 {"x^2+7*x+6*2=0",new List<double>{-3, -4}},
                 {"(x-9)(x-1)=0",new List<double>{1, 9}},
                 {"2(3x)+3x=81",new List<double>{9}},
                 {"2(x-1)+8=4*x-20",new List<double>{13}},
                 {"4x^2-11*2=x^2+5",new List<double>{3, -3}},
             };

            foreach (var k in dic)
            {
                var a = NotationTransform.Infix2Postfix(k.Key);
                var b = Calc(a);
                var c = b.GetResult();
                Assert.IsTrue(UnorderedEqual<double>(c, k.Value));
            }
        }


        static bool UnorderedEqual<T>(ICollection<T> a, ICollection<T> b)
        {
            // 1
            // Require that the counts are equal
            if (a.Count != b.Count)
            {
                return false;
            }
            // 2
            // Initialize new Dictionary of the type
            Dictionary<T, int> d = new Dictionary<T, int>();
            // 3
            // Add each key's frequency from collection A to the Dictionary
            foreach (T item in a)
            {
                int c;
                if (d.TryGetValue(item, out c))
                {
                    d[item] = c + 1;
                }
                else
                {
                    d.Add(item, 1);
                }
            }
            // 4
            // Add each key's frequency from collection B to the Dictionary
            // Return early if we detect a mismatch
            foreach (T item in b)
            {
                int c;
                if (d.TryGetValue(item, out c))
                {
                    if (c == 0)
                    {
                        return false;
                    }
                    else
                    {
                        d[item] = c - 1;
                    }
                }
                else
                {
                    // Not in dictionary
                    return false;
                }
            }
            // 5
            // Verify that all frequencies are zero
            foreach (int v in d.Values)
            {
                if (v != 0)
                {
                    return false;
                }
            }
            // 6
            // We know the collections are equal
            return true;
        }
    }
}
