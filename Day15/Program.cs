using Dijkstra.NET.Graph;
using Dijkstra.NET.ShortestPath;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day15
{
    class Program
    {
        static IEnumerable<(int, uint)> GetAdjacentNodes((int, uint)[,] map, int x, int y)
        {
            List<(int, uint)> ret = new List<(int, uint)>();
            if(x>0)
            {
                ret.Add(map[y, x - 1]);
            }
            if(x < map.GetLength(1) - 1)
            {
                ret.Add(map[y, x + 1]);
            }
            if (y > 0)
            {
                ret.Add(map[y - 1, x]);
            }
            if (y < map.GetLength(0) - 1)
            {
                ret.Add(map[y + 1, x]);
            }
            return ret;
        }

        static void Main(string[] args)
        {
            var g = new Graph<(int, int), long>();

            var nodes = File.ReadAllLines("input.txt").Select((l, y) => (l, y)).ToList().SelectMany(t => t.l.Select((ch, x) => (x, t.y, int.Parse(ch.ToString()), g.AddNode((x, t.y)))).ToList()).ToList();
            int maxX = nodes.Max(tt => tt.x) + 1;
            int maxY = nodes.Max(tt => tt.y) + 1;


            var add = Enumerable.Range(0, 5).SelectMany(dy => Enumerable.Range(0, 5).Select(dx => (dx, dy)))
                .Where(dt => dt.dx != 0 || dt.dy != 0)
                .SelectMany(dt => nodes.Select(n => (x: dt.dx * maxX + n.x, y: dt.dy * maxY + n.y, n.Item3 + dt.dx + dt.dy < 10 ? n.Item3 + dt.dx + dt.dy : n.Item3 + dt.dx + dt.dy -9, g.AddNode((dt.dx * maxX + n.x, dt.dy * maxY + n.y)))).ToList())
                .Union(nodes)
                .ToList();

            (int, uint)[,] map = new (int, uint)[5 * maxY, 5 * maxY];

            add.ForEach(n => { map[n.y, n.x] = (n.Item3, n.Item4); });


            foreach (var node in add)
            {
                foreach(var an in GetAdjacentNodes(map, node.x, node.y))
                {
                    g.Connect(an.Item2, node.Item4, node.Item3, 0);
                    g.Connect(node.Item4, an.Item2, an.Item1, 0);
                }
            }

            var start = add.Single(t => t.x == 0 && t.y == 0);
            var nmx = add.Max(tt => tt.x);
            var nmy = add.Max(tt => tt.y);
            var end = add.Single(t => t.x == nmx && t.y == nmy);

            var part1 = g.Dijkstra(start.Item4, end.Item4);
        }
    }
}
