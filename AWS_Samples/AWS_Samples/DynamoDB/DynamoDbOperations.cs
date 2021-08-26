using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS_Samples.DynamoDB
{
    class DynamoDbOperations
    {
        AmazonDynamoDBClient client;
        public BasicAWSCredentials credentials =
             new BasicAWSCredentials(Config.AccessKey, Config.SecretKey);
        const string TableName = "MyAppsTable";
        const string BackupArn = "arn:aws:dynamodb:us-east-2:184379576642:table/MyAppsTable/backup/01629944300584-37b6fdcf";
        const string BackupTableName = "MyAppsTable";

        public DynamoDbOperations()
        {
            client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USEast2);
        }
        public void CreateTable()
        {
            CreateTableRequest request = new CreateTableRequest
            {
                TableName = TableName,
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Id",
                        AttributeType = "N"
                    },
                    new AttributeDefinition
                    {
                        AttributeName = "Username",
                        AttributeType = "S"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Id",
                        KeyType = "HASH"
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "Username",
                        KeyType = "RANGE"
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 20,
                    WriteCapacityUnits = 10
                }
            };
            var response = client.CreateTable(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Table created successfully");
            }
        }

        public void InsertItem()
        {
            PutItemRequest request = new PutItemRequest
            {
                TableName = TableName,
                Item = new Dictionary<string, AttributeValue>
               {
                   {"Id", new AttributeValue{N = "7"} },
                   {"Username", new AttributeValue{S = "user"} },
                   {"CreatedAt", new AttributeValue{S = "27-4-2019"} }
               }
            };
            var response = client.PutItem(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Item added successfully");
            }
        }

        public void GetItem()
        {
            GetItemRequest request = new GetItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"Id", new AttributeValue{N = "7"} },
                    {"Username", new AttributeValue{S = "user"} }
                }
            };
            var response = client.GetItem(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                if (response.Item.Count > 0)
                {
                    Console.WriteLine("Items(s) retrived successfully");
                    foreach (var item in response.Item)
                    {
                        Console.WriteLine($"Key: {item.Key}, Value:{item.Value.S}{item.Value.N}");
                    }
                }
                else
                {
                    Console.WriteLine("No Items(s) found");
                }
            }
        }

        public void DeleteItem()
        {
            DeleteItemRequest request = new DeleteItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"Id", new AttributeValue{N = "7"} },
                    {"Username", new AttributeValue{S = "user"} }
                }
            };
            var response = client.DeleteItem(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Item has been deleted successfully");
                GetItem();
            }
        }

        public void DescribeTable()
        {
            DescribeTableRequest request = new DescribeTableRequest { TableName = TableName };
            var response = client.DescribeTable(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"TableArn: {response.Table.TableArn}");
            }
        }

        public void DeleteTable()
        {
            var response = client.DeleteTable(TableName);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Table has been deleted successfully");
                Console.WriteLine($"Table status: {response.TableDescription.TableStatus.Value}");
            }
        }
        public void BackupTable()
        {
            CreateBackupRequest request = new CreateBackupRequest { BackupName = "BKP002", TableName = TableName };
            var response = client.CreateBackup(request);
            if (response.HttpStatusCode ==  System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Backup created successfully");
                Console.WriteLine($"Backup BackupArn:{response.BackupDetails.BackupArn}");
                Console.WriteLine($"Backup BackupCreationDateTime:{response.BackupDetails.BackupCreationDateTime}");
                Console.WriteLine($"Backup BackupStatus:{response.BackupDetails.BackupStatus}");
                Console.WriteLine($"Backup BackupSizeBytes:{response.BackupDetails.BackupSizeBytes}");
            }
        }
        public void RestoreBackup()
        {
            RestoreTableFromBackupRequest request = new RestoreTableFromBackupRequest { BackupArn = BackupArn, TargetTableName = BackupTableName };
            var response = client.RestoreTableFromBackup(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Backup restored successfully");
                Console.WriteLine($"Backup Table Arn: {response.TableDescription.TableArn}");
                Console.WriteLine($"Backup Table Status: {response.TableDescription.TableStatus}");
            }
        }
    }
}
