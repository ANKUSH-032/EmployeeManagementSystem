namespace EmployeeManagement.Services
{
    public interface IS3Utility
    {
        Task<string> Download(string filePath, int ExpireIn = 60);



        Task<string> Upload(IFormFile formFile, string fileName, string filePath);



        Task UploadByPath(string sourceFilePath, string fileName, string filePath);



        Task Delete(string filePath);
    }
}
