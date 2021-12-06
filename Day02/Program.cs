using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day02
{
    class Program
    {
        static void Main(string[] args)
        {
            var commands = File.ReadAllLines("input.txt").Select(l => new { v = l }).ToList();

            var hor = commands.Where(c => c.v.StartsWith("forward")).Select(c => int.Parse(c.v.Replace("forward ", ""))).Sum();
            var depth = commands.Where(c => !c.v.StartsWith("forward")).Select(c => int.Parse(c.v.Replace("down ", "").Replace("up ", "-"))).Sum();
            var part1 = hor * depth;

            var depth2 = commands.Where(c => c.v.StartsWith("forward")).Select(c => int.Parse(c.v.Replace("forward ", "")) * commands.TakeWhile(i => i != c).Where(c => !c.v.StartsWith("forward")).Select(c => int.Parse(c.v.Replace("down ", "").Replace("up ", "-"))).Sum()).Sum();
            var part2 = hor * depth2;
        }
    }
}
