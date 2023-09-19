using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clean.Architecture.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Clean.Architecture.Infrastructure.Identity;

public class AppIdentityDbContext : IdentityDbContext<AppUser>
{
  public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
  {
  }

// If we don't use this method, we will have issues with identity and primary key for the ID used by AppUser field
  protected override void OnModelCreating(ModelBuilder builder)
  {
    builder.Entity<Address>()
        .HasKey(a => a.Identity);
        
    base.OnModelCreating(builder);
  }
}