using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day22
{
    class Program
    {
        class Instruction
        {
            public int Prio { get; set; }
            public bool On { get; set; }
            public int MinX { get; set; }
            public int MaxX { get; set; }
            public int MinY { get; set; }
            public int MaxY { get; set; }
            public int MinZ { get; set; }
            public int MaxZ { get; set; }
        }

        static long Part1(List<Instruction> instructions)
        {
            long part1 = 0;
            for (int z = -50; z <= 50; z++)
            {
                var zs = instructions.Where(i => i.MinZ <= z && i.MaxZ >= z);
                for (int y = -50; y <= 50; y++)
                {
                    var ys = zs.Where(i => i.MinY <= y && i.MaxY >= y);
                    for (int x = -50; x <= 50; x++)
                    {
                        var xs = ys.Where(i => i.MinX <= x && i.MaxX >= x);
                        var i = xs.LastOrDefault();
                        if(i != null && i.On)
                        {
                            part1++;
                        }
                    }
                }
            }
            return part1;
        }

        static ulong GetLineScore(List<Instruction> zs, int y)
        {
            var ys = zs.Where(i => i.MinY <= y && i.MaxY >= y).OrderBy(i => i.MinX).ToList();
            if (!ys.Any())
            {
                return 0;
            }

            int cp = ys.First().MinX;
            var curr = ys.First();
            var maxx = ys.Max(i => i.MaxX);
            ulong pp2 = 0;
            while (cp <= maxx)
            {
                var higher = ys.Where(i => i.Prio > curr.Prio && i.MinX >= cp && i.MinX <= curr.MaxX).OrderBy(i => i.MinX).FirstOrDefault();
                if (higher != null)
                {
                    if (curr.On)
                    {
                        pp2 += (ulong)higher.MinX - (ulong)cp;
                    }
                    cp = higher.MinX;
                    curr = higher;
                }
                else
                {
                    if (curr.On)
                    {
                        pp2 += (ulong)curr.MaxX - (ulong)cp + 1;
                    }
                    cp = curr.MaxX + 1;
                    curr = ys.Where(i => cp >= i.MinX && i.MaxX >= cp).OrderBy(i => i.Prio).LastOrDefault();
                    if (curr == null)
                    {
                        curr = ys.Where(i => i.MinX >= cp).OrderBy(i => i.MinX).FirstOrDefault();
                        if (curr != null)
                            cp = curr.MinX;
                        else
                            cp = maxx + 1;
                    }
                }
            }
            return pp2;
        }

        static ulong Part2(List<Instruction> instructions, int minZ, int maxZ)
        {
            ulong part2 = 0;
            for (int z = minZ; z <= maxZ; z++)
            {
                var zs = instructions.Where(i => i.MinZ <= z && i.MaxZ >= z).OrderBy(i => i.MinY).ToList();
                if (!zs.Any())
                {
                    continue;
                }

                var yyst = zs.Select(i => i.MinY).Distinct().OrderBy(n => n).Cast<int?>().ToList();
                var yyen = zs.Select(i => i.MaxY).Distinct().OrderBy(n => n).Cast<int?>().ToList();

                int cpy = yyst.First().Value;
                var maxy = yyen.Last().Value;
                ulong ppy2 = 0;
                while (cpy <= maxy)
                {
                    var nextS = yyst.Where(i => i > cpy).FirstOrDefault();
                    var nextE = yyen.Where(i => i >= cpy).FirstOrDefault();
                    if(!nextS.HasValue)
                    {
                        nextS = int.MaxValue;
                    }
                    if(!nextE.HasValue)
                    {
                        throw new Exception("WTF");
                    }    

                    if (nextS.HasValue && nextE.HasValue)
                    {
                        if (nextS.Value < nextE.Value)
                        {
                            var s = GetLineScore(zs, cpy);
                            ppy2 += s * (ulong)(nextS.Value - cpy);
                            cpy = nextS.Value;
                        }
                        else if (nextE.Value < nextS.Value)
                        {
                            var s = GetLineScore(zs, cpy);
                            ppy2 += s * (ulong)(nextE.Value - cpy + 1);
                            cpy = nextE.Value + 1;
                        }
                        else if (nextE.Value == nextS.Value)
                        {
                            var s = GetLineScore(zs, cpy);
                            ppy2 += s * (ulong)(nextS.Value - cpy);
                            cpy = nextS.Value;
                        }
                    }
                }
                part2 += ppy2;
            }
            return part2;
        }
        
        static void Main(string[] args)
        {
            var h = ulong.MaxValue;
            Regex r = new Regex("(on|off) x=(-?[0-9]*)..(-?[0-9]*),y=(-?[0-9]*)..(-?[0-9]*),z=(-?[0-9]*)..(-?[0-9]*)");
            var instructions = File.ReadAllLines("input.txt").Select(l => r.Match(l)).Select((m,p) => new Instruction
            {
                Prio = p,
                On = m.Groups[1].Value == "on",
                MinX = int.Parse(m.Groups[2].Value),
                MaxX = int.Parse(m.Groups[3].Value),
                MinY = int.Parse(m.Groups[4].Value),
                MaxY = int.Parse(m.Groups[5].Value),
                MinZ = int.Parse(m.Groups[6].Value),
                MaxZ = int.Parse(m.Groups[7].Value)
            });

            var gMinZ = instructions.Min(i => i.MinZ);
            var gMaxZ = instructions.Max(i => i.MaxZ);
            
            var part2 = Part2(instructions.ToList(), gMinZ, gMaxZ);
        }
    }
}
