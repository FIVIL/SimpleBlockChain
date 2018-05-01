using System;
using System.Text;
using NoobChain;

namespace SimpleBlockChain
{
    class Program
    {     
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write(">");
                var s = Console.ReadLine();
                if (s.Equals("stop")) break;
                if (s.Equals("add"))
                {
                    Console.WriteLine("How many Block?!!");
                    var p = int.Parse(Console.ReadLine());
                    NoobChaiN.AddBlock(p);
                }
                else if (s.Equals("show")) Console.WriteLine(NoobChaiN.ToString());
                else if (s.Equals("json")) Console.WriteLine(NoobChaiN.ToJson());
                else if (s.Equals("validation")) Console.WriteLine(NoobChaiN.IsChainValid());
                else if (s.Equals("file")) System.IO.File.WriteAllText("result.json", NoobChaiN.ToJson());
                else Console.WriteLine("wrong Command");
            }
        }
    }
}
