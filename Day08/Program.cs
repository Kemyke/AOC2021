using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day08
{
    class Program
    {
        class SevenSegmentDisplay
        {
            public string FirstSegment { get; set; }
            public string SecondSegment { get; set; }
            public string ThirdSegment { get; set; }
            public string FourthSegment { get; set; }
            public string FifthSegment { get; set; }
            public string SixthSegment { get; set; }
            public string SeventhSegment { get; set; }

            public int GetValue(string s)
            {
                if (s.Length == 2)
                    return 1;
                if (s.Length == 3)
                    return 7;
                if (s.Length == 4)
                    return 4;
                if (s.Length == 7)
                    return 8;
                if (s.Length == 5 && s.Contains(SecondSegment))
                    return 5;
                if (s.Length == 5 && s.Contains(SixthSegment))
                    return 3;
                if (s.Length == 5)
                    return 2;
                if (s.Length == 6 && !s.Contains(FourthSegment))
                    return 0;
                if (s.Length == 6 && s.Contains(ThirdSegment))
                    return 9;
                return 6;
            }

            public long GetValue(List<string> ss)
            {
                long ret = 0;
                for(int i = 0; i < ss.Count; i++)
                {
                    ret += (long)Math.Pow(10, i) * GetValue(ss[ss.Count - i - 1]);
                }
                return ret;
            }
        }

        static SevenSegmentDisplay Deduce(List<string> numbers)
        {
            var one = numbers.Single(n => n.Length == 2);
            var seven = numbers.Single(n => n.Length == 3);
            var four = numbers.Single(n => n.Length == 4);

            SevenSegmentDisplay ret = new SevenSegmentDisplay();

            ret.FirstSegment = seven.Except(one).Single().ToString();

            var fl = numbers.Where(n => n.Length == 5);
            var flm = fl.First().Intersect(fl.Skip(1).First()).Intersect(fl.Skip(2).Single()).Except(ret.FirstSegment);

            ret.FourthSegment = flm.Intersect(four).Single().ToString();
            ret.SeventhSegment = flm.Except(four).Single().ToString();

            var sl = numbers.Where(n => n.Length == 6);
            ret.SecondSegment =  sl.Select(s => s.Except(one).Except(ret.FirstSegment).Except(ret.FourthSegment).Except(ret.SeventhSegment)).Where(s => s.Count() == 1).Single().Single().ToString();

            ret.FifthSegment = sl.Select(s => s.Except(one).Except(ret.FirstSegment).Except(ret.FourthSegment).Except(ret.SeventhSegment).Except(ret.SecondSegment)).Where(s => s.Count() == 1).First().Single().ToString();

            ret.SixthSegment = sl.Select(s => s.Except(ret.FifthSegment).Except(ret.FirstSegment).Except(ret.FourthSegment).Except(ret.SeventhSegment).Except(ret.SecondSegment)).Where(s => s.Count() == 1).Single().Single().ToString();

            var el = numbers.Where(n => n.Length == 7).Single();
            ret.ThirdSegment = el.Except(ret.SixthSegment).Except(ret.FifthSegment).Except(ret.FirstSegment).Except(ret.FourthSegment).Except(ret.SeventhSegment).Except(ret.SecondSegment).Single().ToString();


            return ret;
        }
            


        static void Main(string[] args)
        {
            var part1 = File.ReadAllLines("input.txt").SelectMany(l => l.Split(" | ")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries))
                .Select(n => n.Length).Where(l => l == 2 || l == 3 || l == 4 || l == 7).Count();

            var part2 = File.ReadAllLines("input.txt").Select(l => Deduce(l.Split(" | ")[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList()).GetValue(l.Split(" | ")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList())).Sum();
        }
    }
}
