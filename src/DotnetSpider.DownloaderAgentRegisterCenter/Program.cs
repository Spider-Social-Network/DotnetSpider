﻿using System;
using System.Linq;
using DotnetSpider.Common;
using DotnetSpider.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace DotnetSpider.DownloadAgentRegisterCenter
{
	class Program
	{
		static void Main(string[] args)
		{
			var	configure = new LoggerConfiguration()
#if DEBUG
				.MinimumLevel.Verbose()
#else
				.MinimumLevel.Information()
#endif
				.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
				.Enrich.FromLogContext()
				.WriteTo.Console().WriteTo
				.RollingFile("dotnet-spider.log");
			Log.Logger = configure.CreateLogger();
			
			var host = new HostBuilder()
				.ConfigureLogging(x => { x.AddSerilog(); })
				.ConfigureAppConfiguration(x => x.AddJsonFile("appsettings.json"))
				.ConfigureServices((hostContext, services) =>
				{
					services.AddSingleton<SpiderOptions>();
					services.AddKafkaEventBus();
					services.AddDownloadCenter(x => x.UseMySqlDownloaderAgentStore());
					services.AddStatisticsCenter(x => x.UseMySql());
				})
				.UseEnvironment(args.Contains("/dev") ? EnvironmentName.Development : EnvironmentName.Production)
				.UseContentRoot(AppDomain.CurrentDomain.BaseDirectory)
				.Build();
			host.Run();
		}
	}
}