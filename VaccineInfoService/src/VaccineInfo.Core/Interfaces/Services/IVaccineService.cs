using VaccineInfo.Core.Models;

namespace VaccineInfo.Core.Interfaces.Services
{
    public interface IVaccineService
    {
        Task<IEnumerable<Vaccine>> GetVaccinesAsync();
        Task<Vaccine> GetVaccineAsync(Guid id);
        Task CreateVaccineAsync(Vaccine vaccine);
        Task UpdateVaccineAsync(Vaccine vaccine);
        Task DeleteVaccineAsync(Guid id);
    }
}