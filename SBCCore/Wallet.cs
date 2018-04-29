using System;
using System.Collections.Generic;
using System.Text;
using CEBA.Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System.IO;
using Org.BouncyCastle.Crypto.Parameters;

namespace NoobChain
{
    public class Wallet
    {
        public string PublicKey { get; private set; }
        public string PrivateKey { get; private set; }
        private AsymmetricCipherKeyPair GenerateKeys(int keySize)
        {
            //using ECDSA algorithm for the key generation
            var gen = new ECKeyPairGenerator("ECDSA");

            //Creating Random
            var secureRandom = new SecureRandom();

            //Parameters creation using the random and keysize
            var keyGenParam = new KeyGenerationParameters(secureRandom, keySize);

            //Initializing generation algorithm with the Parameters--This method Init i modified
            gen.Init(keyGenParam);

            //Generation of Key Pair
            return gen.GenerateKeyPair();
        }

        /// <summary>
        /// This method creates 256 bit keys and creates the 
        /// public/private key pair (if they are not yet created only)
        /// </summary>
        private void GeneratePKeys(int intSize=128)
        {
            //Generating p-128 keys 128 specifies strength
            var keyPair = GenerateKeys(intSize);
            TextWriter textWriter = new StringWriter();
            Org.BouncyCastle.OpenSsl.PemWriter pemWriter =
            new Org.BouncyCastle.OpenSsl.PemWriter(textWriter);
            pemWriter.WriteObject(keyPair.Private);
            pemWriter.Writer.Flush();
            string privateKey = textWriter.ToString();
            PrivateKey = privateKey.Replace("BEGIN EC PRIVATE KEY", "")
                .Replace("-", "")
                .Replace("END EC PRIVATE KEY", "")
                .Replace(" ", "")
                .Replace("\r\n", "")
                .Replace("\r", "")
                .Replace("\n", "");
            ECPrivateKeyParameters privateKeyParam = (ECPrivateKeyParameters)keyPair.Private;

            textWriter = new StringWriter();
            pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(textWriter);
            pemWriter.WriteObject(keyPair.Public);
            pemWriter.Writer.Flush();

            ECPublicKeyParameters publicKeyParam = (ECPublicKeyParameters)keyPair.Public;

            string publickey = textWriter.ToString().Replace("BEGIN PUBLIC KEY","")
                .Replace("-","")
                .Replace("END PUBLIC KEY","")
                .Replace(" ","")
                .Replace("\r\n","")
                .Replace("\r","")
                .Replace("\n","");

            PublicKey = publickey;
        }
    }
}
