

using Dapper;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;


namespace EmployeeGeneric.Helper
{
    public class Utility
    {
        private static readonly IConfigurationRoot _iconfiguration;
        private static readonly string? _con = string.Empty;
        private static readonly string? _secretKey = string.Empty;
        private static readonly string? _encryptionKey = string.Empty;

        private static readonly string? _host = string.Empty;
        private static readonly int _port;
        private static readonly string? _userName = string.Empty;
        private static readonly string? _password = string.Empty;
        private static readonly string? _from = string.Empty;
        private static readonly string? _loginUrl = string.Empty;

        private static readonly string? _forgetUrl = string.Empty;
        private static readonly string? _createUrl = string.Empty;


        static Utility()
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                 .AddJsonFile("appsettings.json");
            _iconfiguration = builder.Build();

            _con = _iconfiguration["ConnectionStrings:DataAccessConnection"];
            _secretKey = _iconfiguration["AppSettings:Secret"];
            _encryptionKey = _iconfiguration["AppSettings:EncryptionKey"];
            _host = _iconfiguration["AppSettings:Smtp:Host"];
            _port = Convert.ToInt32(_iconfiguration["AppSettings:Smtp:Port"]);
            _userName = _iconfiguration["AppSettings:Smtp:UserName"];
            _password = _iconfiguration["AppSettings:Smtp:Password"];
            _from = _iconfiguration["AppSettings:Smtp:From"];
            _forgetUrl = _iconfiguration["ForgetUrl"];
            _loginUrl = _iconfiguration["LoginUrl"];
        }

