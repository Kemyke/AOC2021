using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day09
{
    class Program
    {
        static int BasinSize(List<List<(int, int, int)>> items, List<(int, int, int)> map) =>
            items.SelectMany(o => o).Count() + items.Select(i =>
                     BasinSize(new List<List<(int, int, int)>> {
                            i.SelectMany(c => map.Where(m => m.Item3 < 9 && m.Item3 > c.Item3 && ((m.Item1 == c.Item1 && m.Item2 == c.Item2 - 1)
                                                                                                    || (m.Item1 == c.Item1 && m.Item2 == c.Item2 + 1)
                                                                                                    || (m.Item1 == c.Item1 - 1 && m.Item2 == c.Item2)
                                                                                                    || (m.Item1 == c.Item1 + 1 && m.Item2 == c.Item2)
                                                                                                  ))).Distinct()

                            .ToList()
                        }.Where(o => o.Any()).ToList(), map.Except(i).ToList())).Sum();

        static void Main(string[] args)
        {
            var x = File.ReadAllLines("input.txt").Select((l, li) => (l, li)).Select(t => (t.li, t.l.Select((c, ci) => (ci, int.Parse(c.ToString())))))
                .SelectMany(t => t.Item2.Select(i => (t.li, i.ci, i.Item2))).ToList();

            var part1 = x.Where(p => x.Where(pp =>(pp.ci == p.ci && pp.li == p.li - 1) ||
                                      (pp.ci == p.ci && pp.li == p.li + 1) ||
                                      (pp.ci == p.ci - 1 && pp.li == p.li) ||
                                      (pp.ci == p.ci + 1 && pp.li == p.li)
                                ).All(pp => pp.Item3 > p.Item3)).Sum(t => t.Item3 + 1);

            var part2 = x.Where(p => x.Where(pp => (pp.ci == p.ci && pp.li == p.li - 1) ||
                                      (pp.ci == p.ci && pp.li == p.li + 1) ||
                                      (pp.ci == p.ci - 1 && pp.li == p.li) ||
                                      (pp.ci == p.ci + 1 && pp.li == p.li)
                                ).All(pp => pp.Item3 > p.Item3))
                        .Select(p => BasinSize(new List<List<(int, int, int)>> { new List<(int, int, int)> { p }}, x))
                        .OrderByDescending(i => i)
                        .Take(3)
                        .Aggregate((acc, i) => acc * i);
        }
    }
}
