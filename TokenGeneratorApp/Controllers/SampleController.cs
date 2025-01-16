using Microsoft.AspNetCore.Mvc;
using TokenGeneratorApp.Model.Dto;
using TokenGeneratorApp.Services.Interface;

namespace TokenGeneratorApp.Controllers;

public class SampleController : ControllerBase
{
    private readonly IItemService _itemService;
    public SampleController(IItemService itemService)
    {
        _itemService = itemService;
    }
    public async Task<IActionResult> AddItem(ItemDto dto)
    {
        try
        {
            var response = await _itemService.AddItem(dto);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An unexpected error occurred.", Detail = ex.Message });
        }
    }
}