using api.app.account.dto;
using api.app.token.service;
using api.app.user.entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.app.account.controller
{
  [Route("api/account")]
  [ApiController]
  public class AccountController : ControllerBase
  {
    // 유저 생성 / 로그인 매니저 제공
    private readonly UserManager<AppUser> userManager;
    private readonly SignInManager<AppUser> signInManager;
    private readonly ITokenService tokenService;

    public AccountController(UserManager<AppUser> userNanager, ITokenService tokenService, SignInManager<AppUser> signInManager)
    {
      this.userManager = userNanager;
      this.tokenService = tokenService;
      this.signInManager = signInManager;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
      try
      {
        if (!ModelState.IsValid)
          return BadRequest(ModelState);

        var appUser = new AppUser
        {
          UserName = registerDto.Username,
          Email = registerDto.Email,
        };

        var createUserResult = await userManager.CreateAsync(appUser, registerDto.Password);

        // 유저 생성 실패
        if (!createUserResult.Succeeded) return StatusCode(500, createUserResult.Errors);

        var roleResult = await userManager.AddToRoleAsync(appUser, "User");

        if (roleResult.Succeeded)
          return Ok(new UserDto
          {
            Username = appUser.UserName,
            Email = appUser.Email,
            Token = tokenService.CreateToken(appUser)
          });
        else
          return StatusCode(500, roleResult.Errors);
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] SigninDto dto) {
      if(!ModelState.IsValid) return BadRequest(ModelState);

      var user = await userManager.Users.FirstOrDefaultAsync(it => it.UserName == dto.Username);
      if(user == null) return Unauthorized("some user info are invalid");

      // lockout failure = 로그인 N회 실패 시 잠금
      var validate = await signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
      if(!validate.Succeeded) return Unauthorized("some user info are invalid");

      return Ok(new UserDto {
        Username = user.UserName,
        Email = user.Email,
        Token = tokenService.CreateToken(user)
      });
    }
  }
}