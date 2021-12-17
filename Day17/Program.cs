using System;

namespace Day17
{
    class Program
    {
        static (bool, int) IsHit(int minX, int maxX, int minY, int maxY, int ivx, int ivy)
        {
            int x = 0;
            int y = 0;
            int maxhy = 0;
            while (x < maxX && y > minY)
            {
                x += ivx;
                y += ivy;
                if(y > maxhy)
                {
                    maxhy = y;
                }
                if(x >= minX && x <= maxX && y >= minY && y <= maxY)
                {
                    return (true, maxhy);
                }

                if(ivx > 0)
                {
                    ivx--;
                }
                ivy--;
            }

            return (false, -1);
        }


        static void Main(string[] args)
        {
            int minX = 281;
            int maxX = 311;
            int minY = -74;
            int maxY = -54;

            int part1 = 0;
            int part2 = 0;

            for (int vx = 0; vx < maxX + 1; vx++)
                for (int vy = -100; vy < 100; vy++)
                {
                    var h = IsHit(minX, maxX, minY, maxY, vx, vy);
                    if(h.Item1)
                    {
                        part2++;
                    }
                    if (h.Item1 && h.Item2 > part1)
                    {
                        part1 = h.Item2;
                    }
                }
        }
    }
}
