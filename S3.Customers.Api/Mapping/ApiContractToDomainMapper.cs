﻿using S3.Customers.Api.Contracts.Requests;
using S3.Customers.Api.Domain;

namespace S3.Customers.Api.Mapping;

public static class ApiContractToDomainMapper
{
    public static Customer ToCustomer(this CustomerRequest request)
    {
        return new Customer
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            GitHubUsername = request.GitHubUsername,
            FullName = request.FullName,
            DateOfBirth = request.DateOfBirth
        };
    }

    public static Customer ToCustomer(this UpdateCustomerRequest request)
    {
        return new Customer
        {
            Id = request.Id,
            Email = request.Customer.Email,
            GitHubUsername = request.Customer.GitHubUsername,
            FullName = request.Customer.FullName,
            DateOfBirth = request.Customer.DateOfBirth
        };
    }
}
