namespace EmployeeManagement.Services
{
    public class SystemStorageService : IS3Utility
    {
        public async Task Delete(string filePath)
        {
            try
            {
                if (Directory.Exists(filePath))
                {
                    Directory.Delete(filePath, true);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Task<string> Download(string filePath, int ExpireIn = 60)
        {
            throw new NotImplementedException();
        }
        public async Task<string> Upload(IFormFile formFile, string fileName, string filePath)
        {
            try
            {
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);



                string fullPath = Path.Combine(filePath, (!string.IsNullOrEmpty(fileName)) ? fileName : formFile.FileName);



                using var stream = File.Create(fullPath);
                await formFile.CopyToAsync(stream).ConfigureAwait(false);



                fullPath = string.Concat("\\", fullPath);
                return fullPath;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task UploadByPath(string sourceFilePath, string fileName, string filePath)
        {
            try
            {
                if (Directory.Exists(sourceFilePath))
                {
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);



                    byte[] file = File.ReadAllBytes(filePath);
                    Stream stream = new MemoryStream(file);



                    string fullPath = Path.Combine(filePath, fileName);



                    using var outputStream = File.Create(fullPath);
                    await stream.CopyToAsync(outputStream).ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
