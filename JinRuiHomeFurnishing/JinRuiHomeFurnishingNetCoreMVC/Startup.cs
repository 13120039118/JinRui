using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JinRuiHomeFurnishing.ExtensionMethod;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace JinRuiHomeFurnishingNetCoreMVC
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
            //跨域
            services.AddCors(options =>
            options.AddPolicy("AllowAll",
                    builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithExposedHeaders("newToken"))
            );

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            #region 配置Swagger
            // 注册Swagger服务 (在Startup类ConfigureServices方法中添加Swagger服务并配置文档信息)
            services.AddSwaggerGen(c =>
            {
                // 添加文档信息
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "呵呵呵",
                    Version = "v1",
                    Description = "ASP.NET CORE WebApi",
                    Contact = new OpenApiContact
                    {
                        Name = "郭野爹",
                        Email = "瞎逼逼@导管子.com"
                    }
                });
                // 使用反射获取xml文件。并构造出文件的路径
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);// 获取xml文件路径
                // 启用xml注释. 该方法第二个参数启用控制器的注释，默认为false.，true表示显示控制器注释
                c.IncludeXmlComments(xmlPath, true);
            });
            #endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //跨域请求同意
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowCredentials();
                builder.WithExposedHeaders(new string[] { "newToken" });
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();  //添加用于将HTTP请求重定向到HTTPS的中间件
            app.UseStaticFiles();   //为当前请求路径启用静态文件服务
            app.UseCookiePolicy();  //添加Microsoft.AspNetCore.cookie策略.CookiePolicyHandler到 指定的Microsoft.AspNetCore.Builder版本.iaapplicationbuilder，它启用 cookie策略功能

            ServiceLocator.Instance = app.ApplicationServices;

            //在 Startup类Configure 方法中，启用中间件为生成的 JSON 文档和 Swagger UI 提供服务
            // 启用Swagger中间件
            app.UseSwagger();
            // 配置SwaggerUI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "呵呵呵");
                c.RoutePrefix = string.Empty;
            });

        }
    }
}
