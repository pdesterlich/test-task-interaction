using System;

namespace worker
{
    class Program
    {
        static void Main(string[] args)
        {
            var launcher = new Launcher();
            launcher.Launch();

            Console.WriteLine("Press [enter] to exit");
            Console.ReadLine();

            launcher.Stop();
        }
    }
}
