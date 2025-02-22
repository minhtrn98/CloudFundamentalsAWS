using DynamoDb.Customers.Api.Contracts.Data;

namespace DynamoDb.Customers.Api.Repositories;

public interface ICustomerRepository
{
    Task<bool> CreateAsync(CustomerDto customer);

    Task<CustomerDto?> GetAsync(Guid id);

    Task<IEnumerable<CustomerDto>> GetAllAsync();

    Task<bool> UpdateAsync(CustomerDto customer, DateTime requestStarted);

    Task<bool> DeleteAsync(Guid id);
}
