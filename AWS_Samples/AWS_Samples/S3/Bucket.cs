using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS_Samples.S3
{
    class Bucket : IDisposable
    {
        const string bucketName = "testbucketr1";
        const string objectName = "Doc.txt";

        AmazonS3Client client;
        BasicAWSCredentials credentials;
        TransferUtility transferUtil;

        public Bucket()
        {
            credentials =
              new BasicAWSCredentials(Config.AccessKey, Config.SecretKey);
            client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USEast2);
            transferUtil = new TransferUtility(client);
        }

        public async void CreateBucket(string name)
        {
            if (await AmazonS3Util.DoesS3BucketExistV2Async(client, name))
            {
                Console.WriteLine("Bucket already exists");
            }
            else
            {
                var bucket = new PutBucketRequest { BucketName = name, UseClientRegion = true };
                var bucketResponse = await client.PutBucketAsync(bucket);
                if (bucketResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine("Bucket Created Successfully");
                }
            }
        }
        public void UploadFileWithPermissions()
        {
            var path = @"C:\Users\rites\OneDrive\Desktop\Files\Doc.txt";
            var fileTransferRequest = new TransferUtilityUploadRequest
            {
                FilePath = path,
                CannedACL = S3CannedACL.PublicRead,
                BucketName = bucketName
            };

            transferUtil.Upload(fileTransferRequest);
            Console.WriteLine("File Uploaded Successfully with PErmissions");
        }

        public void UploadDirectory()
        {
            var path = @"C:\Users\rites\OneDrive\Desktop\Files\New folder";
            transferUtil.UploadDirectory(path, bucketName);
            Console.WriteLine("Directory Uploaded Successfully");
        }
        public void UploadFile()
        {
            var path = @"C:\Users\rites\OneDrive\Desktop\Files\Doc.txt";
            transferUtil.Upload(path, bucketName);
            Console.WriteLine("File Uploaded Successfully");
        }

        public void List()
        {
            var bucketList = client.ListBuckets();
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

        public async void DownloadFile()
        {
            var content = string.Empty;
            var request = new GetObjectRequest()
            {
                BucketName = bucketName,
                Key = "Doc.txt"
            };

            using (GetObjectResponse response = await client.GetObjectAsync(request))
            {
                using (Stream stream = response.ResponseStream)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        var contentType = response.Headers["Content-Type"];
                        content = reader.ReadToEnd();
                        Console.WriteLine("Content :");
                        Console.WriteLine(content);
                        Console.WriteLine("File Content Type : " + contentType);
                    }
                }
            }
        }

        public void GeneratePresignedUrl()
        {
            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = objectName,
                Expires = DateTime.Now.AddHours(1)
            };

            var url = client.GetPreSignedURL(request);
            Console.WriteLine(url);
        }

        public void GetObjectTagging()
        {
            GetObjectTaggingRequest request = new GetObjectTaggingRequest
            {
                BucketName = bucketName,
                Key = objectName
            };

            GetObjectTaggingResponse response = client.GetObjectTagging(request);
            if(response.Tagging.Count == 0)
            {
                Console.WriteLine("No tags found.");
            }

            foreach (var tag in response.Tagging)
            {
                Console.WriteLine($"Tag Key : {tag.Key}, Value : {tag.Value}");
            }
        }

        public void UpdateObjectTagging()
        {
            GetObjectTagging();
            Tagging tags = new Tagging();
            tags.TagSet = new List<Tag>
            {
                new Tag { Key = "Key1", Value = "Value1"},
                new Tag { Key = "Key2", Value = "Value2"},
            };

            PutObjectTaggingRequest request = new PutObjectTaggingRequest()
            {
                BucketName = bucketName,
                Key = objectName,
                Tagging = tags
            };

            PutObjectTaggingResponse response = client.PutObjectTagging(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Object tags updated successfully.");
            }

            GetObjectTagging();
        }

        public void UpdateObjectACL()
        {
            PutACLRequest request = new PutACLRequest
            {
                BucketName = bucketName,
                Key = objectName,
                CannedACL = S3CannedACL.PublicRead
            };

            PutACLResponse response = client.PutACL(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Permissions updated.");
            }
        }

        public void BucketVersioning()
        {
            PutBucketVersioningRequest request = new PutBucketVersioningRequest
            {
                BucketName = bucketName,
                VersioningConfig = new S3BucketVersioningConfig { EnableMfaDelete = false, Status = VersionStatus.Enabled}
            };

            PutBucketVersioningResponse response = client.PutBucketVersioning(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK) 
            {
                Console.WriteLine("Bucket versioning enabled");
            }
        }

        public void BucketAccelerate()
        {
            PutBucketAccelerateConfigurationRequest request = new PutBucketAccelerateConfigurationRequest
            {
                BucketName = bucketName,
                AccelerateConfiguration = new AccelerateConfiguration { Status = BucketAccelerateStatus.Enabled }
            };

            PutBucketAccelerateConfigurationResponse response = client.PutBucketAccelerateConfiguration(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Bucket Accelerate enabled");
            }
        }
    }
}
