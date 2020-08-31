using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorTextAPITest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*** Translator Text API Test ***");
            Console.WriteLine("Supported languages");
            GetSupportedLanguages();

            Console.WriteLine("** Translating - Hello! How are you? **");

            TranslateText();

            Console.ReadLine();
        }

        static void GetSupportedLanguages()
        {
            var route = "/languages?api-version=3.0";
            var result = MakeTranslatorWebCall(route, string.Empty, HttpMethod.Get, string.Empty);
            Console.WriteLine(result);
        }

        static void TranslateText()
        {
            var route = "/translate?api-version=3.0";
            var parameters = "&to=it";
            string textToProcess = "Hello! How are you?";
            var result = MakeTranslatorWebCall(route, parameters, HttpMethod.Post, textToProcess);
            Console.WriteLine(result);
        }

        private static string MakeTranslatorWebCall(string route, string parameters, HttpMethod httpMethod, string textToProcess)
        {
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage())
                {
                    var body = new System.Object[] { new { Text = textToProcess } };
                    var requestBody = JsonConvert.SerializeObject(body);

                    request.Method = httpMethod;
                    request.RequestUri = new Uri(Properties.Settings.Default.baseURL + route + parameters);

                    //request.Headers.Add("Ocp-Apim-Subscription-Key", Properties.Settings.Default.suscriptionKey);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Properties.Settings.Default.bearerToken);

                    request.Content = httpMethod == HttpMethod.Post ? new StringContent(requestBody, Encoding.UTF8, "application/json") : null;

                    var response = client.SendAsync(request).Result;
                    var jsonResponseContent = response.Content.ReadAsStringAsync().Result;

                    return FormatJSONForPrinting(jsonResponseContent);
                }
            }
        }

        static string FormatJSONForPrinting(string input)
        {
            return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(input), Formatting.Indented);
        }
    }
}
