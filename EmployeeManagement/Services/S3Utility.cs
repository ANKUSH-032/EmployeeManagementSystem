using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace EmployeeManagement.Services
{
    public class S3Utility
    {
        private readonly string? _rootBucketName;
        private readonly string? _accessKey;
        private readonly string? _secretKey;
        private readonly string? _regionEndPoint;
        private readonly AmazonS3Client _client;

        public S3Utility(string? rootBucketName, string? accessKey, string? secretKey, string? regionEndPoint)
        {
            _rootBucketName = rootBucketName;
            _accessKey = accessKey;
            _secretKey = secretKey;
            _regionEndPoint = regionEndPoint;
        }
        public async Task<string> Download(string filePath, int ExpireIn)
        {
            return await Task.Run(() =>
            {
                try
                {
                    string DocUrl = _client.GetPreSignedURL(new GetPreSignedUrlRequest
                    {
                        BucketName = _rootBucketName,
                        Key = filePath,
                        Expires = DateTime.UtcNow.AddMinutes(ExpireIn)

                    });

                    return DocUrl;
                }
                catch (Exception)
                {
                    throw;
                }
            });
        }

        public async Task<string> Upload(IFormFile formFile, string fileName, string filePath)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_client);



                using var memoryStream = new MemoryStream();



                await formFile.CopyToAsync(memoryStream);
                await fileTransferUtility.UploadAsync(memoryStream, _rootBucketName + "/" + filePath, fileName);
                fileTransferUtility.Dispose();



                return filePath + "/" + fileName;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task UploadByPath(string filePath, string fileName, string rootBucketPath)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_client);



                byte[] file = File.ReadAllBytes(filePath);
                Stream stream = new MemoryStream(file);
                await fileTransferUtility.UploadAsync(stream, _rootBucketName + "/" + rootBucketPath, fileName);



                fileTransferUtility.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static bool ValidateExtensions(List<string> lstExtensions, string currentExtension)
        {
            return lstExtensions.Contains(currentExtension.ToUpper());
        }



        public async Task Delete(string filePath)
        {
            try
            {
                await _client.DeleteObjectAsync(new DeleteObjectRequest() { BucketName = _rootBucketName, Key = filePath });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
