using Customers.Consumer.Messagers;
using MediatR;
using System.Text.Json;

namespace Customers.Consumer.Handlers;

public sealed class CustomerUpdatedHandler : IRequestHandler<CustomerUpdated>
{
    private readonly ILogger<CustomerUpdatedHandler> _logger;

    public CustomerUpdatedHandler(ILogger<CustomerUpdatedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CustomerUpdated request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Customer updated: {request}", JsonSerializer.Serialize(request));
        return Task.CompletedTask;
    }
}
