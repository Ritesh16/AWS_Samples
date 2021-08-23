using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS_Samples.SQS
{
    class SQSOperations
    {
        const string queueName = "myappqueue";
        const string queueUrl = "https://sqs.us-east-2.amazonaws.com/184379576642/myappqueue";
        AmazonSQSClient client;
        BasicAWSCredentials credentials =
              new BasicAWSCredentials(Config.AccessKey, Config.SecretKey);
        public SQSOperations()
        {
            client = new AmazonSQSClient(credentials, Amazon.RegionEndpoint.USEast2);
        }

        public void CreateSQSQueue()
        {
            CreateQueueRequest request = new CreateQueueRequest { QueueName = queueName };
            var response = client.CreateQueue(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK || response.HttpStatusCode == System.Net.HttpStatusCode.Created)
            {
                Console.WriteLine("SQS Queue Created Successfully");
                Console.WriteLine($"SQS Queue Url: {response.QueueUrl}");
            }
        }

        public void SendMessage()
        {
            SendMessageRequest request = new SendMessageRequest { QueueUrl = queueUrl, MessageBody = "My Queue Message" };
            var response = client.SendMessage(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Message Sent Successfully");
                Console.WriteLine($"Message Id: {response.MessageId}, Sequence Id: {response.SequenceNumber}");
            }
        }

        public void ReceiveMessage()
        {
            ReceiveMessageRequest request = new ReceiveMessageRequest { QueueUrl = queueUrl };
            var response = client.ReceiveMessage(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Message(s) Received Successfully");
                foreach (var message in response.Messages)
                {
                    Console.WriteLine($"Message Content: {message.Body}");
                    Console.WriteLine($"Message Content: {message.MessageId}");
                    Console.WriteLine($"Message Content: {message.ReceiptHandle}");
                }
            }
        }
        public void SendBatchMessages()
        {
            SendMessageBatchRequest request = new SendMessageBatchRequest();
            request.QueueUrl = queueUrl;
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            Console.WriteLine(id1.ToString());
            Console.WriteLine(id2.ToString());
            request.Entries = new List<SendMessageBatchRequestEntry>
            {
                new SendMessageBatchRequestEntry {Id = id1.ToString(),MessageBody = "First Message"},
                new SendMessageBatchRequestEntry {Id = id2.ToString(),MessageBody = "Second Message"}
            };
            var response = client.SendMessageBatch(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Messages queued successfully");
                foreach (var item in response.Successful)
                {
                    Console.WriteLine($"ID: {item.Id}, SequenceNumber: {item.SequenceNumber}, MessageId: {item.MessageId}");
                }

            }
        }
    }
}
