using Customers.Consumer.Messagers;
using MediatR;
using System.Text.Json;

namespace Customers.Consumer.Handlers;

public sealed class CustomerCreatedHandler(ILogger<CustomerCreatedHandler> logger) : IRequestHandler<CustomerCreated>
{
    public Task Handle(CustomerCreated request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Customer created: {request}", JsonSerializer.Serialize(request));
        return Task.CompletedTask;
    }
}
