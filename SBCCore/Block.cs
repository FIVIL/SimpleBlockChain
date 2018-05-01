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
        public string MerkleRoot { get; private set; }
        public List<Transaction> Transactions { get; private set; }
        public DateTime TimeStamp { get; }
        public uint Nonce { get; private set; }
        private static Random rnd;
        static Block()
        {
            rnd = new Random();
        }
        public Block(string previousHash)
        {
            Transactions = new List<Transaction>();
            PreviousHash = previousHash;
            TimeStamp = DateTime.UtcNow;
            Nonce = (uint)rnd.Next();
            Hash = GetHashString();
        }
        public override string ToString()
        {
            return Hash;
        }
        public string GetHashString()
        {
            return (MerkleRoot + PreviousHash + TimeStamp.ToBinary().ToString() + Nonce).ApplyBlacke2();
        }
        public void Miner(int difficulty)
        {
            MerkleRoot = Transactions.GenerateMerkleRoot();
            Stopwatch SW = new Stopwatch();
            SW.Start();
            string target = new string(new char[difficulty]).Replace('\0', '0');
            while (!Hash.Substring(0, difficulty).Equals(target))
            {
                Nonce++;
                Hash = GetHashString();
            }
            SW.Stop();
            Console.WriteLine("Mined!!! total time: " + SW.ElapsedMilliseconds + " new Hash: " + Hash);
        }
        public bool AddTransaction(Transaction transaction)
        {
            if (transaction == null)
                return false;
            if (!PreviousHash.Equals(NoobChaiN.FirstBlockHash))
            {
                if (!transaction.Process())
                    return false;
            }
            Transactions.Add(transaction);
            return true;
        }
    }
}
