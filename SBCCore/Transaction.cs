using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NoobChain
{
    public class Transaction
    {
        public string ID { get; private set; }
        public ECPublicKeyParameters Sender { get; private set; }
        public ECPublicKeyParameters Reciepient { get; private set; }
        public double Value { get; private set; }
        public byte[] Signature { get; private set; }
        public List<TransactionInput> Inputs { get; private set; }
        public List<TransactionOutput> Outputs { get; private set; }
        private static UInt64 sequence = 0;
        public Transaction(ECPublicKeyParameters from, ECPublicKeyParameters to, double value, List<TransactionInput> inputs)
        {
            Sender = from;
            Reciepient = to;
            Value = value;
            Inputs = inputs;
        }
        public Transaction(ECPublicKeyParameters from, ECPublicKeyParameters to, double value, List<TransactionInput> inputs,string id)
            :this(from,to,value,inputs)
        {
            ID = id;
        }
        private string GetHashString()
        {
            sequence++;
            return (Sender.ToStringKey() + Reciepient.ToStringKey() + Value.ToString() + sequence.ToString()).ApplyBlacke2();
        }
        public void GenerateSignature(ECPrivateKeyParameters privateKey)
        {
            string data = Sender.ToStringKey() + Reciepient.ToStringKey() + Value.ToString();
            Signature = data.GenerateSignture(privateKey);
        }
        public bool IsSignatureVerified
        {
            get
            {
                string data = Sender.ToStringKey() + Reciepient.ToStringKey() + Value.ToString();
                return data.VerifySignature(Sender, Signature);
            }
        }
        public double InputsValue
        {
            get
            {
                double retValue = 0;
                foreach (var item in Inputs)
                {
                    if (item.UTXO != null) retValue += item.UTXO.Value;
                }
                return retValue;
            }
        }
        public double OutputsValue
        {
            get
            {
                double retValue = 0;
                foreach (var item in Outputs)
                {
                    retValue += item.Value;
                }
                return retValue;
            }
        }
        public bool Process()
        {
            if (!IsSignatureVerified) return false;
            foreach (var item in Inputs)
            {
                item.UTXO = NoobChaiN.UTXOs[item.TransactionOutputId];
            }
            var LeftOver = InputsValue - Value;
            ID = GetHashString();
            Outputs.Add(new TransactionOutput(Reciepient, Value, ID));
            Outputs.Add(new TransactionOutput(Sender, LeftOver, ID));
            foreach (var item in Outputs)
            {
                NoobChaiN.UTXOs.Add(item.ID, item);
            }
            foreach (var item in Inputs)
            {
                if (item.UTXO != null)
                    NoobChaiN.UTXOs.Remove(item.UTXO.ID);
            }
            return true;
        }
        public class TransactionInput
        {
            public string TransactionOutputId { get; private set; }
            public TransactionOutput UTXO { get; set; }
            public TransactionInput(string TOID)
            {
                TransactionOutputId = TOID;
            }
        }
        public class TransactionOutput
        {
            public string ID { get; private set; }
            public ECPublicKeyParameters Reciepient { get; private set; }
            public double Value { get; private set; }
            public string ParentTransactionId { get; private set; }
            public TransactionOutput(ECPublicKeyParameters reciepient, double value, string parentTransactionId)
            {
                Reciepient = reciepient;
                Value = value;
                ParentTransactionId = parentTransactionId;
                ID = (Reciepient.ToStringKey() + Value.ToString() + parentTransactionId).ApplyBlacke2();
            }
            public bool IsMine(ECPublicKeyParameters Key)
            {
                return (Key == Reciepient);
            }
        }
    }
}
