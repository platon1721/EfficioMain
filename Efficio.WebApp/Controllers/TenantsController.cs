using Asp.Versioning;
using Efficio.BLL.Contracts;
using Efficio.BLL.Contracts.Exceptions;
using Efficio.DTO.Mappers;
using Efficio.DTO.Tenants.Tenant;
using Efficio.WebApp.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Efficio.WebApp.Controllers;

/// <summary>
/// Tenant management. Platform admins can manage all tenants.
/// </summary>
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TenantsController : ControllerBase
{
    private readonly IEfficioBLL _bll;
    private readonly ILogger<TenantsController> _logger;

    public TenantsController(IEfficioBLL bll, ILogger<TenantsController> logger)
    {
        _bll = bll;
        _logger = logger;
    }

    /// <summary>
    /// Get all active tenants. Platform admin only.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<IEnumerable<TenantResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TenantResponse>>> GetAll()
    {
        if (!User.IsPlatformAdmin())
            throw new ForbiddenException();

        var tenants = await _bll.TenantService.GetActiveTenantsAsync();
        return Ok(tenants.Select(TenantApiMapper.ToResponse));
    }

    /// <summary>
    /// Get tenant by id.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<TenantResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<TenantResponse>> Get(Guid id)
    {
        var entity = await _bll.TenantService.FindAsync(id);
        if (entity == null)
            throw new NotFoundException("Tenant", id);

        if (!User.IsPlatformAdmin())
        {
            var isMember = await _bll.UserAccessService
                .CanAccessTenantAsync(User.GetUserId(), entity.RootDepartmentId);
            if (!isMember)
                throw new ForbiddenException();
        }

        return Ok(TenantApiMapper.ToResponse(entity));
    }

    /// <summary>
    /// Find tenant by unique code.
    /// </summary>
    [HttpGet("by-code/{code}")]
    [ProducesResponseType<TenantResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<TenantResponse>> GetByCode(string code)
    {
        var entity = await _bll.TenantService.FindByCodeAsync(code.Trim().ToLowerInvariant());
        if (entity == null)
            throw new NotFoundException($"Tenant with code '{code}' was not found");

        return Ok(TenantApiMapper.ToResponse(entity));
    }

    /// <summary>
    /// Create a new tenant. Platform admin only.
    /// </summary>
    [HttpPost]
    [ProducesResponseType<TenantResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<TenantResponse>> Create([FromBody] CreateTenantRequest request)
    {
        if (!User.IsPlatformAdmin())
            throw new ForbiddenException();

        var codeExists = await _bll.TenantService.CodeExistsAsync(request.Code.Trim().ToLowerInvariant());
        if (codeExists)
            throw new ConflictException("Tenant", "code", request.Code);

        var bllEntity = TenantApiMapper.ToBll(request);
        _bll.TenantService.Add(bllEntity);
        await _bll.SaveChangesAsync();

        _logger.LogInformation("Tenant {Code} created by {UserId}", request.Code, User.GetUserId());

        var created = await _bll.TenantService.FindAsync(bllEntity.Id);
        return CreatedAtAction(nameof(Get), new { id = bllEntity.Id },
            TenantApiMapper.ToResponse(created!));
    }

    /// <summary>
    /// Update an existing tenant. Platform admin only.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType<TenantResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<TenantResponse>> Update(Guid id, [FromBody] UpdateTenantRequest request)
    {
        if (!User.IsPlatformAdmin())
            throw new ForbiddenException();

        var existing = await _bll.TenantService.FindAsync(id);
        if (existing == null)
            throw new NotFoundException("Tenant", id);

        existing.Name = request.Name.Trim();
        if (request.DefaultTimeZone != null) existing.DefaultTimeZone = request.DefaultTimeZone;
        if (request.DefaultLocale != null) existing.DefaultLocale = request.DefaultLocale;

        _bll.TenantService.Update(existing);
        await _bll.SaveChangesAsync();

        var updated = await _bll.TenantService.FindAsync(id);
        return Ok(TenantApiMapper.ToResponse(updated!));
    }

    /// <summary>
    /// Delete (archive) a tenant. Platform admin only.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(Guid id)
    {
        if (!User.IsPlatformAdmin())
            throw new ForbiddenException();

        var exists = await _bll.TenantService.ExistsAsync(id);
        if (!exists)
            throw new NotFoundException("Tenant", id);

        await _bll.TenantService.RemoveAsync(id);
        await _bll.SaveChangesAsync();

        _logger.LogInformation("Tenant {Id} deleted by {UserId}", id, User.GetUserId());
        return NoContent();
    }
}