using Asp.Versioning;
using Efficio.BLL.Contracts;
using Efficio.BLL.Contracts.Exceptions;
using Efficio.DTO.Mappers;
using Efficio.DTO.Departments.Department;
using Efficio.WebApp.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Efficio.WebApp.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/tenants/{tenantId:guid}/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DepartmentsController : ControllerBase
{
    private readonly IEfficioBLL _bll;

    public DepartmentsController(IEfficioBLL bll)
    {
        _bll = bll;
    }

    /// <summary>
    /// Get all departments for a tenant.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<IEnumerable<DepartmentResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DepartmentResponse>>> GetAll(Guid tenantId)
    {
        var tenant = await _bll.TenantService.FindAsync(tenantId)
                     ?? throw new NotFoundException("Tenant", tenantId);

        var departments = await _bll.DepartmentService.GetByTenantAsync(tenant.RootDepartmentId);
        return Ok(departments.Select(DepartmentApiMapper.ToResponse));
    }

    /// <summary>
    /// Get department by id.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<DepartmentResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<DepartmentResponse>> Get(Guid tenantId, Guid id)
    {
        var entity = await _bll.DepartmentService.FindWithTypeAsync(id)
                     ?? throw new NotFoundException("Department", id);

        return Ok(DepartmentApiMapper.ToResponse(entity));
    }

    /// <summary>
    /// Get departments by type.
    /// </summary>
    [HttpGet("by-type/{departmentTypeId:guid}")]
    [ProducesResponseType<IEnumerable<DepartmentResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DepartmentResponse>>> GetByType(Guid tenantId, Guid departmentTypeId)
    {
        var departments = await _bll.DepartmentService.GetByTypeAsync(departmentTypeId);
        return Ok(departments.Select(DepartmentApiMapper.ToResponse));
    }

    /// <summary>
    /// Get root departments (no parent).
    /// </summary>
    [HttpGet("roots")]
    [ProducesResponseType<IEnumerable<DepartmentResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DepartmentResponse>>> GetRoots(Guid tenantId)
    {
        var tenant = await _bll.TenantService.FindAsync(tenantId)
                     ?? throw new NotFoundException("Tenant", tenantId);

        var departments = await _bll.DepartmentService.GetRootDepartmentsAsync(tenant.RootDepartmentId);
        return Ok(departments.Select(DepartmentApiMapper.ToResponse));
    }

    /// <summary>
    /// Create a new department.
    /// </summary>
    [HttpPost]
    [ProducesResponseType<DepartmentResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<DepartmentResponse>> Create(Guid tenantId, [FromBody] CreateDepartmentRequest request)
    {
        var tenant = await _bll.TenantService.FindAsync(tenantId)
                     ?? throw new NotFoundException("Tenant", tenantId);

        var bllEntity = DepartmentApiMapper.ToBll(request, tenant.RootDepartmentId);
        _bll.DepartmentService.Add(bllEntity);
        await _bll.SaveChangesAsync();

        var created = await _bll.DepartmentService.FindWithTypeAsync(bllEntity.Id);
        return CreatedAtAction(nameof(Get), new { tenantId, id = bllEntity.Id },
            DepartmentApiMapper.ToResponse(created!));
    }

    /// <summary>
    /// Update a department.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType<DepartmentResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<DepartmentResponse>> Update(Guid tenantId, Guid id, [FromBody] UpdateDepartmentRequest request)
    {
        var existing = await _bll.DepartmentService.FindAsync(id)
                       ?? throw new NotFoundException("Department", id);

        existing.Name = request.Name.Trim();
        existing.Description = request.Description?.Trim();
        if (request.DepartmentTypeId.HasValue) existing.DepartmentTypeId = request.DepartmentTypeId.Value;

        _bll.DepartmentService.Update(existing);
        await _bll.SaveChangesAsync();

        var updated = await _bll.DepartmentService.FindWithTypeAsync(id);
        return Ok(DepartmentApiMapper.ToResponse(updated!));
    }

    /// <summary>
    /// Delete a department.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(Guid tenantId, Guid id)
    {
        if (!await _bll.DepartmentService.ExistsAsync(id))
            throw new NotFoundException("Department", id);

        await _bll.DepartmentService.RemoveAsync(id);
        await _bll.SaveChangesAsync();
        return NoContent();
    }
}