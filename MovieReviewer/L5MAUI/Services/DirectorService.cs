using L5Shared;
using L5Shared.DTO;
using L5Shared.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace L5MAUI.Services
{
    internal class DirectorService : IDirectorService
    {
        private readonly HttpClient _httpClient;
        private const string _directorEndpoint = "Director/";

        public DirectorService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<ServiceResponse<DirectorDTO>> AddDirectorAsync(CreateDirectorDTO createDirectorDTO)
        {
            var response = await _httpClient.PostAsJsonAsync(_directorEndpoint, createDirectorDTO);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<DirectorDTO>>();
            return result;
        }

        public async Task<ServiceResponse<List<DirectorDTO>>> GetDirectorsAsync()
        {
            var response = await _httpClient.GetAsync(_directorEndpoint);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ServiceResponse<List<DirectorDTO>>>(json);
            return result;
        }
    }
}
