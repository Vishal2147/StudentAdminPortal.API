using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using StudentAdminPortal.API.DataModels;
using StudentAdminPortal.API.Reopsitories;

namespace StudentAdminPortal.API
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
            services.AddCors((options) =>
            {
                options.AddPolicy("angularApp", (builder) =>
                {
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .WithMethods("GET", "POST", "PUT", "DELETE")
                        .WithExposedHeaders("*");
                });
            });
            services.AddDbContext<StudentAdminContext>(options => options.UseSqlServer(Configuration.GetConnectionString("StudentAdminPortalDb")));

            services.AddScoped<IStudentRepository, SqlStudentRepository>();   // when use IstudentRepo then inject  SqlStudentRepo

            services.AddScoped<IImageRespository, LocalStorageImageRepository>();

            services.AddControllers();
            services.AddFluentValidation(fv=> fv.RegisterValidatorsFromAssemblyContaining<Startup>());   //server side validation

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StudentAdminPortal.API", Version = "v1" });
            });

            services.AddAutoMapper(typeof(Startup).Assembly);   // goto startup and scan assembly to map profileS
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StudentAdminPortal.API v1"));
            }

            app.UseHttpsRedirection();;
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath,"Resources")),
                RequestPath = "/Resources"
            });

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors("angularApp");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
