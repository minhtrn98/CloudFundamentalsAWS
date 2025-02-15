using Customers.Consumer.Messagers;
using MediatR;
using System.Text.Json;

namespace Customers.Consumer.Handlers;

public sealed class CustomerCreatedHandler : IRequestHandler<CustomerCreated>
{
    private readonly ILogger<CustomerCreatedHandler> _logger;

    public CustomerCreatedHandler(ILogger<CustomerCreatedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CustomerCreated request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Customer created: {request}", JsonSerializer.Serialize(request));
        return Task.CompletedTask;
    }
}
