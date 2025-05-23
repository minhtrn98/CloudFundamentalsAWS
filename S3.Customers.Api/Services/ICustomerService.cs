﻿using S3.Customers.Api.Domain;

namespace S3.Customers.Api.Services;

public interface ICustomerService
{
    Task<bool> CreateAsync(Customer customer);

    Task<Customer?> GetAsync(Guid id);

    Task<IEnumerable<Customer>> GetAllAsync();

    Task<bool> UpdateAsync(Customer customer, DateTime requestStarted);

    Task<bool> DeleteAsync(Guid id);
}
