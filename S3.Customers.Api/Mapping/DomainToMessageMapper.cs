namespace S3.Customers.Api.Mapping;

public static class DomainToMessageMapper
{
    public static Contracts.Messages.CustomerCreated ToCustomerCreatedMessage(this Domain.Customer customer)
    {
        return new Contracts.Messages.CustomerCreated
        {
            Id = customer.Id,
            GitHubUsername = customer.GitHubUsername,
            FullName = customer.FullName,
            Email = customer.Email,
            DateOfBirth = customer.DateOfBirth
        };
    }
    public static Contracts.Messages.CustomerUpdated ToCustomerUpdatedMessage(this Domain.Customer customer)
    {
        return new Contracts.Messages.CustomerUpdated
        {
            Id = customer.Id,
            GitHubUsername = customer.GitHubUsername,
            FullName = customer.FullName,
            Email = customer.Email,
            DateOfBirth = customer.DateOfBirth
        };
    }
    public static Contracts.Messages.CustomerDeleted ToCustomerDeletedMessage(this Domain.Customer customer)
    {
        return new Contracts.Messages.CustomerDeleted
        {
            Id = customer.Id
        };
    }
}
