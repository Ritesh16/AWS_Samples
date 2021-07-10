using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS_Samples.S3
{
    class Bucket : IDisposable
    {
        const string bucketName = "myapp564";
        const string objectName = "test.txt";

        AmazonS3Client client;
        public BasicAWSCredentials credentials =
              new BasicAWSCredentials(ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secertKey"]);
        TransferUtility transferUtil;

        public Bucket()
        {
            client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USWest1);
            transferUtil = new TransferUtility(client);
        }

        public async void CreateBucket()
        {
            if (await AmazonS3Util.DoesS3BucketExistV2Async(client, bucketName))
            {
                Console.WriteLine("Bucket already exists");
            }
            else
            {
                var bucket = new PutBucketRequest { BucketName = bucketName, UseClientRegion = true };
                var bucketResponse = await client.PutBucketAsync(bucket);
                if (bucketResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine("Bucket Created Successfully");
                }
            }
        }
        public void UploadFile()
        {
            //transfareUtil.UploadDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\files",bucketName);
            //transfareUtil.Upload(AppDomain.CurrentDomain.BaseDirectory + "\\test.txt",bucketName);
            var fileTransferRequest = new TransferUtilityUploadRequest
            {
                FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\test.txt",
                CannedACL = S3CannedACL.PublicRead,
                BucketName = bucketName
            };
            transferUtil.Upload(fileTransferRequest);
            Console.WriteLine("File Uploaded Successfully");
        }

        public async void List()
        {

            var bucketList = await client.ListBucketsAsync();
            foreach (var bucket in bucketList.Buckets)
            {
                Console.WriteLine($"{bucket.BucketName} {bucket.CreationDate.ToShortDateString()}");
            }
        }
        public void Dispose()
        {
            Console.WriteLine("Dispose");
            client.Dispose();
        }
    }
}
