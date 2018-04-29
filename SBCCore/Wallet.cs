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
        public Wallet()
        {
            GenerateKeys();
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
    }
}
