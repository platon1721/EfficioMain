using Asp.Versioning;
using Efficio.BLL.Contracts;
using Efficio.BLL.Contracts.Exceptions;
using Efficio.DTO.Mappers;
using Efficio.DTO.Security.Permission;
using Efficio.WebApp.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Efficio.WebApp.Controllers;

/// <summary>
/// Platform-level permission management.
/// </summary>
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PermissionsController : ControllerBase
{
    private readonly IEfficioBLL _bll;

    public PermissionsController(IEfficioBLL bll)
    {
        _bll = bll;
    }

    [HttpGet]
    [ProducesResponseType<IEnumerable<PermissionResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PermissionResponse>>> GetAll()
    {
        var permissions = await _bll.PermissionService.GetWithModuleAsync();
        return Ok(permissions.Select(PermissionApiMapper.ToResponse));
    }

    [HttpGet("active")]
    [ProducesResponseType<IEnumerable<PermissionResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PermissionResponse>>> GetActive()
    {
        var permissions = await _bll.PermissionService.GetActivePermissionsAsync();
        return Ok(permissions.Select(PermissionApiMapper.ToResponse));
    }

    [HttpGet("by-module/{moduleId:guid}")]
    [ProducesResponseType<IEnumerable<PermissionResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PermissionResponse>>> GetByModule(Guid moduleId)
    {
        var permissions = await _bll.PermissionService.GetByModuleIdAsync(moduleId);
        return Ok(permissions.Select(PermissionApiMapper.ToResponse));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<PermissionResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<PermissionResponse>> Get(Guid id)
    {
        var entity = await _bll.PermissionService.FindAsync(id)
                     ?? throw new NotFoundException("Permission", id);

        return Ok(PermissionApiMapper.ToResponse(entity));
    }

    [HttpPost]
    [ProducesResponseType<PermissionResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<PermissionResponse>> Create([FromBody] CreatePermissionRequest request)
    {
        if (!User.IsPlatformAdmin())
            throw new ForbiddenException();

        var existing = await _bll.PermissionService.FindByKeyAsync(request.Key.Trim());
        if (existing != null)
            throw new ConflictException("Permission", "key", request.Key);

        var bllEntity = PermissionApiMapper.ToBll(request);
        _bll.PermissionService.Add(bllEntity);
        await _bll.SaveChangesAsync();

        var created = await _bll.PermissionService.FindAsync(bllEntity.Id);
        return CreatedAtAction(nameof(Get), new { id = bllEntity.Id },
            PermissionApiMapper.ToResponse(created!));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(Guid id)
    {
        if (!User.IsPlatformAdmin())
            throw new ForbiddenException();

        if (!await _bll.PermissionService.ExistsAsync(id))
            throw new NotFoundException("Permission", id);

        await _bll.PermissionService.RemoveAsync(id);
        await _bll.SaveChangesAsync();
        return NoContent();
    }
}