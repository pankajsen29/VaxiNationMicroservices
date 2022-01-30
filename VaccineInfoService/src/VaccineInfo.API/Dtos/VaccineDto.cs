namespace VaccineInfo.Api.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public record VaccineDto
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public decimal MaxPrice { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public ushort NumberOfDoses { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public ushort MinDaysBetweenDoses { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public string ManufacturerName { get; init; }
        /// <summary>
        /// 
        /// </summary>
        public string ManufacturerWebsite { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset LocalApprovalDate { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> ApprovedBy { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset CreatedDate { get; init; }
    }
}
