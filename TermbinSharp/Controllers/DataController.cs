using Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using TermbinSharp.Services.Commands;
using TermbinSharp.Services.Queries;

namespace TermbinSharp.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class DataController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMediator _mediator;

    public DataController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }


    [HttpPost]
    public async Task<IActionResult> Set([FromBody] string data)
    {
        try
        {
            var result = await _mediator.Send(new SetCommand(data));
            if (result.IsSuccess)
                return Ok(
                    $"{_httpContextAccessor.HttpContext?.Request?.Scheme}://{_httpContextAccessor.HttpContext?.Request?.Host}/{result.ActualValue}");

            return BadRequest(result.Error.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet]
    [Route("/{path}")]
    [OutputCache]
    public async Task<IActionResult> Get(string path)
    {
        try
        {
            var result = await _mediator.Send(new GetQuery(path));

            if (result.IsSuccess) return Ok(result.ActualValue);

            return BadRequest(result.Error.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}