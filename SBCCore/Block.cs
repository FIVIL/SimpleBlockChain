using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace NoobChain
{
    class Block
    {
        public string Hash { get; private set; }
        public string PreviousHash { get; private set; }
        public string Data { get; private set; }
        public DateTime TimeStamp { get; }
        public int Nonce { get; private set; }
        private static Random rnd;
        static Block()
        {
            rnd = new Random();
        }
        public Block(string data, string previousHash)
        {
            Data = data;
            PreviousHash = previousHash;
            TimeStamp = DateTime.UtcNow;
            Nonce = rnd.Next();
            Hash = GetHashString();
        }
        public override string ToString()
        {
            return Hash;
        }
        public string GetHashString()
        {
            return (Data + PreviousHash + TimeStamp.ToBinary().ToString() + Nonce).ApplyBlacke2();
        }
        public async Task Miner(int difficulty)
        {
             await Task.Run(() =>
            {
                Stopwatch SW = new Stopwatch();
                SW.Start();
                string target = new string(new char[difficulty]).Replace('\0', '0');
                while (!Hash.Substring(0, difficulty).Equals(target))
                {
                    Nonce++;
                    Hash = GetHashString();
                }
                SW.Stop();
                Console.WriteLine("Mining is done!!! total time: " + SW.ElapsedMilliseconds);
            });
        }
    }
}
