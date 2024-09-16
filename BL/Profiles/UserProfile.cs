using AutoMapper;
using BL.DTO;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegistrationDto, User>()
               .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => DateTime.Now.Date))
               .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(x => DateTime.Now.Date))
               .ForMember(x => x.Id, opt => opt.MapFrom(x => Guid.NewGuid()));
        }
    }
}
