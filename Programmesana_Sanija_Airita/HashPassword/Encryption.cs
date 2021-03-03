using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace Programmesana_Sanija_Airita.HashPassword
{
    public class Encryption
    {
        private static byte[] Hash(byte[] originalData)
        {
            var myAlg = SHA512.Create();
            byte[] digest = myAlg.ComputeHash(originalData); 
            return digest;
        }
        public static string HashPassword(string originalPassword)
        {
            byte[] password = Encoding.UTF32.GetBytes(originalPassword);
            byte[] digest = Hash(password);

            return Convert.ToBase64String(digest);
        }
    }
}