        public static IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_con);
            }
        }


        #region Authentication
        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            try
            {
                string storedSaltStr = Encoding.ASCII.GetString(storedSalt);
                var newPassword = DevOne.Security.Cryptography.BCrypt.BCryptHelper.HashPassword(password, storedSaltStr);
                string oldPassword = Encoding.Default.GetString(storedHash);
                return newPassword == oldPassword;
            }
            catch
            {
                throw;
            }
        }
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            var mySalt = DevOne.Security.Cryptography.BCrypt.BCryptHelper.GenerateSalt();
            passwordSalt = Encoding.ASCII.GetBytes(mySalt);

            var myHash = DevOne.Security.Cryptography.BCrypt.BCryptHelper.HashPassword(password, mySalt);
            passwordHash = Encoding.ASCII.GetBytes(myHash);
        }

        #endregion

        #region DataTable helpers

        public static DataTable ConvertToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType;
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        #endregion

        public static string GenerateOtp()
        {
            return RandomNumberGenerator.GetInt32(0, 9999).ToString("D4", CultureInfo.CurrentCulture);
        }

        public enum EmailType
        {
            FORGOTPASSWORD = 1,
            SIGNUPTHROUGHADMIN = 2,
            NOTIFICATION = 3,
            //VERIFYOTP = 2,
            //PASSWORDEXPIRED = 3,
            //ACCOUNTLOCKEDOUT = 4,
            //SIGNUP = 5,
            SIGNUPWITHPASSWORD = 6,
            // cHANGES
            CREATEPASSWORD = 7,
            LEAVEAPPLY = 8
        }

        public static async Task SendEmailAsync(EmailData emailData)
        {
            string body = string.Empty;

            //string link = forgetUrl.
            //    Replace("{{Email}}", Utilities.EncryptString(emailData.user.Email), StringComparison.InvariantCulture).
            //    Replace("{{token}}", Utilities.EncryptString(emailData.Otp), StringComparison.InvariantCulture);

            switch (emailData.EmailType)
            {
                case EmailType.FORGOTPASSWORD:
                    body = GetEmailContent("ForgotPassword").
                    Replace("{{Name}}", !string.IsNullOrEmpty(emailData.User?.Name) ? emailData.User?.Name : string.Empty, StringComparison.InvariantCulture).
                    Replace("{{otp}}", emailData.Otp, StringComparison.InvariantCulture).
                    Replace("{{Link}}", emailData.ResetPwdLink, StringComparison.InvariantCulture).
                    Replace("{{CurrentYear}}", Convert.ToString(DateTime.Now.Year, CultureInfo.InvariantCulture), StringComparison.InvariantCulture);
                    break;

                case EmailType.SIGNUPTHROUGHADMIN:
                    body = GetEmailContent("SignUpThroughAdmin").
                    Replace("{{Name}}", !string.IsNullOrEmpty(emailData.User?.Name) ? emailData.User?.Name : string.Empty, StringComparison.InvariantCulture).
                    Replace("{{Password}}", emailData.User?.Password, StringComparison.InvariantCulture).
                    Replace("{{Email}}", emailData.User?.Email, StringComparison.InvariantCulture).
                    Replace("{{FirstName}}", emailData.User?.Name, StringComparison.InvariantCulture).
                    Replace("{{Link}}", emailData.SiteLink, StringComparison.InvariantCulture).
                    Replace("{{CurrentYear}}", Convert.ToString(DateTime.Now.Year, CultureInfo.InvariantCulture), StringComparison.InvariantCulture);
                    break;

                case EmailType.NOTIFICATION:
                    body = GetEmailContent("NOTIFICATION").
                    Replace("{{Message}}", emailData.Message, StringComparison.InvariantCulture).
                    Replace("{{Header}}", emailData.Header, StringComparison.InvariantCulture).
                    Replace("{{CurrentYear}}", Convert.ToString(DateTime.Now.Year, CultureInfo.InvariantCulture), StringComparison.InvariantCulture);
                    break;

                case EmailType.SIGNUPWITHPASSWORD:
                    body = GetEmailContent("SignupWithPassword").
                    Replace("{{Name}}", !string.IsNullOrEmpty(emailData.User?.Name) ? emailData.User?.Name : string.Empty, StringComparison.InvariantCulture).
                    Replace("{{PASSWORD}}", emailData.User?.Password, StringComparison.InvariantCulture).
                    Replace("{{EMAIL}}", emailData.User.Email, StringComparison.InvariantCulture).
                    Replace("{{LINK}}", emailData.SiteLink, StringComparison.InvariantCulture).
                    Replace("{{CurrentYear}}", Convert.ToString(DateTime.Now.Year, CultureInfo.InvariantCulture), StringComparison.InvariantCulture);
                    break;
                //Changes
                case EmailType.CREATEPASSWORD:
                    body = GetEmailContent("CREATEPASSWORD").
                    Replace("{{Name}}", !string.IsNullOrEmpty(emailData.User?.Name) ? emailData.User?.Name : string.Empty, StringComparison.InvariantCulture).
                    Replace("{{otp}}", emailData.Otp, StringComparison.InvariantCulture).
                    Replace("{{Link}}", emailData.CreatePwdLink, StringComparison.InvariantCulture).
                    Replace("{{CurrentYear}}", Convert.ToString(DateTime.Now.Year, CultureInfo.InvariantCulture), StringComparison.InvariantCulture);
                    break;

                case EmailType.LEAVEAPPLY:
                    body = GetEmailContent("LEAVEAPPLY")
                        .Replace("{{FullNamePlaceholder}}", !string.IsNullOrEmpty(emailData.User?.Name) ? emailData.User?.Name : string.Empty)
                        .Replace("{{FromDatePlaceholder}}", emailData.FromDate.ToString(), StringComparison.InvariantCulture)
                        .Replace("{{ToDatePlaceholder}}", emailData.ToDate.ToString(), StringComparison.InvariantCulture)
                        .Replace("{{ReasonPlaceholder}}", emailData.Reason.ToString(), StringComparison.InvariantCulture)
                        .Replace("{{JoinDatePlaceholder}}", emailData.JoinDate.ToString(), StringComparison.InvariantCulture);
                    break;



                //case EmailType.SIGNUP:
                //    body = GetEmailContent("SignUp")
                //    .Replace("{{FirstName}}", (!string.IsNullOrEmpty(emailData.user.FirstName) ? emailData.user.FirstName : string.Empty), StringComparison.InvariantCulture)
                //    .Replace("{{Link}}", emailData.EmailVerificationLink, StringComparison.InvariantCulture)
                //    .Replace("{{email}}", emailData.user.Email, StringComparison.InvariantCulture)
                //    .Replace("{{password}}", emailData.user.Password, StringComparison.InvariantCulture)
                //    .Replace("{{CurrentYear}}", Convert.ToString(DateTime.Now.Year, CultureInfo.InvariantCulture), StringComparison.InvariantCulture);
                //    break;

                default:
                    break;
            }

            if (!string.IsNullOrEmpty(body))
            {
                await SendEmail(emailData.User?.EmailId, emailData.Subject, body, true).ConfigureAwait(false);
            }
        }


        public static async Task<dynamic> SendEmail(string sendto, string subject, string body, bool isBodyhtml = false)
        {
            string emailResponse = string.Empty;

            SmtpClient client = new(_host, _port);
            MailMessage mailMessage = new();
            try
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_userName, _password);
                client.EnableSsl = true;
                mailMessage.From = new MailAddress(_from);
                mailMessage.To.Add(sendto);
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = isBodyhtml;
                mailMessage.Subject = subject;
                //////string sTime = System.DateTime.UtcNow.AddDays(-1).ToString("dd MMM yyyy", System.Globalization.CultureInfo.InvariantCulture) + " " +
                //////        System.DateTime.UtcNow.ToShortTimeString() + " +0000"; // Fixed, from +0100 - just take UTC - works in .NET 2.0 - no need for offset

                //  mailMessage.Headers.Add("expiry-date", sTime);


                await client.SendMailAsync(mailMessage).ConfigureAwait(false);
                emailResponse = "Success";
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        //Console.WriteLine("Delivery failed - retrying in 5 seconds.");
                        Thread.Sleep(50000);
                        client.Send(mailMessage);
                    }
                    else
                    {
                        Exception exsds = new();
                        // _errorLogger.AddErrorLog("Email Services SmtpFailedRecipientsException", "Execute", ex.InnerExceptions[i].FailedRecipient, exsds, "");
                        //Console.WriteLine("Failed to deliver message to {0}",
                        //ex.InnerExceptions[i].FailedRecipient);
                        emailResponse = "SmtpFailedRecipientsException" + exsds.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                //_errorLogger.AddErrorLog("Email Services", "Execute", "", ex);
                emailResponse = "Error Occured in Email sending" + ex.Message;
            }

            return emailResponse;
        }

        public static string GetEmailContent(string TemplateName)
        {
            string response = string.Empty;
            using (IDbConnection db = Connection)
            {
                response = db.ExecuteScalar<string>("[dbo].[uspEmailTemplateContentList]", new
                {
                    TemplateName
                }, commandType: CommandType.StoredProcedure).ToString(CultureInfo.CurrentCulture);
            }
            return response;
        }

        //public static async void SendCreatPasswordMail(string email, string name, string password)
        //{
        //    string FName = Utilities.TextToProper(name);
        //    EmailData emailData = new()
        //    {
        //        EmailType = Utilities.EmailType.SIGNUPWITHPASSWORD,
        //        user = new { Email = email, Name = FName, Password = password },
        //        Subject = "New Account Added",
        //        SiteLink = loginUrl,
        //    };
        //    await SendEmailAsync(emailData);
        //}

        //Change
        public static async void SendCreatePassword(string email, string name, string link)
        {
            string FName = TextToProper(name);
            EmailData emailData = new()
            {
                EmailType = EmailType.CREATEPASSWORD,
                User = new { Email = email, Name = FName, Link = link },
                Subject = "New Account Added",
                SiteLink = _createUrl,
                CreatePwdLink = link
            };
            await SendEmailAsync(emailData);
        }



        public static string CreateRandomPassword(int length = 10)
        {
            // Create a string of characters, numbers, special characters that allowed in the password
            string validCharsCap = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            string validCharsSmall = "abcdefghijklmnopqrstuvwxyz";
            string validCharsNums = "0123456789";
            string validCharsSpecialChars = "!@#$%^&*?_-";
            Random random = new();



            // Select one random character at a time from the string
            // and create an array of chars
            char[] chars = new char[length];
            for (int i = 0; i < 4; i++)
            {
                chars[i] = validCharsCap[random.Next(0, validCharsCap.Length)];
            }
            for (int i = 4; i < 5; i++)
            {
                chars[i] += validCharsNums[random.Next(0, validCharsNums.Length)];
            }
            for (int i = 5; i < 6; i++)
            {
                chars[i] += validCharsSpecialChars[random.Next(0, validCharsSpecialChars.Length)];
            }
            for (int i = 6; i < 10; i++)
            {
                chars[i] += validCharsSmall[random.Next(0, validCharsSmall.Length)];
            }

            return new string(chars);
        }

        public static string TextToProper(string text)
        {
            string ProperText;
            if (!string.IsNullOrEmpty(text))
            {
                ProperText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
            }
            else
            {
                ProperText = string.Empty;
            }
            return ProperText;
        }

        public class EmailData
        {
            public EmailType EmailType { get; set; }
            public dynamic? User { get; set; }
            public string? Subject { get; set; }
            public string? ResetPwdLink { get; set; }
            public string? SiteLink { get; set; }
            public string? EmailVerificationLink { get; set; }
            public bool? IsBodyHtml { get; set; } = false;
            public string? Otp { get; set; }
            public string? Message { get; set; } = "";
            public string? Header { get; set; } = "";
            public string? CreatePwdLink { get; set; }
            public string? FromDate { get; set; }
            public string? Reason { get; set; }
            public int? LeaveType { get; set; }
            public string? ToDate { get; set; }
            public DateTime JoinDate { get; set; }
        }
    }
}
