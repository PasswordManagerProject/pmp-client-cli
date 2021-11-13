using System;
using Newtonsoft.Json;

namespace pmp_client_cli
{
    public class Record
    {
        private string Type { get; set; }
        private string ID { get; set; }
        private string UserName { get; set; }
        private string Password { get; set; }
        private DateTime DtCreated { get; set; }
        private DateTime DtUpdated { get; set; }

        private class JsonData
        {
            public string Type;
            public string ID;
            public string UserName;
            public string Password;
            public DateTime DtCreated;
            public DateTime DtUpdated;
        }

        public Record(string type, string ID, string userName, string password, DateTime dtCreated, DateTime dtUpdated)
        {
            this.Type = type;
            this.ID = ID;
            this.UserName = userName;
            this.Password = password;
            this.DtCreated = dtCreated;
            this.DtUpdated = dtUpdated;
        }

        public string ToJson()
        {
            try
            {
                JsonData data = new JsonData();
                data.Type = this.Type;
                data.ID = this.ID;
                data.UserName = this.UserName;
                data.Password = this.Password;
                data.DtCreated = this.DtCreated;
                data.DtUpdated = this.DtUpdated;

                string json = JsonConvert.SerializeObject(data);
                return json;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error converting to JSON: " + e.Message);
                throw;
            }
        }
    }
}