using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Blake2Sharp;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

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
    }
}
