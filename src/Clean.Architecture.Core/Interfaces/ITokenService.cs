using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clean.Architecture.Core.Entities.Identity;

namespace Clean.Architecture.Core.Interfaces;

  public interface ITokenService
  {
      string CreateToken(AppUser user);
  }