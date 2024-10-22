﻿namespace Mediator;

public interface IChatRoom
{
	void Join(IParticipant participant);
	void Send(ChatMessage message);
}

public interface IParticipant
{
	string Name { get; }
	void Send(string message);
	void ReceiveMessage(ChatMessage message);
	void ChatRoomJoined(IChatRoom chatRoom);
}

public record class ChatMessage(IParticipant Sender, string Content);

public class User : IParticipant
{
	private readonly IMessageWriter<ChatMessage> _messageWriter;
	private IChatRoom? _chatRoom;

	public User(IMessageWriter<ChatMessage> messageWriter, string name)
	{
		_messageWriter = messageWriter ?? throw new ArgumentNullException(nameof(messageWriter));
		Name = name ?? throw new ArgumentNullException(nameof(name));
	}

	public string Name { get; }

	public void ChatRoomJoined(IChatRoom chatRoom)
	{
		_chatRoom = chatRoom;
	}

	public void ReceiveMessage(ChatMessage message)
	{
		_messageWriter.Write(message);
	}

	public void Send(string message)
	{
		if (_chatRoom == null) throw new ChatRoomNotJoinedException();
		_chatRoom.Send(new ChatMessage(this, message));
	}
}

public class ChatRoomNotJoinedException : Exception
{
	public ChatRoomNotJoinedException()
		: base("You must join a chat room before sending a message.")
	{
	}
}

public class ChatRoom : IChatRoom
{
	private readonly List<IParticipant> _participants = new();

	public void Join(IParticipant participant)
	{
		_participants.Add(participant);
		participant.ChatRoomJoined(this);
		Send(new ChatMessage(participant, "Has joined the channel"));
	}

	public void Send(ChatMessage message)
	{
		_participants.ForEach(participant
			=> participant.ReceiveMessage(message));
	}
}