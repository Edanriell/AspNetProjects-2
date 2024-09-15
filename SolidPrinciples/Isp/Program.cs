using Isp.After;

var publicProductReader      = new PublicProductReader();
var privateProductRepository = new PrivateProductRepository();

ReadProducts(publicProductReader);
ReadProducts(privateProductRepository);

// Error: Cannot convert from PublicProductReader to IProductWriter
// ModifyProducts(publicProductReader); // Invalid
WriteProducts(privateProductRepository);

ReadAndWriteProducts(privateProductRepository, privateProductRepository);
ReadAndWriteProducts(publicProductReader,      privateProductRepository);

void ReadProducts(IProductReader productReader)
{
	Console.WriteLine(
			"Reading from {0}.",
			productReader.GetType().Name
		);
}

void WriteProducts(IProductWriter productWriter)
{
	Console.WriteLine(
			"Writing to {0}.",
			productWriter.GetType().Name
		);
}

void ReadAndWriteProducts(IProductReader productReader, IProductWriter productWriter)
{
	Console.WriteLine(
			"Reading from {0} and writing to {1}.",
			productReader.GetType().Name,
			productWriter.GetType().Name
		);
}

// ”Many client-specific interfaces are better than one general-purpose interface.”
//What      does that mean? It means the following:
//	• You should create interfaces.
//	• You should value small interfaces more.
//	• You should not   create multipurpose interfaces.

//Next are some more details that overview interfaces:
//	• The role of an interface is to define a cohesive contract (public methods, properties, and
//	events). In its theoretical form, an interface contains no code; it is only a contract. In practice,
//	since C# 8, we can create default implementation in interfaces, which could be helpful to limit
//breaking changes in a library (such as adding a method to an interface without breaking any
//class implementing that interface).
//	• An interface following the ISP should be small. Its members should commit to the SRP and
//strive toward a common goal (cohesion).
//	• In C#, a class can implement multiple interfaces, exposing multiples of those public contracts
//	or, more accurately, be any and all of them. By leveraging polymorphism, we can consume a
//class as if it were any of the interfaces it implements or its supertype if it inherits another class.

//To summarize the idea behind the ISP, if you have multiple smaller interfaces, it is easier to reuse
//	them and expose only the features you need instead of exposing APIs that part of your program doesn’t
//need. Furthermore, it is easier to compose bigger pieces using multiple specialized interfaces by
//implementing them as needed than remove methods from a big interface if we don’t need them in
//one of its implementations.

// The main takeaway is to only depend on the interfaces that you consume.