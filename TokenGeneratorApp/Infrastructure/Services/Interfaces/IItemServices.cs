using TokenGeneratorApp.Model.Dto;

namespace TokenGeneratorApp.Services.Interface;

public interface IItemService
{
    Task<ItemDto> AddItem(ItemDto dto);
}
