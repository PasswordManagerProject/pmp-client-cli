using System;
using System.IO;
using Newtonsoft.Json;

namespace pmp_client_cli
{
    public static class Utils
    {
        private const string MasterPassFile = "./master.json";
        private const string MasterKey = "MASTERKEY"; //TODO: Move out obviously

        private class JsonData
        {
            public string Key;
        }

        public static bool MasterFileExists()
        {
            try
            {
                return File.Exists(MasterPassFile);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to run file check: " + e.Message);
                throw;
            }
        }

        public static bool InitMasterFile(string pass)
        {
            try
            {
                JsonData data = new JsonData();
                data.Key = Crypto.Encrypt(pass, MasterKey);

                string json = JsonConvert.SerializeObject(data);
                File.WriteAllText(MasterPassFile, json);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to create master key file: " + e.Message);
                return false;
            }
        }

        public static bool ReadMasterFile(string pass)
        {
            try
            {
                JsonData data = new JsonData();
                data = JsonConvert.DeserializeObject<JsonData>(File.ReadAllText(MasterPassFile));
                string filePass = data.Key;

                if (Crypto.Decrypt(filePass, MasterKey) == pass)
                    return true;

                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to read from master key file: " + e.Message);
                return false;
            }
        }
    }
}