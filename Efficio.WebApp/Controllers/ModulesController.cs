using Asp.Versioning;
using Efficio.BLL.Contracts;
using Efficio.BLL.Contracts.Exceptions;
using Efficio.DTO.Mappers;
using Efficio.DTO.Security.Module;
using Efficio.WebApp.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Efficio.WebApp.Controllers;

/// <summary>
/// Platform-level module management. Platform admin only.
/// </summary>
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ModulesController : ControllerBase
{
    private readonly IEfficioBLL _bll;

    public ModulesController(IEfficioBLL bll)
    {
        _bll = bll;
    }

    [HttpGet]
    [ProducesResponseType<IEnumerable<ModuleResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ModuleResponse>>> GetAll()
    {
        var modules = await _bll.ModuleService.GetActiveModulesAsync();
        return Ok(modules.Select(ModuleApiMapper.ToResponse));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<ModuleResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ModuleResponse>> Get(Guid id)
    {
        var entity = await _bll.ModuleService.FindAsync(id)
                     ?? throw new NotFoundException("Module", id);

        return Ok(ModuleApiMapper.ToResponse(entity));
    }

    [HttpGet("by-code/{code}")]
    [ProducesResponseType<ModuleResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ModuleResponse>> GetByCode(string code)
    {
        var entity = await _bll.ModuleService.FindByCodeAsync(code.Trim())
                     ?? throw new NotFoundException($"Module with code '{code}' was not found");

        return Ok(ModuleApiMapper.ToResponse(entity));
    }

    [HttpPost]
    [ProducesResponseType<ModuleResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<ModuleResponse>> Create([FromBody] CreateModuleRequest request)
    {
        if (!User.IsPlatformAdmin())
            throw new ForbiddenException();

        var existing = await _bll.ModuleService.FindByCodeAsync(request.Code.Trim());
        if (existing != null)
            throw new ConflictException("Module", "code", request.Code);

        var bllEntity = ModuleApiMapper.ToBll(request);
        _bll.ModuleService.Add(bllEntity);
        await _bll.SaveChangesAsync();

        var created = await _bll.ModuleService.FindAsync(bllEntity.Id);
        return CreatedAtAction(nameof(Get), new { id = bllEntity.Id },
            ModuleApiMapper.ToResponse(created!));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(Guid id)
    {
        if (!User.IsPlatformAdmin())
            throw new ForbiddenException();

        if (!await _bll.ModuleService.ExistsAsync(id))
            throw new NotFoundException("Module", id);

        await _bll.ModuleService.RemoveAsync(id);
        await _bll.SaveChangesAsync();
        return NoContent();
    }
}