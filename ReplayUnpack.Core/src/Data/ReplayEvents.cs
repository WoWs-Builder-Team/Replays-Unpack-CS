namespace ReplaysUnpackCS.Data
{
    public interface IReplayEvent : IReplayData
    {
    }

    public record ChatMessage(uint EntityId, string MessageGroup, string MessageContent) : IReplayEvent;
}