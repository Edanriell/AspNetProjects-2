namespace MyApp;

public partial class xUnitFeaturesTest
{
	[Trait("group", nameof(AssertionTest))]
	public class AssertionTest
	{
		[Fact]
		public void Exploring_xUnit_assertions()
		{
			object  obj1 = new MyClass { Name = "Object 1" };
			object  obj2 = new MyClass { Name = "Object 1" };
			var     obj3 = obj1;
			object? obj4 = default(MyClass);

			Assert.Equal(2, 2);
			Assert.NotEqual(2, 1);

			Assert.Same(obj1, obj3);
			Assert.NotSame(obj1, obj2);
			Assert.Equal(obj1, obj2);

			Assert.Null(obj4);
			Assert.NotNull(obj3);

			var instanceOfMyClass = Assert.IsType<MyClass>(obj1);
			Assert.Equal("Object 1", instanceOfMyClass.Name);

			var exception = Assert.Throws<SomeCustomException>(
					() => OperationThatThrows("Toto")
				);
			Assert.Equal("Toto", exception.Name);

			static void OperationThatThrows(string name)
			{
				throw new SomeCustomException { Name = name };
			}
		}

		private record class MyClass
		{
			public string? Name { get; set; }
		}

		private class SomeCustomException : Exception
		{
			public string? Name { get; set; }
		}
	}
}