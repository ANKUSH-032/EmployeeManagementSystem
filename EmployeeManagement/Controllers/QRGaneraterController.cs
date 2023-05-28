using Core.Model;
using CORE.Interface;
using EmployeeManagement.Services;
using iTextSharp.text.pdf.qrcode;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using QRCode = QRCoder.QRCode;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRGaneraterController : Controller
    {
#pragma warning disable


        private readonly IQRGaneraterRepository _qrGaneraterRepository;
        public QRGaneraterController(IQRGaneraterRepository qrGaneraterRepository)
        {
            _qrGaneraterRepository = qrGaneraterRepository;
        }

        [NonAction]
        public byte[] ImageToByteArray(Image imageIn)
        {
            MemoryStream meroryStream = new();
            imageIn.Save(meroryStream, ImageFormat.Jpeg);
            return meroryStream.ToArray();
        }

        [HttpPost]
        public async Task<IActionResult> QRGeneratorCode([FromBody] EmployeeSalaryGetId employeeSalaryGet)
        {
            try
            {
                var res = await _qrGaneraterRepository.QRCodeGenerator(employeeSalaryGet);

                EmployeeGetFormPDF employeeSalaryGetDetails = res.Data;
                if (res.Data == null)
                {
                    return Ok("No such record exists with this details.");
                }

                QRCodeGenerator qrCodeGenerator = new();
                string data = "Name : " + employeeSalaryGetDetails.Name + "\n DOB : " + employeeSalaryGetDetails.DayofBirth.ToString() + "\n Email Address : " + employeeSalaryGetDetails.EmailId.ToString();
                QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new(qrCodeData);
                Bitmap qrCodeAsAsciiArt = qrCode.GetGraphic(20);

                var bytes = ImageToByteArray(qrCodeAsAsciiArt);


                string fileType = "image/tmp";
                string documentName = "QRCodeEmployeeSalary.jpeg";
                string filePath = "SaveFile";
                //string pdfByteSave = Utilitiess.ConvertHtmlToPDF(html);

                using var ms = new MemoryStream(bytes);

                IFormFile file = new FormFile(ms, 0, ms.Length, "t", "t");
                SystemStorageService systemStorageService = new();
                var saveFile = systemStorageService.Upload(file, documentName, filePath);

                return File(bytes, fileType, documentName);
               
            }
            catch (Exception ex)
            {
                return StatusCode(CrudOperation.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);

            }
        }
    }
}
