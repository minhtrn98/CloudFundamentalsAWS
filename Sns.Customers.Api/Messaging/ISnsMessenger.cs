
using Amazon.SimpleNotificationService.Model;

namespace Sns.Customers.Api.Messaging;

public interface ISnsMessenger
{
    Task<PublishResponse> PublishAsync<T>(T message);
}
