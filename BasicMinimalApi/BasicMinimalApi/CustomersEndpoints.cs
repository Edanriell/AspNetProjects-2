﻿using Shared.Data;
using Shared.Models;

namespace BasicMinimalApi;

public static class CustomersEndpoints
{
	public static void MapCustomerEndpoints(this IEndpointRouteBuilder routes)
	{
		var group = routes
		   .MapGroup("/raw/customers")
		   .WithTags("Customers Raw");

		group.MapGet("/", async (ICustomerRepository customerRepository, CancellationToken cancellationToken) =>
		{
			return await customerRepository
					  .AllAsync(cancellationToken);
		});

		group.MapGet("/{customerId}",
			async (int customerId, ICustomerRepository customerRepository, CancellationToken cancellationToken) =>
			{
				var customer = await customerRepository.FindAsync(customerId, cancellationToken);
				if (customer == null) return Results.NotFound();
				return Results.Ok(customer);
			});

		group.MapPut("/{customerId}", async (int customerId, Customer input, ICustomerRepository customerRepository,
											 CancellationToken cancellationToken) =>
		{
			var updatedCustomer = await customerRepository.UpdateAsync(input, cancellationToken);
			if (updatedCustomer == null) return Results.NotFound();
			return Results.Ok(updatedCustomer);
		});

		group.MapPost("/",
			async (Customer input, ICustomerRepository customerRepository, CancellationToken cancellationToken) =>
			{
				var createdCustomer = await customerRepository.CreateAsync(input, cancellationToken);
				return TypedResults.Created($"/api/Customers/{createdCustomer.Id}", createdCustomer);
			});

		group.MapDelete("/{customerId}",
			async (int customerId, ICustomerRepository customerRepository, CancellationToken cancellationToken) =>
			{
				var deletedCustomer = await customerRepository.DeleteAsync(customerId, cancellationToken);
				if (deletedCustomer == null) return Results.NotFound();
				return TypedResults.Ok(deletedCustomer);
			});
	}
}