using System.Security.Cryptography;
using System.Text;

namespace LoginSystem.Security
{
    public class SaltPassword
    {
        public string CreatingSalt()
        {
            byte[] salting = RandomNumberGenerator.GetBytes(128 / 8);
            string saltToString = Convert.ToBase64String(salting);

            return saltToString;
        }

        public async Task<string> HashingPassword(string password, string salt)
        {
            return await Task.Run(() =>
            {
                string spicePassword = password + salt;

                using (SHA256 hash = SHA256.Create())
                {
                    var passwordBytes = Encoding.Default.GetBytes(spicePassword);
                    var hashedPassword = hash.ComputeHash(passwordBytes);

                    return Convert.ToHexString(hashedPassword);
                }
            });
        }
    }
}