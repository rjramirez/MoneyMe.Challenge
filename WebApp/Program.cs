using Common.DataTransferObjects.AppSettings;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

/* CONFIGURE SERVICES CONTAINER */
ApiResourceUrl apiResourceUrl = new();
builder.Configuration.Bind("ApiResourceUrl", apiResourceUrl);
builder.Services.AddSingleton(apiResourceUrl);

builder.Services.AddHttpClient("ProjectApiClient", opt =>
{
    opt.Timeout = TimeSpan.FromMinutes(5);
    opt.BaseAddress = new Uri(apiResourceUrl.ProjectApiBaseUrl);
});

// Security Group Policy (You can remove it if you don't need authorization at all)
SecurityGroup securityGroup = new();
builder.Configuration.Bind("SecurityGroup", securityGroup);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Support", policy => policy.RequireClaim("groups", securityGroup.ProjectTemplateSupport));
});

// Configure forwarded headers (no changes)
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

// Removed the authorization policy to skip authorization
builder.Services.AddControllersWithViews(); // Do not add authorization filter here anymore

builder.Services.AddHttpContextAccessor();
builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(365);
});

/* CONFIGURE HTTP REQUEST PIPELINE */
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseStatusCodePagesWithRedirects("/Error/StatusPage?code={0}");
app.UseExceptionHandler("/Error/LogError");
app.UseForwardedHeaders();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// No authentication and no authorization middleware
// Removed app.UseAuthentication() and app.UseAuthorization()

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();