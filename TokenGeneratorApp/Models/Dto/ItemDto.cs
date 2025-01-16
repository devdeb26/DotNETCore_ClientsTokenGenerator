using TokenGeneratorApp.Services.Interface;

namespace TokenGeneratorApp.Model.Dto;

public class ItemDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public int Amount { get; set; }
    public int TotalCount { get; set; }
}
