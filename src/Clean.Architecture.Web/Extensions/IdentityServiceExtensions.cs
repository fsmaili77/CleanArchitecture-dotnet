using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clean.Architecture.Core.Entities.Identity;
using Clean.Architecture.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options =>
          {
            var tokenKey = config["Token:Key"] ?? "DefaultTokenKey";
            options.TokenValidationParameters = new TokenValidationParameters
            {
              ValidateIssuerSigningKey = true,
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
              ValidIssuer = config["Token:Issuer"],
              ValidateIssuer = true,
              ValidateAudience = false
            };
          });
          
        services.AddAuthorization();
        
        return services;       
      }
  }