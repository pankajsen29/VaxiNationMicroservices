using VaccineInfo.Api.Dtos;
using VaccineInfo.Core.Models;

namespace VaccineInfo.Api.Extensions
{
    public static class Extension
    {
        public static VaccineDto AsDto(this Vaccine vaccine)
        {
            return new VaccineDto
            {
                Id = vaccine.Id,
                Name = vaccine.Name,
                MaxPrice = vaccine.MaxPrice,
                NumberOfDoses = vaccine.NumberOfDoses,
                MinDaysBetweenDoses = vaccine.MinDaysBetweenDoses,
                ManufacturerName = vaccine.ManufacturerName,
                ManufacturerWebsite = vaccine.ManufacturerWebsite,
                LocalApprovalDate = vaccine.LocalApprovalDate,
                ApprovedBy = vaccine.ApprovedBy,
                CreatedDate = vaccine.CreatedDate
            };
        }
    }
}
