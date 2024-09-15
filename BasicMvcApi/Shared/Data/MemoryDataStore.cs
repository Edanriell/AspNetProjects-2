using Shared.Models;

namespace Shared.Data;

internal static class MemoryDataStore
{
	public static List<Customer> Customers { get; } = new();

	public static void Seed()
	{
		Customers.Add(new Customer(
				1,
				"Jonny Boy Inc.",
				new List<Contract>
				{
					new(
							1,
							"First contract",
							"This is the first contract.",
							PrimaryContact: new ContactInformation(
									"John",
									"Doe",
									"john.doe@jonnyboy.com"
								),
							Status: new WorkStatus(
									100,
									100
								)
						),
					new(
							2,
							"Some other contract",
							"This is another contract.",
							PrimaryContact: new ContactInformation(
									"Jane",
									"Doe",
									"jane.doe@jonnyboy.com"
								),
							Status: new WorkStatus(
									100,
									25
								)
						)
				}
			));
		Customers.Add(new Customer(
				2,
				"Some mega-corporation",
				new List<Contract>
				{
					new(
							3,
							"Huge contract",
							"This is a huge contract.",
							PrimaryContact: new ContactInformation(
									"Kory",
									"O'Neill",
									"kory.oneill@megacorp.com"
								),
							Status: new WorkStatus(
									15000,
									0
								)
						)
				}
			));
	}
}