using Dapper;
using DependencyStore.Models;
using DependencyStore.Repositories.Contracts;
using Microsoft.Data.SqlClient;

namespace DependencyStore.Repositories
{
    public class PromoCodeRepository : IPromoCodeRepository
    {
        private readonly SqlConnection _sqlConnection;

        public PromoCodeRepository(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public Task<PromoCode?> GetPromoCodeAsync(string promoCode)
        {
            var query = $"SELECT * FROM PROMO_CODES WHERE CODE={promoCode}";
            return _sqlConnection.QueryFirstOrDefaultAsync<PromoCode>(query);
        }
    }
}
