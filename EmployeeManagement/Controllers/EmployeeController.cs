using App.Metrics.Formatters.Prometheus;
using BenchmarkDotNet.Reports;
using Core.Interface;
using Core.Model;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using EmployeeGeneric.Helper;
using EmployeeGeneric.Utilities;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prometheus;
using Swashbuckle.Swagger.Annotations;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]"), Authorize, ActivityLog]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHostingEnvironment _hostingEnvironment;
        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeRepository employeeController, IWebHostEnvironment webHostEnvironment, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _employeeRepository = employeeController;
            _webHostEnvironment = webHostEnvironment;
            _hostingEnvironment = hostingEnvironment;
        }

        // [SwaggerOperation(
        //  Summary = "Employee Insert", Description = "This API will allow to add employee into system.", OperationId = "")]
        [HttpPost, Route("insert")]
        public async Task<IActionResult> EmployeeInsert([FromQuery] EmployeeInsert employeeInsert)
        {
            #region Insert Employee Info
            try
            {
                #region Upload file in local storege and save path code

                if (employeeInsert.AttachmentPhoto.Length > 0 && (employeeInsert.AttachmentSignature.Length > 0))
                {
                    if (!Directory.Exists(_webHostEnvironment.WebRootPath + "\\UploadPhoto\\"))
                    {
                        Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "\\UploadPhoto\\");
                    }
                    using (FileStream fileStream = System.IO.File.Create(_webHostEnvironment.WebRootPath + "\\UploadPhoto\\" /*+ Guid.NewGuid().ToString() */+ employeeInsert.AttachmentPhoto.FileName))
                    {
                        employeeInsert.AttachmentPhoto.CopyTo(fileStream);
                        fileStream.Flush();
                        // Uplaod path in database
                        employeeInsert.Photopath = "F:\\Projects\\EmployeeManagentSystemTask\\EmployeeTask\\EmployeeManagement\\wwwroot\\"+ "UploadPhoto" + "\\" /*+ Guid.NewGuid().ToString() + " / "*/ + employeeInsert.AttachmentPhoto.FileName;

                    }
                    if (!Directory.Exists(_webHostEnvironment.WebRootPath + "\\UploadSign\\"))
                    {
                        Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "\\UploadSign\\");
                    }
                    using (FileStream fileStream = System.IO.File.Create(_webHostEnvironment.WebRootPath + "\\UploadSign\\" /*+ Guid.NewGuid().ToString()*/ + employeeInsert.AttachmentSignature.FileName))
                    {
                        employeeInsert.AttachmentSignature.CopyTo(fileStream);
                        fileStream.Flush();
                        // Uplaod path in database
                        employeeInsert.SignaturePath = "F:\\Projects\\EmployeeManagentSystemTask\\EmployeeTask\\EmployeeManagement\\wwwroot\\"+ "UploadSign" + "\\" /*+ Guid.NewGuid().ToString() + " / "*/ + employeeInsert.AttachmentSignature.FileName;
                    }
                    #endregion
                }
                var res = await _employeeRepository.EmployeeInsert(employeeInsert);
                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                Logger.AddErrorLog(ControllerContext.ActionDescriptor.ControllerName, "Employee Insert", User.Identity.Name, ex);
                return StatusCode(CrudOperations.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
            #endregion
        }
        [HttpPost, Route("update")]
        public async Task<IActionResult> EmployeeUpdate([FromQuery] EmployeeUpdate employeeUpdate)
        {
            #region Update Employee Info
            try
            {
                #region Upload file in local storege and save path code
                if (employeeUpdate.AttachmentPhoto.Length > 0 && (employeeUpdate.AttachmentSignature.Length > 0))
                {
                    if (!Directory.Exists(_webHostEnvironment.WebRootPath + "\\UploadPhoto\\"))     
                    {
                        Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "\\UploadPhoto\\");
                    }
                    using (FileStream fileStream = System.IO.File.Create(_webHostEnvironment.WebRootPath + "\\UploadPhoto\\" + /*Guid.NewGuid().ToString()+ */employeeUpdate.AttachmentPhoto.FileName))
                    {
                        employeeUpdate.AttachmentPhoto.CopyTo(fileStream);
                        fileStream.Flush();
                        employeeUpdate.PhotoPath = "F:\\Projects\\EmployeeManagentSystemTask\\EmployeeTask\\EmployeeManagement\\wwwroot\\"+ "UploadPhoto" + "\\"/* + Guid.NewGuid().ToString() + "\\"*/ + employeeUpdate.AttachmentPhoto.FileName;

                    }
                    if (!Directory.Exists(_webHostEnvironment.WebRootPath + "\\UploadSign\\"))
                    {
                        Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "\\UploadSign\\");
                    }
                    using (FileStream fileStream = System.IO.File.Create(_webHostEnvironment.WebRootPath + "\\UploadSign\\" + /*Guid.NewGuid().ToString() +*/  employeeUpdate.AttachmentSignature.FileName))
                    {
                        employeeUpdate.AttachmentSignature.CopyTo(fileStream);
                        fileStream.Flush();
                        // Uplaod path in database
                        employeeUpdate.SignaturePath = "F:\\Projects\\EmployeeManagentSystemTask\\EmployeeTask\\EmployeeManagement\\wwwroot\\"+ "UploadSign" + "\\"  /*+ Guid.NewGuid().ToString() + "/" */+ employeeUpdate.AttachmentSignature.FileName;
                    }
                }
                #endregion
                var res = await _employeeRepository.EmployeeUpdate(employeeUpdate);
                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                Logger.AddErrorLog(ControllerContext.ActionDescriptor.ControllerName, "Employee Update", User.Identity.Name, ex);
                return StatusCode(CrudOperations.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
            #endregion
        }

        [HttpGet]
        public async Task<IActionResult> EmployeeGetDetails([FromBody] EmployeeGetDetails employeeGetDetails)
        {
            try
            {

                var res = await _employeeRepository.EmployeeGetDetails(employeeGetDetails);
                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)

            {
                Logger.AddErrorLog(ControllerContext.ActionDescriptor.ControllerName, "Employee Get", User.Identity.Name, ex);
                return StatusCode(CrudOperations.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> EmployeeDelete([FromBody] EmployeeDelete employeeDelete)
        {
            try
            {
                var res = await _employeeRepository.EmployeeDelete(employeeDelete);
                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                Logger.AddErrorLog(ControllerContext.ActionDescriptor.ControllerName, "Employee Delete", User.Identity.Name, ex);
                return StatusCode(CrudOperations.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        [HttpGet("list")]
        public async Task<IActionResult> EmployeeList([FromQuery] Core.Comman.JqueryDataTable list)
        {
            try
            {
                var res = await _employeeRepository.EmployeeList(list);
                if (res.Status && res.Data.Count == 0)
                {
                    res.RecordsFiltered = 0;
                    res.TotalRecords = 0;
                    return StatusCode(200, res);
                }
                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                Logger.AddErrorLog(ControllerContext.ActionDescriptor.ControllerName, "Employee List", User.Identity.Name, ex);
                return StatusCode(CrudOperations.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }

        }
        [HttpGet("details/pdf")]
        public async Task<IActionResult> GetEmployeeFormPDF(string employeeId)
        {
            var res = await _employeeRepository.GetEmployeeFormPDF(employeeId);
            EmployeeGetFormPDF getEmployeeFormPDF = res.Data;
            if (res.Data == null)
            {
                return Ok("No such record exists with this details.");
            }

            var html = System.IO.File.ReadAllText("F:\\Projects\\EmployeeManagentSystemTask\\EmployeeTask\\EmployeeManagement\\employeeform.html");

            html = html.Replace("@CompanyName", getEmployeeFormPDF.CompanyName);
            html = html.Replace("@CompanyAddress", getEmployeeFormPDF.CompanyAddress);
            html = html.Replace("@EmployeeId", getEmployeeFormPDF.EmployeeId?.ToString());
            html = html.Replace("@EmployeeName", getEmployeeFormPDF.Name);
            html = html.Replace("@DayofBirth", getEmployeeFormPDF.DayofBirth.ToString());
            html = html.Replace("@EmailId", getEmployeeFormPDF.EmailId);
            html = html.Replace("@Gender", getEmployeeFormPDF.Gender);
            html = html.Replace("@PhoneNumber", getEmployeeFormPDF.PhoneNumber.ToString());
            html = html.Replace("@Address", getEmployeeFormPDF.FullAddress?.ToString());
            html = html.Replace("@DesignationName", getEmployeeFormPDF.DesignationName);
            html = html.Replace("@FatherName", getEmployeeFormPDF.FatherName);
            html = html.Replace("@FatherOccupation", getEmployeeFormPDF.FatherOccupation);
            html = html.Replace("@MotherName", getEmployeeFormPDF.MotherName);
            html = html.Replace("@MotherOcupation", getEmployeeFormPDF.MotherOcupation);
            html = html.Replace("@MaritalStatus", getEmployeeFormPDF.MaritalStatus);
            html = html.Replace("@JoinDate", getEmployeeFormPDF.JoinDate);
            html = html.Replace("@CurrentExperience", getEmployeeFormPDF.CurrentExperience);
            html = html.Replace("@HomePhoneNumber", getEmployeeFormPDF.HomePhoneNumber.ToString());
            html = html.Replace("@Qualification", getEmployeeFormPDF.Qualification);
            html = html.Replace("@PhotoPath", getEmployeeFormPDF.Photopath);
            html = html.Replace("@SignaturePath", getEmployeeFormPDF.SignaturePath);

            string fileType = "application/pdf";
            string documentName = "EmployeeFormDetails.pdf";
            byte[] pdfByte = Utilitiess.ConvertHtmlToPDF(html);
            
            string sourcePath = Path.GetFullPath("~StorageFile");
            string filePath = "SaveFile";
           
            using var ms = new MemoryStream(pdfByte);

            IFormFile file = new FormFile(ms, 0, ms.Length, "t", "t");
            SystemStorageService systemStorageService = new();
            var saveFile = systemStorageService.Upload(file, documentName, filePath);
            return File(pdfByte, fileType, documentName);
        }
    }
}
