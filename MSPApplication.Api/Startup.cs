using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MSPApplication.Data;
using MSPApplication.Data.Repositories;
using MSPApplication.Shared;
using MSPApplication.Shared.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Reflection;

namespace MSPApplication.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			//services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(databaseName: "MSPApplication"));

			services.AddDbContext<AppDbContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			services.AddScoped<ICountryRepository, CountryRepository>();
			services.AddScoped<IJobCategoryRepository, JobCategoryRepository>();
			services.AddScoped<IEmployeeRepository, EmployeeRepository>();
			services.AddScoped<IExpenseRepository, ExpenseRepository>();
			services.AddScoped<ICurrencyRepository, CurrencyRepository>();
			services.AddScoped<ITaskRepository, TaskRepository>();
			services.AddScoped<ISurveyRepository, SurveyRepository>();
			services.AddScoped<INoticeRepository, NoticeRepository>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IRoleRepository, RoleRepository>();
			services.AddScoped<ICompanyDetailRepository, CompanyDetailRepository>();
			services.AddCors(options =>
			{
				options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader());
			});
			services.AddControllers()
				.AddNewtonsoftJson(options =>
				{
					options.SerializerSettings.ContractResolver = new DefaultContractResolver();
					options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
				});
			// Register the Swagger generator, defining 1 or more Swagger documents
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "MSP API",
					Description = "A simple example ASP.NET Core Web API",
					TermsOfService = new Uri("https://example.com/terms"),
					Contact = new OpenApiContact
					{
						Name = "Mark Phillipson",
						Email = "MPhillipson0@Gmail.com",
						Url = new Uri("https://www.upwork.com/fl/markp2"),
					},
					License = new OpenApiLicense
					{
						Name = "Use under LICX",
						Url = new Uri("https://example.com/license"),
					}
				});
				// Set the comments path for the Swagger JSON and UI.
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});

			services.AddAutoMapper(typeof(Startup),typeof(MapperProfile));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseSwagger();
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "MSP API V1");
				c.RoutePrefix = string.Empty;
			});

			app.UseRouting();

			app.UseAuthorization();

			app.UseCors("Open");

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();

			});
		}
	}
}
