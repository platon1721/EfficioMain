using Asp.Versioning;
using Efficio.BLL.Contracts;
using Efficio.BLL.Contracts.Exceptions;
using Efficio.BLL.DTO.Tenants;
using Efficio.DTO.Mappers;
using Efficio.DTO.Tenants.TenantModule;
using Efficio.WebApp.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Efficio.WebApp.Controllers;

/// <summary>
/// Manage modules assigned to a tenant.
/// </summary>
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/tenants/{tenantId:guid}/modules")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TenantModulesController : ControllerBase
{
    private readonly IEfficioBLL _bll;

    public TenantModulesController(IEfficioBLL bll)
    {
        _bll = bll;
    }

    /// <summary>
    /// Get all modules for a tenant.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<IEnumerable<TenantModuleResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TenantModuleResponse>>> GetAll(Guid tenantId)
    {
        var tenant = await _bll.TenantService.FindAsync(tenantId)
                     ?? throw new NotFoundException("Tenant", tenantId);

        var modules = await _bll.TenantModuleService.GetByTenantAsync(tenant.RootDepartmentId);
        return Ok(modules.Select(TenantModuleApiMapper.ToResponse));
    }

    /// <summary>
    /// Get active (non-expired) modules for a tenant.
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType<IEnumerable<TenantModuleResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TenantModuleResponse>>> GetActive(Guid tenantId)
    {
        var tenant = await _bll.TenantService.FindAsync(tenantId)
                     ?? throw new NotFoundException("Tenant", tenantId);

        var modules = await _bll.TenantModuleService.GetActiveModulesForTenantAsync(tenant.RootDepartmentId);
        return Ok(modules.Select(TenantModuleApiMapper.ToResponse));
    }

    /// <summary>
    /// Assign a module to a tenant. Platform admin only.
    /// </summary>
    [HttpPost]
    [ProducesResponseType<TenantModuleResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<TenantModuleResponse>> Assign(Guid tenantId, [FromBody] AssignTenantModuleRequest request)
    {
        if (!User.IsPlatformAdmin())
            throw new ForbiddenException();

        var tenant = await _bll.TenantService.FindAsync(tenantId)
                     ?? throw new NotFoundException("Tenant", tenantId);

        var alreadyHas = await _bll.TenantModuleService.HasModuleAsync(tenant.RootDepartmentId, request.ModuleId);
        if (alreadyHas)
            throw new ConflictException("This module is already assigned to the tenant");

        var entity = new TenantModule
        {
            Id = Guid.NewGuid(),
            TenantRootDepartmentId = tenant.RootDepartmentId,
            ModuleId = request.ModuleId,
            ExpiresAt = request.ExpiresAt
        };

        _bll.TenantModuleService.Add(entity);
        await _bll.SaveChangesAsync();

        var created = await _bll.TenantModuleService.FindAsync(entity.Id);
        return CreatedAtAction(nameof(GetAll), new { tenantId },
            TenantModuleApiMapper.ToResponse(created!));
    }

    /// <summary>
    /// Remove a module from a tenant. Platform admin only.
    /// </summary>
    [HttpDelete("{moduleId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Remove(Guid tenantId, Guid moduleId)
    {
        if (!User.IsPlatformAdmin())
            throw new ForbiddenException();

        var tenant = await _bll.TenantService.FindAsync(tenantId)
                     ?? throw new NotFoundException("Tenant", tenantId);

        var tenantModule = await _bll.TenantModuleService.FindByTenantAndModuleAsync(tenant.RootDepartmentId, moduleId)
                           ?? throw new NotFoundException("TenantModule", moduleId);

        await _bll.TenantModuleService.RemoveAsync(tenantModule.Id);
        await _bll.SaveChangesAsync();

        return NoContent();
    }
}