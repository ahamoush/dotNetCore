using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.DTOs.Auth;
using ProjectManagementSystem.Interfaces;

namespace ProjectManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // POST /api/auth/register
    // يسجل مستخدم جديد ويعطيه دور User فقط
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var (succeeded, data, errors) = await _authService.RegisterAsync(dto);

        if (!succeeded)
        {
            return BadRequest(new { message = "Registration failed.", errors });
        }

        return Ok(data);
    }

    // POST /api/auth/login
    // يرجع JWT + معلومات المستخدم
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var (succeeded, data, error) = await _authService.LoginAsync(dto);

        if (!succeeded)
        {
            // 401 Unauthorized لبيانات الدخول الخاطئة
            return Unauthorized(new { message = error });
        }

        return Ok(data);
    }

    // ملاحظة عن Logout:
    // JWT Stateless = السيرفر مش بيخزن التوكن.
    // Logout عادة = العميل يمسح التوكن من عنده.
    // التوكن يفضل صالح لحد ما ينتهي Expiration (إلا لو عملنا Blacklist لاحقاً).
    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok(new
        {
            message = "Logout successful on the client by deleting the token. " +
                      "This API does not revoke JWT tokens server-side in v1."
        });
    }
}
