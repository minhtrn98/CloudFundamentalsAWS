﻿using Dapper;
using Sns.Customers.Api.Contracts.Data;
using Sns.Customers.Api.Database;

namespace Sns.Customers.Api.Repositories;

public class CustomerRepository(IDbConnectionFactory connectionFactory) : ICustomerRepository
{
    public async Task<bool> CreateAsync(CustomerDto customer)
    {
        using System.Data.IDbConnection connection = await connectionFactory.CreateConnectionAsync();
        int result = await connection.ExecuteAsync(
            @"INSERT INTO Customers (Id, GitHubUsername, FullName, Email, DateOfBirth) 
            VALUES (@Id, @GitHubUsername, @FullName, @Email, @DateOfBirth)",
            customer);
        return result > 0;
    }

    public async Task<CustomerDto?> GetAsync(Guid id)
    {
        using System.Data.IDbConnection connection = await connectionFactory.CreateConnectionAsync();
        return await connection.QuerySingleOrDefaultAsync<CustomerDto>(
            "SELECT * FROM Customers WHERE Id = @Id LIMIT 1", new { Id = id });
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        using System.Data.IDbConnection connection = await connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<CustomerDto>("SELECT * FROM Customers");
    }

    public async Task<bool> UpdateAsync(CustomerDto customer)
    {
        using System.Data.IDbConnection connection = await connectionFactory.CreateConnectionAsync();
        int result = await connection.ExecuteAsync(
            @"UPDATE Customers SET GitHubUsername = @GitHubUsername, FullName = @FullName, Email = @Email, 
                 DateOfBirth = @DateOfBirth WHERE Id = @Id",
            customer);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using System.Data.IDbConnection connection = await connectionFactory.CreateConnectionAsync();
        int result = await connection.ExecuteAsync(@"DELETE FROM Customers WHERE Id = @Id",
            new { Id = id });
        return result > 0;
    }
}
