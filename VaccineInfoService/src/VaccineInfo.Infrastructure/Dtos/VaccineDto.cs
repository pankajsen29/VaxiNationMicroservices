namespace VaccineInfo.Infrastructure.Dtos
{
    /// <summary>
    /// when there is a change needed in the database document/column name, 
    /// we can easily do that and adapt only this "Infrastrucure" layer,
    /// we don't have to touch the "Core" projet layer for this
    /// </summary>
    public record VaccineDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public decimal MaxPrice { get; init; }        
        public ushort NumberOfDoses { get; init; }
        public ushort MinDaysBetweenDoses { get; init; }
        public string ManufacturerName { get; init; }
        public string ManufacturerWebsite { get; init; }
        public DateTimeOffset LocalApprovalDate { get; init; }
        public IEnumerable<string> ApprovedBy { get; init; }
        public DateTimeOffset CreatedDate { get; init; }
    }
}
