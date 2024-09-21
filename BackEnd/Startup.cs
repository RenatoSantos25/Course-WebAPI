using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ReserveiAPI.Context;
using ReserveiAPI.Services.Server;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text.Json.Serialization;

namespace ReserveiAPI
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }
		public void ConfigureServices(IServiceCollection services)
		{
			//configurando banco de dados

			services.AddDbContext<AppDBContext>(options =>
			options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

			//Configuracao do Swagger
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "CourseGuide", Version = "v1" });

			});
			services.AddControllers().AddJsonOptions(
				c => c.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
				);
			services.AddEndpointsApiExplorer();
			services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
			{
				builder.WithOrigins("http://localhost:3000", "http://localhost:5173")
				.AllowAnyMethod()
				.AllowAnyHeader()
				.AllowCredentials();
			}));
			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
			//Injecao de dependencias
			services.AddUserDepencies();
		}
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sua API V1");
					c.DocExpansion(DocExpansion.None);
					c.DisplayRequestDuration();
					c.EnableDeepLinking();
					c.EnableFilter();
					c.ShowExtensions();
					c.EnableValidator();
					c.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Post, SubmitMethod.Put, SubmitMethod.Delete, SubmitMethod.Patch);


				});
			}else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseCors("MyPolicy");
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
			

	}
}
