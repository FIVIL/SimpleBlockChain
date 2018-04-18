using System;
using System.Collections.Generic;
using System.Text;

namespace NoobChain
{
    class Block
    {
        public string Hash { get; private set; }
        public string PreviousHash { get; private set; }
        public string Data { get; private set; }
        public DateTime TimeStamp { get; }
        public Block(string data,string previousHash)
        {
            Data = data;
            PreviousHash = previousHash;
            TimeStamp = DateTime.UtcNow;
            Hash = GetHashString();
        }
        public override string ToString()
        {
            return Hash;
        }
        public string GetHashString()
        {
            return (Data + PreviousHash + TimeStamp.ToBinary().ToString()).ApplyBlacke2();
        }
    }
}
