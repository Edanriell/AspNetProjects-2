using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Data;
using Shared.DTO;
using Shared.Models;

namespace BasicMinimalApi;

public static class DTOEndpoints
{
	public static void MapCustomerDtoEndpoints(this IEndpointRouteBuilder routes)
	{
		var group = routes
		   .MapGroup("/dto/customers")
		   .WithTags("Customers DTO");

		group.MapGet("/", GetCustomersSummaryAsync);
		group.MapGet("/{customerId}", GetCustomerDetailsAsync);
		group.MapPut("/{customerId}", UpdateCustomerAsync);
		group.MapPost("/", CreateCustomerAsync);
		group.MapDelete("/{customerId}", DeleteCustomerAsync);
	}

	private static async Task<Ok<IEnumerable<CustomerSummary>>> GetCustomersSummaryAsync(
		ICustomerRepository customerRepository,
		CancellationToken cancellationToken)
	{
		// Get all customers
		var customers = await customerRepository.AllAsync(cancellationToken);

		// Map customers to CustomerSummary DTOs
		var customersSummary = customers.Select(customer => new CustomerSummary(
				customer.Id,
				customer.Name,
				customer.Contracts.Count,
				customer.Contracts
				   .Count(x => x.Status.State != WorkState.Completed)
			));

		// Return the DTOs
		return TypedResults.Ok(customersSummary);
	}

	private static async Task<Results<Ok<CustomerDetails>, NotFound>> GetCustomerDetailsAsync(
		int customerId, ICustomerRepository customerRepository, CancellationToken cancellationToken)
	{
		// Get the customers
		var customer = await customerRepository.FindAsync(customerId, cancellationToken);
		if (customer == null) return TypedResults.NotFound();

		// Map the customers to a CustomerDetails DTO
		var dto = MapCustomerToCustomerDetails(customer);

		// Return the DTO
		return TypedResults.Ok(dto);
	}

	private static async Task<Results<
			Ok<CustomerDetails>,
			NotFound,
			Conflict
		>> UpdateCustomerAsync(
		int customerId,
		UpdateCustomer input,
		ICustomerRepository customerRepository,
		CancellationToken cancellationToken)
	{
		// Get the customer
		var customer = await customerRepository.FindAsync(
							   customerId,
							   cancellationToken
						   );
		if (customer == null) return TypedResults.NotFound();

		// Update the customer's name using the UpdateCustomer DTO
		var updatedCustomer = await customerRepository.UpdateAsync(
									  customer with { Name = input.Name },
									  cancellationToken
								  );
		if (updatedCustomer == null) return TypedResults.Conflict();

		// Map the updated customer to a CustomerDetails DTO
		var dto = MapCustomerToCustomerDetails(updatedCustomer);

		// Return the DTO
		return TypedResults.Ok(dto);
	}

	private static async Task<Results<Created<CustomerDetails>, NotFound>> CreateCustomerAsync(
		CreateCustomer input, ICustomerRepository customerRepository, CancellationToken cancellationToken)
	{
		// Create the customer
		var createdCustomer = await customerRepository.CreateAsync(
									  new Customer(0, input.Name, new List<Contract>()),
									  cancellationToken
								  );

		// Map the updated customer to a CustomerDetails DTO
		var dto = MapCustomerToCustomerDetails(createdCustomer);

		// Return the DTO
		return TypedResults.Created($"/dto/Customers/{createdCustomer.Id}", dto);
	}

	private static async Task<Results<Ok<CustomerDetails>, NotFound, Conflict>> DeleteCustomerAsync(
		int customerId, ICustomerRepository customerRepository, CancellationToken cancellationToken)
	{
		// Delete the customer
		var deletedCustomer = await customerRepository.DeleteAsync(customerId, cancellationToken);
		if (deletedCustomer == null) return TypedResults.NotFound();

		// Map the updated customer to a CustomerDetails DTO
		var dto = MapCustomerToCustomerDetails(deletedCustomer);

		// Return the DTO
		return TypedResults.Ok(dto);
	}

	private static CustomerDetails MapCustomerToCustomerDetails(Customer customer)
	{
		var dto = new CustomerDetails(
				customer.Id,
				customer.Name,
				customer.Contracts.Select(contract => new ContractDetails(
						contract.Id,
						contract.Name,
						contract.Description,

						// Flattening PrimaryContact
						PrimaryContactEmail: contract.PrimaryContact.Email,
						PrimaryContactFirstName: contract.PrimaryContact.FirstName,
						PrimaryContactLastName: contract.PrimaryContact.LastName,

						// Flattening Work
						StatusWorkDone: contract.Status.WorkDone,
						StatusTotalWork: contract.Status.TotalWork,
						StatusWorkState: contract.Status.State.ToString()
					))
			);
		return dto;
	}
}