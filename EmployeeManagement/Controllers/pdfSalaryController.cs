using Core.Interface;
using Core.Model;
using DinkToPdf;
using DinkToPdf.Contracts;
using EmployeeGeneric.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]"), Authorize, ActivityLog]
    [ApiController]
    public class pdfSalaryController : Controller
    {
        private readonly IConverter _converter;
        private readonly ILogger<pdfSalaryController> _logger;
        private readonly IEmployeeSalaryRepository _employeeSalaryRepository;
        public pdfSalaryController(ILogger<pdfSalaryController> logger, IConverter converter, IEmployeeSalaryRepository employeeSalaryRepository)
        {
            _converter = converter;
            _logger = logger;
            _employeeSalaryRepository = employeeSalaryRepository;
        }
        [HttpGet("GeneratePDF")]
        public async Task<IActionResult> GeneratePDFAsync([FromQuery] EmployeeSalaryGetId employeeGetDetails)
        {

            var res = await _employeeSalaryRepository.PDFGenerateSalary(employeeGetDetails);

            EmployeeGetFormPDF employeeSalaryGetDetails = res.Data;
           if(res.Data == null)
            {
                return Ok("No such record exists with this details.");
            }


            var html = System.IO.File.ReadAllText("F:\\Projects\\EmployeeManagentSystemTask\\EmployeeTask\\EmployeeManagement\\htmlpage.html");

            //HTML CODE FOR REPLACING DYNAMIC CONTENT IN PDF
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

            GlobalSettings globalSettings = new GlobalSettings();
            globalSettings.ColorMode = ColorMode.Color;
            globalSettings.Orientation = Orientation.Portrait;
            globalSettings.PaperSize = PaperKind.A4;
            globalSettings.Margins = new MarginSettings { Top = 25, Bottom = 10 };//bottom = 25
            ObjectSettings objectSettings = new ObjectSettings();
            objectSettings.PagesCount = true;
            objectSettings.HtmlContent = html;
            WebSettings webSettings = new WebSettings();
            webSettings.DefaultEncoding = "utf-8";
            HeaderSettings headerSettings = new HeaderSettings();
            headerSettings.FontSize = 35;
            headerSettings.FontName = "Ariel";
            headerSettings.Right = "Page [page] of [toPage]";
            headerSettings.Line = true;
            FooterSettings footerSettings = new FooterSettings();
            footerSettings.FontSize = 22;
            footerSettings.FontName = "Ariel";
            footerSettings.Center = "This is for demonstration purposes only. " + DateTime.Now;
            footerSettings.Line = true;
            objectSettings.HeaderSettings = headerSettings;
            objectSettings.FooterSettings = footerSettings;
            objectSettings.WebSettings = webSettings;
            HtmlToPdfDocument htmlToPdfDocument = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },

            };

            var pdfFile = _converter.Convert(htmlToPdfDocument);
            var pfdname = "EmployeeSalaryName" + employeeSalaryGetDetails.Name + "_" + DateTime.Now.ToString("F") + ".pdf";
            return File(pdfFile, "application/octet-stream", pfdname);
        }
    }
}
