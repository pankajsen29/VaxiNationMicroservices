using Microsoft.Extensions.Logging;
using VaccineInfo.Core.Interfaces.Data;
using VaccineInfo.Core.Interfaces.Services;
using VaccineInfo.Core.Models;

namespace VaccineInfo.Core.Services
{
    public class VaccineService:IVaccineService
    {
        private readonly IVaccinesDataStore _vaccinesDataStore;
        private readonly ILogger<VaccineService> _logger;

        public VaccineService(IVaccinesDataStore vaccinesDataStore, ILogger<VaccineService> logger)
        {
            this._vaccinesDataStore = vaccinesDataStore;
            this._logger = logger;
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
            //based on business logic any core exception (e.g. CoreNotFoundException/CoreValidationException) can be thrown here
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
