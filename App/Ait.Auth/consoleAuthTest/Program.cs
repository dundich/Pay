using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace consoleAuthTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //UserOwner.DoUserGrand(args);

            do
            {

                UserOwner.DoClientGrand();
            }
            while (string.IsNullOrWhiteSpace(Console.ReadLine()));
        }
    }


    class UserOwner
    {
        private const string APP_PATH = "http://localhost:26264";
        private static string token;

        internal static void DoClientGrand()
        {
            //grant_type = client_credentials&client_id = CLIENT_ID&client_secret = CLIENT_SECRET

            var pairs = new Dictionary<string, string>
            {
                    { "grant_type", "client_credentials" },
                    { "client_id", "consoleApp" },
                    //{ "scope", "bob tod kod" },
                    { "client_secret", "123@abc" },
                    { "response_type", "token" }
            };

            var content = new FormUrlEncodedContent(pairs);

            using (var client = new HttpClient())
            {
                var response =
                    client.PostAsync(APP_PATH + "/token", content).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                Console.WriteLine(result);

                // Десериализация полученного JSON-объекта                
            }





        }

        internal static void DoUserGrand(string[] args)
        {
            Console.WriteLine("Введите логин:");
            string userName = Console.ReadLine();

            Console.WriteLine("Введите пароль:");
            string password = Console.ReadLine();

            try
            {
                Dictionary<string, string> tokenDictionary = GetTokenDictionary(userName, password);


                if (tokenDictionary.ContainsKey("access_token"))
                {
                    token = tokenDictionary["access_token"];

                    Console.WriteLine();
                    Console.WriteLine("Access Token:");
                    Console.WriteLine(token);

                    //using (var client = new HttpClient())
                    //{
                    //    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    //    var response = client.GetAsync(APP_PATH + "/token").Result;
                    //    var result = response.Content.ReadAsStringAsync().Result;
                    //    // Десериализация полученного JSON-объекта
                    //    Console.WriteLine(result);
                    //}

                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
            }
        }

        // получение токена
        static Dictionary<string, string> GetTokenDictionary(string userName, string password)
        {
            var pairs = new Dictionary<string, string>
            {
                    { "grant_type", "password" },
                    { "username", userName },
                    { "Password", password },
                    { "client_id", "consoleApp" },
                    { "scope", "bob tod kod" },
                    { "client_secret", "123@abc" },
            };

            var content = new FormUrlEncodedContent(pairs);

            using (var client = new HttpClient())
            {
                var response =
                    client.PostAsync(APP_PATH + "/token", content).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                Console.WriteLine(result);

                // Десериализация полученного JSON-объекта
                Dictionary<string, string> tokenDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                return tokenDictionary;
            }
        }

        //// создаем http-клиента с токеном 
        //static HttpClient CreateClient(string accessToken = "")
        //{
        //    var client = new HttpClient();
        //    if (!string.IsNullOrWhiteSpace(accessToken))
        //    {
        //        client.DefaultRequestHeaders.Authorization =
        //            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        //    }
        //    return client;
        //}

        //// получаем информацию о клиенте 
        //static string GetUserInfo(string token)
        //{
        //    using (var client = CreateClient(token))
        //    {
        //        var response = client.GetAsync(APP_PATH + "/api/Account/UserInfo").Result;
        //        return response.Content.ReadAsStringAsync().Result;
        //    }
        //}

        //// обращаемся по маршруту api/values 
        //static string GetValues(string token)
        //{
        //    using (var client = CreateClient(token))
        //    {
        //        var response = client.GetAsync(APP_PATH + "/api/values").Result;
        //        return response.Content.ReadAsStringAsync().Result;
        //    }
        //}
    }
}
