using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Day23
{
    class Program
    {
        static int BurrowSize = 4;
        static List<int> HallwayPoss = new List<int> { 0, 1, 3, 5, 7, 9, 10 };
        static Dictionary<string, int> CostMultipliers = new Dictionary<string, int> 
            { { "A", 1 },
            { "B", 10 },
            { "C", 100 },
            { "D", 1000 },
        };

        class Amphiod
        {
            public string Type { get; set; }
            public int X { get; set; }
            public int Y { get; set; }

            public int BurrowX()
            {
                switch (Type)
                {
                    case "A":
                        return 2;
                    case "B":
                        return 4;
                    case "C":
                        return 6;
                    case "D":
                        return 8;
                }
                throw new Exception();
            }
            public bool InFinishState(Dictionary<string, List<Amphiod>> burrows)
            {
                if (Y == 0 || X != BurrowX())
                    return false;

                return burrows[Type].Skip(BurrowSize - Y).All(a => a.Type == Type);

            }

            public int GetHeuristicCost(Dictionary<string, List<Amphiod>> burrows)
            {
                if (InFinishState(burrows))
                    return 0;
                if (X == BurrowX())
                    return (2 * Y + 2) * CostMultipliers[Type];
                
                return (int)(1.5d * Y + Math.Abs(X - BurrowX()) + 1) * CostMultipliers[Type];
            }
        }

        class Map
        {
            public Map OrigMap { get; set; }
            public int Step { get; set; }
            public List<Amphiod> Hallway { get; set; } = new List<Amphiod>();
            public Dictionary<string, List<Amphiod>> Burrows { get; set; } = new Dictionary<string, List<Amphiod>>();
            public int ActualCost { get; set; }
            public int HeuristicCost
            {
                get
                {
                    return Hallway.Union(Burrows.SelectMany(kvp => kvp.Value)).Sum(a => a.GetHeuristicCost(Burrows));
                }
            }

            private bool PathFree(int fromX, int toX)
            {
                if (fromX < toX && Hallway.Any(a => a.X > fromX && a.X <= toX))
                    return false;
                if (fromX > toX && Hallway.Any(a => a.X < fromX && a.X >= toX))
                    return false;
                return true;
            }

            public List<Map> GetNexSteps()
            {
                List<Map> ret = new List<Map>();
                foreach (var a in Hallway)
                {
                    if(Burrows[a.Type].All(ba => ba.Type == a.Type) && PathFree(a.X, a.BurrowX()))
                    {
                        var y = BurrowSize - Burrows[a.Type].Count();
                        if (y == 0)
                            continue;
                        var x = Math.Abs(a.X - a.BurrowX());
                        var cost = CostMultipliers[a.Type] * (x + y);
                        Map m = new Map { OrigMap = this, Step = Step + 1, Hallway = Hallway.Except(new List<Amphiod> { a }).ToList(), ActualCost = ActualCost + cost };
                        m.Burrows = Burrows.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToList());
                        m.Burrows[a.Type].Insert(0, new Amphiod { Type = a.Type, X = a.BurrowX(), Y = y });
                        ret.Add(m);
                    }
                }
                
                if (ret.Any()) 
                    return ret;

                foreach(var kvp in Burrows)
                {
                    if(!kvp.Value.All(a =>a.Type == kvp.Key))
                    {
                        var a = kvp.Value.First();
                        if (Burrows[a.Type].All(ba => ba.Type == a.Type) && PathFree(a.X, a.BurrowX()))
                        {
                            var y = BurrowSize - Burrows[a.Type].Count();
                            if (y == 0)
                                continue;
                            var x = Math.Abs(a.X - a.BurrowX());
                            var cost = CostMultipliers[a.Type] * (a.Y + x + y);

                            Map m = new Map { OrigMap = this, Step = Step + 1, Hallway = Hallway.ToList(), ActualCost = ActualCost + cost };
                            m.Burrows = Burrows.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToList());
                            m.Burrows[kvp.Key].Remove(a);
                            m.Burrows[a.Type].Insert(0, new Amphiod { Type = a.Type, X = a.BurrowX(), Y = y });
                            ret.Add(m);
                        }
                        else
                        {
                            foreach (var px in HallwayPoss)
                            {
                                if (PathFree(a.X, px))
                                {
                                    var y = a.Y;
                                    var x = Math.Abs(a.X - px);
                                    var cost = CostMultipliers[a.Type] * (x + y);

                                    Map m = new Map { OrigMap = this, Step = Step + 1, Hallway = Hallway.Append(new Amphiod { Type = a.Type, X = px, Y = 0 }).ToList(), ActualCost = ActualCost + cost };
                                    m.Burrows = Burrows.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToList());
                                    m.Burrows[kvp.Key].Remove(a);
                                    ret.Add(m);
                                }
                            }
                        }
                    }
                }
                return ret;
            }
            public void VisualizeR()
            {
                if (OrigMap != null)
                {
                    OrigMap.VisualizeR();
                }
                Console.WriteLine();
                Visualize();
            }

            public void Visualize()
            {
                Console.WriteLine("#############");
                Console.Write("#");
                for(int i = 0; i <= 10; i++)
                {
                    var am = Hallway.SingleOrDefault(a => a.X == i);
                    if (am != null)
                        Console.Write(am.Type);
                    else
                        Console.Write(".");
                }
                Console.Write("#");

                Console.WriteLine();
                for (int i = 0; i < BurrowSize; i++)
                {
                    Console.Write("###");
                    var am = Burrows["A"].SingleOrDefault(a => a.Y == i + 1);
                    if (am != null)
                        Console.Write(am.Type);
                    else
                        Console.Write(".");
                    Console.Write("#");
                    am = Burrows["B"].SingleOrDefault(a => a.Y == i + 1);
                    if (am != null)
                        Console.Write(am.Type);
                    else
                        Console.Write(".");
                    Console.Write("#");
                    am = Burrows["C"].SingleOrDefault(a => a.Y == i + 1);
                    if (am != null)
                        Console.Write(am.Type);
                    else
                        Console.Write(".");
                    Console.Write("#");
                    am = Burrows["D"].SingleOrDefault(a => a.Y == i + 1);
                    if (am != null)
                        Console.Write(am.Type);
                    else
                        Console.Write(".");
                    Console.Write("###");
                    Console.WriteLine();

                }
                Console.WriteLine("#############");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            Map currMap = new Map { ActualCost = 0, Hallway = new List<Amphiod> { },
                Burrows = new Dictionary<string, List<Amphiod>>
                {
                    { "A", new List<Amphiod>{ new Amphiod { Type = "C", X = 2, Y = 1 }, new Amphiod { Type = "D", X = 2, Y = 2 }, new Amphiod { Type = "D", X = 2, Y = 3 }, new Amphiod { Type = "B", X = 2, Y = 4 } } },
                    { "B", new List<Amphiod>{ new Amphiod { Type = "A", X = 4, Y = 1 }, new Amphiod { Type = "C", X = 4, Y = 2 }, new Amphiod { Type = "B", X = 4, Y = 3 }, new Amphiod { Type = "A", X = 4, Y = 4 } } },
                    { "C", new List<Amphiod>{ new Amphiod { Type = "B", X = 6, Y = 1 }, new Amphiod { Type = "B", X = 6, Y = 2 }, new Amphiod { Type = "A", X = 6, Y = 3 }, new Amphiod { Type = "D", X = 6, Y = 4 } } },
                    { "D", new List<Amphiod>{ new Amphiod { Type = "D", X = 8, Y = 1 }, new Amphiod { Type = "A", X = 8, Y = 2 }, new Amphiod { Type = "C", X = 8, Y = 3 }, new Amphiod { Type = "C", X = 8, Y = 4 } } }
                }
            };

            PriorityQueues.FibonacciHeap<Map, int> pq = new PriorityQueues.FibonacciHeap<Map, int>(PriorityQueues.PriorityQueueType.Minimum);
            pq.Enqueue(currMap, currMap.HeuristicCost + currMap.ActualCost);
            Map result;

            while(pq.Any())
            {
                currMap = pq.Dequeue();

                var ps = currMap.GetNexSteps();
                result = ps.FirstOrDefault(m => m.HeuristicCost == 0);
                if (result != null)
                {
                    break;
                }
                
                foreach (var nm in ps.Where(m => m.HeuristicCost > 0 ))
                {
                    pq.Enqueue(nm, (int)(currMap.ActualCost + nm.HeuristicCost));
                }
            }
        }
    }
}
