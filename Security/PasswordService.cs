using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace SkillsAPI.Security
{
    public class PasswordService
    {
        public static byte[] CreateHash(byte[] password, byte[] salt)
        {
            using var argon2 = new Argon2id(password);
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 8;
            argon2.Iterations = 4;
            argon2.MemorySize = 1024 * 128;

            return argon2.GetBytes(32);
        }

        public static bool VerifyHash(byte[] password, byte[] salt, byte[] hash) =>
            CreateHash(password, salt).SequenceEqual(hash);

        public static byte[] GenerateSalt()
        {
            var buffer = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(buffer);
            return buffer;
        }

        public static string CreatePassword(string password)
        {
            var salt = GenerateSalt();
            var hash = CreateHash(Encoding.UTF8.GetBytes(password),salt);
            var hashString = $"{Encoding.UTF8.GetString(salt)}:{Encoding.UTF8.GetString(hash)}";
            return hashString;
        }

        public static bool VerifyPassword(string password, string hashString)
        {
            var pwd = hashString.Split(':');
            var salt = pwd[0];
            var hash = pwd[1];

            return VerifyHash(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(salt), Encoding.UTF8.GetBytes(hash));
        }
    }
}
