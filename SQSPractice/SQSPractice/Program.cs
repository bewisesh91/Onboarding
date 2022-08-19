using System;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace SendMessageToQueueExample
{
    public class SendMessageToQueue
    {
        public static async Task Main()
        {
            var client = new AmazonSQSClient();
            var queue = "SQSPractice";
            var queueUrl = await GetQueueUrl(client, queue);

            while (true)
            {
                var actions = new List<Action>();

                for (var i = 0; i < 16; i++)
                {
                    //actions.Add(async () =>
                    //{
                    //    var messageBody = "This is a sample message to send to SQSPractice.";

                    //    var request = new SendMessageRequest
                    //    {
                    //        MessageBody = messageBody,
                    //        QueueUrl = queueUrl,
                    //    };

                    //    var sendResponse = await client.SendMessageAsync(request);

                    //    if (sendResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    //    {
                    //        Console.WriteLine($"Successfully sent message. Message ID: {sendResponse.MessageId}");
                    //    }
                    //    else
                    //    {
                    //        Console.WriteLine("Could not send message.");
                    //    }
                    //});

                    actions.Add(async () =>
                    {
                        var receiveResponse = await ReceiveAndDeleteMessage(client, queueUrl);

                        Console.WriteLine($"Message: {receiveResponse.Messages[0].Body}");
                    });
                }

                Parallel.Invoke(actions.ToArray());
                Thread.Sleep(10000);
            };
            
            Console.ReadLine();
        }

        public static async Task<string> GetQueueUrl(IAmazonSQS client, string queueName)
        {
            var request = new GetQueueUrlRequest
            {
                QueueName = queueName,
            };

            GetQueueUrlResponse response = await client.GetQueueUrlAsync(request);
            return response.QueueUrl;
        }

        public static async Task<ReceiveMessageResponse> ReceiveAndDeleteMessage(IAmazonSQS client, string queueUrl)
        {
            var receiveMessageRequest = new ReceiveMessageRequest
            {
                AttributeNames = { "SentTimestamp" },
                MaxNumberOfMessages = 1,
                MessageAttributeNames = { "All" },
                QueueUrl = queueUrl,
                VisibilityTimeout = 30,
                WaitTimeSeconds = 0,
            };

            var receiveMessageResponse = await client.ReceiveMessageAsync(receiveMessageRequest);

            //var deleteMessageRequest = new DeleteMessageRequest
            //{
            //    QueueUrl = queueUrl,
            //    ReceiptHandle = receiveMessageResponse.Messages[0].ReceiptHandle,
            //};

            //await client.DeleteMessageAsync(deleteMessageRequest);

            return receiveMessageResponse;
        }
    }
}