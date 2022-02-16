using System.ComponentModel.DataAnnotations;

namespace VaccineInfo.Api.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class PatchVaccineDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// 
        /// </summary>
        [Range(1, 1000)]
        public decimal MaxPrice { get; init; }

        /// <summary>
        /// 
        /// </summary>
        [Range(1, 5)]
        public ushort NumberOfDoses { get; init; }

        /// <summary>
        /// 
        /// </summary>
        [Range(0, 365)]
        public ushort MinDaysBetweenDoses { get; init; }

        /// <summary>
        /// 
        /// </summary>
        [Url]
        public string ManufacturerWebsite { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> ApprovedBy { get; init; }
    }
}
