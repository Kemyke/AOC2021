using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day11
{
    class Program
    {
        static IEnumerable<(int rowNum, int columnNum, int value)> Step(IEnumerable<(int, int, int)> map) =>
            map.Select(v => (v.Item1, v.Item2, v.Item3 + 1)).ToList();

        static (int, IEnumerable<(int rowNum, int columnNum, int value)>) Flash(IEnumerable<(int, int, int)> map) =>
            (map.Count(v => v.Item3 >= 10), map.Except(map.Where(v => v.Item3 >= 10)).Union(map.Where(v => v.Item3 >= 10).Select(v => (v.Item1, v.Item2, -1))).ToList());

        static IEnumerable<(int rowNum, int columnNum, int value)> EndFlash(IEnumerable<(int, int, int)> map) =>
            map.Except(map.Where(v => v.Item3 == -1)).Union(map.Where(v => v.Item3 == -1).Select(v => (v.Item1, v.Item2, 0))).ToList();


        static IEnumerable<(int rowNum, int columnNum)> Adjacents(IEnumerable<(int, int, int)> flashes) =>
            flashes.SelectMany(v => new List<(int, int)>
            {
                (v.Item1 + 1, v.Item2),  (v.Item1 - 1, v.Item2),  (v.Item1, v.Item2 + 1), (v.Item1, v.Item2 - 1), 
                (v.Item1 + 1, v.Item2 - 1),  (v.Item1 + 1, v.Item2 + 1), (v.Item1 - 1, v.Item2 - 1), (v.Item1 - 1, v.Item2 + 1),
            });

        static void Main(string[] args)
        {
           var map = File.ReadAllLines("input.txt").Select((row, rowNum) => (rowNum, row)).SelectMany(r => r.row.Select((value, columnNum) => (r.rowNum, columnNum, int.Parse(value.ToString()))));
            int part1 = 0;
            for(int i = 0; i < int.MaxValue; i++)
            {
                int part2 = 0;
                map = Step(map);
                int nf;
                (nf, map) = Flash(map);
                part1 += nf;
                part2 += nf;
                while (map.Any(m => m.Item3 == -1))
                {
                    foreach (var a in Adjacents(map.Where(m => m.Item3 == -1).ToList()))
                    {
                        var c = map.Where(v => v.rowNum == a.rowNum && v.columnNum == a.columnNum && v.Item3 > 0);
                        if (c.Count() == 1)
                        {
                            map = map.Except(c).Append((c.Single().rowNum, c.Single().columnNum, c.Single().Item3 + 1)).ToList();
                        }
                        else if(c.Count() == 0)
                        {

                        }
                        else
                        {

                        }
                    }
                    map = EndFlash(map);
                    (nf, map) = Flash(map);
                    part1 += nf;
                    part2 += nf;                    
                }
                if (part2 == 100)
                {
                    Console.WriteLine(i + 1);
                    break;
                }
                if (i == 99)
                {
                    Console.WriteLine(part1);
                }
            }
           
        }
    }
}
