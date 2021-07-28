using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WowMythicScanner
{
    class ApiReader
    {
        private readonly HttpClient httpClient;
        private readonly IApiTokenProvider tokenProvider;

        public ApiReader(IHttpClientFactory httpClientFactory, IApiTokenProvider tokenProvider)
        {
            this.httpClient = httpClientFactory.CreateClient("apiClient");
            this.tokenProvider = tokenProvider;
        }

        internal async Task<string> RequestAsync(string query, string additionalParams = null)
        {
            var token = await tokenProvider.GetTokenAsync();

            var path = query + "?namespace=static-eu&locale=en_US&access_token=" + token;

            var response = await httpClient.GetAsync(path);

            return await response.Content.ReadAsStringAsync(); 
        }
    }
}
