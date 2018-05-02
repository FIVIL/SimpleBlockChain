using System;
using System.Collections.Generic;
using System.Text;

namespace NoobChain
{
    public class TransactionModel
    {
        public string ID { get; set; }
        public string Sender { get; private set; }
        public string Reciepient { get; private set; }
        public double Value { get; private set; }
        public string Signature { get; private set; }
        public List<TransactionInputModel> Inputs { get; private set; }
        public List<TransactionOutputModel> Outputs { get; private set; }
        public TransactionModel(Transaction t)
        {
            ID = t.ID;
            Sender = t.Sender.ToStringKey();
            Reciepient = t.Reciepient.ToStringKey();
            Value = t.Value;
            Signature = Convert.ToBase64String(t.Signature);
            Inputs = new List<TransactionInputModel>();
            try
            {
                foreach (var item in t.Inputs)
                {
                    Inputs.Add(new TransactionInputModel(item));
                }
            }
            catch { }
            Outputs = new List<TransactionOutputModel>();
            try
            {
                foreach (var item in t.Outputs)
                {
                    Outputs.Add(new TransactionOutputModel(item));
                }
            }
            catch { }
        }
        public class TransactionInputModel
        {
            public string TransactionOutputId { get; private set; }
            public TransactionOutputModel UTXO { get; set; }
            public TransactionInputModel(Transaction.TransactionInput inp)
            {
                TransactionOutputId = inp.TransactionOutputId;
                UTXO = new TransactionOutputModel(inp.UTXO);
            }
        }
        public class TransactionOutputModel
        {
            public string ID { get; private set; }
            public string Reciepient { get; private set; }
            public double Value { get; private set; }
            public string ParentTransactionId { get; private set; }
            public TransactionOutputModel(Transaction.TransactionOutput ot)
            {
                ID = ot.ID;
                Reciepient = ot.Reciepient.ToStringKey();
                Value = ot.Value;
                ParentTransactionId = ot.ParentTransactionId;
            }
        }
    }
}
