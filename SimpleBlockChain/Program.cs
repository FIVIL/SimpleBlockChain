using System;
using System.Collections.Generic;
using System.Text;
using NoobChain;

namespace SimpleBlockChain
{
    class Program
    {
        static void Main(string[] args)
        {
            var wallets = new List<Wallet>();
            wallets.Add(NoobChaiN.FirstWallet);
            while (true)
            {
                Console.Write(">");
                var s = Console.ReadLine();
                if (s.Equals("stop")) break;
                else if (s.Equals("show")) Console.WriteLine(NoobChaiN.ToString());
                else if (s.Equals("json")) Console.WriteLine(NoobChaiN.ToJson());
                else if (s.Equals("validation")) Console.WriteLine(NoobChaiN.IsChainValid());
                else if (s.Equals("file")) System.IO.File.WriteAllText("result.json", NoobChaiN.ToJson());
                else if (s.Equals("cw")) wallets.Add(new Wallet());
                else if (s.Equals("bs")) NoobChaiN.BlockSize++;
                else if (s.Equals("s"))
                {
                    Console.WriteLine("Enter Sender Wallet Number:");
                    var sender = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter Reciepient Wallet Number:");
                    var Reciepient = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter amount:");
                    var amount = double.Parse(Console.ReadLine());
                    NoobChaiN.SendFunds(wallets[sender - 1], wallets[Reciepient - 1].PublicKey, amount);
                }
                else if (s.Equals("ws"))
                {
                    foreach (var item in wallets)
                    {
                        Console.WriteLine(item.PublicKey.ToStringKey() + " : " + item.Balance);
                    }
                }
                else Console.WriteLine("wrong Command");
            }
        }
    }
}
