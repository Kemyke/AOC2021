using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day10
{
    class Program
    {
        static int ErrorCode(char s)
        {
            switch(s)
            {
                case ')':
                    return 3;
                case ']':
                    return 57;
                case '}':
                    return 1197;
                case '>':
                    return 25137;
            }
            throw new Exception();
        }

        static long Code2(char s)
        {
            switch (s)
            {
                case '(':
                    return 1;
                case '[':
                    return 2;
                case '{':
                    return 3;
                case '<':
                    return 4;
            }
            throw new Exception();
        }

        static int Eval(IEnumerable<char> commands, IEnumerable<char> state) =>
            !commands.Any()
            ? 0
            : new List<char> { '(', '[', '{', '<' }.Contains(commands.First())
                ? Eval(commands.Skip(1), state.Append(commands.First()))
                : (commands.First() == state.Last() + 1) || (commands.First() == state.Last() + 2)
                    ? Eval(commands.Skip(1), state.Take(state.Count() -1))
                    : ErrorCode(commands.First());

        static List<char> Eval2(IEnumerable<char> commands, IEnumerable<char> state) =>
            !commands.Any()
            ? state.Reverse().ToList()
            : new List<char> { '(', '[', '{', '<' }.Contains(commands.First())
                ? Eval2(commands.Skip(1), state.Append(commands.First()))
                : (commands.First() == state.Last() + 1) || (commands.First() == state.Last() + 2)
                    ? Eval2(commands.Skip(1), state.Take(state.Count() - 1))
                    : new List<char>();


        static void Main(string[] args)
        {
            var part1 = File.ReadAllLines("input.txt").Select(l => Eval(l.ToList(), new List<char>())).Sum();

            var part2 = File.ReadAllLines("input.txt").Select(l => Eval2(l.ToList(), new List<char>()))
                        .Where(l => l.Any());

            List<long> totals = new List<long>(); ;
            foreach(var x in part2)
            {
                long total = 0;
                foreach (var ch in x)
                {
                    total = 5 * total;
                    total += Code2(ch);
                }
                totals.Add(total);
            }

            var part2b = MathNet.Numerics.Statistics.Statistics.Median(totals.Select(n => (double)n));
        }
    }
}
