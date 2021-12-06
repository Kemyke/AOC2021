using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day03
{
    class Program
    {
        static void Main(string[] args)
        {
            var bins = File.ReadAllLines("input.txt").ToList();

            var gr = bins.Aggregate(Enumerable.Range(0, bins.First().Length).Select(x => (0, x)).ToList(), 
                (acc, rp) => { return acc.Select(a => (a.Item1 + int.Parse(rp[a.Item2].ToString()), a.Item2)).ToList(); })
                .Select(t => (t.Item1 / (decimal)bins.Count > 0.5m) ? 1 : 0).Aggregate("", (acc, i) => acc += i.ToString());

            var er = gr.Select(c => (c == '0' ? "1" : "0")).Aggregate("", (acc, i) => acc += i.ToString());

            var part1 = Convert.ToInt32(gr, 2) * Convert.ToInt32(er, 2);

            var ogr = Enumerable.Range(0, bins.First().Length)
                        .Aggregate("", (acc, i) => bins.Where(b => b.StartsWith(acc)).Select(bb => int.Parse(bb[i].ToString())).Sum() >= bins.Where(bb => bb.StartsWith(acc)).Count() / 2m ? acc + "1": acc + "0")
                        .Aggregate(new List<(string, int)> { ("", bins.Count) }, (acc, ch) => acc.Append((acc.Last().Item1 + ch, bins.Where(b=>b.StartsWith(acc.Last().Item1 + ch)).Count() )).ToList())
                        .Where(a=>a.Item2 == 1).SelectMany(b=>bins.Where(bb=>bb.StartsWith(b.Item1))).Distinct().Single();

            var csr = Enumerable.Range(0, bins.First().Length)
                        .Aggregate("", (acc, i) => bins.Where(b => b.StartsWith(acc)).Select(bb => int.Parse(bb[i].ToString())).Sum() >= bins.Where(bb => bb.StartsWith(acc)).Count() / 2m ? acc + "0" : acc + "1")
                        .Aggregate(new List<(string, int)> { ("", bins.Count) }, (acc, ch) => acc.Append((acc.Last().Item1 + ch, bins.Where(b => b.StartsWith(acc.Last().Item1 + ch)).Count())).ToList())
                        .Where(a => a.Item2 == 1).SelectMany(b => bins.Where(bb => bb.StartsWith(b.Item1))).Distinct().Single();

            var part2 = Convert.ToInt32(ogr, 2) * Convert.ToInt32(csr, 2);
        }
    }
}
