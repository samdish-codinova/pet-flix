using System.Security.Cryptography;
using System.Text;

namespace PasswordHashing
{
  class PasswordHasher
  {
    public static void CreatePasswordHash(string password, out byte[] passwordSalt, out byte[] passwordHash)
    {

      using (var hmac = new HMACSHA512())
      {
        passwordSalt = hmac.Key;
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        passwordHash = hmac.ComputeHash(passwordBytes);
      }
    }

    public static bool VerifyPasswordHash(string password, byte[] passwordSalt, byte[] passwordHash)
    {
      using (var hmac = new HMACSHA512(passwordSalt))
      {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] computedPasswordHash = hmac.ComputeHash(passwordBytes);
        bool arePasswordsSame = computedPasswordHash.SequenceEqual(passwordHash);

        return arePasswordsSame;
      }
    }
  }
}
