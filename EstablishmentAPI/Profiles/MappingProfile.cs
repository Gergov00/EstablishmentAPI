// Profiles/MappingProfile.cs
using AutoMapper;
using EstablishmentAPI.DTOs;
using EstablishmentAPI.Models;
using System.Linq;

namespace EstablishmentAPI.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Маппинг для Category
            CreateMap<Category, CategoryDTO>();
            CreateMap<CreateCategoryDTO, Category>();

            // Маппинг для Tag
            CreateMap<Tag, TagDTO>();
            CreateMap<TagDTO, Tag>();

            // Маппинг для Establishment → EstablishmentDTO
            CreateMap<Establishment, EstablishmentDTO>()
                .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src => src.EstablishmentTags.Select(et => et.TagId).ToList()));

            // Маппинг для CreateEstablishmentDTO → Establishment с игнорированием EstablishmentTags
            CreateMap<CreateEstablishmentDTO, Establishment>()
                .ForMember(dest => dest.EstablishmentTags, opt => opt.Ignore());
        }
    }
}
