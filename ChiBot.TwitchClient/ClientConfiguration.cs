using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ChiBot.TwitchClient
{
    public class ClientConfiguration
    {
        public static ClientConfiguration Instance { get; private set; }
        public string Nickname { get; private set; }
        public string OauthToken { get; private set; }
        public string ServerHostName { get; private set; }
        public int ServerPort { get; private set; }
        public string ApiBaseUrl { get; private set; }
        public string ApiUsername { get; private set; }
        public string ApiPassword { get; private set; }

        static ClientConfiguration() {
            try
            {
                using (StreamReader fileReader = new StreamReader(Path.Combine(Environment.CurrentDirectory, "configuration.json")))
                {
                    string configurationJson = fileReader.ReadToEnd();
                    Instance = JsonConvert.DeserializeObject<ClientConfiguration>(configurationJson);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Configuration file not found.");
            }
        }

        [JsonConstructor]
        private ClientConfiguration(string nickname, string oauthToken, string serverHostName, int serverPort, string apiUrl, string apiBaseUrl, string apiUsername, string apiPassword) {
            Nickname = nickname;
            OauthToken = oauthToken;
            ServerHostName = serverHostName;
            ServerPort = serverPort;
            ApiBaseUrl = apiBaseUrl;
            ApiUsername = apiUsername;
            ApiPassword = apiPassword;
        }
    }
}
