
using L5Shared;
using L5Shared.DTO;
using L5Shared.Services;

namespace Lab5.Services;

public class DirectorService : IDirectorService
{
    private readonly HttpClient _httpClient;

    public DirectorService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        var apiUrl = configuration.GetValue<Uri>("apiUrl");
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = apiUrl;
    }

    public async Task<ServiceResponse<List<DirectorDTO>>> GetDirectorsAsync()
    {
        return await _httpClient.GetFromJsonAsync<ServiceResponse<List<DirectorDTO>>>("/api/Director");
    }

    public async Task<ServiceResponse<DirectorDTO>> AddDirectorAsync(CreateDirectorDTO createDirectorDTO)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Director", createDirectorDTO);
        return await response.Content.ReadFromJsonAsync<ServiceResponse<DirectorDTO>>();
    }
}
