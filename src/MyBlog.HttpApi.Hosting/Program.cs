﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MyBolg.ToolKits.Extensions;
using System.Threading.Tasks;

namespace MyBlog.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .UseLog4Net()
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseIISIntegration()
                           .UseStartup<Startup>();
                }).UseAutofac().Build().RunAsync();
        }
    }
}