
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Food_Center.Services;
using Shopping.Middleware;
using FOODProject.Core.AccountManager.OfficeDetails;
using FOODProject.Core.Common.Accounts;
using FOODProject.Core.Shop.StoreDetails;

namespace FOODProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.


        //data source=20.204.136.73;initial catalog=FoodCenter;integrated security=False;persist security info=True;user id=fcDB;password=****************


        /* data source = DESKTOP - 5H6J294;initial catalog = "Food Center"; integrated security = True; persist security info=True*/

        public void ConfigureServices(IServiceCollection services)
        {
            
            FoodCenterContext.FoodCenterDataContext db = new FoodCenterContext.FoodCenterDataContext("data source=20.204.136.73;initial catalog=FoodCenter;integrated security=False;persist security info=True;user id=fcDB;password=Otobit@2022@Food;License Key=qHnH5wx/L422kFN4WQussVkqbelF0xGMaZi+DGL6lhFu+VTasW/ZRA22+dVoDbuQ64trDZsBMziLDE9kumHeTDKlcRSCvsotqn7rHn9VHFXS3Jmh/rFBVSxav6UlKmT4POdU+hnX8ACaigXhFdBiZ4NeHNVRNTqJ4fUTou0czKt8ATWxOB2MjUrprbYTV2ECFJOo2uLgwGzqeEpv1gGPLKR3p5DOKdeMu61FRAak23fmjt8PPQpz50o1E0r0FFdoQrJIYKkMxqRiD2IhVxlcVCvpIqR31rWwKJ1sNquGBMU=;");
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(option =>
               {
                   option.SaveToken = true;
                   option.RequireHttpsMetadata = false;
                   option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidAudience = Configuration["JWT:ValidAudience"],
                       ValidIssuer = Configuration["JWT:ValidIssuer"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                   };
               });
            //JSON Serializer


            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver
                = new DefaultContractResolver());
            
            services.AddControllers().AddNewtonsoftJson();

            services.AddControllers();

            services.AddTransient<Accounts>();
            services.AddTransient<Core.Shop.StoreDetails.StoreDetails>();
           // services.AddTransient<Core.StoreDetails.Addresses>();
            services.AddTransient<Core.Shop.Categories.Categories>();
            services.AddTransient<Core.Shop.Products.ProductTypes>();
            services.AddTransient<Core.Shop.Products.Products>();
            services.AddTransient<Core.Common.Accounts.UpdatePassword>();
            services.AddTransient<TokenService>();
            services.AddTransient<OfficeDetails>();
            services.AddTransient<Core.Shop.StoreDetails.AddMoreAddress>();
            services.AddTransient<Core.AccountManager.Employees.AddEmployees>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FOODProject", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement 
                {
                 {
            new OpenApiSecurityScheme 
            {
                Reference = new OpenApiReference 
                {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }       
    });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FOODProject v1"));
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseMiddleware<ErrorHandler>();
            app.UseMiddleware<JwtHandler>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
