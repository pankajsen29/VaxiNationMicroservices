namespace VaccineInfo.Core.Models
{
    public record Vaccine
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
