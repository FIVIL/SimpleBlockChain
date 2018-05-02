using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;

namespace NoobChain
{
    public static class NoobChaiN
    {
        private static List<Block> Blocks { get; set; }
        private static Block Genesis { get; set; }
        private static Block Prev { get; set; }
        private static Transaction GenesisTransaction { get; set; }
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
        public static int BlockSize = 1;
        public static Wallet FirstWallet { get; private set; }
        private static List<Transaction> TempTransactions { get; set; }
        static NoobChaiN()
        {
            UTXOs = new Dictionary<string, Transaction.TransactionOutput>();
            TempTransactions = new List<Transaction>();
            difficulty = 3;
            settingDifficulty = false;
            var God = new Wallet();
            FirstWallet = new Wallet();
            GenesisTransaction = new Transaction(God.PublicKey, FirstWallet.PublicKey, 100, null,
                (Guid.NewGuid().ToString()).ApplyBlacke2());
            GenesisTransaction.GenerateSignature(God.PrivateKey);
            GenesisTransaction.Outputs.Add(new Transaction.TransactionOutput(GenesisTransaction.Reciepient, GenesisTransaction.Value, GenesisTransaction.ID));
            UTXOs.Add(GenesisTransaction.Outputs[0].ID, GenesisTransaction.Outputs[0]);
            Genesis = new Block(Guid.NewGuid().ToString().ApplyBlacke2());
            FirstBlockHash = Genesis.PreviousHash;
            Genesis.AddTransaction(GenesisTransaction);
            //Genesis = new Block("Genesis", "0");
            Blocks = new List<Block>()
            {
               Genesis
            };
            Prev = Genesis;
            Prev.Miner(difficulty);
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
            foreach (var item in TempTransactions)
            {
                block.AddTransaction(item);
            }
            block.Miner(difficulty);
            Blocks.Add(block);
            Prev = block;
            TempTransactions.Clear();
        }
        public static void SendFunds(Wallet Sender, Org.BouncyCastle.Crypto.Parameters.ECPublicKeyParameters Reciepient, double value)
        {
            var Tr = Sender.SendFunds(Reciepient, value);
            if (Tr == null) return;
            //needed because either way if wallet send new transaction withoud processing the prev one it 
            //will use spended trans while it shoud use left out tran
            //Tr.Process();
            TempTransactions.Add(Tr);
            if (TempTransactions.Count == BlockSize)
            {
                AddBlock();
            }

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
            return JsonConvert.SerializeObject(Blocks.ToModle());
        }
        public static ValidationTypes IsChainValid()
        {
            string hashTarget = new string(new char[difficulty]).Replace('\0', '0');
            Block curr, prev;
            var TempUTXOs = new Dictionary<string, Transaction.TransactionOutput>();
            TempUTXOs.Add(GenesisTransaction.Outputs[0].ID, GenesisTransaction.Outputs[0]);
            for (int i = 1; i < Blocks.Count; i++)
            {
                curr = Blocks[i];
                prev = Blocks[i - 1];
                if (!curr.Hash.Equals(curr.GetHashString())) return ValidationTypes.NotValidCurrent;
                if (!prev.Hash.Equals(curr.PreviousHash)) return ValidationTypes.NotValidPervious;
                if (!curr.Hash.Substring(0, difficulty).Equals(hashTarget)) return ValidationTypes.HasntMinedYet;
                Transaction.TransactionOutput tempOutput = null;
                foreach (var item in curr.Transactions)
                {
                    if (!item.IsSignatureVerified) return ValidationTypes.TransactionSignatureIsInvalide;
                    if (item.InputsValue != item.OutputsValue) return ValidationTypes.TransactionInputsValueandOutputsValuearentEqual;
                    foreach (var item2 in item.Inputs)
                    {
                        TempUTXOs.TryGetValue(item2.TransactionOutputId, out tempOutput);
                        if (tempOutput == null) return ValidationTypes.TransactionInputsisMissing;
                        if (item2.UTXO.Value != tempOutput.Value) return ValidationTypes.TransactionInputsValueisInvalide;
                        try
                        {
                            TempUTXOs.Remove(item2.TransactionOutputId);
                        }
                        catch { }
                    }
                    foreach (var item2 in item.Outputs)
                    {
                        TempUTXOs.Add(item2.ID, item2);
                    }
                    if (item.Outputs[0].Reciepient != item.Reciepient) return ValidationTypes.WrongReciepient;
                    try
                    {
                        if (item.Outputs[1].Reciepient != item.Sender) return ValidationTypes.ChangeSenderInvalide;
                    }
                    catch { }
                }
            }
            return ValidationTypes.Valid;
        }
    }
}
