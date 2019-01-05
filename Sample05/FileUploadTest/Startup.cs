using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;

namespace FileUploadTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment _env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            //.AddJsonOptions(options => { options.SerializerSettings.ContractResolver = new DefaultContractResolver(); });

            var ConnString = Configuration["ConnString"];
            var UploadFile = Configuration.GetSection("SystemConfig")["UploadFile"];
            var AdminName = Configuration.GetSection("SystemConfig").GetSection("Admin")["Name"];

            //ConnString = Configuration["ConnString"];
            //UploadFile = Configuration["SystemConfig:UploadFile"];
            //AdminName = Configuration["SystemConfig:Admin:Name"];

            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory factory)
        {

            ////使用NLog作为日志记录工具
            //factory.AddNLog();
            ////引入Nlog配置文件
            //env.ConfigureNLog("Nlog.config");

            //// 将 NLog
            //factory.AddConsole(Configuration.GetSection("Logging"))
            //       .AddDebug();

            //var nlogFile = System.IO.Path.Combine(env.ContentRootPath, "nlog.config");
            //env.ConfigureNLog(nlogFile);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<DemoAMiddleware>();
            app.UseMvc();
        }
    }
}
