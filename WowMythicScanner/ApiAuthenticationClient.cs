using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WowMythicScanner
{
    class ApiAuthenticationClient : IApiTokenProvider
    {
        private HttpClient _httpClient;

        private DateTime _tokenExpirationTime = DateTime.MinValue;
        private string _tokenValue = string.Empty;

        public ApiAuthenticationClient(IHttpClientFactory httpClientFactory, IConfigurationRoot configurationRoot)
        {
            _httpClient = httpClientFactory.CreateClient("apiAuthenticationClient");

            var configuration = configurationRoot.GetSection("blizzardApi").Get<BlizzardApiConfiguration>();
            string encodedClientInfo = Convert.ToBase64String(
                Encoding.GetEncoding("ISO-8859-1")
                        .GetBytes(configuration.ClientId + ":" + configuration.ClientSecret));

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", encodedClientInfo);
        }

        public async Task<string> GetTokenAsync()
        {
            if(DateTime.Now < _tokenExpirationTime)
            {
                return _tokenValue;
            }

            FormUrlEncodedContent authRequestContent = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" }
                });

            var response = await _httpClient.PostAsync("https://eu.battle.net/oauth/token", authRequestContent);
            var body = await response.Content.ReadAsStringAsync();

            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(body);
            _tokenValue = tokenResponse.AccessToken;
            _tokenExpirationTime = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn);

            return _tokenValue;
        }

        private class TokenResponse
        {
            [JsonPropertyName("access_token")]   
            public string AccessToken { get; set; }
            [JsonPropertyName("expires_in")]
            public int ExpiresIn { get; set; }
        }

        private class BlizzardApiConfiguration
        {
            public string ClientSecret { get; set; }
            public string ClientId { get; set; }
        }
    }
}
