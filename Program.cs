using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{   
    class Program
    {
        private static string coso = null;
        static void Main(string[] args)
        {
            Console.WriteLine("Ingresar código largo");
            coso = Console.ReadLine();
            var asd = Encrypt(coso);
            var hash = FnvHash.GetHash(asd, 30).ToHexString();
            //BigInteger.Parse(str, System.Globalization.NumberStyles.HexNumber).ToByteArray().Reverse().ToArray();
            //var byteArray = Encoding.ASCII.GetBytes(hash);
            //string s = Encoding.ASCII.GetString(byteArray);
            //FALTA RECUPERAR EL CÓDIGO LARGO DEL CORTO
            Console.WriteLine("Código corto: " + hash);
            Console.WriteLine("Y desencriptado: " + Decrypt(hash));
        }

        private static TripleDESCryptoServiceProvider tdDes = new
        TripleDESCryptoServiceProvider();
        public Program(string strKey)
        {
            tdDes.Key = Truncate(strKey, tdDes.KeySize / 8);
            tdDes.IV = Truncate("", tdDes.BlockSize / 8);
        }
        public static string Encrypt(string strInput)
        {
            byte[] btInputBytes = System.Text.Encoding.Unicode
               .GetBytes(strInput);
            System.IO.MemoryStream msInput = new System.IO
               .MemoryStream();
            CryptoStream csEncrypt = new CryptoStream(msInput,
               tdDes.CreateEncryptor(), CryptoStreamMode.Write);

            csEncrypt.Write(btInputBytes, 0, btInputBytes.Length);
            csEncrypt.FlushFinalBlock();

            return Convert.ToBase64String(msInput.ToArray());
        }
        public static string Decrypt(string strOutput)
        {
            byte[] btOutputBytes = Convert
               .FromBase64String(strOutput);
            System.IO.MemoryStream msOutput = new
               System.IO.MemoryStream();
            CryptoStream csDecrypt = new CryptoStream(msOutput,
               tdDes.CreateDecryptor(), CryptoStreamMode.Write);

            csDecrypt.Write(btOutputBytes, 0, btOutputBytes.Length);
            csDecrypt.FlushFinalBlock();

            return System.Text.Encoding.Unicode
               .GetString(msOutput.ToArray());
        }
        private byte[] Truncate(string strKey, int intLength)
        {
            SHA1CryptoServiceProvider shaCrypto = new
               SHA1CryptoServiceProvider();
            byte[] btKeyBytes = Encoding.Unicode.GetBytes(strKey);
            byte[] btHash = shaCrypto.ComputeHash(btKeyBytes);
            var oldBtHash = btHash;
            btHash = new byte[intLength - 1 + 1];
            if (oldBtHash != null)
                Array.Copy(oldBtHash, btHash, Math.Min(intLength - 1 +
                   1, oldBtHash.Length));
            return btHash;
        }
    }
}
