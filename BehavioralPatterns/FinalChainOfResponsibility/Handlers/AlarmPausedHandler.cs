namespace FinalChainOfResponsibility;

public class AlarmPausedHandler : SingleMessageHandlerBase
{
	public AlarmPausedHandler(IMessageHandler? next = null)
		: base(next)
	{
	}

	protected override string HandledMessageName => "AlarmPaused";

	protected override void Process(Message message)
	{
		// Do something clever with the Payload
	}
}