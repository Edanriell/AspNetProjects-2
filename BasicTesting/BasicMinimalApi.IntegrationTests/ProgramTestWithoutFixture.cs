using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BasicMinimalApi.IntegrationTests;

public class ProgramTestWithoutFixture : IAsyncDisposable
{
	private readonly HttpClient                     _httpClient;
	private readonly WebApplicationFactory<Program> _webApplicationFactory;

	public ProgramTestWithoutFixture()
	{
		_webApplicationFactory = new WebApplicationFactory<Program>();
		_httpClient            = _webApplicationFactory.CreateClient();
	}

	public ValueTask DisposeAsync()
	{
		return ((IAsyncDisposable)_webApplicationFactory).DisposeAsync();
	}

	public class Get : ProgramTestWithoutFixture
	{
		[Fact]
		public async Task Should_respond_a_status_200_OK()
		{
			// Act
			var result = await _httpClient.GetAsync("/");

			// Assert
			Assert.Equal(HttpStatusCode.OK, result.StatusCode);
		}

		[Fact]
		public async Task Should_respond_hello_world()
		{
			// Act
			var result = await _httpClient.GetStringAsync("/");

			// Assert
			Assert.Equal("Hello World!", result);
		}
	}
}