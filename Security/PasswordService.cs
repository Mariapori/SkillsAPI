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
    }
}
