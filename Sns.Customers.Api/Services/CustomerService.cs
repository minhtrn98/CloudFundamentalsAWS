using FluentValidation;
using FluentValidation.Results;
using Sns.Customers.Api.Contracts.Messages;
using Sns.Customers.Api.Domain;
using Sns.Customers.Api.Mapping;
using Sns.Customers.Api.Messaging;
using Sns.Customers.Api.Repositories;

namespace Sns.Customers.Api.Services;

public class CustomerService(
    ICustomerRepository customerRepository,
    IGitHubService gitHubService,
    ISnsMessenger sqsMessenger
    ) : ICustomerService
{
    public async Task<bool> CreateAsync(Customer customer)
    {
        Contracts.Data.CustomerDto? existingUser = await customerRepository.GetAsync(customer.Id);
        if (existingUser is not null)
        {
            string message = $"A user with id {customer.Id} already exists";
            throw new ValidationException(message, GenerateValidationError(nameof(Customer), message));
        }

        bool isValidGitHubUser = await gitHubService.IsValidGitHubUser(customer.GitHubUsername);
        if (!isValidGitHubUser)
        {
            string message = $"There is no GitHub user with username {customer.GitHubUsername}";
            throw new ValidationException(message, GenerateValidationError(nameof(customer.GitHubUsername), message));
        }

        Contracts.Data.CustomerDto customerDto = customer.ToCustomerDto();
        bool response = await customerRepository.CreateAsync(customerDto);
        if (response)
        {
            await sqsMessenger.PublishAsync(customer.ToCustomerCreatedMessage());
        }

        return response;
    }

    public async Task<Customer?> GetAsync(Guid id)
    {
        Contracts.Data.CustomerDto? customerDto = await customerRepository.GetAsync(id);
        return customerDto?.ToCustomer();
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        IEnumerable<Contracts.Data.CustomerDto> customerDtos = await customerRepository.GetAllAsync();
        return customerDtos.Select(x => x.ToCustomer());
    }

    public async Task<bool> UpdateAsync(Customer customer)
    {
        Contracts.Data.CustomerDto customerDto = customer.ToCustomerDto();

        bool isValidGitHubUser = await gitHubService.IsValidGitHubUser(customer.GitHubUsername);
        if (!isValidGitHubUser)
        {
            string message = $"There is no GitHub user with username {customer.GitHubUsername}";
            throw new ValidationException(message, GenerateValidationError(nameof(customer.GitHubUsername), message));
        }

        bool response = await customerRepository.UpdateAsync(customerDto);
        if (response)
        {
            await sqsMessenger.PublishAsync(customer.ToCustomerUpdatedMessage());
        }
        return response;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        bool response = await customerRepository.DeleteAsync(id);
        if (response)
        {
            await sqsMessenger.PublishAsync(new CustomerDeleted() { Id = id });
        }
        return response;
    }

    private static ValidationFailure[] GenerateValidationError(string paramName, string message)
    {
        return
        [
            new ValidationFailure(paramName, message)
        ];
    }
}
