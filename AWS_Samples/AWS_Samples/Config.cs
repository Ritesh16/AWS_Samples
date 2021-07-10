using System.Configuration;

namespace AWS_Samples
{
    static class Config
    {
        public static string AccessKey { 
            get
            {
                return ConfigurationManager.AppSettings["AccessKey"];
            }
        }

        public static string SecretKey
        {
            get
            {
                return ConfigurationManager.AppSettings["SecretKey"];
            }
        }
    }
}
