using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using NLog.Web;

namespace ExceptionAndNLogTest
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
            // 将异常过滤器注入到容器中   方式一
            //services.AddScoped<CustomerExceptionFilter>();
            services.AddMemoryCache();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory factory)
        {

            // 将 NLog
            factory.AddConsole(Configuration.GetSection("Logging"))
                   .AddNLog()
                   .AddDebug();

            var nlogFile = System.IO.Path.Combine(env.ContentRootPath, "nlog.config");
            env.ConfigureNLog(nlogFile);

            //方式二
            // ExceptionMiddleware 加入管道
            app.UseMiddleware<ExceptionMiddleware>();  //该代码与下语句实质是一样的
            //app.UseException();

            //下面代码得注释掉
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseMvc();
        }
    }
}
