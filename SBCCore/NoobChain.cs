using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace NoobChain
{
    public static class NoobChaiN
    {
        private static List<Block> Blocks;
        private static Block Genesis;
        private static Block Prev;
        static NoobChaiN()
        {
            Genesis = new Block("Genesis", Guid.NewGuid().ToString().ApplyBlacke2());
            Blocks = new List<Block>()
            {
               Genesis
            };
            Prev = Genesis;
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
            var block = new Block("Block number: " + (Blocks.Count), Prev.Hash);
            Blocks.Add(block);
            Prev = block;
        }
        public static string ToString()
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
            Block curr, prev;
            for (int i = 1; i < Blocks.Count; i++)
            {
                curr = Blocks[i];
                prev = Blocks[i - 1];
                if (!curr.Hash.Equals(curr.GetHashString())) return ValidationTypes.NotValidCurrent;
                if (!prev.Hash.Equals(curr.PreviousHash)) return ValidationTypes.NotValidPervious;
            }
            return ValidationTypes.Valid;
        }
    }
}
