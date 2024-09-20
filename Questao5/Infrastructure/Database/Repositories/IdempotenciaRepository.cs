using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Repository;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.Repositories
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly DatabaseConfig _databaseConfig;

        public IdempotenciaRepository(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async Task<Idempotencia> GetByKeyAsync(string chaveIdempotencia)
        {
            const string query = "SELECT * FROM idempotencia WHERE chave_idempotencia = @ChaveIdempotencia;";
            using var connection = new SqliteConnection(_databaseConfig.Name);
            await connection.OpenAsync();
            return await connection.QuerySingleOrDefaultAsync<Idempotencia>(query, new { ChaveIdempotencia = chaveIdempotencia });
        }

        public async Task AddAsync(Idempotencia idempotencia)
        {
            const string sql = @"INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) 
                             VALUES (@ChaveIdempotencia, @Requisicao, @Resultado)";
            using var connection = new SqliteConnection(_databaseConfig.Name);
            await connection.OpenAsync();
            await connection.ExecuteAsync(sql, idempotencia);
        }
    }
}
