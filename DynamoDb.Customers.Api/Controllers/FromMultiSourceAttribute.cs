﻿using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DynamoDb.Customers.Api.Controllers;

public sealed class FromMultiSourceAttribute : Attribute, IBindingSourceMetadata
{
    public BindingSource BindingSource { get; } = CompositeBindingSource.Create(
        new[] { BindingSource.Path, BindingSource.Query },
        nameof(FromMultiSourceAttribute));
}
