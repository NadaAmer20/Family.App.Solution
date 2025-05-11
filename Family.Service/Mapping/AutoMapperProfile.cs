using AutoMapper;
using Family.Core.Entities;
using Family.Service.DTOS.Family.Service.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family.Service.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Clan mappings
            CreateMap<Clan, ClanReturnDto>()
                .ForMember(dest => dest.Branches, opt => opt.MapFrom(src => src.Branches));
                //.ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Members));

            CreateMap<ClanCreateDto, Clan>();
            CreateMap<ClanUpdateDto, Clan>();

            // Branch mappings
            CreateMap<Branch, BranchReturnDto>()
                .ForMember(dest => dest.Clan, opt => opt.MapFrom(src => src.Clan));
                //.ForMember(dest => dest.Persons, opt => opt.MapFrom(src => src.Persons));

            CreateMap<BranchCreateDto, Branch>();
            CreateMap<BranchUpdateDto, Branch>();

            // Person mappings
            CreateMap<Person, PersonReturnDto>()
                .ForMember(dest => dest.Clan, opt => opt.MapFrom(src => src.Clan))
                .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch));

            CreateMap<PersonCreateDto, Person>();
            CreateMap<PersonUpdateDto, Person>();

            // Assignment DTOs
            CreateMap<ClanAssignmentDto, Person>();
            CreateMap<BranchAssignmentDto, Person>();
        }
    }
}
