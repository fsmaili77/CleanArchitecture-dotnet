using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Clean.Architecture.Core.Entities.Identity;

  public class AppUser : IdentityUser
  {
      public string? DisplayName { get; set; }
      public Address? Address { get; set; }
  }