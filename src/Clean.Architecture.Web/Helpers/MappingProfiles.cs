using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Clean.Architecture.Web.Dtos;
using Clean.Architecture.Core.Entities.OrderAggregate;
using Clean.Architecture.Core.Entities.Identity;

namespace Clean.Architecture.Web.Helpers;

public class MappingProfiles : Profile
{
  public MappingProfiles()
  {
    CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
    CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();
  }
}