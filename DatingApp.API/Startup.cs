using System;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using DatingApp.API.DTOs;
using DatingApp.API.Helper;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using DatingApp.API.Extentions;
using Microsoft.AspNetCore.Http;
using FluentValidation.AspNetCore;
using DatingApp.API.Filters;

namespace DatingApp.API
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
            // services.AddDbContext<DataContext>(
            //     x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection"))
            // );

            //services.AddDbContext<DataContext>(ServiceLifetime.Transient);

            services.AddDbContext<DataContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddControllers(
                options => options.Filters.Add(new ValidationFilter())
            )
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>())
            .AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            //services.AddAutoMapper(typeof(DatingRepository).Assembly);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddCors();


            services.Configure<CloudinarySettings>(Configuration.GetSection("Cloudinary"));

            //Authentication MiddleWare
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false
                };
            });

            // services.AddAuthentication(JwtBearerDefaults.JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
            //     options.TokenValidationParameters = new TokenValidationParameters{

            //     }
            // })

            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IDatingRepository, DatingRepository>();
            
            //services.AddTransient<LastActive>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());//app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            //it gonna look for index.html in the content folder (our api)
            app.UseDefaultFiles();

            //it gona look for the wwwroot folder
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Index","Fallback");
            });
        }
    }
}
