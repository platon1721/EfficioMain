using Asp.Versioning;
using Efficio.BLL.Contracts;
using Efficio.BLL.Contracts.Exceptions;
using Efficio.BLL.DTO.Departments;
using Efficio.DTO.Mappers;
using Efficio.DTO.Departments.DepartmentHierarchy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Efficio.WebApp.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/tenants/{tenantId:guid}/department-links")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DepartmentHierarchyController : ControllerBase
{
    private readonly IEfficioBLL _bll;

    public DepartmentHierarchyController(IEfficioBLL bll)
    {
        _bll = bll;
    }

    /// <summary>
    /// Get all department links for a tenant.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<IEnumerable<DepartmentLinkResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DepartmentLinkResponse>>> GetAll(Guid tenantId)
    {
        var tenant = await _bll.TenantService.FindAsync(tenantId)
                     ?? throw new NotFoundException("Tenant", tenantId);

        var links = await _bll.DepartmentInDepartmentService.GetByTenantAsync(tenant.RootDepartmentId);
        return Ok(links.Select(DepartmentLinkApiMapper.ToResponse));
    }

    /// <summary>
    /// Get children of a department.
    /// </summary>
    [HttpGet("children/{parentId:guid}")]
    [ProducesResponseType<IEnumerable<DepartmentLinkResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DepartmentLinkResponse>>> GetChildren(Guid tenantId, Guid parentId)
    {
        var links = await _bll.DepartmentInDepartmentService.GetChildrenAsync(parentId);
        return Ok(links.Select(DepartmentLinkApiMapper.ToResponse));
    }

    /// <summary>
    /// Get parents of a department.
    /// </summary>
    [HttpGet("parents/{childId:guid}")]
    [ProducesResponseType<IEnumerable<DepartmentLinkResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DepartmentLinkResponse>>> GetParents(Guid tenantId, Guid childId)
    {
        var links = await _bll.DepartmentInDepartmentService.GetParentsAsync(childId);
        return Ok(links.Select(DepartmentLinkApiMapper.ToResponse));
    }

    /// <summary>
    /// Create a parent-child link between departments.
    /// </summary>
    [HttpPost]
    [ProducesResponseType<DepartmentLinkResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<DepartmentLinkResponse>> Create(Guid tenantId, [FromBody] CreateDepartmentLinkRequest request)
    {
        var tenant = await _bll.TenantService.FindAsync(tenantId)
                     ?? throw new NotFoundException("Tenant", tenantId);

        if (request.ParentDepartmentId == request.ChildDepartmentId)
            throw new ValidationException("Department cannot be its own parent");

        var linkExists = await _bll.DepartmentInDepartmentService
            .LinkExistsAsync(request.ParentDepartmentId, request.ChildDepartmentId);
        if (linkExists)
            throw new ConflictException("This department link already exists");

        var entity = new DepartmentInDepartment
        {
            Id = Guid.NewGuid(),
            TenantRootDepartmentId = tenant.RootDepartmentId,
            ParentDepartmentId = request.ParentDepartmentId,
            ChildDepartmentId = request.ChildDepartmentId
        };

        _bll.DepartmentInDepartmentService.Add(entity);
        await _bll.SaveChangesAsync();

        var created = await _bll.DepartmentInDepartmentService
            .FindLinkAsync(request.ParentDepartmentId, request.ChildDepartmentId);
        return CreatedAtAction(nameof(GetAll), new { tenantId },
            DepartmentLinkApiMapper.ToResponse(created!));
    }

    /// <summary>
    /// Remove a department link.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(Guid tenantId, Guid id)
    {
        if (!await _bll.DepartmentInDepartmentService.ExistsAsync(id))
            throw new NotFoundException("DepartmentLink", id);

        await _bll.DepartmentInDepartmentService.RemoveAsync(id);
        await _bll.SaveChangesAsync();
        return NoContent();
    }
}