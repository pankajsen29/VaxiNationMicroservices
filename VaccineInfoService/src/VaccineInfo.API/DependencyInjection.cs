using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using VaccineInfo.Core.Interfaces.Data;
using VaccineInfo.Core.Interfaces.Services;
using VaccineInfo.Core.Services;
using VaccineInfo.Infrastructure.Data;
using VaccineInfo.Infrastructure.Data.Config;

namespace VaccineInfo.API
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Extension method to inject all the dependencies in a separate place, 
        /// this helps when the dependencies grow or the application grows.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            //MongoDB serialization: it tells, anytime our entity/model contains a Guid/DateTimeOffset, it should actually get serialized as string in the DB
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            //Register MongoDB client
            var mongoDbsettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                return new MongoClient(mongoDbsettings.ConnectionString);
            });

            //Register datastore
            services.AddScoped<IVaccinesDataStore, MongoDbVaccinesDataStore>();
            services.AddScoped<IVaccineService, VaccineService>();
            return services;
        }
    }
}
