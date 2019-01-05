using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using XunitTest;

namespace XunitTestDemo
{
    public class ValuesTests
    {
        //public ValuesTests()
        //{
        //    var server = new TestServer(WebHost.CreateDefaultBuilder()
        //        .UseStartup<Startup>());
        //    Client = server.CreateClient();
        //}

        public HttpClient Client { get; }

        public ValuesTests(ITestOutputHelper outputHelper)
        {
            var server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>());
            Client = server.CreateClient();
            Output = outputHelper;
        }

        public ITestOutputHelper Output { get; }

        [Fact]
        public async Task GetById_ShouldBe_Ok()
        {
            // Arrange
            var id = 1;

            // Act
            var response = await Client.GetAsync($"/api/values/{id}");

            // Output
            var responseText = await response.Content.ReadAsStringAsync();
            Output.WriteLine(responseText);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
