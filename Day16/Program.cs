using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day16
{
    class Program
    {
        class Package
        {
            public int Version { get; set; }
            public int TypeId { get; set; }
            public long LiteralValue { get; set; }
            public List<Package> SubPackages { get; set; } = new List<Package>();

            public int SumVersion()
            {
                int ret = Version;
                foreach(var p in SubPackages)
                {
                    ret += p.SumVersion();
                }
                return ret;
            }

            public long Eval()
            {
                switch(TypeId)
                {
                    case 0:
                        return SubPackages.Select(p => p.Eval()).Sum();
                    case 1:
                        return SubPackages.Aggregate((long)1, (acc, p) => acc * p.Eval());
                    case 2:
                        return SubPackages.Select(p => p.Eval()).Min();
                    case 3:
                        return SubPackages.Select(p => p.Eval()).Max();
                    case 4:
                        return LiteralValue;
                    case 5:
                        return SubPackages[0].Eval() > SubPackages[1].Eval() ? 1 : 0;
                    case 6:
                        return SubPackages[0].Eval() < SubPackages[1].Eval() ? 1 : 0;
                    case 7:
                        return SubPackages[0].Eval() == SubPackages[1].Eval() ? 1 : 0;
                }
                throw new Exception(TypeId.ToString());
            }

            public static (Package, string) ParseX(string input)
            {
                var r = Parse(input);
                return r;
            }

            public static (Package, string) Parse(string input)
            {
                Package ret = new Package();
                string residue = "";
                ret.Version = Convert.ToInt32(input.Substring(0, 3), 2);
                ret.TypeId = Convert.ToInt32(input.Substring(3, 3), 2);
                if(ret.TypeId == 4)
                {
                    var o = ParseLiteral(input.Substring(6));
                    ret.LiteralValue = o.Item1;
                    residue = o.Item2;
                    
                }
                else
                {
                    var o = ParseOp(input.Substring(6));
                    ret.SubPackages = o.Item1;
                    residue = o.Item2;
                }
                return (ret, residue);
            }

            private static (List<Package>, string) ParseOp(string input)
            {
                List<Package> ret = new List<Package>();
                string res = input.Substring(1);
                int lengTypeId = int.Parse(input[0].ToString());
                if(lengTypeId == 0)
                {
                    res = input.Substring(16);
                    var origLength = res.Length;
                    long totLen = Convert.ToInt64(input.Substring(1, 15), 2);
                    while(origLength - res.Length != totLen)
                    {
                        var pr = Parse(res);
                        ret.Add(pr.Item1);
                        res = pr.Item2;
                    }
                }
                else
                {
                    res = input.Substring(12);
                    ulong packageNum = Convert.ToUInt64(input.Substring(1, 11), 2);
                    for (ulong i = 0; i < packageNum; i++)
                    {
                        var pr = Parse(res);
                        ret.Add(pr.Item1);
                        res = pr.Item2;
                    }
                }

                return (ret, res);
            }

            private static (long, string) ParseLiteral(string input)
            {
                string retStr = "";
                string residue = "";
                string chunk = input.Substring(0, 5);
                int i = 0;
                while(chunk[0] == '1')
                {
                    retStr += chunk.Substring(1);
                    i++;
                    chunk = input.Substring(i * 5, 5);
                }
                retStr += chunk.Substring(1);
                residue = input.Substring((i + 1) * 5);
                return (Convert.ToInt64(retStr, 2), residue);
            }
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt");
            string binaryInput = "";
            foreach(var ch in input)
            {
                binaryInput += Convert.ToString(int.Parse(ch.ToString(), System.Globalization.NumberStyles.HexNumber), 2).PadLeft(4, '0');
            }

             var c = Package.ParseX(binaryInput);
            var part1 = c.Item1.SumVersion();
            var part2 = c.Item1.Eval();
        }
    }
}
