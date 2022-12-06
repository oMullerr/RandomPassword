namespace RandomPassword
{
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// Defines the <see cref="Program" />.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Min password Length.
        /// </summary>
        private static int minPasswordLength = 10;

        /// <summary>
        /// Max password Length.
        /// </summary>
        private static int maxPasswordLength = 12;

        /// <summary>
        /// lower Cases Char.
        /// </summary>
        private static string lowerCaseChar = "abcdefgijkmnopqrstwxyz";

        /// <summary>
        /// uper Cases Char.
        /// </summary>
        private static string uperCaseChar = "ABCDEFGHJKLMNPQRSTWXYZ";

        /// <summary>
        /// Defines the numericChar.
        /// </summary>
        private static string numericChar = "23456789";

        /// <summary>
        /// Defines the specialChar.
        /// </summary>
        private static string specialChar = "*$-+?_&=!%{}/";

        /// <summary>
        /// The Generate.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public static string Generate()
        {
            return Generate(minPasswordLength, maxPasswordLength);
        }

        /// <summary>
        /// The Generate.
        /// </summary>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string Generate(int length)
        {
            return Generate(length, length);
        }

        /// <summary>
        /// The Generate.
        /// </summary>
        /// <param name="minLength">The minLength<see cref="int"/>.</param>
        /// <param name="maxLength">The maxLength<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string Generate(int minLength, int maxLength)
        {
            // Make sure that input parameters are valid.
            if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
                return null;

            char[][] charGroups = new char[][]
            {
            lowerCaseChar.ToCharArray(),
            uperCaseChar.ToCharArray(),
            numericChar.ToCharArray(),
            specialChar.ToCharArray()
            };
            Console.WriteLine("GENERATE:");
            Console.WriteLine(" ");

            int[] charsLeftInGroup = new int[charGroups.Length];

            for (int i = 0; i < charsLeftInGroup.Length; i++)
            {
                charsLeftInGroup[i] = charGroups[i].Length;
            }

            int[] leftGroupsOrder = new int[charGroups.Length];

            for (int i = 0; i < leftGroupsOrder.Length; i++)
            {
                leftGroupsOrder[i] = i;
            }

            byte[] randomBytes = new byte[4];

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            int seed = BitConverter.ToInt32(randomBytes, 0);

            Random random = new Random(seed);

            char[] password = null;

            if (minLength < maxLength)
            {
                password = new char[random.Next(minLength, maxLength + 1)];
            }else
            {
                password = new char[minLength];
            }

            int nextCharIdx;
            int nextGroupIdx;
            int nextLeftGroupsOrderIdx;
            int lastCharIdx;
            int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

            for (int i = 0; i < password.Length; i++)
            {
                if (lastLeftGroupsOrderIdx == 0)
                {
                    nextLeftGroupsOrderIdx = 0;
                }else
                {
                    nextLeftGroupsOrderIdx = random.Next(0, lastLeftGroupsOrderIdx);
                }

                nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

                lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

                if (lastCharIdx == 0)
                {
                    nextCharIdx = 0;
                }else
                {
                    nextCharIdx = random.Next(0, lastCharIdx + 1);
                }

                password[i] = charGroups[nextGroupIdx][nextCharIdx];

                if (lastCharIdx == 0)
                {
                    charsLeftInGroup[nextGroupIdx] = charGroups[nextGroupIdx].Length;
                }else
                {
                    if (lastCharIdx != nextCharIdx)
                    {
                        char temp = charGroups[nextGroupIdx][lastCharIdx];
                        charGroups[nextGroupIdx][lastCharIdx] = charGroups[nextGroupIdx][nextCharIdx];
                        charGroups[nextGroupIdx][nextCharIdx] = temp;
                    }
                    charsLeftInGroup[nextGroupIdx]--;
                }

                if (lastLeftGroupsOrderIdx == 0)
                {
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                }else
                {
                    if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                    {
                        int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                        leftGroupsOrder[lastLeftGroupsOrderIdx] = leftGroupsOrder[nextLeftGroupsOrderIdx];
                        leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                    }
                    lastLeftGroupsOrderIdx--;
                }
            }
            return new string(password);
        }
    }

    /// <summary>
    /// Defines the <see cref="RandomPasswordTest" />.
    /// </summary>
    public class RandomPasswordTest
    {
        /// <summary>
        /// The Main.
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/>.</param>
        [STAThread]
        internal static void Main(string[] args)
        {
            // Print 100 randomly generated passwords (8-to-10 char long).
            for (int i = 0; i < 10; i++)
                ////Console.WriteLine(RandomPassword.Generate(8, 10));
                Console.WriteLine(RandomPassword.Program.Generate(8, 10));
        }
    }
}
