using Core.Interface;
using Core.Interface.Helper;
using Core.Model;
using CORE.Interface;
using DinkToPdf;
using DinkToPdf.Contracts;
using EmployeeGeneric.Helper;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Helper;
using log4net.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServiceStack;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using FluentAssertions.Common;
using CrudOperation;
using CORE.Comman;
using DocumentFormat.OpenXml.Presentation;
using Swashbuckle.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
#pragma warning disable

builder.Services.AddSingleton(typeof(IConverter),
    new SynchronizedConverter(new PdfTools()));

#region LOG 4 NET
builder.Services.AddLogging(loggingBuilder =>
{
    // Configure Log4Net
    log4net.Config.XmlConfigurator.Configure();
    XmlConfigurator.Configure(new FileInfo("log4net.config"));

    // Add Log4Net as the logging provider
    loggingBuilder.AddLog4Net();
});
#endregion

#region Model state 

builder.Services.AddControllers(config =>
{
    config.Filters.Add(new ValidationFilter());
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
#endregion

//builder.Services.AddControllers(config =>
//{
//config.Filters.Add(new ValidationFilter());
//}).AddJsonOptions(options =>
//{
//options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
//});

#region Cors Policy blocked
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("CorsPolicy",
//    options =>
//    {
//        options.WithOrigins(builder.Configuration.GetSection("APICallerURL").Get<string>()).AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
//    });
//});
#endregion

#region allow cors policy
builder.Services.AddCors(options =>
{
    // options.AddPolicy("AllowSpecificOrigin",
     options.AddPolicy("AllowAllOrigins",
         builder =>
        {
            //builder.WithOrigins("http://localhost:4200")
            //       .AllowAnyHeader()
            //       .AllowAnyMethod();
            builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
        });
});
#endregion



#region JWT Authentication
var secretKey = builder.Configuration.GetSection("AppSettings:Secret").Value;
var key = Encoding.ASCII.GetBytes(secretKey);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            var userId = context?.Principal?.Identity?.Name;

            var user = Authenticates.GetUserDetails<User>(UserId: userId);

            if (user == null)
            {
                // return unauthorized if user no longer exists
                context?.Fail("Unauthorized");
            }
            return Task.CompletedTask;
        }
    };
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
       IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});
#endregion

/// <summary>
/// Add JWT Token
/// </summary>
/// <returns></returns>

builder.Services.AddDataProtection();

builder.Services.AddSwaggerGen(setup =>
{
    setup.EnableAnnotations();
    setup.SwaggerDoc("v1", new OpenApiInfo { Title = "EmployeeManagement.API", Version = "v1" });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                {
                        jwtSecurityScheme, Array.Empty<string>() }
                });
});

#region API Versioning 
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
   // options.ApiVersionReader = new UrlSegmentApiVersionReader();
});
builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

#endregion

/// <summary>
/// Add Controller 
/// </summary>
/// <returns></returns>

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ValidationFilter>();
builder.Services.AddScoped<ICrudOperationService>(x => new CrudOperationDataAccess(builder.Configuration, builder.Configuration["ConnectionStrings:DataAccessConnection"]));

//builder.Services.AddScoped<ICrudOperationService>(x => new CrudOperationDataAccess(builder.Configuration, builder.Configuration["ConnectionStrings:DataAccessConnection"]));
//builder.Services.AddScoped<ICrudOperationService>(x => new DataAccess(builder.Configuration, builder.Configuration["ConnectionStrings:DataAccessConnection"]));
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IAttendanceReportRepository, AttendanceReportRepository>();
builder.Services.AddScoped<IDeductionRepository, DeductionRepository>();
builder.Services.AddScoped<IAllowanceRepository, AllowanceRepository>();
builder.Services.AddScoped<IEmployeeSalaryRepository, EmployeeSalaryRepository>();
builder.Services.AddScoped<IUserRepositroy, UserRepositroy>();
builder.Services.AddScoped<IDataProtectionRepository, DataProtectionRepository>();
builder.Services.AddScoped<ILeaveRepository, LeaveRepository>();
builder.Services.AddScoped<IQRGaneraterRepository, QRGaneraterRepository>();
builder.Services.AddScoped<ICommanDDLRepository, CommanDDLRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EMP v1"));
}




/// <summary>
/// Use Authentication and Authorization for Authoriz the APi
/// </summary>
/// <returns></returns>
///
//app.UseStaticFiles();
//app.UseHttpsRedirection();

//app.UseCors("CorsPolicy");
//app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
//app.UseCors("AllowSpecificOrigin");
app.UseCors("AllowAllOrigins");
app.MapControllers();
app.Run();

