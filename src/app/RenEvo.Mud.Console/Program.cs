using System;

namespace RenEvo.Mud
{
    public class Program
    {

        public static void Main(string[] args)
        {
            using (new Bootstrap())
            {
                Console.ReadLine();
            }
        }
    }
}
