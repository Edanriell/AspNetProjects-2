namespace FinalChainOfResponsibility;

public abstract class MultipleMessageHandlerBase : MessageHandlerBase
{
	public MultipleMessageHandlerBase(IMessageHandler? next = null)
		: base(next)
	{
	}

	protected abstract string[] HandledMessagesName { get; }

	protected override bool CanHandle(Message message)
	{
		return HandledMessagesName.Contains(message.Name);
	}
}