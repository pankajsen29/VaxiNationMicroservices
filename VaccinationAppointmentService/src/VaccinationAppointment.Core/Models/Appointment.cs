using VaccinationAppointment.Core.Enums;

namespace VaccinationAppointment.Core.Models
{
    public record Appointment
    {
        public long Sl_No { get; init; }
        public string Member_Photo_Id { get; init; }
        public DateTimeOffset Appointment_Date { get; init; }
        public DateTimeOffset Appointment_Time { get; init; }
        public AppointmentStatus Appointment_Status { get; init; }
        public Guid Vaccine_Id { get; init; }
        public short Vaccine_Dose_No { get; init; }
        public Guid Vaccine_Centre_Id { get; init; }
        public long Vaccine_Centre_Zipcode { get; init; }
        
    }
}
