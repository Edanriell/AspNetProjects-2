﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace OptionsValidation;

public class ByPassingInterfaces
{
	[Fact]
	public void Should_support_any_scope()
	{
		// Arrange
		var services = new ServiceCollection();
		services.AddOptions<MyOptions>()
		   .Configure(o => o.Name = "John Doe");

		//services.AddScoped(serviceProvider
		//    => serviceProvider.GetRequiredService<IOptionsSnapshot<Options>>().Value);
		//
		services.AddScoped(serviceProvider =>
		{
			var snapshot = serviceProvider
			   .GetRequiredService<IOptionsSnapshot<MyOptions>>();
			return snapshot.Value;
		});
		var serviceProvider = services.BuildServiceProvider();

		// Act & Assert
		using var scope1 = serviceProvider.CreateScope();
		var options1 = scope1.ServiceProvider.GetService<MyOptions>();
		var options2 = scope1.ServiceProvider.GetService<MyOptions>();
		Assert.Same(options1, options2);

		using var scope2 = serviceProvider.CreateScope();
		var options3 = scope2.ServiceProvider.GetService<MyOptions>();
		Assert.NotSame(options2, options3);
	}

	private class MyOptions
	{
		public string? Name { get; set; }
	}
}