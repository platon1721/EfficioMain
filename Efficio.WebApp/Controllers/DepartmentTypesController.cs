using Asp.Versioning;
using Efficio.BLL.Contracts;
using Efficio.BLL.Contracts.Exceptions;
using Efficio.DTO.Mappers;
using Efficio.DTO.Departments.DepartmentType;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Efficio.WebApp.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/tenants/{tenantId:guid}/department-types")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DepartmentTypesController : ControllerBase
{
    private readonly IEfficioBLL _bll;

    public DepartmentTypesController(IEfficioBLL bll)
    {
        _bll = bll;
    }

    [HttpGet]
    [ProducesResponseType<IEnumerable<DepartmentTypeResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DepartmentTypeResponse>>> GetAll(Guid tenantId)
    {
        var tenant = await _bll.TenantService.FindAsync(tenantId)
                     ?? throw new NotFoundException("Tenant", tenantId);

        var types = await _bll.DepartmentTypeService.GetByTenantAsync(tenant.RootDepartmentId);
        return Ok(types.Select(DepartmentTypeApiMapper.ToResponse));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<DepartmentTypeResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<DepartmentTypeResponse>> Get(Guid tenantId, Guid id)
    {
        var entity = await _bll.DepartmentTypeService.FindAsync(id)
                     ?? throw new NotFoundException("DepartmentType", id);

        return Ok(DepartmentTypeApiMapper.ToResponse(entity));
    }

    [HttpPost]
    [ProducesResponseType<DepartmentTypeResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<DepartmentTypeResponse>> Create(Guid tenantId, [FromBody] CreateDepartmentTypeRequest request)
    {
        var tenant = await _bll.TenantService.FindAsync(tenantId)
                     ?? throw new NotFoundException("Tenant", tenantId);

        var nameExists = await _bll.DepartmentTypeService.NameExistsAsync(request.Name.Trim());
        if (nameExists)
            throw new ConflictException("DepartmentType", "name", request.Name);

        var bllEntity = DepartmentTypeApiMapper.ToBll(request, tenant.RootDepartmentId);
        _bll.DepartmentTypeService.Add(bllEntity);
        await _bll.SaveChangesAsync();

        var created = await _bll.DepartmentTypeService.FindAsync(bllEntity.Id);
        return CreatedAtAction(nameof(Get), new { tenantId, id = bllEntity.Id },
            DepartmentTypeApiMapper.ToResponse(created!));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType<DepartmentTypeResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<DepartmentTypeResponse>> Update(Guid tenantId, Guid id, [FromBody] UpdateDepartmentTypeRequest request)
    {
        var existing = await _bll.DepartmentTypeService.FindAsync(id)
                       ?? throw new NotFoundException("DepartmentType", id);

        var nameExists = await _bll.DepartmentTypeService.NameExistsAsync(request.Name.Trim(), id);
        if (nameExists)
            throw new ConflictException("DepartmentType", "name", request.Name);

        existing.Name = request.Name.Trim();
        existing.Description = request.Description?.Trim();

        _bll.DepartmentTypeService.Update(existing);
        await _bll.SaveChangesAsync();

        var updated = await _bll.DepartmentTypeService.FindAsync(id);
        return Ok(DepartmentTypeApiMapper.ToResponse(updated!));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(Guid tenantId, Guid id)
    {
        if (!await _bll.DepartmentTypeService.ExistsAsync(id))
            throw new NotFoundException("DepartmentType", id);

        await _bll.DepartmentTypeService.RemoveAsync(id);
        await _bll.SaveChangesAsync();
        return NoContent();
    }
}