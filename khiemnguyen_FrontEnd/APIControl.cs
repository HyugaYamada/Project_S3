using khiemnguyen_FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace khiemnguyen_FrontEnd
{

    public class APIControl
    {
        public static string APIToken;
        public static string baseAddress = "https://localhost:7022";
        public static string Role="";
        public static int UserID =0;
        public static string UserName = "";
        public static string FullName = "";
        public APIControl()
        {
            GenerateToke();
        }
        public static void GenerateToke()
        {
                        
            Token token = new Token();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

               // var form = new Dictionary<string, string>
               //{

               // { "Email", "abc@abc.abc"},
               //     { "Password", "abc"}
               // };

                var command = new User()
                {

                    Email = "abc@abc.abc",
                    Password = "abc"
                };

                HttpResponseMessage response = client.PostAsJsonAsync(baseAddress + "/api/token", command).Result;

                //var tokenResponse = client.PostAsJsonAsync (baseAddress + "/api/token", new FormUrlEncodedContent(form)).Result;
                var token1 = response.Content.ReadAsStringAsync().Result;
                //   token = token1.Content.ReadFromJsonAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
                token.AccessToken = token1;
                dynamic ss = JsonConvert.DeserializeObject(token1);

                APIToken = ss;
            }
        }


        public IEnumerable<T> GetEndPoint<T>(string EndpointName)
        {
            
         
            dynamic Emp1;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseAddress);
                //httpClient1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.AccessToken);
                httpClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", APIToken));
                HttpResponseMessage response = httpClient.GetAsync(EndpointName).Result;
                if (response.IsSuccessStatusCode)
                {
                    System.Console.WriteLine("Success");
                }
                string message = response.Content.ReadAsStringAsync().Result;
                dynamic response2 = JsonConvert.DeserializeObject(message);

                
                Emp1 = response2.ToObject<IEnumerable<T>>();
           

            }

             return (IEnumerable<T>)Emp1;
        }


        public IEnumerable<T> PostEndPoint<T>(string EndpointName,object values)
        {


            var json = JsonConvert.SerializeObject(values);
            string accessToken1 = APIToken;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken1);

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            //  var content = new FormUrlEncodedContent(values);
            var response = client.PostAsync(baseAddress+EndpointName, content);
			
			HttpResponseMessage apiResponse = response.Result;
            var dd = apiResponse.Content.ReadAsStringAsync();


            return null;
        }
    }
}
