using System.ComponentModel.DataAnnotations;

namespace VaccineInfo.Api.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateVaccineDto
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
        [Range(0, 365)]
        public ushort MinDaysBetweenDoses { get; init; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string ManufacturerName { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        [Url]
        public string ManufacturerWebsite { get; init; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        public DateTimeOffset LocalApprovalDate { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public IEnumerable<string> ApprovedBy { get; init; }
    }
}
