using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Clean.Architecture.Infrastructure.Services;

  public static class KeyGenerator
  {
      public static byte[] GenerateRandomKey(int keySizeInBytes)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var key = new byte[keySizeInBytes];
                rng.GetBytes(key);
                return key;
            }
        }
  }