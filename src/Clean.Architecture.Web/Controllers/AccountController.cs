using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clean.Architecture.Core.Entities.Identity;
using Clean.Architecture.Web.Api;
using Clean.Architecture.Web.Dtos;
using Clean.Architecture.Web.Errors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Architecture.Web.Controllers;

public class AccountController : BaseApiController
{
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
  public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
  {
            _signInManager = signInManager;
            _userManager = userManager;
  }

  [HttpPost("login")]
  public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
  {
      if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
      {
          return BadRequest(new ApiResponse(400));
      }

      var user = await _userManager.FindByEmailAsync(loginDto.Email);

      if (user == null)
      {
          return Unauthorized(new ApiResponse(401));
      }

      var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
      if (!result.Succeeded)
      {
          return Unauthorized(new ApiResponse(401));
      }

      return new UserDto
      {
          Email = user.Email,
          Token = "This will be a token",
          DisplayName = user.DisplayName
      };
  }

  [HttpPost("register")]
  public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
  {
    if (registerDto == null || string.IsNullOrEmpty(registerDto.Password))
    {
      return BadRequest(new ApiResponse(400));
    }
    var user = new AppUser
    {
      DisplayName = registerDto.DisplayName,
      Email = registerDto.Email,
      UserName = registerDto.Email
    };
    var result =  await   _userManager.CreateAsync(user, registerDto.Password);
    if(!result.Succeeded)
    {
      return BadRequest(new ApiResponse(400));      
    }
    return new UserDto
    {
      DisplayName = user.DisplayName,
      Token = "This will be token",
      Email = user.Email
    };
  }
}