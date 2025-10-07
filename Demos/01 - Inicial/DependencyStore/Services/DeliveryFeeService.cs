using DependencyStore.Services.Contracts;
using RestSharp;

namespace DependencyStore.Services
{
    public class DeliveryFeeService : IDeliveryFeeService
    {
        public async Task<decimal> GetDeliveryFeeAsync(string zipCode)
        {
            var client = new RestSharp.RestClient("https://consultafrete.io/cep/");
            var request = new RestRequest()
            .AddJsonBody(new
            {
                zipCode
            });

            var response = await client.PostAsync<decimal>(request, new CancellationToken());
            return response < 5 ? 5 : response;
        }
    }
}
