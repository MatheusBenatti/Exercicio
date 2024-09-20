using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Repository;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.Repositories
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly DatabaseConfig _databaseConfig;

        public ContaCorrenteRepository(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async Task<ContaCorrente> GetByIdAsync(string idContaCorrente)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            await connection.OpenAsync();

            var sql = "SELECT * FROM contacorrente WHERE idcontacorrente = @IdContaCorrente";
            return await connection.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new { IdContaCorrente = idContaCorrente });
        }
    }
}
