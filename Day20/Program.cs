using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day20
{
    class Program
    {
        class Image
        {
            public Dictionary<long, Dictionary<long, bool>> Map { get; set; }
            public string OutSidePixels = "0";

            public int PixelBinary(long x, long y)
            {
                string bi = "";
                for(int dy = -1; dy <= 1 ; dy++)
                  for (int dx = -1; dx <= 1; dx++)
                    {
                        if(Map.TryGetValue(y + dy, out Dictionary<long, bool> value))
                        {
                            if (value.TryGetValue(x + dx, out bool p))
                            {
                                bi += p ? "1" : "0";
                            }
                            else
                            {
                                bi += OutSidePixels;
                            }
                        }
                        else
                        {
                            bi += OutSidePixels;
                        }
                    }

                var ret = Convert.ToInt32(bi, 2);
                return ret;
            }

            public long MinY { get; set; }
            public long MaxY { get; set; }

            public long MinX { get; set; }
            public long MaxX { get; set; }

            public int LightPixelNum { get { return Map.SelectMany(kvp => kvp.Value.Values).Where(p => p).Count(); } }

            public void Draw()
            {
                for (long y = MinY; y < MaxY + 1; y++)
                {
                    for (long x = MinX; x < MaxX + 1; x++)
                    {
                        Console.Write((Map[y][x])? "#" : ".");
                    }

                    Console.WriteLine();
                }

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }

            public static Image Parse(List<string> rows)
            {
                Image ret = new Image { Map = new Dictionary<long, Dictionary<long, bool>>() };
                for(int y = 0; y <rows.Count;y++)
                {
                    ret.Map.Add(y, new Dictionary<long, bool>());
                    for (int x = 0; x < rows.First().Length; x++)
                    {
                        ret.Map[y].Add(x, rows[y][x] == '#');
                    }
                }
                ret.MinY = 0;
                ret.MinX = 0;
                ret.MaxY = rows.Count;
                ret.MaxX = rows.First().Length;
                return ret;
            }
        }

        static Image Step(Image image, string iea)
        {
            var ret = new Image { Map = new Dictionary<long, Dictionary<long, bool>>() };
            ret.MinY = image.MinY - 3;
            ret.MaxY = image.MaxY + 3;
            ret.MinX = image.MinX - 3;
            ret.MaxX = image.MaxX + 3;

            for (long y = image.MinY - 3; y < image.MaxY + 4; y ++)
            {
                ret.Map.Add(y, new Dictionary<long, bool>());
                for (long x = image.MinX - 3; x < image.MaxX + 4; x++)
                {
                    var bi = image.PixelBinary(x, y);
                    var p = (iea[bi] == '#') ? true : false;
                    ret.Map[y].Add(x, p);
                }
            }
            ret.OutSidePixels = image.OutSidePixels == "0" ? "1" : "0";
            return ret;
        }

        static void Main(string[] args)
        {
            string iea = File.ReadAllText("input1.txt");
            Image image = Image.Parse(File.ReadAllLines("input2.txt").ToList());
            
            for(int i = 0; i < 50; i ++)
            {
                image = Step(image, iea);
            }

            image.Draw();
            var part2 = image.LightPixelNum;
        }
    }
}
