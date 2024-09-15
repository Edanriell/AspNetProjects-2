using Ocp;
using Ocp.DependencyInjection;

// Create the entity in database 1
var repository1 = new EntityRepository( /* connection string 1 */);
var service1    = new EntityService(repository1);

// Create the entity in database 2
var repository2 = new EntityRepository( /* connection string 2 */);
var service2    = new EntityService(repository2);

// Save an entity in two different databases
var entity = new Entity();
await service1.ComplexBusinessProcessAsync(entity);
await service2.ComplexBusinessProcessAsync(entity);

Console.WriteLine("This program does nothing and will throw an exception!");

// Software entities (classes, modules, functions, and so on) should be open for extension but closed for modification.
//  It means you should be able to change the class behaviors from the outside without altering the code.
//The best way of implementing the OCP is the Strategy pattern
// Valid OCP