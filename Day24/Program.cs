using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day24
{
    class Program
    {
        class Alu
        {
            public string Input { get; set; }
            public int GetNextInput()
            {
                int ret = int.Parse(Input[0].ToString());
                Input = Input.Substring(1);
                return ret;
            }

            public Dictionary<string, long> Registers { get; set; }
                        = new Dictionary<string, long> { { "w", 0 }, { "x", 0 }, { "y", 0 }, { "z", 0 } };

            public void Process(string inst, string lp, string rp)
            {
                long rpi = 0;
                if (rp.Length > 0)
                {
                    if (char.IsLetter(rp[0]))
                    {
                        rpi = Registers[rp];
                    }
                    else
                    {
                        rpi = long.Parse(rp);
                    }
                }

                switch (inst)
                {
                    case "add":
                        Registers[lp] = Registers[lp] + rpi;
                        break;
                    case "mul":
                        Registers[lp] = Registers[lp] * rpi;
                        break;
                    case "div":
                        var t = Registers[lp] / (decimal)rpi;
                        t = t < 0 ? Math.Ceiling(t) : Math.Floor(t);
                        Registers[lp] = (long)t;
                        break;
                    case "mod":
                        Registers[lp] = Registers[lp] % rpi;
                        break;
                    case "eql":
                        Registers[lp] = (Registers[lp] == rpi) ? 1 : 0;
                        break;
                    case "inp":
                        Registers[lp] = GetNextInput();
                        break;
                    default:
                        throw new Exception();
                }
            }
        }

        static void Main(string[] args)
        {
            Regex regex = new Regex("(...) (.)(.*)");
            var instructions = File.ReadAllLines("input.txt").Select(l => regex.Match(l)).Select(m => (inst: m.Groups[1].Value, lp: m.Groups[2].Value, rp: m.Groups[3].Value.Trim()));

            int numNum = 0;
            var possibles = new Dictionary<int, Dictionary<long, string>>();
            possibles.Add(-1, new Dictionary<long, string> { { 0, "" } });
            while (numNum < 14)
            {
                var currPart = instructions.ToList().Skip(numNum * 18).Take(18).ToList();
                possibles.Add(numNum, new Dictionary<long, string>());

                var pps = possibles[numNum - 1];
                if(possibles[numNum - 1].Any(p => p.Key < 1000))
                {
                    pps = possibles[numNum - 1].Where(p => p.Key < 1000).ToDictionary(k => k.Key, v => v.Value);
                }

                foreach (var inZ in pps)
                {
                    for (int n = 1; n <= 9; n++)
                    {
                        Alu alu = new Alu { Input = n.ToString() };
                        alu.Registers["z"] = inZ.Key;
                        foreach (var instruction in currPart)
                        {
                            alu.Process(instruction.inst, instruction.lp, instruction.rp);
                        }

                        if (possibles[numNum].TryGetValue(alu.Registers["z"], out string last))
                        {
                            var ll = long.Parse(last);
                            var cl = long.Parse(inZ.Value + n.ToString());
                            if (cl < ll)
                            {
                                possibles[numNum][alu.Registers["z"]] = cl.ToString();
                            }
                        }
                        else
                        {
                            possibles[numNum].Add(alu.Registers["z"], inZ.Value + n.ToString());
                        }
                    }
                }

                numNum++;
            }

            var res = possibles[13][0];
        }
    }
}