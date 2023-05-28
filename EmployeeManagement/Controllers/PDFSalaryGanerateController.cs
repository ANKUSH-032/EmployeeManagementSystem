using Core.Interface;
using Core.Model;
using CrudOperation;
using EmployeeGeneric.Helper;
using EmployeeGeneric.Utilities;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]"), Authorize, ActivityLog]
    [ApiController]
    public class PDFSalaryGanerateController : Controller
    {

        private readonly ILogger<pdfSalaryController> _logger;
        private readonly IEmployeeSalaryRepository _employeeSalaryRepository;
        public PDFSalaryGanerateController(ILogger<pdfSalaryController> logger, IEmployeeSalaryRepository employeeSalaryRepository)
        {

            _logger = logger;
            _employeeSalaryRepository = employeeSalaryRepository;
        }
        [NonAction]
        public byte[] ImageToByteArray(Image imageIn)
        {
            MemoryStream meroryStream = new();
            imageIn.Save(meroryStream, ImageFormat.Jpeg);
            return meroryStream.ToArray();
        }

        [HttpGet, Route("report/pdf")]
        public async Task<IActionResult> GetEmployeeSalaryPDF([FromQuery] EmployeeSalaryGetId employeeGetDetails)
        {
            var response = await _employeeSalaryRepository.GetEmployeeSalaryPDF(employeeGetDetails);
            EmployeeGetFormPDF employeeSalaryGetDetails = response.Data;


            if (response.Data == null)
            {
                return Ok("No such record exists with this details.");
            }
            QRCodeGenerator qrCodeGenerator = new();
            string data = "Name : " + employeeSalaryGetDetails.Name + "\n DOB : " + employeeSalaryGetDetails.DayofBirth.ToString() + "\n Email Address : " + employeeSalaryGetDetails.EmailId.ToString();
            QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new(qrCodeData);
            Bitmap qrCodeAsAsciiArt = qrCode.GetGraphic(20);

            var bytes = ImageToByteArray(qrCodeAsAsciiArt);
            string fileType = "image/tmp";    // Type of File
            string documentName = "QRCodeEmployeeSalary.jpeg";  // Name of File
            string filePath = "SaveFile";
            //string pdfByteSave = Utilitiess.ConvertHtmlToPDF(html);

            using var ms = new MemoryStream(bytes); // Using Memory Storege

            IFormFile file = new FormFile(ms, 0, ms.Length, "t", "t");
            SystemStorageService systemStorageService = new();
            var saveFile = systemStorageService.Upload(file, documentName, filePath);



            var html = System.IO.File.ReadAllText("F:\\Projects\\EmployeeManagentSystemTask\\EmployeeTask\\EmployeeManagement\\htmlpage.html");

            html = html.Replace("@CompanyName", employeeSalaryGetDetails.CompanyName);
            html = html.Replace("@CompanyAddress", employeeSalaryGetDetails.CompanyAddress);
            html = html.Replace("@EmployeeId", employeeSalaryGetDetails.EmployeeId?.ToString());
            html = html.Replace("@EmployeeName", employeeSalaryGetDetails.Name);
            html = html.Replace("@DayofBirth", employeeSalaryGetDetails.DayofBirth.ToString());
            html = html.Replace("@EmailId", employeeSalaryGetDetails.EmailId);
            html = html.Replace("@Gender", employeeSalaryGetDetails.Gender);
            html = html.Replace("@PhoneNumber", employeeSalaryGetDetails.PhoneNumber.ToString());
            html = html.Replace("@Address", employeeSalaryGetDetails.FullAddress?.ToString());
            html = html.Replace("@DesignationName", employeeSalaryGetDetails.DesignationName);
            html = html.Replace("@FatherName", employeeSalaryGetDetails.FatherName);
            html = html.Replace("@BasicSalary", employeeSalaryGetDetails.BasicSalary.ToString());
            html = html.Replace("@EmployeesProvidentFund", employeeSalaryGetDetails.EmployeesProvidentFund.ToString());
            html = html.Replace("@HouseAllowance", employeeSalaryGetDetails.HouseAllowance.ToString());
            html = html.Replace("@PF", employeeSalaryGetDetails.PF.ToString());
            html = html.Replace("@SpecialAllowance", employeeSalaryGetDetails.SpecialAllowance.ToString());
            html = html.Replace("@ToxicologicalRiskAssessments", employeeSalaryGetDetails.ToxicologicalRiskAssessments.ToString());
            html = html.Replace("@ConveyanceAllowance", employeeSalaryGetDetails.ConveyanceAllowance.ToString());
            html = html.Replace("@HouseRentAllowance", employeeSalaryGetDetails.HouseRentAllowance.ToString());
            html = html.Replace("@TaxDeductedAtSource", employeeSalaryGetDetails.TaxDeductedAtSource.ToString());
            html = html.Replace("@TotalAllowance", employeeSalaryGetDetails.TotalAllowance.ToString());
            html = html.Replace("@TotalDeduction", employeeSalaryGetDetails.TotalDeduction.ToString());
            html = html.Replace("@NetSalary", employeeSalaryGetDetails.NetSalary.ToString());
            html = html.Replace("@monthName", employeeSalaryGetDetails.MonthName);
            html = html.Replace("@year", employeeSalaryGetDetails.Year.ToString());

            string fileTypePDF = "application/pdf"; // Type of File

            string documentNameQR = "EmployeeSalary.pdf"; // Name of File

            string sourcePath = Path.GetFullPath("~StorageFile");
            string filePathPDF = "SaveFilePdf";
            byte[] pdfByte = Utilitiess.ConvertHtmlToPDF(html); // Convert HTML to PDF

            ////string pdfByteSave = Utilitiess.ConvertHtmlToPDF(html);
            // using var ms = new MemoryStream(pdfByte);

            IFormFile filePDF = new FormFile(ms, 0, ms.Length, "t", "t"); // Save file in local storege using IFormFile
            //SystemStorageService systemStorageService = new ();      // Creeate Object Local Storege code fro unding Upload Method
            var saveFilePDF = systemStorageService.Upload(filePDF, documentNameQR, filePathPDF);   // Save File in local Savefilepdf

            return File(pdfByte, fileTypePDF, documentNameQR);
        }

    }
}
