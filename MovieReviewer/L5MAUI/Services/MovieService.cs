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
    internal class MovieService : IMovieService
    {
        private readonly HttpClient _httpClient;
        private const string _movieEndpoint = "Movie/";

        public MovieService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<ServiceResponse<MovieDTO>> AddMovieAsync(CreateMovieDTO createMovieDTO)
        {
            var response = await _httpClient.PostAsJsonAsync(_movieEndpoint, createMovieDTO);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<MovieDTO>>();
            return result;
        }

        public async Task<ServiceResponse<List<MovieDTO>>> GetMoviesAsync()
        {
            var response = await _httpClient.GetAsync(_movieEndpoint);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ServiceResponse<List<MovieDTO>>>(json);
            return result;
        }

        public async Task<ServiceResponse<bool>> DeleteMovieAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(_movieEndpoint + $"{id}");
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
            return result;
        }

        public async Task<ServiceResponse<MovieDTO>> UpdateMovieAsync(UpdateMovieDTO updatedMovieDTO)
        {
            var response = await _httpClient.PutAsJsonAsync(_movieEndpoint, updatedMovieDTO);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<MovieDTO>>();
            return result;
        }

        public Task<ServiceResponse<MovieDTO>> GetMovieAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
