using System;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Auto.Core.Plus.UI;

namespace TRO.PageObjects
{
	class Program
	{
		static void Main(string[] args)
		{
			DotEnv.Load();
			using var host = CreateHostBuilder(args).Build();
			host.Services.CreateScope().ServiceProvider.GetRequiredService<Playground>().Run();
		}

		private static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((ctx, cfg) =>
				{
					if (!ctx.HostingEnvironment.IsDevelopment())
					{
						Console.WriteLine("Playground can only be ran in development mode.");
						Console.WriteLine("Ensure that you have set the environment variable \"DOTNET_ENVIRONMENT\" to \"Development\" in your launchSettings.json");
						Console.WriteLine("Press any key to exit.");
						Console.ReadKey();
						Environment.Exit(1);
					}

					var env = AppContext.GetData("env");
					Trace.WriteLine($"Build configuration: {env}");

					cfg.AddJsonFile("appsettings.json", false)
						.AddJsonFile($"appsettings.{env}.json", false)
						.AddUserSecrets<Program>()
						.AddEnvironmentVariables();
				})
				.ConfigureServices((ctx, services) =>
				{
					services
						.AddScoped<Playground>()
						.AddAutoCoreUi()
						.AddPageObjects();
				});
	}
}
