using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BlackBird
{
    public static class Security
    {
        public static string HashPassword(string password)
        {
            SHA256 hasher = SHA256.Create();
            return Encoding.UTF8.GetString(hasher.ComputeHash(Encoding.UTF8.GetBytes(password.ToCharArray())));
        }

        public static byte[] HashPasswordToBytes(string password)
        {
            SHA256 hasher = SHA256.Create();
            return hasher.ComputeHash(Encoding.UTF8.GetBytes(password.ToCharArray()));
        }
    }
}
