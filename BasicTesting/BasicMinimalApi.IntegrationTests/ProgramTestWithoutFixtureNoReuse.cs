using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BasicMinimalApi.IntegrationTests;

public class ProgramTestWithoutFixtureNoReuse
{
	public class Get : ProgramTestWithoutFixture
	{
		[Fact]
		public async Task Should_respond_a_status_200_OK()
		{
			// Arrange
			await using var webAppFactory = new WebApplicationFactory<Program>();
			var             httpClient    = webAppFactory.CreateClient();

			// Act
			var result = await httpClient.GetAsync("/");

			// Assert
			Assert.Equal(HttpStatusCode.OK, result.StatusCode);
		}

		[Fact]
		public async Task Should_respond_hello_world()
		{
			// Arrange
			await using var webAppFactory = new WebApplicationFactory<Program>();
			var             httpClient    = webAppFactory.CreateClient();

			// Act
			var result = await httpClient.GetAsync("/");

			// Assert
			var contentText = await result.Content.ReadAsStringAsync();
			Assert.Equal("Hello World!", contentText);
		}
	}
}