using Orange.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlackMesa
{
    class Facility591
    {
        private static double _fractional { get; set; }

        static void Main()
        {


            int numberOfPirates = 3; // 8 appears to be the max
            int upperBound = 500; // 2147483647 Int32 maximum
            GetViableNumbers(numberOfPirates, upperBound);

            Console.WriteLine("End.");
            Console.ReadLine();
        }

        /// <summary>
        /// A loop to identify numbers that match the pirate treasure problem.
        /// </summary>
        /// <param name="maxDepth">The total number of pirates. +1 will be added to this to account for the final round of treasure divvying.</param>
        /// <param name="upperBound">The upper bound of integers to check.</param>
        public static void GetViableNumbers(int maxDepth, int upperBound)
        {
            _fractional = Math.Round((maxDepth) / Convert.ToDouble(((maxDepth) * (maxDepth))), 2, MidpointRounding.AwayFromZero);
            maxDepth++;
            for (int n = 79; n < upperBound; ++n)
            {
                //if (n == 800000) // 4 pirates; 5117
                //{
                //    bool derping = false;
                //}

                if ((n - maxDepth) % (maxDepth - 1) != 0) continue; // filter out false positives. No need to go further.      
                DivisionCheck(n, n, 1, maxDepth);
            }
        }

        /// <summary>
        /// The provided number, originalCount, must meet two criteria to match the original problem. First, when divided by the total 
        /// number of pirates (maxDepth - 1), the number must have a remainder equal to its fractional representation; a number split 
        /// amongst three pirates must yield a remainder of .33. Second, this trend of fractional matching must continue through the end of 
        /// "maxDepth" to yield the final share for each pirate.
        /// </summary>
        /// <param name="originalCount">The number being checked for suitability.</param>
        /// <param name="treasureCount">The current treasure total. This value changes as recursion progresses (as pirates divide the treasure).</param>
        /// <param name="recursiveDepth">The current depth of the recursion.</param>
        /// <param name="maxDepth">The maximum depth of the recursion. This can be thought of as (TotalPirates + 1).</param>
        public static void DivisionCheck(int originalCount, double treasureCount, int recursiveDepth, int maxDepth)
        {
            if (recursiveDepth == maxDepth)
            {
                double finalShare = Math.Abs((treasureCount - 1) / (maxDepth - 1));
                if (finalShare % 1 == 0.0)
                {
                    //if (originalCount == 49900)
                    //{
                    //    bool derping = false;
                    //}
                    Console.WriteLine("Found: " + originalCount + "; Single share: " + finalShare + Environment.NewLine);
                }
                return;
            }

            double equalShare = Math.Round(treasureCount / (maxDepth - 1), 2, MidpointRounding.AwayFromZero); // (maxDept - 1) = number of pirates
            double shareRemainder = Math.Round((equalShare - Math.Truncate(equalShare)), 2, MidpointRounding.AwayFromZero);

            //if(comparison == multiplier)
            //{
            //     bool derping = false;
            //}

            if (shareRemainder == _fractional)
            {
                equalShare = Math.Truncate(equalShare) * (maxDepth - 2); // drop the extra one and multiply by remaining pirates (ditch the pilfered pile)
                DivisionCheck(originalCount, equalShare, recursiveDepth + 1, maxDepth);
            }
        }



        public static void PasswordTesting()
        {
            //string randomPassword = SecurityOps.CreateRandomPassword(64);
            //int randomSalt = SecurityOps.CreateRandomSalt();

            //string password = "!Qronos2015!";
            //int salt = 12324823;

            //SecurityOps security = new SecurityOps(password, salt);
            //string saltedHash = security.ComputeSaltedHash();

            //string password2 = "!Qronos2014!";
            //int salt2 = 12000823;

            //SecurityOps security2 = new SecurityOps(password2, salt2);
            //string saltedHash2 = security2.ComputeSaltedHash();

            //Console.WriteLine(saltedHash + Environment.NewLine + saltedHash2);

            //string hash = PasswordHash.CreateHash(password2);
            //bool isValid = PasswordHash.ValidatePassword(password, hash);

            //Console.WriteLine(hash + Environment.NewLine + isValid);
        }

        public static void TaskTesting()
        {
            Task task1 = new Task(() => TaskCounter("TaskOne", 10));
            task1.Start();
            Task task2 = new Task(() => TaskCounter("TaskTwo", 20));
            task2.Start();
            Task task3 = new Task(() => TaskCounter("TaskThree", 30));
            task3.Start();
        }

        private static void TaskCounter(string name, int upperLimit)
        {
            for (int n = 0; n < upperLimit; n++)
            {
                Thread.Sleep(upperLimit * 50); // this helps to show that they're running concurrently
                Console.WriteLine(name + "; " + n);
            }
        }
    }
}
