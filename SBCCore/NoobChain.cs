using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;

namespace NoobChain
{
    public static class NoobChaiN
    {
        private static List<Block> Blocks;
        private static Block Genesis;
        private static Block Prev;
        public static string FirstBlockHash { get; private set; }
        private static int difficulty;
        private static bool settingDifficulty;
        public static int Difficulty
        {
            get => difficulty;
            set
            {
                if (!settingDifficulty)
                {
                    difficulty = value;
                    settingDifficulty = true;
                }
                else { }
            }
        }
        public static Dictionary<string, Transaction.TransactionOutput> UTXOs { get; private set; }
        private const int BlockSize = 50;
        static NoobChaiN()
        {
            difficulty = 3;
            settingDifficulty = false;
            Genesis = new Block(Guid.NewGuid().ToString().ApplyBlacke2());
            //Genesis = new Block("Genesis", "0");
            FirstBlockHash = Genesis.PreviousHash;
            Blocks = new List<Block>()
            {
               Genesis
            };
            Prev = Genesis;
            Prev.Miner(difficulty);
            UTXOs = new Dictionary<string, Transaction.TransactionOutput>();
        }
        public static void AddBlock(int count)
        {
            for (int i = 0; i < count; i++)
            {
                AddBlock();
            }
        }
        private static void AddBlock()

        {
            var block = new Block(Prev.Hash);
            Blocks.Add(block);
            Prev = block;
            Prev.Miner(difficulty);
        }
        public static new string ToString()
        {
            StringBuilder sb = new StringBuilder("Blocks Lists:");
            sb.AppendLine();
            int index = 0;
            foreach (var item in Blocks)
            {
                sb.Append(++index);
                sb.Append(" : ");
                sb.Append(item.ToString());
                sb.AppendLine();
            }
            return sb.ToString();
        }
        public static string ToJson()
        {
            return JsonConvert.SerializeObject(Blocks);
        }
        public static ValidationTypes IsChainValid()
        {
            string hashTarget = new string(new char[difficulty]).Replace('\0', '0');
            Block curr, prev;
            for (int i = 1; i < Blocks.Count; i++)
            {
                curr = Blocks[i];
                prev = Blocks[i - 1];
                if (!curr.Hash.Equals(curr.GetHashString())) return ValidationTypes.NotValidCurrent;
                if (!prev.Hash.Equals(curr.PreviousHash)) return ValidationTypes.NotValidPervious;
                if (!curr.Hash.Substring(0, difficulty).Equals(hashTarget)) return ValidationTypes.HasntMinedYet;
            }
            return ValidationTypes.Valid;
        }
    }
}
