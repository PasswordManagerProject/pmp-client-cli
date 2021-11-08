using System;
using TextCopy;

namespace pmp_client_cli
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Data data = new Data();

            data.Init(args);
            String pass = data.GeneratePass();
            
            ClipboardService.SetText(pass);

            string encPass = Crypto.Encrypt(pass, "testkey");
            Console.WriteLine(encPass);
            Console.WriteLine(Crypto.Decrypt(encPass, "testkey"));
            
        }
    }
}