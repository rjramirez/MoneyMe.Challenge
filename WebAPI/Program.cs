using ApiConfiguration;
using Common.Constants;
using Common.DataTransferObjects.AppSettings;
using Common.DataTransferObjects.ErrorLog;
using Common.DataTransferObjects.Version;
using DataAccess.DBContexts.MoneyMeChallengeDB;
using DataAccess.UnitOfWorks.MoneyMeChallengeDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebAPI.Services;
using WebAPI.Services.Interfaces;

/*SERVICES CONTAINER*/
var builder = WebApplication.CreateBuilder(args);

IdentityServerApiDefinition identityServerApiDefinition = new();
builder.Configuration.Bind("IdentityServerApiDefinition", identityServerApiDefinition);
builder.Services.AddSingleton(identityServerApiDefinition);

ApiServices.ConfigureServices(builder.Services, identityServerApiDefinition);

//Api Policy Authorization
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("SystemLog", builder =>
//    {
//        builder.RequireScope("ProjectTemplateApi.SystemLog");
//    });
//});

//DBContext Registration
builder.Services.AddDbContext<MoneyMeChallengeDBContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("MoneyMeChallengeDB")));

//UoW Registration
builder.Services.AddScoped<IMoneyMeChallengeDBUnitOfWork, MoneyMeChallengeDBUnitOfWork>();

//Internal Service Registration
builder.Services.AddScoped<IErrorLogService, ErrorLogService>();

/*HTTP REQUEST PIPELINE*/
var app = builder.Build();

app.UseExceptionHandler(errorLogger =>
{
    errorLogger.Run(async context =>
    {
        var scoped = app.Services.CreateScope();
        IErrorLogService errorLogService = scoped.ServiceProvider.GetRequiredService<IErrorLogService>();
        context.Response.StatusCode = 500;
        ErrorMessageDetail unhandledErrorDetail = await errorLogService.LogApiError(context);
        await context.Response.WriteAsync(JsonConvert.SerializeObject(unhandledErrorDetail));
    });
});

app.UseSwagger();
IApiVersionDescriptionProvider apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwaggerUI(options =>
{
    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant());
        options.RoutePrefix = "docs";
    }
});

app.UseHttpsRedirection();
app.UseRouting();
//app.UseAuthentication();
//app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGet("/", async context =>
    {
        context.Response.ContentType = ApiHomePageConstant.ContentType;
        await context.Response.WriteAsync(
            string.Format(ApiHomePageConstant.ContentFormat,
            "MoneyMeAPI",
            app.Environment.EnvironmentName,
            context.Request.Scheme,
            context.Request.Host.Value,
            VersionDetail.DisplayVersion()));
    });
});

app.Run();

