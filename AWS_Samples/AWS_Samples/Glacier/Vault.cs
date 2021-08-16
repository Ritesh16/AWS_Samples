using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Glacier;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.Glacier.Model;

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
    }
}
