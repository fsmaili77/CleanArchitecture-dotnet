using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Clean.Architecture.Core.Entities.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clean.Architecture.Web.Extensions;

  public static class UserManagerExtensions
  {
    public static async Task<AppUser> FindUserByClaimsPrincipleWithAddress(this UserManager<AppUser> userManager,
    ClaimsPrincipal user)
    {
        var email = user.FindFirstValue(ClaimTypes.Email);

        var appUser = await userManager.Users.Include(x => x.Address)
            .SingleOrDefaultAsync(x => x.Email == email);

        if (appUser == null)
        {
            throw new InvalidOperationException($"User with email '{email}' not found.");
        }

        return appUser;
    }

    public static async Task<AppUser?> FindByEmailFromClaimsPrincipal(this UserManager<AppUser> userManager, ClaimsPrincipal user)
    {
        var email = user.FindFirstValue(ClaimTypes.Email);
        if (email != null)
        {
            return await userManager.Users.SingleOrDefaultAsync(x => x.Email == email);
        }
        else
        {
            return null; // Handle the case where the email claim is not found or is null.
        }
    }
  }