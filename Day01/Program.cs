using System;
using System.IO;
using System.Linq;

namespace Day01
{
    class Program
    {
        static void Main(string[] args)
        {
            var depths = File.ReadAllLines("input.txt").Select(t => int.Parse(t));
            var part1 = depths.Skip(1).Zip(depths).Select(t=> t.First - t.Second).Where(d=>d>0).Count();

            var aggDepths = depths.Skip(2).Zip(depths.Skip(1).Zip(depths)).Select(d => d.First + d.Second.First + d.Second.Second);
            var part2 = aggDepths.Skip(1).Zip(aggDepths).Select(t => t.First - t.Second).Where(d => d > 0).Count();
        }
    }
}
