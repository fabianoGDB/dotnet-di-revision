using Dapper;
using DependencyStore.Models;
using DependencyStore.Repositories.Contracts;
using Microsoft.Data.SqlClient;

namespace DependencyStore.Repositories
{
    public class CostumerRepository : ICostumerRepository
    {
        private readonly SqlConnection _sqlConnection;
        public CostumerRepository(SqlConnection sqlConnection) {
            _sqlConnection = sqlConnection;
        }
        public async Task<Customer?> GetByIdAsync(string customerId)
        {
                const string query = "SELECT [Id], [Name], [Email] FROM CUSTOMER WHERE ID=@id";
                return await _sqlConnection.QueryFirstOrDefaultAsync<Customer>(query, new { id = customerId });
           
        }
    }
}
