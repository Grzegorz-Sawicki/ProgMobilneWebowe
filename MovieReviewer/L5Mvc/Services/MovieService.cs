using L5Shared;
using L5Shared.DTO;
using L5Shared.Services;

namespace Lab5.Services;

public class MovieService : IMovieService
{
    private readonly ILogger<MovieService> _logger;
    private readonly HttpClient _httpClient;

    public MovieService(IConfiguration configuration, ILogger<MovieService> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;

        var apiUrl = configuration.GetValue<Uri>("apiUrl");
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = apiUrl;
    }

    public async Task<ServiceResponse<MovieDTO>> GetMovieAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<ServiceResponse<MovieDTO>>($"/api/Movie/{id}");
    }

    public async Task<ServiceResponse<List<MovieDTO>>> GetMoviesAsync()
    {
        return await _httpClient.GetFromJsonAsync<ServiceResponse<List<MovieDTO>>>("/api/Movie");
    }

    public async Task<ServiceResponse<MovieDTO>> AddMovieAsync(CreateMovieDTO createMovieDTO)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Movie", createMovieDTO);
        _logger.LogInformation("{}", System.Text.Json.JsonSerializer.Serialize(createMovieDTO));
        _logger.LogInformation("{}", System.Text.Json.JsonSerializer.Serialize(response.Content));
        return await response.Content.ReadFromJsonAsync<ServiceResponse<MovieDTO>>();
    }

    public async Task<ServiceResponse<MovieDTO>> UpdateMovieAsync(UpdateMovieDTO updatedMovieDTO)
    {
        var response = await _httpClient.PutAsJsonAsync("/api/Movie", updatedMovieDTO);
        return await response.Content.ReadFromJsonAsync<ServiceResponse<MovieDTO>>();
    }

    public async Task<ServiceResponse<bool>> DeleteMovieAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"/api/Movie/{id}");
        return await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
    }


}
