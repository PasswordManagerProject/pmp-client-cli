using System;

namespace pmp_client_cli
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (!Utils.MasterFileExists())
            {
                Console.Write("Master password not configured. Please type in a new master password: ");
                string input = Console.ReadLine();
                Console.Write("Please type full server address: ");
                string path = Console.ReadLine();
                Utils.InitMasterFile(input, path);
                return;
            }
            else
            {
                Console.Write("Please enter your master password: ");
                string input = Console.ReadLine();
                
                if (!Utils.ReadMasterFile(input))
                {
                    Console.WriteLine("Error initializing master file. Please delete the file " +
                                      "and generate a new one.");
                    return;
                }
            }
        }
    }
}