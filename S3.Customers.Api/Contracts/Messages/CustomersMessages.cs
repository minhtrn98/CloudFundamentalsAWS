namespace S3.Customers.Api.Contracts.Messages;

public sealed class CustomerCreated
{
    public required Guid Id { get; init; }

    public required string GitHubUsername { get; init; }

    public required string FullName { get; init; }

    public required string Email { get; init; }

    public required DateTime DateOfBirth { get; init; }
}

public sealed class CustomerUpdated
{
    public required Guid Id { get; init; }

    public required string GitHubUsername { get; init; }

    public required string FullName { get; init; }

    public required string Email { get; init; }

    public required DateTime DateOfBirth { get; init; }
}

public sealed class CustomerDeleted
{
    public required Guid Id { get; init; }
}
