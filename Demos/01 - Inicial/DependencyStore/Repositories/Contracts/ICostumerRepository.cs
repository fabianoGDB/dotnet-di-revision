using DependencyStore.Models;

namespace DependencyStore.Repositories.Contracts
{
    public interface ICostumerRepository
    {
        Task <Customer?> GetByIdAsync(string customerId);
    }
}
