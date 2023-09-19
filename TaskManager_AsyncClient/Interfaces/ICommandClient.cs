using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager_AsyncClient.Interfaces
{
    public interface ICommandClient
    {
        Task PollQueueAsync<TMessage>(string queueUrl, Func<TMessage?, CancellationToken, Task> delegateFunc, CancellationToken cancellationToken, int numberOfMessages = 1, int timeOutInSeconds = 30);

        Task SendCommand<TMessage>(string queueUrl, TMessage payload, CancellationToken cancellationToken);

        Task SendCommand<TMessage>(string queueUrl, TMessage payload, string? messageGroupId = null, CancellationToken cancellationToken = default);
    }
}
