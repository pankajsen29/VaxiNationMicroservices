using System.ComponentModel.DataAnnotations;

namespace VaccineInfo.Api.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateVaccineDto
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Name { get; init; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Range(1, 1000)]
        public decimal MaxPrice { get; init; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Range(1, 5)]
        public ushort NumberOfDoses { get; init; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Range(15, 365)]
        public ushort MinDaysBetweenDoses { get; init; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Url]
        public string ManufacturerWebsite { get; init; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public IEnumerable<string> ApprovedBy { get; init; }
    }
}
