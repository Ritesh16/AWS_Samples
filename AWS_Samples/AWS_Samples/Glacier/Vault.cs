using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Glacier;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.Glacier.Model;
using System.IO;

namespace AWS_Samples.Glacier
{
    public class Vault
    {
        AmazonGlacierClient client;
        BasicAWSCredentials credentials;
        string vaultName = "Vaut1";
        public Vault()
        {
            credentials =
              new BasicAWSCredentials(Config.AccessKey, Config.SecretKey);
            client = new AmazonGlacierClient(credentials, Amazon.RegionEndpoint.USEast2);
        }

        public void Create()
        {
            CreateVaultRequest createVaultRequest = new CreateVaultRequest
            {
                AccountId = "-",
                VaultName = vaultName
            };

            var response = client.CreateVault(createVaultRequest);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Vault created successfully.");
            }
            else
            {
                Console.WriteLine("PRoblem in creating vault.");
            }
        }

        public void UploadFile()
        {
            var path = @"C:\Users\rites\OneDrive\Desktop\Files\Doc.txt";
            var stream = File.OpenRead(path);

            UploadArchiveRequest uploadFileRequest = new UploadArchiveRequest()
            {
                AccountId= "-",
                VaultName = vaultName,  
                ArchiveDescription = "Test Description",
                Checksum = TreeHashGenerator.CalculateTreeHash(stream),
                Body = stream
            };

            uploadFileRequest.StreamTransferProgress += StreamProgress;
            var response = client.UploadArchive(uploadFileRequest);
            if(response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("File archived successfully.");
                Console.WriteLine("Request Id : " + response.ResponseMetadata.RequestId);
                foreach (var item in response.ResponseMetadata.Metadata)
                {
                    Console.WriteLine($"Key : {item.Key} , Value : {item.Value}");
                }
            }
        }

        private void StreamProgress(object sender, StreamTransferProgressArgs e)
        {
            Console.WriteLine("PErcentage Done : " + e.PercentDone);
            Console.WriteLine("Total Transfer : " + e.TransferredBytes/e.TotalBytes);

            Console.WriteLine("Increment Transfer : " + e.IncrementTransferred);

        }
    }
}
