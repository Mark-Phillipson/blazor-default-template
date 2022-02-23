using Blazored.Modal;
using Blazored.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using MSPApplicationDotNet6.UI.Areas.Identity;
using MSPApplicationDotNet6.UI.Data;
using MSPApplicationDotNet6.UI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<HttpClient>(s =>
{
	var client = new HttpClient { BaseAddress = new System.Uri("https://localhost:44340/") };
	return client;
});

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazoredToast();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
builder.Services.AddBlazoredModal();
builder.Services.AddScoped<IEmployeeDataService, EmployeeDataService>();
builder.Services.AddScoped<ICountryDataService, CountryDataService>();
builder.Services.AddScoped<IJobCategoryDataService, JobCategoryDataService>();
builder.Services.AddScoped<IExpenseDataService, ExpenseDataService>();
builder.Services.AddScoped<ITaskDataService, TaskDataService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ISurveyDataService, SurveyDataService>();
builder.Services.AddScoped<INoticeDataService, NoticeDataService>();
builder.Services.AddScoped<IUserDataService, UserDataService>();
builder.Services.AddScoped<IRoleDataService, RoleDataService>();
builder.Services.AddScoped<ICompanyDetailDataService, CompanyDetailDataService>();

builder.Services.AddSingleton<NotifierService>();
		


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");



app.Run();
