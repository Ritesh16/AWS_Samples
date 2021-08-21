using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS_Samples.SNS
{
    public class SNSOperations
    {
        const string topicName = "Topic1";
        const string topicArn = "arn:aws:sns:us-east-2:184379576642:Topic1";
        AmazonSimpleNotificationServiceClient client;
        BasicAWSCredentials credentials =
              new BasicAWSCredentials(Config.AccessKey, Config.SecretKey);
        public SNSOperations()
        {
            client = new AmazonSimpleNotificationServiceClient(credentials, Amazon.RegionEndpoint.USEast2);
        }

        public void CreateTopic()
        {
            var request = new CreateTopicRequest()
            {
                Name = topicName
            };

            var result = client.CreateTopic(request);
            if (result.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Topic created successfully.");
                Console.WriteLine($"Topic ARN : ", result.TopicArn);
            }
        }

        public void Subscribe()
        {
            //var request = new SubscribeRequest()
            //{
            //    TopicArn = topicArn,
            //    Protocol = "email",
            //    Endpoint = "ritesh_sharma@outlook.com"
            //};
            var request = new SubscribeRequest()
            {
                TopicArn = topicArn,
                Protocol = "sms",
                Endpoint = "+13025403214"
            };

            var result = client.Subscribe(request);
            if (result.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Subscribed successfully.");
                Console.WriteLine(result.SubscriptionArn);
                Console.WriteLine(request.ReturnSubscriptionArn);
            }
        }

        public void PublishToTopic()
        {
            var request = new PublishRequest()
            {
                //TopicArn = topicArn,
                PhoneNumber = "+13025403214",
                //Subject = "New message",
                Message = "SMS"
            };

            var response = client.Publish(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Message sent successfully.");
                Console.WriteLine($"Message Id : " , response.MessageId);
            }

        }

        public void ListTopics()
        {
            ListTopicsRequest request = new ListTopicsRequest
            {

            };
            var response = client.ListTopics(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                foreach (var topic in response.Topics)
                {
                    Console.WriteLine(topic.TopicArn);
                }
            }
        }

        public void ListSubscriptions()
        {
            ListSubscriptionsByTopicRequest request = new ListSubscriptionsByTopicRequest { TopicArn = topicArn };
            var response = client.ListSubscriptionsByTopic(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                foreach (var subscription in response.Subscriptions)
                {
                    Console.WriteLine(subscription.Owner);
                    Console.WriteLine(subscription.Protocol);
                    Console.WriteLine(subscription.SubscriptionArn);
                    Console.WriteLine(subscription.Endpoint);

                    Console.WriteLine();
                }
            }
        }
        public void Unsubscribe()
        {
            var subscribtion = "arn:aws:sns:us-east-2:184379576642:Topic1:0e76c389-fae5-492b-9a55-203e26b4917b";

            ListSubscriptions();
            
            UnsubscribeRequest request = new UnsubscribeRequest { SubscriptionArn = subscribtion };
            var response = client.Unsubscribe(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Unsubscribe successfully");
            }

            Console.WriteLine("************************************");
            ListSubscriptions();
        }

        public void DeleteTopic()
        {
            ListTopics();
            Console.WriteLine("======Previuos Topics=======");
            DeleteTopicRequest request = new DeleteTopicRequest { TopicArn = topicArn };
            var response = client.DeleteTopic(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Topic has been deleted successfully");
                ListTopics();
            }
        }
    }
}
