using JobExchange.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using JobExchange.Areas.Identity.Data;
using JobExchange.Helper;
using Stripe;
using Microsoft.Extensions.DependencyInjection;
using JobExchange.Repository.RepositoryInterfaces;
using JobExchange.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddRouting();

builder.Services.AddDbContext<JobExchangeContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("JobExchange_Connection")));

builder.Services.AddIdentity<JobExchangeUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<JobExchangeContext>()
    .AddDefaultTokenProviders();

var mailsettings = builder.Configuration.GetSection("MailSettings");
builder.Services.AddOptions();  // Kích hoạt Options
builder.Services.Configure<MailSettings>(mailsettings);  // đăng ký để Inject

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();
builder.Services.AddScoped<ISaveJobRepository, SaveJobRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IRecruitmentRepository, RecruitmentRepository>();
builder.Services.AddScoped<IIndustryRepository, IndustryRepository>();
builder.Services.AddScoped<IRecruitmentRepository, RecruitmentRepository>();
builder.Services.AddScoped<ICandidateRecruitmentRepository, CandidateRecruitmentRepository>();

// Chuyển hướng người dùng
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/AccessDenied";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Job}/{action=Index}/{id?}");


app.Run();
