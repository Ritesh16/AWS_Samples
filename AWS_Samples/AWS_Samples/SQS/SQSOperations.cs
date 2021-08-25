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
        const string queueName = "myappqueue1";
        const string queueUrl = "https://sqs.us-east-2.amazonaws.com/184379576642/myappqueue";
        const string queueUrl1 = "https://sqs.us-east-2.amazonaws.com/184379576642/myappqueue1";
        const string receiptHandle = "AQEBRFWplk8FoG2gbS2Jet8gJnMvffNedfrHzJwldrCLIRdPypdUUQgUBAXkPwhkzImlcoSZWdgg7qcGxWqBLHEOw0ERlNTI2AUBjvTf/jlBbgn8R1Layx4bwPJhv1U9yjuQF2d3Bzj/U65lsVcmp7HeiTa2j05sc8EGN1CVffXrC6mIFl+peI2WkUaQhIQcLP/S3V4GCUljzI7wAagi/zqJuAPGqeoOTYel/mvdFIbSDtLAciHsLLUoTUQlQdFJ5m3QyKESX+4kpsKq8+DPOLP1ZTyZ31Ez4dFwd2vmxZatv+UP5hr9Pq8RDnEGICtErbvqzJ750DA2r5hhgNbAKweCPXISkPh1SGT+Ot9TJc91kNL+xNuVkFjfGoqeL1q7KJ9GMJzzkG0+iEXZOwm4Cf4PcA==";
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
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
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

        // Gets receipt handle
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
        public void DeleteMessage()
        {
            DeleteMessageRequest request = new DeleteMessageRequest { QueueUrl = queueUrl, ReceiptHandle = receiptHandle };
            var response = client.DeleteMessage(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Message Deleted Succsessfully");
            }
        }
        public void PurgeMessages()
        {
            PurgeQueueRequest request = new PurgeQueueRequest { QueueUrl = queueUrl };
            var response = client.PurgeQueue(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Message(s) Deleted Succsessfully");
            }
        }
        public void ListQueues()
        {
            ListQueuesRequest request = new ListQueuesRequest { };
            var respone = client.ListQueues(request);
            if (respone.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                foreach (var queue in respone.QueueUrls)
                {
                    Console.WriteLine(queue);
                }
            }
        }
        public void DeleteQueues()
        {
            DeleteQueueRequest request = new DeleteQueueRequest { QueueUrl = queueUrl1 };
            var response = client.DeleteQueue(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Queue has been deleted successfully");
                Console.WriteLine("Current Queues");
                ListQueues();
            }
        }

        public void AddTags()
        {
            TagQueueRequest request = new TagQueueRequest();
            request.QueueUrl = queueUrl;
            request.Tags.Add("env", "staging");
            var response = client.TagQueue(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Tags added successfully");
                ListTags();
            }
        }
        public void ListTags()
        {
            var response = client.ListQueueTags(new ListQueueTagsRequest { QueueUrl = queueUrl });
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Tags List");
                foreach (var tag in response.Tags)
                {
                    Console.WriteLine($"Key: {tag.Key}, Value: {tag.Value}");
                }
            }
        }
    }
}
