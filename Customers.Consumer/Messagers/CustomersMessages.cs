﻿using MediatR;

namespace Customers.Consumer.Messagers;

public sealed class CustomerCreated : IRequest, ISqsMessage
{
    public required Guid Id { get; init; }

    public required string GitHubUsername { get; init; }

    public required string FullName { get; init; }

    public required string Email { get; init; }

    public required DateTime DateOfBirth { get; init; }
}

public sealed class CustomerUpdated : IRequest, ISqsMessage
{
    public required Guid Id { get; init; }

    public required string GitHubUsername { get; init; }

    public required string FullName { get; init; }

    public required string Email { get; init; }

    public required DateTime DateOfBirth { get; init; }
}

public sealed class CustomerDeleted : IRequest, ISqsMessage
{
    public required Guid Id { get; init; }
}
