using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Blake2Sharp;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Linq;

namespace NoobChain
{
    static class StringUtil
    {
        public static string ApplyBlacke2(this string data)
        {
            var p = Blake2B.ComputeHash(System.Text.Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(p);
        }
        public static bool VerifySignature(this string plainText, ECPublicKeyParameters Publickey, byte[] signature)
        {
            var encoder = new ASCIIEncoding();
            var inputData = encoder.GetBytes(plainText);
            var signer = SignerUtilities.GetSigner("ECDSA");
            signer.Init(false, Publickey);
            signer.BlockUpdate(inputData, 0, inputData.Length);
            return signer.VerifySignature(signature);
        }
        public static byte[] GenerateSignture(this string plainText, ECPrivateKeyParameters PrivateKey)
        {
            var bytetext = System.Text.Encoding.UTF8.GetBytes(plainText);
            var dsa = SignerUtilities.GetSigner("ECDSA");
            dsa.Init(true, PrivateKey);
            dsa.BlockUpdate(bytetext, 0, bytetext.Length);
            return dsa.GenerateSignature();
        }
        public static string ToStringKey(this AsymmetricKeyParameter Key)
        {
            TextWriter textWriter = new StringWriter();
            var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(textWriter);
            pemWriter.WriteObject(Key);
            pemWriter.Writer.Flush();
            return textWriter.ToString().Replace("BEGIN PUBLIC KEY", "")
                .Replace("BEGIN EC PRIVATE KEY", "")
                .Replace("END EC PRIVATE KEY", "")
                .Replace("-", "")
                .Replace("END PUBLIC KEY", "")
                .Replace(" ", "")
                .Replace("\r\n", "")
                .Replace("\r", "")
                .Replace("\n", "");
        }
        public static string GenerateMerkleRoot(this List<Transaction> transactions)
        {
            var TIDS = transactions.Select(x => x.ID).ToArray();
            while (true)
            {
                string[] TopLvl = new string[(TIDS.Length / 2) + 1];
                int j = 0;
                for (int i = 0; i < TIDS.Length; i+=2)
                {
                    if (i + 1 < TIDS.Length)
                    {
                        TopLvl[j++] = ApplyBlacke2(TIDS[i] + TIDS[i + 1]);
                    }
                    else
                    {
                        TopLvl[j++] = ApplyBlacke2(TIDS[i]);
                    }
                }
                if (TopLvl.Length == 1) return TopLvl[0];
                if (TopLvl.Length == 2)
                    if (TIDS.Length == 2) return TopLvl[0];
                TIDS = TopLvl;
            }
            
        }
    }
}
