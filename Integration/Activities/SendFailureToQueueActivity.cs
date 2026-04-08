using Azure.Messaging.ServiceBus;
using Integration.Application.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Integration.Activities
{
    public class SendFailureToQueueActivity
    {
        private readonly ILogger logger;
        private readonly ServiceBusClient serviceBusClient;


        public SendFailureToQueueActivity(ILoggerFactory loggerFactory, ServiceBusClient serviceBusClient)
        {
            this.logger = loggerFactory.CreateLogger<SendFailureToQueueActivity>();
            this.serviceBusClient = serviceBusClient;
        }


        [Function(nameof(SendFailureToQueueActivity))]
        public async Task Run([ActivityTrigger] ErrorDetails input)
        {
            var sender = serviceBusClient.CreateSender("notifications-queue");
            var message = new ServiceBusMessage(JsonSerializer.Serialize(input));

            await sender.SendMessageAsync(message);
            logger.LogInformation("Failure message sent to notifications queue");
        }
    }
}
