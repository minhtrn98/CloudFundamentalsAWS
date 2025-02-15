using Customers.Consumer.Messagers;
using MediatR;
using System.Text.Json;

namespace Customers.Consumer.Handlers;

public sealed class CustomerDeletedHandler : IRequestHandler<CustomerDeleted>
{
    private readonly ILogger<CustomerDeletedHandler> _logger;

    public CustomerDeletedHandler(ILogger<CustomerDeletedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CustomerDeleted request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Customer deleted: {request}", JsonSerializer.Serialize(request));
        return Task.CompletedTask;
    }
}
