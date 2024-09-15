namespace FinalChainOfResponsibility;

public abstract class SingleMessageHandlerBase : MessageHandlerBase
{
	public SingleMessageHandlerBase(IMessageHandler? next = null)
		: base(next)
	{
	}

	protected abstract string HandledMessageName { get; }

	protected override bool CanHandle(Message message)
	{
		return message.Name == HandledMessageName;
	}
}