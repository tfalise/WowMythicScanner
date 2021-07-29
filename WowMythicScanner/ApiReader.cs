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
        private readonly ApiConfiguration configuration;

        public ApiReader(IHttpClientFactory httpClientFactory, IApiTokenProvider tokenProvider, ApiConfiguration configuration)
        {
            this.httpClient = httpClientFactory.CreateClient("apiClient");

            this.tokenProvider = tokenProvider;
            this.configuration = configuration;
        }

        internal async Task<string> RequestAsync(string query, string apiNamespace, string additionalParams = null)
        {
            var token = await tokenProvider.GetTokenAsync();
            var fullNamespace = $@"{apiNamespace}-{configuration.GetRegionUrlPrefix()}";

            var path = @$"{GetApiRootUrl()}{query}?namespace={fullNamespace}&locale={configuration.GetLocale()}&access_token={token}";
            var response = await httpClient.GetAsync(path);

            return await response.Content.ReadAsStringAsync(); 
        }

        private string GetApiRootUrl() => $@"https://{configuration.GetRegionUrlPrefix()}.api.blizzard.com";
    }
}
