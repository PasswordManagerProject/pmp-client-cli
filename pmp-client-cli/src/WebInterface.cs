using System;
using System.Net.Http;
using System.Text;

namespace pmp_client_cli
{
    public static class WebInterface
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        
        public static bool WebInsert(string ID, string userName, string password)
        {
            try
            {
                Record rec = new Record("INSERT", ID, userName, password, 
                                DateTime.Now, DateTime.Now);
                
                string jsonRec = rec.ToJson();
                
                using (StringContent content = new StringContent(jsonRec, Encoding.UTF8, "application/json"))
                {
                    HttpResponseMessage result = HttpClient.PostAsync(Utils.ServerAddr, content).Result;

                    if (!result.IsSuccessStatusCode)
                        throw new Exception(result.ReasonPhrase);
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error sending insert request: " + e.Message);
                return false;
            }
        }
        
        public static bool WebUpdate(string ID, string password)
        {
            try
            {
                Record rec = new Record("INSERT", ID, "", password, 
                    DateTime.Now, DateTime.Now);

                string jsonRec = rec.ToJson();
                
                using (StringContent content = new StringContent(jsonRec, Encoding.UTF8, "application/json"))
                {
                    HttpResponseMessage result = HttpClient.PutAsync(Utils.ServerAddr, content).Result;

                    if (!result.IsSuccessStatusCode)
                        throw new Exception(result.ReasonPhrase);
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error sending update request: " + e.Message);
                return false;
            }
        }
    }
}