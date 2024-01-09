using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;



namespace Tafeltennis.Services
{

    public class Player
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Wins { get; set; }
        
    }

    public class PlayerService
    {
        private readonly string BaseUrl = "https://192.168.101.146/"; 

        public HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                string issuer = cert.Issuer;

                Console.WriteLine("Certificate Issuer: " + issuer);
                if (cert.Issuer.Equals("CN=TafeltennisAPI, O=Internet Widgits Pty Ltd, S=Some-State, C=AU"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;

            }
            };
            return handler;
        }

        public async Task<Player> CreatePlayerAsync(Player player)
        {
            using (var client = new HttpClient(GetInsecureHandler()))
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(player);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("/api/player", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Player>(responseData);
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception(errorContent);
                }
            }
        }
        public async Task<List<Player>> GetAllPlayersAsync()
        {
            using (var client = new HttpClient(GetInsecureHandler()))
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("/api/players");

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Player>>(responseData);
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception(errorContent);
                }
            }
        }
        public async Task<Player> GetPlayerByIdAsync(int playerId)
        {

            using (var client = new HttpClient(GetInsecureHandler()))
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"/api/player/{playerId}");

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Player>(responseData);
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception(errorContent);
                }
            }
        }
        public async Task UpdatePlayerAsync(int playerId, string updatedWinsValue)
        {
            using (var client = new HttpClient(GetInsecureHandler()))
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var updatedPlayer = new Player() { Id = playerId.ToString(), Wins = updatedWinsValue };
                var json = JsonConvert.SerializeObject(updatedPlayer);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync($"/api/player/{playerId}", content);

                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception(errorContent);
                }
            }
        }

        public async Task DeletePlayerAsync(int playerId)
        {
            using (var client = new HttpClient(GetInsecureHandler()))
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.DeleteAsync($"/api/player/{playerId}");

                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception(errorContent);
                }
            }
        }
    }
}
