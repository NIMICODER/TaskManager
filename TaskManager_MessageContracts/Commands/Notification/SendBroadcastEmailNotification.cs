namespace TaskManager_MessageContracts.Commands.Notification
{
    public class SendBroadcastEmailNotification
    {
        public string Subject { get; init; } = null!;
        public bool IsTransactional { get; set; }
        public TimeSpan TTL { get; set; }
        public DateTime CommandSentAt { get; set; }
        public IEnumerable<string> Content { get; set; } = Enumerable.Empty<string>();
        public string Source { get; set; } = null!;
        public string MessageId { get; set; } = null!;
        public Guid EmailListId { get; set; }
    }

}
