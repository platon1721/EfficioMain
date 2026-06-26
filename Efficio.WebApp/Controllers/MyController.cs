using Asp.Versioning;
using Efficio.BLL.Contracts;
using Efficio.DTO.Mappers;
using Efficio.DTO.Tenants.UserTenantMembership;
using Efficio.WebApp.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Efficio.WebApp.Controllers;

/// <summary>
/// Endpoints for the currently authenticated user.
/// </summary>
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/my")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MyController : ControllerBase
{
    private readonly IEfficioBLL _bll;

    public MyController(IEfficioBLL bll)
    {
        _bll = bll;
    }

    /// <summary>
    /// Get current user's memberships across all tenants.
    /// </summary>
    [HttpGet("memberships")]
    [ProducesResponseType<IEnumerable<UserTenantMembershipResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserTenantMembershipResponse>>> GetMyMemberships()
    {
        var userId = User.GetUserId();
        var memberships = await _bll.UserTenantMembershipService.GetByUserIdAsync(userId);
        return Ok(memberships.Select(UserTenantMembershipApiMapper.ToResponse));
    }
}