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
    internal class ActorService : IActorService
    {
        private readonly HttpClient _httpClient;
        private const string _actorEndpoint = "Actor/";

        public ActorService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<ServiceResponse<ActorDTO>> AddActorAsync(CreateActorDTO createActorDTO)
        {
            var response = await _httpClient.PostAsJsonAsync(_actorEndpoint, createActorDTO);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<ActorDTO>>();
            return result;
        }

        public async Task<ServiceResponse<List<ActorDTO>>> GetActorsAsync()
        {
            var response = await _httpClient.GetAsync(_actorEndpoint);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ServiceResponse<List<ActorDTO>>>(json);
            return result;
        }
    }
}
