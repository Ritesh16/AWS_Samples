using AWS_Samples.Glacier;
using AWS_Samples.S3;
using AWS_Samples.SQS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS_Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var bucket = new SQSOperations();
            //bucket.SendMessage();
            bucket.ReceiveMessage();
            


            //bucket.CreateBucket("testbucketr2");

            Console.ReadLine();
        }
    }
}
