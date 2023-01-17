namespace EmployeeManagement.Services
{
    public class S3UtilityClass
    {
    }
    public class AWSSettings
    {
        public string? AWSAccessKey { get; set; }
        public string? AWSSecretKey { get; set; }
        public string? BucketName { get; set; }
    }
    public class FileToWriteSingle
    {
        public IFormFile? File { get; set; }
        public string? Key { get; set; }
        public AWSSettings? AwsSettings { get; set; }
    }

    public class FileToRead
    {
        public string? Key { get; set; }
        public AWSSettings? AwsSettings { get; set; }
    }

    public class FileToDelete
    {
        public string? Key { get; set; }
        public AWSSettings? AwsSettings { get; set; }
    }
}
