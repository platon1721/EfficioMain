using Asp.Versioning;
using Efficio.BLL.Contracts;
using Efficio.BLL.Contracts.Exceptions;
using Efficio.DTO.Mappers;
using Efficio.DTO.Tenants.UserTenantMembership;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Efficio.WebApp.Controllers;

/// <summary>
/// Manage user memberships within a tenant.
/// </summary>
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/tenants/{tenantId:guid}/members")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserTenantMembershipsController : ControllerBase
{
    private readonly IEfficioBLL _bll;

    public UserTenantMembershipsController(IEfficioBLL bll)
    {
        _bll = bll;
    }

    /// <summary>
    /// Get all members of a tenant.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<IEnumerable<UserTenantMembershipResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserTenantMembershipResponse>>> GetAll(Guid tenantId)
    {
        var tenant = await _bll.TenantService.FindAsync(tenantId)
                     ?? throw new NotFoundException("Tenant", tenantId);

        var members = await _bll.UserTenantMembershipService.GetByTenantAsync(tenant.RootDepartmentId);
        return Ok(members.Select(UserTenantMembershipApiMapper.ToResponse));
    }

    /// <summary>
    /// Get active members of a tenant.
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType<IEnumerable<UserTenantMembershipResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserTenantMembershipResponse>>> GetActive(Guid tenantId)
    {
        var tenant = await _bll.TenantService.FindAsync(tenantId)
                     ?? throw new NotFoundException("Tenant", tenantId);

        var members = await _bll.UserTenantMembershipService.GetActiveUsersByTenantAsync(tenant.RootDepartmentId);
        return Ok(members.Select(UserTenantMembershipApiMapper.ToResponse));
    }

    /// <summary>
    /// Invite a user to the tenant.
    /// </summary>
    [HttpPost("invite")]
    [ProducesResponseType<UserTenantMembershipResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<UserTenantMembershipResponse>> Invite(Guid tenantId, [FromBody] InviteUserRequest request)
    {
        var tenant = await _bll.TenantService.FindAsync(tenantId)
                     ?? throw new NotFoundException("Tenant", tenantId);

        var result = await _bll.UserTenantMembershipService
            .InviteUserAsync(request.UserId, tenant.RootDepartmentId);

        if (result == null)
            throw new ValidationException("Could not invite user");

        await _bll.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { tenantId },
            UserTenantMembershipApiMapper.ToResponse(result));
    }

    /// <summary>
    /// Activate a user's membership.
    /// </summary>
    [HttpPost("{userId:guid}/activate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Activate(Guid tenantId, Guid userId)
    {
        var tenant = await _bll.TenantService.FindAsync(tenantId)
                     ?? throw new NotFoundException("Tenant", tenantId);

        var success = await _bll.UserTenantMembershipService
            .ActivateMembershipAsync(userId, tenant.RootDepartmentId);
        if (!success)
            throw new NotFoundException("Membership", userId);

        await _bll.SaveChangesAsync();
        return Ok(new { message = "Membership activated" });
    }

    /// <summary>
    /// Suspend a user's membership.
    /// </summary>
    [HttpPost("{userId:guid}/suspend")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Suspend(Guid tenantId, Guid userId)
    {
        var tenant = await _bll.TenantService.FindAsync(tenantId)
                     ?? throw new NotFoundException("Tenant", tenantId);

        var success = await _bll.UserTenantMembershipService
            .SuspendMembershipAsync(userId, tenant.RootDepartmentId);
        if (!success)
            throw new NotFoundException("Membership", userId);

        await _bll.SaveChangesAsync();
        return Ok(new { message = "Membership suspended" });
    }

    /// <summary>
    /// Remove a user from the tenant.
    /// </summary>
    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Remove(Guid tenantId, Guid userId)
    {
        var tenant = await _bll.TenantService.FindAsync(tenantId)
                     ?? throw new NotFoundException("Tenant", tenantId);

        var success = await _bll.UserTenantMembershipService
            .RemoveMembershipAsync(userId, tenant.RootDepartmentId);
        if (!success)
            throw new NotFoundException("Membership", userId);

        await _bll.SaveChangesAsync();
        return NoContent();
    }
}