using System;
using System.Collections.Generic;
using System.Text;

namespace NoobChain
{
    public class Transaction
    {
        public string ID { get; private set; }
        public string Sender { get; private set; }
        public string Reciepient { get; private set; }
        public double Value { get; private set; }
        public byte[] Signature { get; private set; }
        public List<TransactionInput> Inputs { get; private set; }
        public List<TransactionOutput> Outputs { get; private set; }
        private static UInt64 sequence = 0;
        public Transaction(string from,string to,double value,List<TransactionInput> inputs)
        {
            Sender = from;
            Reciepient = to;
            Value = value;
            Inputs = inputs;
        }
        private string GetHashString()
        {
            sequence++;
            return (Sender + Reciepient + Value.ToString() + sequence.ToString()).ApplyBlacke2();
        }
        public class TransactionInput
        {

        }
        public class TransactionOutput
        {

        }
    }
}
