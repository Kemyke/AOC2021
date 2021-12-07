using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day07
{
    class Program
    {
        static int FuelCost(IEnumerable<int> crabs, int pos) =>
            crabs.Select(c => Math.Abs(c - pos)).Sum();

        static long FuelCost2(IEnumerable<int> crabs, int pos) =>
            crabs.Select(c => (long)SpecialFunctions.Binomial(Math.Abs(c - pos)+1, 2)).Sum();


        static void Main(string[] args)
        {
            var crabs = File.ReadAllText("input.txt").Split(",").Select(n => int.Parse(n));
            var part1 = Enumerable.Range(crabs.Min(), crabs.Max() - crabs.Min()).Select(p => (p, FuelCost(crabs, p))).OrderBy(t=>t.Item2).First().Item2;
            var part2 = Enumerable.Range(crabs.Min(), crabs.Max() - crabs.Min()).Select(p => (p, FuelCost2(crabs, p))).OrderBy(t=>t.Item2).First().Item2;
        }
    }
}
