using System;

namespace VultrManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Execution cli = new Execution();
            Console.Write(cli.GetMenus());
            string line;
            while ((line = Console.ReadLine()) != null)
            {
                if (int.TryParse(line, out int choice))
                {
                    cli.Execute(choice);
                }
                else
                {
                    Console.WriteLine($"Error: {line} is not a valid input!\r\n");
                }
                Console.Write(cli.GetMenus());
            }
        }
    }
}
