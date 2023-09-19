using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using TermbinSharp.Services.Commands;
using TermbinSharp.Services.Queries;

namespace TermbinSharp.Controllers;


[ApiController]
[Route("[controller]/[action]")]

public class DataController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

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
            
            var request = _httpContextAccessor.HttpContext?.Request;
            var domain = $"{request?.Scheme}://{request.Host}";
            if (result.IsT0) return Ok(domain + "/" + result.AsT0);

            return BadRequest(result.AsT1.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
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
            if (result.IsT0) return Ok(result.AsT0);

            return BadRequest(result.AsT1.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

}