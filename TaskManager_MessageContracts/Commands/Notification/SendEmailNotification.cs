namespace TaskManager_MessageContracts.Commands.Notification
{
    public class SendEmailNotification
    {
        public string Subject { get; init; } = null!;
        public Personality To { get; set; } = null!;
        public bool IsTransactional { get; set; }
        public TimeSpan TTL { get; set; }
        public DateTime CommandSentAt { get; set; }
        public IEnumerable<Personality> CCs { get; set; } = Enumerable.Empty<Personality>();
        public IEnumerable<Personality> BCCs { get; set; } = Enumerable.Empty<Personality>();
        public IEnumerable<string> Content { get; set; } = Enumerable.Empty<string>();
        public string Source { get; set; } = null!;
        public string MessageId { get; set; } = null!;
    }

}
