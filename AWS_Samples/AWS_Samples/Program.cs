using AWS_Samples.S3;
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
            var bucket = new Bucket();
            bucket.UpdateObjectACL();
            //bucket.CreateBucket("testbucketr2");

            Console.ReadLine();
        }
    }
}
