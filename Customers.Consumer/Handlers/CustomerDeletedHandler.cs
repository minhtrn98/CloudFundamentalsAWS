using Customers.Consumer.Messagers;
using MediatR;
using System.Text.Json;

namespace Customers.Consumer.Handlers;

public sealed class CustomerDeletedHandler(ILogger<CustomerDeletedHandler> logger) : IRequestHandler<CustomerDeleted>
{
    public Task Handle(CustomerDeleted request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Customer deleted: {request}", JsonSerializer.Serialize(request));
        return Task.CompletedTask;
    }
}
