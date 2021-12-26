using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day25
{
    class Program
    {

        class Map
        {
            public int MaxX { get; set; }
            public int MaxY { get; set; }
            public Dictionary<int, HashSet<int>> SouthFishes { get; set; } = new Dictionary<int, HashSet<int>>();
            public Dictionary<int, HashSet<int>> EastFishes { get; set; } = new Dictionary<int, HashSet<int>>();

            public void Init()
            {
                for (int y = 0; y < MaxY; y++)
                {
                    SouthFishes.Add(y, new HashSet<int>());
                    EastFishes.Add(y, new HashSet<int>());
                }
            }

            private bool IsEmpty(int y, int x)
            {
                if(SouthFishes.TryGetValue(y, out HashSet<int> line))
                {
                    if(line.Contains(x))
                    {
                        return false;
                    }
                }

                if (EastFishes.TryGetValue(y, out HashSet<int> line2))
                {
                    if (line2.Contains(x))
                    {
                        return false;
                    }
                }
                return true;
            }

            public int Step()
            {
                int ret = 0;
                foreach(var f in EastFishes.SelectMany(kvp=> kvp.Value.Where(f => IsEmpty(kvp.Key, (f + 1) % MaxX)).Select(f=>(y: kvp.Key, x: f))).ToList())
                {
                    ret++;
                    EastFishes[f.y].Remove(f.x);
                    EastFishes[f.y].Add((f.x + 1) % MaxX);
                }

                foreach (var f in SouthFishes.SelectMany(kvp => kvp.Value.Where(f => IsEmpty((kvp.Key + 1) % MaxY, f)).Select(f => (y: kvp.Key, x: f))).ToList())
                {
                    ret++;
                    SouthFishes[f.y].Remove(f.x);
                    SouthFishes[(f.y + 1) % MaxY].Add(f.x);
                }
                return ret;
            }

            public void Visualize()
            {
                for (int y = 0; y < MaxY; y++)
                {
                    for (int x = 0; x < MaxX; x++)
                    {
                        if (EastFishes[y].Contains(x)) Console.Write(">");
                        else if (SouthFishes[y].Contains(x)) Console.Write("v");
                        else Console.Write(".");
                    }
                    Console.WriteLine();
                }

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            Map map = new Map { MaxY = lines.Length, MaxX = lines.First().Length };
            map.Init();

            int y = 0;
            foreach(var line in lines)
            {
                int x = 0;
                foreach(var ch in line)
                {
                    if (ch == '>')
                        map.EastFishes[y].Add(x);
                    else if (ch =='v')
                        map.SouthFishes[y].Add(x);

                    x++;
                }
                y++;
            }

            int i = 0;
            int step = 0;
            do
            {
                i++;
                step = map.Step();
            }
            while (step > 0);

            map.Visualize();
        }
    }
}
