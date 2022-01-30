using AutoMapper;

namespace VaccineInfo.Api.Mappings
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// class to maintain all mappings
        /// </summary>
        public MappingProfile()
        {
            //source:VaccineInfo.Core.Models.Vaccine, Destination:VaccineInfo.Infrastructure.Dtos.VaccineDto
            CreateMap<VaccineInfo.Core.Models.Vaccine, VaccineInfo.Infrastructure.Dtos.VaccineDto>().ReverseMap();

            CreateMap<VaccineInfo.Core.Models.Vaccine, VaccineInfo.Api.Dtos.PatchVaccineDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)).ReverseMap();        
        }
    }
}
