using TokenGeneratorApp.Services.Interface;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
namespace TokenGeneratorApp.Services;

public class ClientService : IClientService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ClientService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<HttpClient> CreateClient(string clientName)
    {
        var client = _httpClientFactory.CreateClient(clientName);
        await SetRequestHeaders(client);
        return client;
    }

    protected async Task SetRequestHeaders(HttpClient client)
    {
        var token = await GenerateClientToken();

        if (!String.IsNullOrEmpty(token))
        {
            if (client.DefaultRequestHeaders.Contains("Authorization"))
            {
                client.DefaultRequestHeaders.Remove("Authorization");
            }
            client.DefaultRequestHeaders.Add("Authorization", token);
        }
    }

    private async Task<string> GenerateClientToken()
    {
        var privateKey = @"----------BEGIN KEY -----------dhafkljasdhfahsdfsahflsadhlfhas----------END KEY------------";
        var rsa = RSA.Create();
        rsa.ImportFromPem(privateKey);
        var securityKey = new RsaSecurityKey(rsa);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);
        var header = new JwtHeader(credentials);
        var scope = new[]
            {
                "concept:content_structure",
                "concept:content_design"
            };
        var roles = new[]
            {
                "ROLE_INTERNAL"
            };
        
        var expirationTime = DateTime.UtcNow.AddHours(1);
        var expirationUnixTime = new DateTimeOffset(expirationTime).ToUnixTimeSeconds();

        var issueAtTime = DateTime.UtcNow.AddHours(1);
        var issueAtTimeUnixTime = new DateTimeOffset(issueAtTime).ToUnixTimeSeconds();

        var payload = new JwtPayload
        {
            {JwtRegisteredClaimNames.Sub, "sample-non-prod"},
            {JwtRegisteredClaimNames.Aud, "sample-api"},
            {JwtRegisteredClaimNames.Iss, "sample"},
            {"scope", scope},
            {"authorities", roles},
            {JwtRegisteredClaimNames.Iat, issueAtTimeUnixTime},
            {JwtRegisteredClaimNames.Exp, expirationUnixTime},
        };

        var token = new JwtSecurityToken(header, payload);

        if (token is JwtSecurityToken jwtSecurityToken)
        {
            jwtSecurityToken.Header["kid"] = "23456789045678-786-67887998";
            jwtSecurityToken.Header.Remove("typ");
            jwtSecurityToken.Payload.Remove("nbf");
        }
        
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = jwtSecurityTokenHandler.WriteToken(token);
        return "Bearer " + jwtToken;
    }
}
