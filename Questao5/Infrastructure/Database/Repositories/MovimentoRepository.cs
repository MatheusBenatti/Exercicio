using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Repository;
using Questao5.Infrastructure.Sqlite;
using System.Data.Common;

namespace Questao5.Infrastructure.Database.Repositories
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly DatabaseConfig _databaseConfig;

        public MovimentoRepository(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }
        public async Task<decimal> GetSomaCreditosAsync(string idContaCorrente)
        {
            const string query = @"
                SELECT SUM(valor) 
                FROM movimento 
                WHERE idcontacorrente = @IdContaCorrente 
                AND tipomovimento = 'C';";

            using var connection = new SqliteConnection(_databaseConfig.Name);
            await connection.OpenAsync();

            var resultado = await connection.QuerySingleOrDefaultAsync<decimal?>(query, new { IdContaCorrente = idContaCorrente });
            return resultado ?? 0;
        }

        public async Task<decimal> GetSomaDebitosAsync(string idContaCorrente)
        {
            const string query = @"
                SELECT SUM(valor) 
                FROM movimento 
                WHERE idcontacorrente = @IdContaCorrente 
                AND tipomovimento = 'D';";

            using var connection = new SqliteConnection(_databaseConfig.Name);
            await connection.OpenAsync();

            var resultado = await connection.QuerySingleOrDefaultAsync<decimal?>(query, new { IdContaCorrente = idContaCorrente });
            return resultado ?? 0;
        }

        public async Task AddAsync(Movimento movimento)
        {
            const string sql = @"INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) 
                                 VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";

            using var connection = new SqliteConnection(_databaseConfig.Name);
            await connection.OpenAsync();
            await connection.ExecuteAsync(sql, movimento);
        }
    }
}
