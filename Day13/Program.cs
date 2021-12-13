using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day13
{
    class Program
    {
        static IEnumerable<(int, int)> FoldY(IEnumerable<(int, int)> paper, int y) =>
            paper.Where(d => d.Item2 < y).Union(paper.Where(d => d.Item2 > y).Select(d => (d.Item1, y - (d.Item2 - y)))).Distinct();

        static IEnumerable<(int, int)> FoldX(IEnumerable<(int, int)> paper, int x) =>
            paper.Where(d => d.Item1 < x).Union(paper.Where(d => d.Item1 > x).Select(d => (x - (d.Item1 -x), d.Item2))).Distinct();

        static void Visualize(IEnumerable<(int, int)> paper)
        {
            for (int y = 0; y < paper.Max(d => d.Item2) + 1; y++)
            {
                for (int x = 0; x < paper.Max(d => d.Item1) + 1; x++)
                {
                    Console.Write(paper.Any(d => d.Item1 == x && d.Item2 == y) ? "#" : ".");
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            var dots = File.ReadAllLines("input.txt").Select(l => (int.Parse(l.Split(",")[0]), int.Parse(l.Split(",")[1])));
            var part1 = FoldX(dots, 655).Count();

            var folds = File.ReadAllLines("input2.txt").Select(l => (l.Contains("x") ? "x" : "y", int.Parse(l.Split("=")[1])))
                .Aggregate(dots, (acc, fold) => fold.Item1 == "x" ? FoldX(acc, fold.Item2) : FoldY(acc, fold.Item2));

            Visualize(new HashSet<(int, int)>(folds));
            Console.ReadLine();
        }
    }
}
