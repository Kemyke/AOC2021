using System;
using System.IO;
using System.Linq;

namespace Day18
{
    class Program
    {
        class SnailValue
        {
            public int Value { get; set; } = -1;
            public SnailNum Values { get; set; }
        }

        class SnailNum
        {
            public SnailNum Parent { get; set; }
            public SnailValue Left { get; set; }
            public SnailValue Right { get; set; }

            private static int FindComma(string str)
            {
                int level = 0;
                for (int i = 0; i < str.Length; i++)
                {
                    var ch = str[i];
                    if (level == 1 && ch == ',')
                    {
                        return i;
                    }
                    else if (ch == '[')
                    {
                        level++;
                    }
                    else if (ch == ']')
                    {
                        level--;
                    }
                }
                throw new Exception("WTF");
            }

            private static SnailValue ParseValue(string str, SnailNum par)
            {
                if (str.Length == 1)
                    return new SnailValue { Value = int.Parse(str) };
                else
                    return new SnailValue { Values = Parse(str, par) };
            }

            public static SnailNum Parse(string str, SnailNum par)
            {
                SnailNum ret = new SnailNum();
                var idx = FindComma(str);
                var left = str.Substring(0, idx);
                var right = str.Substring(idx + 1);

                ret.Parent = par;
                ret.Left = ParseValue(left.Substring(1), ret);

                ret.Right = ParseValue(right.Substring(0, right.Length - 1), ret);
                return ret;
            }

            public static SnailNum Add(SnailNum lp, SnailNum rp)
            {
                var ret = new SnailNum();
                lp.Parent = ret;
                rp.Parent = ret;
                ret.Left = new SnailValue { Values = lp };
                ret.Right = new SnailValue { Values = rp };
                return ret;
            }

            public int Depth { get { int ret = 0; SnailNum par = Parent; while (par != null) { par = par.Parent; ret++; } return ret; } }

            public static (SnailNum, bool) LeftRegular(SnailNum num)
            {
                SnailNum n = num;
                SnailNum p = num.Parent;
                bool left = true;

                while(p!= null && p.Right.Values != n)
                {
                    n = p;
                    p = p.Parent;
                }

                if(p!= null)
                {
                    if(p.Left.Values != null)
                    {
                        left = false;
                        p = p.Left.Values;
                        while (p.Right.Values != null)
                        {
                            p = p.Right.Values;
                        }
                    }
                }

                return (p, left);
            }

            public static (SnailNum, bool) RightRegular(SnailNum num)
            {
                SnailNum n = num;
                SnailNum p = num.Parent;
                bool left = false;

                while (p != null && p.Left.Values != n)
                {
                    n = p;
                    p = p.Parent;
                }

                if (p != null)
                {
                    if (p.Right.Values != null)
                    {
                        left = true;
                        p = p.Right.Values;
                        while (p.Left.Values != null)
                        {
                            p = p.Left.Values;
                        }
                    }
                }

                return (p, left);
            }

            public bool Explode()
            {
                if(Depth == 4)
                {
                    Explode(this);
                    return true;
                }
                if(Left.Values != null)
                {
                    if(Left.Values.Explode())
                    {
                        return true;
                    }
                }
                if (Right.Values != null)
                {
                    if (Right.Values.Explode())
                    {
                        return true;
                    }
                }

                return false;
            }

            public long Magnitude()
            {
                long lm;
                if(Left.Values == null)
                {
                    lm = Left.Value;
                }
                else
                {
                    lm = Left.Values.Magnitude();
                }

                long rm;
                if (Right.Values == null)
                {
                    rm = Right.Value;
                }
                else
                {
                    rm = Right.Values.Magnitude();
                }

                return 3 * lm + 2 * rm;
            }

            public bool Split()
            {
                if(Left.Values == null && Left.Value > 9)
                {
                    Split(Left, this);
                    return true;
                }
                else if (Left.Values != null)
                {
                    if(Left.Values.Split())
                    {
                        return true;
                    }
                }

                if (Right.Values == null && Right.Value > 9)
                {
                    Split(Right, this);
                    return true;
                }
                else if (Right.Values != null)
                {
                    if (Right.Values.Split())
                    {
                        return true;
                    }
                }

                return false;
            }

            public static void Explode(SnailNum num)
            {
                if(num.Left.Values != null || num.Right.Values != null)
                {
                    throw new Exception("WTF");
                }

                var l = LeftRegular(num);
                if(l.Item1 != null)
                {
                    if (l.Item2)
                    {
                        l.Item1.Left.Value += num.Left.Value;
                    }
                    else
                    {
                        l.Item1.Right.Value += num.Left.Value;
                    }
                }

                var r = RightRegular(num);
                if (r.Item1 != null)
                {
                    if (!r.Item2)
                    {
                        r.Item1.Right.Value += num.Right.Value;
                    }
                    else
                    {
                        r.Item1.Left.Value += num.Right.Value;
                    }
                }

                if(num.Parent.Left.Values == num)
                {
                    num.Parent.Left = new SnailValue { Value = 0 };
                }
                else if (num.Parent.Right.Values == num)
                {
                    num.Parent.Right = new SnailValue { Value = 0 };
                }
                else
                {
                    throw new Exception("WTF");
                }
            }

            private void InnerPrint()
            {
                Console.Write("[");
                if (Left.Values == null)
                    Console.Write(Left.Value);
                else
                    Left.Values.InnerPrint();

                Console.Write(",");

                if (Right.Values == null)
                    Console.Write(Right.Value);
                else
                    Right.Values.InnerPrint();

                Console.Write("]");
            }

            public static void Split(SnailValue val, SnailNum par)
            {
                if(val.Values != null || val.Value < 10)
                {
                    throw new Exception("WTF");
                }

                var lv = (int)Math.Floor(val.Value / 2m);
                var rv = (int)Math.Ceiling(val.Value / 2m);

                val.Value = -1;
                val.Values = new SnailNum { Left = new SnailValue { Value = lv }, Right = new SnailValue { Value = rv }, Parent = par };
            }

            public void Print()
            {
                InnerPrint();
                Console.WriteLine();
            }

            public void Reduce()
            {
                bool changed = false;
                do
                {
                    if(Explode())
                    {
                        changed = true;
                        continue;
                    }
                    if(!Split())
                    {
                        changed = false;
                    }
                }
                while (changed);
            }
        }

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt").ToList();
            var sum = SnailNum.Parse(lines.First(), null);

            foreach(var line in lines.Skip(1))
            {
                sum = SnailNum.Add(sum, SnailNum.Parse(line, null));
                sum.Reduce();
            }
            var part1 = sum.Magnitude();

            long maxMagnitude = 0;

            for(int i = 0; i < lines.Count;i++ )
            {
                for (int j = i; j < lines.Count; j++)
                {
                    if (i == j) continue;
                    var ssum = SnailNum.Add(SnailNum.Parse(lines[i], null), SnailNum.Parse(lines[j], null));
                    ssum.Reduce();
                    var mag = ssum.Magnitude();
                    if(mag > maxMagnitude)
                    {
                        maxMagnitude = mag;
                    }

                    ssum = SnailNum.Add(SnailNum.Parse(lines[j], null), SnailNum.Parse(lines[i], null));
                    ssum.Reduce();
                    mag = ssum.Magnitude();
                    if (mag > maxMagnitude)
                    {
                        maxMagnitude = mag;
                    }

                }
            }
        }
    }
}
