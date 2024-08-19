namespace Application.Helper
{
    public static class EnvironmentVariablesHelper
    {
        public static string GetUrlKafka()
        {

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                return Environment.GetEnvironmentVariable("URL_KAFKA");
            }
                return "localhost:9092";
        }
        public static string GetGroupIDKafka()
        {

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                return Environment.GetEnvironmentVariable("GroupID_KAFKA");
            }
            return "1";
        }
    }
}
