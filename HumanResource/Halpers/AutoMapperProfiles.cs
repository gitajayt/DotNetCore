using AutoMapper;
using HumanResource.DTOS;
using HumanResource.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResource.Halpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                 .ForMember(dest => dest.Age, opt =>
                 {
                     opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
                 });


            CreateMap<User, UserForDetailedDto>()
             .ForMember(dest => dest.PhotoUrl, opt =>
              {
                  opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
              })
             .ForMember(dest => dest.Age, opt =>
              {
                  opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
              });
            CreateMap<Photo, PhotosForDetailedDto>();
        }
    }
}
