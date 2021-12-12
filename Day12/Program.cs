using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day12
{
    class Program
    {
        static IEnumerable<IEnumerable<string>> Step(IEnumerable<string> path, IEnumerable<(string, string)> rules) =>
            rules.Where(r => r.Item1 == path.Last() && (char.IsUpper(r.Item2[0]) || !path.Contains(r.Item2)))
            .Select(n => path.Append(n.Item2));

        static IEnumerable<IEnumerable<string>> Step2(IEnumerable<string> path, IEnumerable<(string, string)> rules) =>
            rules.Where(r => r.Item1 == path.Last() && (char.IsUpper(r.Item2[0]) || !path.Contains(r.Item2) || path.Where(p => char.IsLower(p[0])).GroupBy(p=>p).Max(g=>g.Count()) == 1))
            .Select(n => path.Append(n.Item2));


        static void Main(string[] args)
        {
            var rules = File.ReadAllLines("input.txt").Select(l => (l.Split("-")[0], l.Split("-")[1]))
                            .SelectMany(r => new List<(string, string)> { (r.Item1, r.Item2), (r.Item2, r.Item1) })
                            .Where(r => r.Item2 != "start" && r.Item1 != "end");
            var pathes = new List<IEnumerable<string>> { new List<string> { "start" } };
            var completePathes = new List<IEnumerable<string>>();

            while (pathes.Any())
            {
                completePathes.AddRange(pathes.Where(p => p.Last() == "end"));
                var newPathes = new List<IEnumerable<string>>();                
                foreach (var path in pathes)
                {
                    var x = Step2(path, rules);
                    newPathes.AddRange(x);
                }
                pathes = newPathes;
            }
            var res = completePathes.Count();
        }
    }
}
