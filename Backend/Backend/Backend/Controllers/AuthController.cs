using BookLibrary.Auth.Model;
using BookLibrary.DTOs.Auth;
using BookLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, "User");

            var token = await _tokenService.CreateTokenAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new TokenResponseDto
            {
                Token = token,
                Email = user.Email ?? "",
                Roles = roles.ToList()
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid credentials");
            }

            var token = await _tokenService.CreateTokenAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new TokenResponseDto
            {
                Token = token,
                Email = user.Email ?? "",
                Roles = roles.ToList()
            });
        }

        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto registerDto)
        {
            // This endpoint should be secured in production
            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, "Admin");

            var token = await _tokenService.CreateTokenAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new TokenResponseDto
            {
                Token = token,
                Email = user.Email ?? "",
                Roles = roles.ToList()
            });
        }
    }
}