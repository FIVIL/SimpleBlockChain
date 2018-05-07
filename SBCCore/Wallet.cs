using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace NoobChain
{
    public class Wallet
    {
        public ECPublicKeyParameters PublicKey { get; private set; }
        public ECPrivateKeyParameters PrivateKey { get; private set; }
        public Dictionary<string, Transaction.TransactionOutput> UTXOs { get; private set; }
        public Wallet()
        {
            GenerateKeys();
            UTXOs = new Dictionary<string, Transaction.TransactionOutput>();
        }
        private void GenerateKeys(int keySize = 256)
        {
            var gen = new ECKeyPairGenerator();
            var secureRandom = new SecureRandom();
            var keyGenParam = new KeyGenerationParameters(secureRandom, keySize);
            gen.Init(keyGenParam);
            var KeyPair = gen.GenerateKeyPair();
            PublicKey = (ECPublicKeyParameters)KeyPair.Public;
            PrivateKey = (ECPrivateKeyParameters)KeyPair.Private;
        }
        public double Balance
        {
            get
            {
                double retValue = 0;
                foreach (var item in NoobChaiN.UTXOs.Values)
                {
                    if (item.IsMine(PublicKey))
                    {
                        if (!UTXOs.ContainsKey(item.ID))
                        {
                            UTXOs.Add(item.ID, item);
                            retValue += item.Value;
                        }
                    }
                }
                return retValue;
            }
        }
        public Transaction SendFunds(ECPublicKeyParameters recipient, double value)
        {
            if (Balance < value)
            {
                Console.WriteLine("Impossible!!");
                return null;
            }
            var inputs = new List<Transaction.TransactionInput>();

            double total = 0;
            foreach (var item in UTXOs.Values)
            {
                total += item.Value;
                inputs.Add(new Transaction.TransactionInput(item.ID));
                if (total > value) break;
            }

            var NewTransaction = new Transaction(PublicKey, recipient, value, inputs);
            NewTransaction.GenerateSignature(PrivateKey);

            foreach (var item in inputs)
            {
                UTXOs.Remove(item.TransactionOutputId);
            }
            return NewTransaction;
        }
    }
}
