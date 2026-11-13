using System;
using System.Security.Cryptography;
using System.Text;

namespace PasswordHashing
{
    public class PasswordHasher
    {
        public string GetPasswordHash(string password, string salt)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] data = Encoding.UTF8.GetBytes(salt + password);
                byte[] result = sha.ComputeHash(data);

                return BitConverter.ToString(result).Replace("-", "").ToUpper();
            }
        }
    }
}