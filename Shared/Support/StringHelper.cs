using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Support
{
    public static class StringHelper
    {
        public static int CreateMD5Int(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                var md5 = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToInt32(md5, 0);
            }
        }
        public static string CalculateMD5(string input)
        {
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
