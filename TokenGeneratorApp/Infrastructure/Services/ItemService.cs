using System.Runtime.CompilerServices;
using TokenGeneratorApp.Model.Dto;
using TokenGeneratorApp.Services.Interface;
using System.Text.Json;

namespace TokenGeneratorApp.Services;

public class ItemService : IItemService
{
    private readonly IClientService _clientService;

    public ItemService(IClientService clientService)
    {
        _clientService = clientService;

    }

    public async Task<ItemDto> AddItem(ItemDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        var url = "/api/test/add-item";
        HttpClient client = await _clientService.CreateClient("ABCClient");
        var json = JsonSerializer.Serialize(dto);
        using var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, content);

        var contentResponse = await response.Content.ReadAsStringAsync();
        var contentJson = JsonSerializer.Deserialize<ItemDto>(contentResponse);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error in AddItem. Error Code: {response.StatusCode} {response.Content} {contentJson}");
        }
        
        return contentJson;
    }
}
