using System;

namespace RenEvo.Mud
{
    public class Program
    {

        public static void Main(string[] args)
        {
            using (var bootstrap = new Bootstrap())
            {
                Console.ReadLine();
                bootstrap.Shutdown(TimeSpan.MinValue);
            }
        }
    }
}
