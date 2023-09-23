using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Clean.Architecture.Core.Entities.Identity;
using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Web.Api;
using Clean.Architecture.Web.Dtos;
using Clean.Architecture.Web.Errors;
using Clean.Architecture.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Architecture.Web.Controllers;

public class AccountController : BaseApiController
{
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
  public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, 
      ITokenService tokenService, IMapper mapper)
  {
            _mapper = mapper;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
  }

  [Authorize]
  [HttpGet]
  public async Task<ActionResult<UserDto>> GetCurrentUser()
  {
    var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
    
    if (user is null)
    {
      return Unauthorized(new ApiResponse(401));
    }

    return new UserDto
        {
            Email = user.Email,
            DisplayName = user.DisplayName,
            Token = _tokenService.CreateToken(user)
        };
  }  

  [HttpGet("emailexists")]
  public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
  {
    return await _userManager.FindByEmailAsync(email) != null;    
  }

  [Authorize]
  [HttpGet("address")]
  public async Task<ActionResult<AddressDto>> GetUserAddress()
  {
    var user = await _userManager.FindUserByClaimsPrincipleWithAddress(User);
    if (user.Address != null)
    {
      var addressDTO = _mapper.Map<Address, AddressDto>(user.Address);
      return addressDTO;
    }    
    else
    {
      // Handle the case where user.Address is null, e.g., by returning NotFound or another appropriate response.
      return NotFound();
    } 
  }
  [Authorize]
  [HttpPut("address")]
  public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
  {
    var user = await _userManager.FindUserByClaimsPrincipleWithAddress(User);
    user.Address = _mapper.Map<AddressDto, Address>(address);
    var result = await _userManager.UpdateAsync(user);
    if (result.Succeeded) return Ok(_mapper.Map<AddressDto>(user.Address));
    return BadRequest("Problem updating the user");
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
            DisplayName = user.DisplayName,
            Token = _tokenService.CreateToken(user)
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
      Token = _tokenService.CreateToken(user),
      Email = user.Email
    };
  }
}