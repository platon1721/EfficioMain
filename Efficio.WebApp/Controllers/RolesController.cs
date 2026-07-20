using Asp.Versioning;
using Efficio.BLL.Contracts;
using Efficio.BLL.Contracts.Exceptions;
using Efficio.DTO.Mappers;
using Efficio.DTO.Security.Role;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Efficio.WebApp.Controllers;

/// <summary>
/// Department-scoped role management.
/// </summary>
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/tenants/{tenantId:guid}/departments/{departmentId:guid}/roles")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RolesController : ControllerBase
{
    private readonly IEfficioBLL _bll;

    public RolesController(IEfficioBLL bll)
    {
        _bll = bll;
    }

    /// <summary>
    /// Get all roles in a department.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<IEnumerable<RoleResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RoleResponse>>> GetAll(Guid tenantId, Guid departmentId)
    {
        var roles = await _bll.RoleService.GetByDepartmentAsync(departmentId);
        return Ok(roles.Select(RoleApiMapper.ToResponse));
    }

    /// <summary>
    /// Get role by id.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<RoleWithPermissionsResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<RoleWithPermissionsResponse>> Get(Guid tenantId, Guid departmentId, Guid id)
    {
        var entity = await _bll.RoleService.FindWithPermissionsAsync(id)
                     ?? throw new NotFoundException("Role", id);

        return Ok(RoleApiMapper.ToDetailResponse(entity));
    }

    /// <summary>
    /// Create a new role.
    /// </summary>
    [HttpPost]
    [ProducesResponseType<RoleResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<RoleResponse>> Create(Guid tenantId, Guid departmentId, [FromBody] CreateRoleRequest request)
    {
        var tenant = await _bll.TenantService.FindAsync(tenantId)
                     ?? throw new NotFoundException("Tenant", tenantId);

        var nameExists = await _bll.RoleService.NameExistsAsync(departmentId, request.Name.Trim());
        if (nameExists)
            throw new ConflictException("Role", "name", request.Name);

        var bllEntity = RoleApiMapper.ToBll(request, tenant.RootDepartmentId);
        _bll.RoleService.Add(bllEntity);
        await _bll.SaveChangesAsync();

        var created = await _bll.RoleService.FindAsync(bllEntity.Id);
        return CreatedAtAction(nameof(Get), new { tenantId, departmentId, id = bllEntity.Id },
            RoleApiMapper.ToResponse(created!));
    }

    /// <summary>
    /// Update a role.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType<RoleResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<RoleResponse>> Update(Guid tenantId, Guid departmentId, Guid id, [FromBody] UpdateRoleRequest request)
    {
        var existing = await _bll.RoleService.FindAsync(id)
                       ?? throw new NotFoundException("Role", id);

        var nameExists = await _bll.RoleService.NameExistsAsync(departmentId, request.Name.Trim(), id);
        if (nameExists)
            throw new ConflictException("Role", "name", request.Name);

        existing.Name = request.Name.Trim();
        existing.Description = request.Description?.Trim();

        _bll.RoleService.Update(existing);
        await _bll.SaveChangesAsync();

        var updated = await _bll.RoleService.FindAsync(id);
        return Ok(RoleApiMapper.ToResponse(updated!));
    }

    /// <summary>
    /// Delete a role.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(Guid tenantId, Guid departmentId, Guid id)
    {
        if (!await _bll.RoleService.ExistsAsync(id))
            throw new NotFoundException("Role", id);

        await _bll.RoleService.RemoveAsync(id);
        await _bll.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Assign a permission to a role.
    /// </summary>
    [HttpPost("{id:guid}/permissions")]
    [ProducesResponseType<RoleWithPermissionsResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<RoleWithPermissionsResponse>> AssignPermission(Guid tenantId, Guid departmentId, Guid id, [FromBody] AssignPermissionRequest request)
    {
        var result = await _bll.RoleService.AssignPermissionAsync(id, request.PermissionId)
                     ?? throw new NotFoundException("Role", id);

        await _bll.SaveChangesAsync();
        return Ok(RoleApiMapper.ToDetailResponse(result));
    }

    /// <summary>
    /// Remove a permission from a role.
    /// </summary>
    [HttpDelete("{id:guid}/permissions/{permissionId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> RemovePermission(Guid tenantId, Guid departmentId, Guid id, Guid permissionId)
    {
        var removed = await _bll.RoleService.RemovePermissionAsync(id, permissionId);
        if (!removed)
            throw new NotFoundException("Role-Permission link not found");

        await _bll.SaveChangesAsync();
        return NoContent();
    }
}