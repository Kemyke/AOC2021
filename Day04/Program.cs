using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day04
{
    class Program
    {

        static bool Bingo(List<List<int>> board, IEnumerable<int> nums)
         => board.Select(r => r.Select(n => nums.Contains(n) ? -1 : n).ToList()).Any(r => r.All(n => n == -1))
            || board.Select(r => r.Select(n => nums.Contains(n) ? -1 : n).ToList()).Aggregate(new List<int> { 0, 0, 0, 0, 0 }, (acc, r) => r.Select((n, i) => n == -1 ? acc[i]+1 : acc[i]).ToList()).Any(c => c == 5);

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var nums = lines.First().Split(",").Select(n=>int.Parse(n)).ToList();
            var boards = lines.Skip(2).Where(l => !string.IsNullOrEmpty(l))
                         .Select(l => l.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToList())
                         .Select((x, i) => new { Index = i, Value = x }).GroupBy(x => x.Index / 5).Select(x => x.Select(v => v.Value).ToList()).ToList();

            var part1 = Enumerable.Range(1, nums.Count).Select(n => (n, boards.Where(b => Bingo(b, nums.Take(n))).FirstOrDefault())).SkipWhile(bingos => bingos.Item2 == null || !bingos.Item2.Any()).Take(1)
                        .Select(res => res.Item2.Select(r => r.Select(n => nums.Take(res.n).Contains(n) ? 0 : n).Sum()).Sum() * nums[res.n - 1]).First();

            var part2 = Enumerable.Range(1, nums.Count).Select(n => (n, boards.Where(b => !Bingo(b, nums.Take(n))))).SkipWhile(bingos => bingos.Item2.Count() > 1).TakeWhile(bingos => bingos.Item2.Count() == 1).Reverse().Take(1).Select(b => (b.n, b.Item2.First()))
                        .Select(res => res.Item2.Select(r => r.Select(n => nums.Take(res.n + 1).Contains(n) ? 0 : n).Sum()).Sum() * nums[res.n]).First();
        }
    }
}
