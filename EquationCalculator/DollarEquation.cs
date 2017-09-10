using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationCalculator
{
    class DollarEquation
    {
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }
        public List<double> GetResult()
        {
            List<double> list = new List<double>();
            if (A == 0)
            {
                if (B == 0)
                    return list;
                list.Add(-C / B);
                return list;
            }
            else
            {
                // Ax + Bx + C = 0
                // -B +- sqr(b*b-)
                double d = B * B - 4 * A * C;
                if (d < 0)
                {
                    return list;
                }
                else
                {
                    double a1 = -B + Math.Sqrt(d);
                    double a2 = -B - Math.Sqrt(d);
                    double b1 = a1 / (2 * A);
                    double b2 = a2 / (2 * A);
                    list.Add(b1);
                    list.Add(b2);
                    return list;
                }
            }

        }

        public static DollarEquation operator +(DollarEquation b, DollarEquation a)
        {
            DollarEquation o = new DollarEquation
            {
                A = a.A + b.A,
                B = a.B + b.B,
                C = a.C + b.C
            };
            return o;
        }

        public static DollarEquation operator -(DollarEquation b, DollarEquation a)
        {
            DollarEquation o = new DollarEquation
            {
                A = a.A - b.A,
                B = a.B - b.B,
                C = a.C - b.C
            };
            return o;
        }

        public static DollarEquation operator *(DollarEquation b, DollarEquation a)
        {
            DollarEquation o = new DollarEquation();
            // one dollar equation
            if (a.A == 0 && b.A == 0)
            {
                o.A = a.B * b.B;
                o.B = a.B * b.C + a.C * b.B;
                o.C = a.C * b.C;
            }

            return o;
        }

        public static DollarEquation operator /(DollarEquation b, DollarEquation a)
        {
            DollarEquation o = new DollarEquation();
            if (a.A == 0 && b.A == 0)
            {
                // 1x / 2
                // B = 1, C = 0
                // B = 0, C = 2
                if (a.C == 0 && b.B == 0.0 && b.C != 0)
                {
                    o.B = a.B / b.C;
                }
                // 1 / 2x
                // B = 0, C = 1
                // B = 2, C = 0
                else if (a.B == 0 && b.C == 0 && b.B != 0)
                {
                    o.B = a.C / b.B;
                }
                // 1 / 2
                // B = 0, C = 1
                // B = 0, C = 2
                else if (a.B == 0 && b.B == 0 && b.C != 0)
                {
                    o.C = a.C / b.C;
                }
                // 1x / 2x
                // B = 1, C = 0
                // B = 2, C = 0
                else if (a.C == 0 && b.C == 0 && b.B != 0)
                {
                    o.C = a.B / b.B;
                }
            }

            return o;
        }
    }
}
