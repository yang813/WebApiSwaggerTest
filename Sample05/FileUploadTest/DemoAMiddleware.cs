using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadTest
{
    public class DemoAMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public DemoAMiddleware(RequestDelegate next, ILogger<DemoAMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation("(1) DemoAMiddleware.Invoke front");
            await _next(context);
            _logger.LogInformation("[1] DemoAMiddleware:Invoke back");
        }
    }
}
