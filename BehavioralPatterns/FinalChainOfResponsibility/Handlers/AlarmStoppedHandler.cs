namespace FinalChainOfResponsibility;

public class AlarmStoppedHandler : SingleMessageHandlerBase
{
	public AlarmStoppedHandler(IMessageHandler? next = null)
		: base(next)
	{
	}

	protected override string HandledMessageName => "AlarmStopped";

	protected override void Process(Message message)
	{
		// Do something clever with the Payload
	}
}