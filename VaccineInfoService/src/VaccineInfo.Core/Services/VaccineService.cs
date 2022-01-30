using VaccineInfo.Core.Interfaces.Data;
using VaccineInfo.Core.Interfaces.Services;
using VaccineInfo.Core.Models;

namespace VaccineInfo.Core.Services
{
    public class VaccineService:IVaccineService
    {
        private readonly IVaccinesDataStore _vaccinesDataStore;
        public VaccineService(IVaccinesDataStore vaccinesDataStore)
        {
            this._vaccinesDataStore = vaccinesDataStore;
        }

        public async Task CreateVaccineAsync(Vaccine vaccine)
        {
            await _vaccinesDataStore.CreateVaccineAsync(vaccine);
        }
        public async Task<IEnumerable<Vaccine>> GetVaccinesAsync()
        {
            return await _vaccinesDataStore.GetVaccinesAsync();
        }
        public async Task<Vaccine> GetVaccineAsync(Guid id)
        {
            return await _vaccinesDataStore.GetVaccineAsync(id);
        }
        public async Task UpdateVaccineAsync(Vaccine vaccine)
        {
            await _vaccinesDataStore.UpdateVaccineAsync(vaccine);
        }
        public async Task DeleteVaccineAsync(Guid id)
        {
            await _vaccinesDataStore.DeleteVaccineAsync(id);
        }
    }
}
