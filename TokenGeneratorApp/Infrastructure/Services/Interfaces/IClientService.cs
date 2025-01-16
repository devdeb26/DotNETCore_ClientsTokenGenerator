using TokenGeneratorApp.Model.Dto;

namespace TokenGeneratorApp.Services.Interface;

public interface IClientService
{
    Task<HttpClient> CreateClient(string clientName);
}
