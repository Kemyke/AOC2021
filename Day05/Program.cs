using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day05
{
    class Program
    {
        static List<(int, int)> GetLine(List<int> start, List<int> end) =>
            Enumerable.Range(0, (start[0] - end[0]) == 0 ? Math.Abs(start[1] - end[1]) + 1 : Math.Abs(start[0] - end[0]) + 1)
            .Select(r => (r, start[0] == end[0] ? 0: start[0] > end[0] ? -1 : 1, start[1] == end[1] ? 0 : start[1] > end[1] ? -1 : 1))
            .Select(l => (start[0] + l.r * l.Item2, start[1] + l.r * l.Item3)).ToList();

        static void Main(string[] args)
        {
            var coordinates = File.ReadAllLines("input.txt").Select(l => (l.Split(" -> ")[0].Split(",").Select(n => int.Parse(n)).ToList(), l.Split(" -> ")[1].Split(",").Select(n => int.Parse(n)).ToList()));

            var part1 = coordinates.Where(c => c.Item1[0] == c.Item2[0] || c.Item1[1] == c.Item2[1])
                                .SelectMany(l => GetLine(l.Item1, l.Item2)).GroupBy(k => k).Where(g => g.Count() > 1).Count();
            
            var part2 = coordinates.SelectMany(l => GetLine(l.Item1, l.Item2)).GroupBy(k => k).Where(g => g.Count() > 1).Count();
        }
    }
}
