using System;
using System.Collections.Generic;
using System.Text;

namespace NoobChain
{
    public class BlockModel
    {
        public string Hash { get; private set; }
        public string PreviousHash { get; private set; }
        public string MerkleRoot { get; private set; }
        public List<TransactionModel> Transactions { get; private set; }
        public DateTime TimeStamp { get; }
        public uint Nonce { get; private set; }
        public BlockModel(Block b)
        {
            Hash = b.Hash;
            PreviousHash = b.PreviousHash;
            MerkleRoot = b.MerkleRoot;
            Transactions = new List<TransactionModel>();
            foreach (var item in b.Transactions)
            {
                Transactions.Add(new TransactionModel(item));
            }
            TimeStamp = b.TimeStamp;
            Nonce = b.Nonce;
        }
    }
}
