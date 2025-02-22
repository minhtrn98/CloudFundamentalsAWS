using Customers.Consumer.Messagers;
using MediatR;
using System.Text.Json;

namespace Customers.Consumer.Handlers;

public sealed class CustomerUpdatedHandler(ILogger<CustomerUpdatedHandler> logger) : IRequestHandler<CustomerUpdated>
{
    public Task Handle(CustomerUpdated request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Customer updated: {request}", JsonSerializer.Serialize(request));
        return Task.CompletedTask;
    }
}
