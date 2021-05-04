using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Messaging.EventHubs.Consumer;

// Examples from: https://docs.microsoft.com/en-us/dotnet/api/overview/azure/storage.blobs-readme
// Starter Guid here: https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-dotnet-standard-getstarted-send


namespace ApiPatternsClient
{
    // Good intro video to EventHubs can be viewed here: https://www.youtube.com/watch?v=DDDjFQSQyF4
    // Code examples here come from documentation:  https://docs.microsoft.com/en-us/dotnet/api/overview/azure/event-hubs

    class EventHubHandler
    {
        public async static Task<string> produceEvents(string ehConnectionString)
        {
            string ehName = "telemetryhub";

            await using (var producerClient = new EventHubProducerClient(ehConnectionString, ehName))
            {
                // Create a batch of events 
                using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

                // Add events to the batch. An event is a represented by a collection of bytes and metadata. 
                eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes("First event")));
                eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes("Second event")));
                eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes("Third event")));

                // Use the producer client to send the batch of events to the event hub
                try
                {
                    await producerClient.SendAsync(eventBatch);
                    return "A batch of 3 events has been published.";
                }
                catch (EventHubsException ex)
                {
                    return ex.Message.ToString();
                }
                catch (System.Exception ex)
                {
                    return ex.Message.ToString();
                }
                
                
            }

        }

        // This approach uses an EventPRocessorClient to maintain state of events read and to process new events.
        // To read more on this, see: https://docs.microsoft.com/en-us/dotnet/api/overview/azure/messaging.eventhubs.processor-readme
        public async static Task<string> processEvents(string ehConnectionString, string blobConnectionString)
        {
            string ehName = "telemetryhub";
            string blobContainerName = "sample-container";
            // For now, read as the default consumer group: $Default
            // Change this to match certain consumer types/permissions you establish
            // in the EventHub properties.
            string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;
            

            // Create a blob container client that the event processor will use to keep track of what has been 
            // historically read or processed already and what events are new.
            BlobContainerClient storageClient = new BlobContainerClient(blobConnectionString, blobContainerName); 

            // Create an event processor client to process events in the event hub
            EventProcessorClient processor = new EventProcessorClient(storageClient, consumerGroup, ehConnectionString, ehName);

            // Register handlers for processing events and handling errors (see static Task definitions later in this class)
            processor.ProcessEventAsync += ProcessEventHandler;
            processor.ProcessErrorAsync += ProcessErrorHandler;

            // By its nature, this approach runs continuously to process events as they're added to the queue
            // Therefore, for this sample code, we will start it and stop it after 15 seconds just to see how
            // it works.  
            // 
            // In the real world, you will likely have a dedicated processor class/function running
            // in perpetuity so that new events can be processed as they're recieved.
            await processor.StartProcessingAsync();
            await Task.Delay(TimeSpan.FromSeconds(15));
            await processor.StopProcessingAsync();

            return "Complete";
        }

        static async Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            // Write the body of the event to the console window
            Console.WriteLine("\tReceived event: {0}", Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));

            // Update checkpoint in the blob storage so that the app receives only new events the next time it's run
            await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
        }

        static Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            // Write details about the error to the console window
            Console.WriteLine($"\tPartition '{ eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
            Console.WriteLine(eventArgs.Exception.Message);
            return Task.CompletedTask;
        }    
    }

}