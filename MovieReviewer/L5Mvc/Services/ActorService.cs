
using L5Shared;
using L5Shared.DTO;
using L5Shared.Services;

namespace Lab5.Services;

public class ActorService : IActorService
{
    private readonly HttpClient _httpClient;

    public ActorService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        var apiUrl = configuration.GetValue<Uri>("apiUrl");
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = apiUrl;
    }

    public async Task<ServiceResponse<List<ActorDTO>>> GetActorsAsync()
    {
        return await _httpClient.GetFromJsonAsync<ServiceResponse<List<ActorDTO>>>("/api/Actor");
    }

    public async Task<ServiceResponse<ActorDTO>> AddActorAsync(CreateActorDTO createActorDTO)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Actor", createActorDTO);
        return await response.Content.ReadFromJsonAsync<ServiceResponse<ActorDTO>>();
    }
}
