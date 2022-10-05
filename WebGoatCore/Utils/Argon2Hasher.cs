using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using System;

namespace WebGoatCore.Utils
{
    /// <summary>
    /// Argon 2 Password Hasher. This class implements the IPasswordHasher interface which will then be used as part of ASP.NET Core Identity Framework
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class Argon2Hasher<TUser>: IPasswordHasher<TUser> where TUser : class
    {
        // Salt Size
        private const int saltSize = 16;

        // Password Hash Size
        private const int hashSize = 16;


        /// <summary>
        /// Returns a hashed representation of the supplied <paramref name="password"/> for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose password is to be hashed.</param>
        /// <param name="password">The password to hash.</param>
        /// <returns>A hashed representation of the supplied <paramref name="password"/> for the specified <paramref name="user"/>.</returns>
        public string HashPassword(TUser user, string password)
        {
            return Convert.ToBase64String(HashPassword(password));
        }

        /// <summary>
        /// Returns a <see cref="PasswordVerificationResult"/> indicating the result of a password hash comparison.
        /// </summary>
        /// <param name="user">The user whose password should be verified.</param>
        /// <param name="hashedPassword">The hash value for a user's stored password.</param>
        /// <param name="providedPassword">The password supplied for comparison.</param>
        /// <returns>A <see cref="PasswordVerificationResult"/> indicating the result of a password hash comparison.</returns>
        /// <remarks>Implementations of this method should be time consistent.</remarks>
        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            if (VerifyHashedPassword(hashedPassword, providedPassword).Equals(true))
                return PasswordVerificationResult.Success;
            else return PasswordVerificationResult.Failed;
        }

        private static byte[] HashPassword(string password)
        {
            /*
             * TODO:
             * 
             * 1. Generate a secure random salt
             * 2. Convert the string password to bytes array
             * 3. Using the password and salt, generate an Argon2 hash
             * 4. Create a new bytes array buffer and copy the salt and password hash in it
             * 
             * Hint:
             *  For copying bytes in 4th step, you can use: Buffer.BlockCopy(Array src, int srcOffset, Array dst, int dstOffset, int count);
             */
            
            // This is a sample code. Update it as you see fit.

            // Final array that contains the salt + password hash bytes
            byte[] outputBytes = new byte[1];

            // Return the final bytes
            return outputBytes;
        }

        private static bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            try
            {
                /*
                 * TODO:
                 * 
                 * 1. Convert the hashed password (Base64 string) to bytes array
                 * 2. Extract the salt from the hashed password bytes, and store it in a variable "salt"
                 * 3. Extract the actual password hash from hashed password bytes, and store it in a variable called "expectedSubkey"
                 * 4. Using the Argon 2 hashing algorithm, hash the providedPassword string, and store it in a variable called "actualSubkey"
                 * 5. Compare the "expectedSubkey" and "actualSubkey" byte arrays securely i.e., in a fixed time, to prevent side-channel attack or timing attacks
                 * 
                 * Hint:
                 * 
                 * For converting a Base64 string to bytes array, you can use: Convert.FromBase64String(string s)
                 * For secure comparison of bytes, you can checkout https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.cryptographicoperations?view=netcore-3.1
                 */

                // This is a sample code. Update it as you see fit.

                // Get the Password Length and read from the offset
                byte[] expectedSubkey = new byte[1];

                // Convert the password to bytes and then perform hashing
                byte[] actualSubkey = new byte[1];

                // Perform the comparison
                return ByteArraysEqual(actualSubkey, expectedSubkey);
            }
            catch
            {
                return false;
            }
        }

        /*
         * GenerateArgon2Hash method takes in a password and a salt in bytes
         * and returns the generated hash with the set computation values.
         */
        private static byte[] GenerateArgon2Hash(byte[] password, byte[] salt)
        {
            // Four Cores
            int degreeOfParallelism = 8;

            // TODO: This should have a better count. Maybe > 3?
            int iterations = 0;

            // TODO: Must use a bigger memory size
            // Currently set to 1 MB
            int memorySize = 1024 * 1;

            var argon2 = new Argon2id(password)
            {
                Salt = salt,
                DegreeOfParallelism = degreeOfParallelism,
                Iterations = iterations,
                MemorySize = memorySize
            };

            return argon2.GetBytes(hashSize);
        }

        /*
         * Generates a random salt that will be used as part of hashing the passwords
         * The random generator used is secure and will not generate predictable numbers
         */
        private static byte[] CreateSalt()
        {
            var buffer = new byte[saltSize];
            
            // TODO: Generate random bytes which fill the `buffer` array

            return buffer;
        }

        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }
            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }
    }
}
