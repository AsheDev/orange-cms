using System;
using System.Data;
using System.Text;
using Orange.Core.Values;
using System.Data.SqlClient;
using Orange.Core.Repositories;
using System.Security.Cryptography;

namespace Orange.Core.Entities
{
    /// <summary>
    /// Salted password hashing with PBKDF2-SHA1.
    /// </summary>
    public class Authentication
    {
        // I may want to store these in the database, so they can easily change, and pass them to the constructor
        private const int SaltByteSize = 24; // the resulting size of the salt
        private const int HashByteSize = 24; // the resulting size of the hash
        private const int PBKDF2Iterations = 50000; // Based on it running on the server, this should take about half a second.

        private const int IterationIndex = 0;
        private const int SaltIndex = 1;
        private const int PBKDF2Index = 2;

        private const int RecoveryUrlLength = 32;
        private const char Delimiter = ':';

        // This should take a settings object from the database or something

        //public Authentication(Repository repo)
        //{
        //    _repo = repo;
        //}

        /// <summary>
        /// Leveraging the RNGCryptoServiceProvider, this generates a unique salt.
        /// </summary>
        /// <returns></returns>
        public byte[] GenerateSalt()
        {
            RNGCryptoServiceProvider cryptoBitches = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SaltByteSize];
            cryptoBitches.GetBytes(salt);
            return salt;
        }

        /// <summary>
        /// Taking a plaintext password and a pre-computed salt byte array this method 
        /// will return a salted hash.
        /// </summary>
        /// <param name="password">A plaintext password.</param>
        /// <param name="salt">A byte array.</param>
        /// <returns></returns>
        public string CreateSaltedHash(string password, byte[] salt)
        {
            byte[] hash = CalculatePBKDF2(password, salt, PBKDF2Iterations, HashByteSize);
            return PBKDF2Iterations + Convert.ToString(Delimiter) + Convert.ToBase64String(salt) + Convert.ToString(Delimiter) + Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Using only a password, this will generate a unique salt, and return a salted and 
        /// hashed password to be stored alongside the user's record.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public HashedPassword SaltAndHashPassword(string password)
        {
            byte[] salt = GenerateSalt();
            byte[] hash = CalculatePBKDF2(password, salt, PBKDF2Iterations, HashByteSize);
            return new HashedPassword(Convert.ToBase64String(salt), Convert.ToBase64String(hash), PBKDF2Iterations);
        }

        /// <summary>
        /// Accepts a plaintext password and a salted and hashed password and compares 
        /// their values to determine if they match. Implements key stretching to  
        /// artificially slow down the validation process.
        /// </summary>
        /// <param name="password">A plaintext password.</param>
        /// <param name="correctHash">The corresponding salted and hashed password.</param>
        /// <returns></returns>
        public bool Validate(string password, string correctHash)
        {
            // Extract the parameters from the hash
            //char[] delimiter = { Delimiter };
            string[] split = correctHash.Split(Delimiter);
            int iterations = Int32.Parse(split[IterationIndex]);
            byte[] salt = Convert.FromBase64String(split[SaltIndex]);
            byte[] hash = Convert.FromBase64String(split[PBKDF2Index]);

            byte[] testHash = CalculatePBKDF2(password, salt, iterations, hash.Length);
            return KeyStretch(hash, testHash);
        }

        /// <summary>
        /// This will generate a unique, web-safe, string that may be used for a url's query 
        /// parameter when a user needs to reset their password.
        /// </summary>
        /// <returns></returns>
        public string GenerateRecoveryKey()
        {
            String _allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()";
            Byte[] randomBytes = new Byte[RecoveryUrlLength];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);
            char[] chars = new char[RecoveryUrlLength];
            int allowedCharCount = _allowedChars.Length;

            for (int i = 0; i < RecoveryUrlLength; i++)
            {
                chars[i] = _allowedChars[(int)randomBytes[i] % allowedCharCount];
            }

            return new string(chars);
        }

        /// <summary>
        /// A length-constant comparison of two byte arrays. This protects the system
        /// from having passwords extracted online and then attacked offline.
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        private bool KeyStretch(byte[] one, byte[] two)
        {
            uint difference = (uint)one.Length ^ (uint)two.Length; // calculate bitwise exclusive-OR
            for (int i = 0; i < one.Length && i < two.Length; i++)
            {
                difference |= (uint)(one[i] ^ two[i]); // bitwise-inclusive-OR on each bit
            }
            return (difference == 0);
        }

        /// <summary>
        /// Calculates the PBKDF2-SHA1 hash of a password.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="iterations"></param>
        /// <param name="outputBytes"></param>
        /// <returns></returns>
        private byte[] CalculatePBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}
