using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;



namespace EmployeeGeneric.Helper
{
    public class ActivityLogAttribute : ActionFilterAttribute
    {
        private readonly string _controller = "UserActivityLog";

        public override async void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Stores the Request in an Accessible object
            var request = filterContext.HttpContext.Request;
            //var response = filterContext.HttpContext.Response;

            try
            {
                // Generate the log of user activity
                UserActivityLog log = new()
                {
                    UserID = filterContext.HttpContext.User.Identity?.Name ?? "Anonymous",
                    IpAddress = request.HttpContext.Connection?.RemoteIpAddress?.ToString(),
                    AreaAccessed = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(request),
                    TimeStamp = DateTime.UtcNow,
                    Body = await ReadBodyAsString(request).ConfigureAwait(false),
                    StatusCode = filterContext.HttpContext.Response.StatusCode,
                    Method = request.Method
                };


                //saves the log to database
                Logger.SaveLog(log);

                // Finishes executing the Action as normal 
                base.OnActionExecuted(filterContext);
            }
            catch (Exception ex)
            {
                Logger.AddErrorLog(_controller, ex.TargetSite?.ToString(), filterContext.HttpContext.User.Identity?.Name ?? "Anonymous", ex);
            }
        }

        private async static Task<string> ReadBodyAsString(HttpRequest request)
        {
            var initialBody = request.Body; // Workaround
                                            // if (initialBody == null)
            initialBody ??= (Stream)request.Form;
            try
            {
                using StreamReader reader = new(request.Body);

                string text = await reader.ReadToEndAsync().ConfigureAwait(false);
                return text;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                // Workaround so MVC action will be able to read body as well
                request.Body = initialBody;
            }
        }
    }
}
