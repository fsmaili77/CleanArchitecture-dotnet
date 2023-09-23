using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Infrastructure.Services;
using Clean.Architecture.Web.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Clean.Architecture.Web.Extensions;

  public static class ApplicationServicesExtensions
  {
      public static IServiceCollection AddApplicationServices(this IServiceCollection services, 
        IConfiguration config)
      {
        services.AddScoped<ITokenService, TokenService>();
        services.AddAutoMapper(typeof(MappingProfiles));

        return services;
      }
      
  }
  