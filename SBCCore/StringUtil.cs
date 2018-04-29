using System;
using System.Collections.Generic;
using System.Text;
using Blake2Sharp;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace NoobChain
{
    public static class StringUtil
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
    }
}
