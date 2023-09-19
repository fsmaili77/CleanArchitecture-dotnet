using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clean.Architecture.Core.Entities.Identity;
using Clean.Architecture.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Clean.Architecture.Web.Extensions;

  public static class IdentityServiceExtensions
  {
      public static IServiceCollection AddIdentityServices(this IServiceCollection services,
            IConfiguration config)
      {
        services.AddDbContext<AppIdentityDbContext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("IdentityConnection"));
        }); 

        services.AddIdentityCore<AppUser>(opt =>
        {
            // add identity options here
        })
        .AddEntityFrameworkStores<AppIdentityDbContext>()
        .AddSignInManager<SignInManager<AppUser>>();

        services.AddAuthentication();
        services.AddAuthorization();
        
        return services;       
      }
  }