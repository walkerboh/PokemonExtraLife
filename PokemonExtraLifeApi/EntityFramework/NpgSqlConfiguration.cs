using System.Data.Entity;
using Npgsql;

namespace PokemonExtraLifeApi.EntityFramework
{
    public class NpgSqlConfiguration : DbConfiguration
    {
        public NpgSqlConfiguration()
        {
            SetProviderFactory("Npgsql", NpgsqlFactory.Instance);
            
            SetProviderServices("Npgsql", NpgsqlServices.Instance);
            
            SetDefaultConnectionFactory(new NpgsqlConnectionFactory());
        }
    }
}