using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using VaccineInfo.Core.Interfaces.Data;
using VaccineInfo.Core.Models;
using VaccineInfo.Infrastructure.Dtos;

namespace VaccineInfo.Infrastructure.Data
{
    public class MongoDbVaccinesDataStore : IVaccinesDataStore
    {
        private const string _databaseName = "VaxiNation";
        private const string _collectionName = "vaccine_info";
        private readonly IMapper _mapper;

        private readonly IMongoCollection<VaccineDto> _vaccinesCollection;
        private readonly FilterDefinitionBuilder<VaccineDto> _filterDefinitionBuilder = Builders<VaccineDto>.Filter;

        public MongoDbVaccinesDataStore(IMongoClient mongoClient, IMapper mapper)
        {
            IMongoDatabase database = mongoClient.GetDatabase(_databaseName);
            _vaccinesCollection = database.GetCollection<VaccineDto>(_collectionName);
            this._mapper = mapper;
        }
        public async Task CreateVaccineAsync(Vaccine vaccine)
        {
            await _vaccinesCollection.InsertOneAsync(_mapper.Map<VaccineDto>(vaccine));
        }
        public async Task<IEnumerable<Vaccine>> GetVaccinesAsync()
        {
            return _mapper.Map<List<VaccineDto>, IEnumerable<Vaccine>>(await _vaccinesCollection.Find(new BsonDocument()).ToListAsync());
        }
        public async Task<Vaccine> GetVaccineAsync(Guid id)
        {
            var filter = _filterDefinitionBuilder.Eq(item => item.Id, id);
            return _mapper.Map<VaccineDto,Vaccine>(await _vaccinesCollection.Find(filter).FirstOrDefaultAsync());
        }
        public async Task UpdateVaccineAsync(Vaccine vaccine)
        {
            var filter = _filterDefinitionBuilder.Eq(existingItem => existingItem.Id, vaccine.Id);
            await _vaccinesCollection.ReplaceOneAsync(filter, _mapper.Map<VaccineDto>(vaccine));
        }
        public async Task DeleteVaccineAsync(Guid id)
        {
            var filter = _filterDefinitionBuilder.Eq(item => item.Id, id);
            await _vaccinesCollection.DeleteOneAsync(filter);
        }
    }
}
