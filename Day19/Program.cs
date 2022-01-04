using GeometRi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day19
{
    class Program
    {
        struct Point
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }

            public Point(int x, int y, int z) { X = x; Y = y; Z = z; }

            public Point Translate(Point v)
            {
                return new Point(X + v.X, Y + v.Y, Z + v.Z);
            }

            public override string ToString()
            {
                return $"({X},{Y},{Z})";
            }
        }

        class Scanner
        {
            public string Name { get; set; }
            public Point? Position { get; set; }
            public List<Point> Beacons { get; set; }

            public (bool success, List<Point> absolutes) Match(List<Point> rotatedTranslatedBeacons)
            {
                if (Beacons.Count(b => rotatedTranslatedBeacons.Contains(b)) >= 12)
                {
                    return (true, rotatedTranslatedBeacons.ToList());
                }
                return (false, null);
            }

            public (bool success, List<Point> absolutes, Point? scannerTrans, List<Point> relativesRotated) FindMatch(List<Point> rotatedRelativeBeacons)
            {
                foreach(var b in Beacons)
                {
                    foreach(var ob in rotatedRelativeBeacons)
                    {
                        var db = new Point(b.X - ob.X, b.Y - ob.Y, b.Z - ob.Z);
                        if(Math.Abs(db.X) > 2000 || Math.Abs(db.Y) > 2000 || Math.Abs(db.Z) > 2000)
                        {
                            continue;
                        }
                        var trbs = rotatedRelativeBeacons.Select(tb => tb.Translate(db)).ToList();
                        var isMatch = Match(trbs);
                        if(isMatch.success)
                        {
                            return (true, isMatch.absolutes, db, rotatedRelativeBeacons.ToList());
                        }
                    }
                }
                return (false, null, null, null);
            }

            public (bool success, List<Point> absolutes, Point? scannerTrans, List<Point> relativesRotated) FindOriantation(List<Point> otherBeacons)
            {
                var aobs = otherBeacons.Select(b => GetPermutations(b)).ToList();
                for(int i = 0; i < aobs.First().Count; i++)
                {
                    var nls = aobs.Select(b => b[i]).ToList();
                    var x = FindMatch(nls);
                    if (x.success)
                    {
                        return x;
                    }
                }
                return (false, null, null, null);
            }
        }


        static List<Point> GetPermutations(Point p)
        {
            List<Point> ret = new List<Point>();
            ret.Add(new Point(p.X, p.Y, p.Z));
            ret.Add(new Point(p.X, p.Y, -p.Z ));
            ret.Add(new Point(p.X, -p.Y, p.Z ));
            ret.Add(new Point(p.X, -p.Y, -p.Z ));
            ret.Add(new Point(-p.X, p.Y, p.Z ));
            ret.Add(new Point(-p.X, p.Y, -p.Z ));
            ret.Add(new Point(-p.X, -p.Y, p.Z));
            ret.Add(new Point(-p.X, -p.Y, -p.Z));

            ret.Add(new Point(p.X, p.Z, p.Y));
            ret.Add(new Point(p.X, p.Z, -p.Y));
            ret.Add(new Point(p.X, -p.Z, p.Y));
            ret.Add(new Point(p.X, -p.Z, -p.Y));
            ret.Add(new Point(-p.X, p.Z, p.Y));
            ret.Add(new Point(-p.X, p.Z, -p.Y));
            ret.Add(new Point(-p.X, -p.Z, p.Y));
            ret.Add(new Point(-p.X, -p.Z, -p.Y));

            ret.Add(new Point(p.Y, p.X, p.Z));
            ret.Add(new Point(p.Y, p.X, -p.Z));
            ret.Add(new Point(p.Y, -p.X, p.Z));
            ret.Add(new Point(p.Y, -p.X, -p.Z));
            ret.Add(new Point(-p.Y, p.X, p.Z));
            ret.Add(new Point(-p.Y, p.X, -p.Z));
            ret.Add(new Point(-p.Y, -p.X, p.Z));
            ret.Add(new Point(-p.Y, -p.X, -p.Z));

            ret.Add(new Point(p.Y, p.Z, p.X));
            ret.Add(new Point(p.Y, p.Z, -p.X));
            ret.Add(new Point(p.Y, -p.Z, p.X));
            ret.Add(new Point(p.Y, -p.Z, -p.X));
            ret.Add(new Point(-p.Y, p.Z, p.X));
            ret.Add(new Point(-p.Y, p.Z, -p.X));
            ret.Add(new Point(-p.Y, -p.Z, p.X));
            ret.Add(new Point(-p.Y, -p.Z, -p.X));

            ret.Add(new Point(p.Z, p.X, p.Y));
            ret.Add(new Point(p.Z, p.X, -p.Y));
            ret.Add(new Point(p.Z, -p.X, p.Y));
            ret.Add(new Point(p.Z, -p.X, -p.Y));
            ret.Add(new Point(-p.Z, p.X, p.Y));
            ret.Add(new Point(-p.Z, p.X, -p.Y));
            ret.Add(new Point(-p.Z, -p.X, p.Y));
            ret.Add(new Point(-p.Z, -p.X, -p.Y));

            ret.Add(new Point(p.Z, p.Y, p.X));
            ret.Add(new Point(p.Z, p.Y, -p.X));
            ret.Add(new Point(p.Z, -p.Y, p.X));
            ret.Add(new Point(p.Z, -p.Y, -p.X));
            ret.Add(new Point(-p.Z, p.Y, p.X));
            ret.Add(new Point(-p.Z, p.Y, -p.X));
            ret.Add(new Point(-p.Z, -p.Y, p.X));
            ret.Add(new Point(-p.Z, -p.Y, -p.X));


            return ret;
        }

        static void Main(string[] args)
        {
            List<Scanner> scanners = new List<Scanner>();
            var lines = File.ReadAllLines("input.txt");
            var cs = new Scanner { Beacons = new List<Point> { } };
            foreach(var line in lines)
            {
                if(line.StartsWith("---"))
                {
                    cs.Name = line;
                    continue;
                }
                else if(string.IsNullOrEmpty(line))
                {
                    scanners.Add(cs);
                    cs = new Scanner { Beacons = new List<Point> { } };
                }
                else
                {
                    var coords = line.Split(",");
                    cs.Beacons.Add(new Point(int.Parse(coords[0]), int.Parse(coords[1]), int.Parse(coords[2])));
                }
            }
            scanners.Add(cs);

            scanners[0].Position = new Point(0, 0, 0);
            Dictionary<string, HashSet<string>> falses = new Dictionary<string, HashSet<string>>();
            foreach (var s in scanners) falses.Add(s.Name, new HashSet<string>());
            while(scanners.Any(s=>s.Position == null))
            {
                var unknownScanners = scanners.Where(ts => ts.Position == null).ToList();
                var knownscanners = scanners.Where(ts => ts.Position != null).ToList();
                Console.WriteLine($"un: {unknownScanners.Count} known: {knownscanners.Count}");

                foreach (var us in unknownScanners)
                {
                    bool found = false;
                    foreach(var ks in knownscanners)
                    {
                        if (falses[ks.Name].Contains(us.Name))
                            continue;
                        var m = ks.FindOriantation(us.Beacons);
                        if (m.success)
                        {
                            Console.WriteLine($"{ks.Name} {us.Name}");
                            found = true;
                            us.Beacons = m.relativesRotated.ToList();
                            us.Position = ks.Position.Value.Translate(m.scannerTrans.Value);
                            break;
                        }
                        else
                        {
                            falses[us.Name].Add(ks.Name);
                            falses[ks.Name].Add(us.Name);
                        }
                    }
                    if (found)
                        break;
                }
            }

            var b2s = new List<Point>();
            foreach(var s in scanners)
            {
                foreach(var b in s.Beacons)
                {
                    var ab = b.Translate(s.Position.Value);
                    if (!b2s.Contains(ab))
                        b2s.Add(ab);
                }
            }

            int maxDistance = 0;
            for(int i = 0; i < scanners.Count - 1; i++)
            {
                for(int j = i + 1;j <scanners.Count;j++)
                {
                    var dist = Math.Abs(scanners[i].Position.Value.X - scanners[j].Position.Value.X) +
                        Math.Abs(scanners[i].Position.Value.Y - scanners[j].Position.Value.Y) +
                        Math.Abs(scanners[i].Position.Value.Z - scanners[j].Position.Value.Z);
                    if (dist > maxDistance)
                        maxDistance = dist;
                }
            }
        }
    }
}
