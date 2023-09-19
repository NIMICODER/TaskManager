using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager_AsyncClient.Implementation
{
    public class CommandClient
    {
        private readonly IAmazonSQS _sqsClient;
        private static readonly StringEnumConverter _stringEnumConverter = new StringEnumConverter();
        private readonly ILogger<CommandClient> _logger;

        public CommandClient(ILogger<CommandClient> logger, IAmazonSQS sqsClient)
        {
            _logger = logger;
            _sqsClient = sqsClient;
        }

        public async Task PollQueueAsync<TMessage>(string queueUrl, Func<TMessage?, CancellationToken, Task> delegateFunc, CancellationToken cancellationToken, int numberOfMessages, int timeOutInSeconds)
        {
            try
            {
                ReceiveMessageResponse messageResponse = await _sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
                {
                    VisibilityTimeout = timeOutInSeconds,
                    QueueUrl = queueUrl,
                    MaxNumberOfMessages = numberOfMessages
                });

                foreach (var message in messageResponse.Messages)
                {
                    try
                    {
                        if (message != null)
                        {
                            _logger.LogInformation(message.Body);
                            TMessage? data = JsonConvert.DeserializeObject<TMessage>(message.Body);
                            await delegateFunc(data, cancellationToken);
                            await DeleteMessage(queueUrl, message);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task SendCommand<TMessage>(string queueUrl, TMessage payload, string? messageGroupId, CancellationToken cancellationToken)
        {
            try
            {
                string messageBody = JsonConvert.SerializeObject(payload, _stringEnumConverter);
                SendMessageRequest message;

                if (!string.IsNullOrWhiteSpace(messageGroupId))
                {
                    message = new SendMessageRequest
                    {
                        MessageBody = messageBody,
                        QueueUrl = queueUrl,
                        MessageGroupId = messageGroupId
                    };
                }
                else
                {
                    message = new SendMessageRequest
                    {
                        MessageBody = messageBody,
                        QueueUrl = queueUrl,
                    };
                }

                SendMessageResponse response = await _sqsClient.SendMessageAsync(message, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task SendCommand<TMessage>(string queueUrl, TMessage payload, CancellationToken cancellationToken)
        {
            try
            {
                string messageBody = JsonConvert.SerializeObject(payload, _stringEnumConverter);
                //We are using STANDARD queus so no need to pass a message group id
                SendMessageRequest message = new SendMessageRequest
                {
                    MessageBody = messageBody,
                    QueueUrl = queueUrl,
                };
                SendMessageResponse response = await _sqsClient.SendMessageAsync(message, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async Task DeleteMessage(string queueUrl, Message message)
        {
            await _sqsClient.DeleteMessageAsync(queueUrl, message.ReceiptHandle);
        }

    }
}
