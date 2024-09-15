using Microsoft.AspNetCore.Mvc;

namespace BasicApi.Controllers;

[Route("")]
[ApiController]
public class HelloWorldController : ControllerBase
{
	[HttpGet]
	public string Hello()
	{
		return "Hello World!";
	}
}