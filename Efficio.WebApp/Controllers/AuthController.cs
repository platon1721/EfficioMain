using System.Security.Claims;
using Asp.Versioning;
using Efficio.BLL.Contracts.Exceptions;
using Efficio.DAL.EF;
using Efficio.Domain.Identity;
using Efficio.DTO;
using Efficio.DTO.Auth;
using Efficio.DTO.Auth.Login;
using Efficio.DTO.Auth.Logout;
using Efficio.DTO.Auth.RefreshToken;
using Efficio.DTO.Auth.Register;
using Efficio.DTO.Auth.UserInfo;
using Efficio.WebApp.Auth;
using Efficio.WebApp.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Efficio.WebApp.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly EfficioDbContext _context;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService,
        EfficioDbContext context,
        ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Register a new user account.
    /// </summary>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType<JwtResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<RestApiErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<RestApiErrorResponse>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<JwtResponse>> Register([FromBody] RegisterRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            throw new ConflictException("User", "email", request.Email);

        var user = new AppUser
        {
            Email = request.Email,
            UserName = request.Email,
            IsPlatformAdmin = false
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new ValidationException(
                result.Errors.ToDictionary(e => e.Code, e => new[] { e.Description }));
        }

        _logger.LogInformation("User {Email} registered successfully", request.Email);
        return Ok(await CreateTokenPairAsync(user));
    }

    /// <summary>
    /// Log in with email and password.
    /// </summary>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType<JwtResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<RestApiErrorResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<JwtResponse>> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            throw new UnauthorizedException("Invalid email or password");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
            throw new UnauthorizedException("Invalid email or password");

        await _tokenService.RevokeOldRefreshTokensAsync(user.Id);

        _logger.LogInformation("User {Email} logged in", request.Email);
        return Ok(await CreateTokenPairAsync(user));
    }

    /// <summary>
    /// Refresh an expired JWT using a valid refresh token.
    /// </summary>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType<JwtResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<RestApiErrorResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<JwtResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(request.Token);
        if (principal == null)
            throw new UnauthorizedException("Invalid token");

        var userId = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var storedToken = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .OrderByDescending(rt => rt.ExpiresAt)
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken ||
                                       rt.PreviousToken == request.RefreshToken);

        if (storedToken == null || !storedToken.MatchesToken(request.RefreshToken))
            throw new UnauthorizedException("Invalid refresh token");

        if (storedToken.IsExpired)
            throw new UnauthorizedException("Refresh token expired");

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new NotFoundException("User", userId);

        storedToken.Rotate();
        var (jwt, expiresAt) = _tokenService.GenerateJwt(user);
        await _context.SaveChangesAsync();

        return Ok(new JwtResponse
        {
            Token = jwt,
            RefreshToken = storedToken.Token,
            ExpiresAt = expiresAt
        });
    }

    /// <summary>
    /// Log out by revoking the refresh token.
    /// </summary>
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Logout([FromBody] LogoutRequest request)
    {
        var userId = User.GetUserId();

        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.UserId == userId &&
                                       rt.Token == request.RefreshToken &&
                                       !rt.IsRevoked);

        if (token != null)
        {
            token.Revoke();
            await _context.SaveChangesAsync();
        }

        return Ok(new { message = "Logged out successfully" });
    }

    /// <summary>
    /// Get info about the currently authenticated user.
    /// </summary>
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [ProducesResponseType<UserInfoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<RestApiErrorResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserInfoResponse>> UserInfo()
    {
        var user = await _userManager.FindByIdAsync(User.GetUserId().ToString());
        if (user == null)
            throw new NotFoundException("User", User.GetUserId());

        return Ok(new UserInfoResponse
        {
            Id = user.Id,
            Email = user.Email!,
            IsPlatformAdmin = user.IsPlatformAdmin
        });
    }

    // ==================== Private Helpers ====================

    private async Task<JwtResponse> CreateTokenPairAsync(AppUser user)
    {
        var (jwt, expiresAt) = _tokenService.GenerateJwt(user);
        var refreshToken = _tokenService.CreateRefreshToken(user.Id);
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return new JwtResponse
        {
            Token = jwt,
            RefreshToken = refreshToken.Token,
            ExpiresAt = expiresAt
        };
    }
}