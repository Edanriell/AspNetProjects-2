using Microsoft.AspNetCore.Mvc;

namespace BasicMvcApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
	[HttpGet("IActionResult")]
	[ProducesResponseType(typeof(Model), StatusCodes.Status200OK)]
	public IActionResult InterfaceAction()
	{
		return Ok(new Model(nameof(InterfaceAction)));
	}

	[HttpGet("ActionResult")]
	[ProducesResponseType(typeof(Model), StatusCodes.Status200OK)]
	public ActionResult ClassAction()
	{
		return Ok(new Model(nameof(ClassAction)));
	}

	[HttpGet("DirectModel")]
	public Model DirectModel()
	{
		return new Model(nameof(DirectModel));
	}

	[HttpGet("ActionResultT")]
	public ActionResult<Model> ActionResultT()
	{
		return new Model(nameof(ActionResultT));
	}

	[HttpGet("MultipleResults")]
	public ActionResult<Model> MultipleResults()
	{
		var condition = Random.Shared
		   .GetItems(new[] { true, false }, 1)
		   .First();
		return condition
				   ? Ok(new Model(nameof(MultipleResults)))
				   : NotFound();
	}

	public record class Model(string Name);
}