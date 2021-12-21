using System;
using System.Collections.Generic;
using System.Linq;

namespace Day21
{
    class Program
    {
        class Player
        {
            public int Pos { get; set; }
            public int Point { get; set; }
            public void Move(int step)
            {
                Pos += step;
                Pos = Pos % 10;
                if(Pos == 0)
                { 
                    Pos = 10;
                }
                
                Point += Pos;
            }
        }

        class Dice
        {
            public int Rolls { get; set; }
            public int State { get; set; }
            public int Roll()
            {
                Rolls+=3;
                if(State < 98)
                {
                    var ret = 3 * State + 6;
                    State += 3;
                    State %= 100;
                    return ret;
                }
                else if (State == 98)
                {
                    State = 1;
                    return 99 + 100 + 1;
                }
                else if (State == 99)
                {
                    State = 2;
                    return 100 + 1 + 2;
                }
                throw new Exception("WTF");
            }
        }

        static void Main(string[] args)
        {
            var p1 = new Player { Pos = 10 };
            var p2 = new Player { Pos = 8 };
            var d = new Dice { };
            bool p1turn = true;
            while (p1.Point < 1000 && p2.Point < 1000)
            {
                var r = d.Roll();
                if (p1turn)
                {
                    p1turn = false;
                    p1.Move(r);
                    Console.WriteLine($"P1, pos: {p1.Pos}, point {p1.Point}");
                }
                else
                {
                    p1turn = true;
                    p2.Move(r);
                    Console.WriteLine($"P2, pos: {p2.Pos}, point {p2.Point}");
                }
            }

            var pp1 = new Player { Pos = 10 };
            Dictionary<int, Dictionary<int, int>> helper = new Dictionary<int, Dictionary<int, int>>();
            for (int pos = 1; pos <= 10; pos++)
            {
                helper.Add(pos, new Dictionary<int, int>());
                for (int step = 3; step <= 9; step++)
                {
                    pp1.Pos = pos;
                    pp1.Move(step);
                    helper[pos].Add(step, pp1.Pos);
                }
            }
            var p1Unis = ComputeUnis(helper, new List<(string, int, int)> { ("", 10, 0) });
            var p2Unis = ComputeUnis(helper, new List<(string, int, int)> { ("", 8, 0) });

            long p1Wins = p1Unis.Item1[3] * 27 * 27;
            for (int i = 4; i <= 10; i++)
            {
                p1Wins += p1Unis.Item1[i] * p2Unis.Item2[i - 1];
            }

            long p2Wins = 0;
            for (int i = 3; i <= 10; i++)
            {
                p2Wins += p2Unis.Item1[i] * p1Unis.Item2[i];
            }
        }

        private static long UniNum(string uni)
        {
            long ret = 1;
            foreach(var ch in uni)
            {
                switch(ch)
                {
                    case '4':
                        ret *= 3; break;
                    case '5':
                        ret *= 6; break;
                    case '6':
                        ret *= 7; break;
                    case '7':
                        ret *= 6; break;
                    case '8':
                        ret *= 3; break;
                }
            }
            return ret;
        }

        private static (Dictionary<int, long>, Dictionary<int, long>) ComputeUnis(Dictionary<int, Dictionary<int, int>> helper, List<(string path, int pos, int points)> unis)
        {
            Dictionary<int, long> unum = new Dictionary<int, long>();
            for (int i = 3; i <= 10; i++)
            {
                unum[i] = 0;
            }
            while (unis.Any())
            {
                List<(string path, int pos, int points)> newUnis = new List<(string path, int pos, int points)>();
                foreach (var uni in unis)
                {
                    for (int step = 3; step <= 9; step++)
                    {
                        var newPos = helper[uni.pos][step];
                        var newPoint = uni.points + newPos;
                        if (newPoint >= 21)
                        {
                            var cp = uni.path + step.ToString();
                            unum[cp.Length] += UniNum(cp);
                        }
                        else
                        {
                            newUnis.Add((uni.path + step.ToString(), newPos, uni.points + newPos));
                        }
                    }
                }
                unis.Clear();
                unis = newUnis;
            }

            Dictionary<int, long> funum = new Dictionary<int, long>();
            funum[3] = (long)Math.Pow(27, 3) - unum[3];
            for (int i = 4; i <= 10; i++)
            {

                funum[i] = (funum[i - 1] * 27) - unum[i];
            }
            return (unum, funum);
        }
    }
}
