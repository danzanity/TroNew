using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Applitools;
using Applitools.Selenium;
using Applitools.VisualGrid;
using Auto.Core.Plus.UI;
using Core.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SolidToken.SpecFlow.DependencyInjection;
using TechTalk.SpecFlow;
using TRO.Clients;
using TRO.PageObjects;
using Xunit.Abstractions;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace TRO.Tests
{
	[Binding]
	public sealed class Hooks
	{
		private static IConfigurationRoot _configuration;
		private static bool _eyesDisabled;
		private static readonly int _viewPortWidth = 1920;
		private static readonly int _viewPortHeight = 1080;

		[BeforeTestRun]
		public static void BeforeTestRun()
		{
			DotEnv.Load();
			InitConfiguration();
			InitEyesConfiguration();
		}

		[ScenarioDependencies]
		public static IServiceCollection ConfigureServices() => new ServiceCollection()
			.AddScoped<ITestOutputHelper>(p =>
				p.GetRequiredService<ScenarioContext>().ScenarioContainer.Resolve<ITestOutputHelper>())
			.AddSingleton<IConfiguration>(_ => _configuration)
			.Configure<PersonaOptions>(_configuration.GetSection(nameof(PersonaOptions)))
			.AddScoped<PersonaDto>()
			.Configure<TestDataOptions>(_configuration.GetSection(nameof(TestDataOptions)))
			.AddScoped<TestDataDto>()
			.AddAutoCoreUi()
			.AddPageObjects()
			.AddTroClients()
			.AddTransient<UiLogin>();

		[BeforeScenario]
		public void BeforeScenario(FeatureContext featCtx, ScenarioContext sceCtx, ITestOutputHelper testOutputHelper)
		{
			var tags = string.Join(" @", featCtx.FeatureInfo.Tags.Concat(sceCtx.ScenarioInfo.Tags));
			if (!string.IsNullOrEmpty(tags)) testOutputHelper.WriteLine($"@{tags}");
			testOutputHelper.WriteLine($"Scenario: {sceCtx.ScenarioInfo.Title}");
		}

		[BeforeScenario]
		[Scope(Tag = "visual")]
		public void BeforeScenarioUI(FeatureContext featCtx, Eyes eyes, IWebDriver driver, ITestOutputHelper testOutputHelper)
		{
			eyes.IsDisabled = _eyesDisabled;
			if (eyes.IsDisabled) return;
			var name = $"{featCtx.FeatureInfo.Title} - {testOutputHelper.DisplayName()}";
			eyes.Open(driver, name);
			eyes.SetLogHandler(new FileLogHandler(Path.Combine(AppContext.BaseDirectory, "eyes.log"), true, true));
		}

		[AfterStep]
		[Scope(Tag = "ui")]
		public void AfterStep(
			ScenarioContext sceCtx,
			Eyes eyes,
			IWebDriver driver,
			ITestOutputHelper testOutputHelper)
		{
			if (driver != null) testOutputHelper.WriteLine($"-> url: {driver.Url}");
			if (sceCtx.TestError != null)
			{
				eyes?.AbortAsync();
				driver.TakeScreenshot();
			}
		}

		[AfterScenario]
		public void AfterScenario(PersonaDto persona, TestDataDto testData, ITestOutputHelper testOutputHelper)
		{
			testOutputHelper.WriteLine($"-> persona: {persona.Username}");
			testOutputHelper.WriteLine($"-> projectName: {testData.ProjectName}");

			if (testData.TenantId == 0) return;
			testOutputHelper.WriteLine($"-> tenantId: {testData.TenantId}");
		}

		[AfterScenario]
		[Scope(Tag = "ui")]
		public void AfterScenarioUI(ScenarioContext sceCtx, Eyes eyes, IWebDriver driver)
		{
			if (sceCtx.TestError == null && eyes != null && eyes.IsOpen)
			{
				eyes.Close().IsDifferent.ShouldBeFalse();
			}
			eyes.AbortIfNotClosed();
			driver?.Quit();
		}

		private static void InitConfiguration()
		{
			var env = AppContext.GetData("env");
			Trace.WriteLine($"Build configuration: {env}");

			_configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", false)
				.AddJsonFile($"appsettings.{env}.json", true)
				.AddJsonFile("testdata.json", true)
				.AddJsonFile($"testdata.{env}.json", true)
				.AddUserSecrets<Hooks>()
				.AddEnvironmentVariables()
				.Build();
		}

		private static void InitEyesConfiguration()
		{
			_eyesDisabled = bool.Parse(Environment.GetEnvironmentVariable("APPLITOOLS_DISABLE") ?? "false");
			if (_eyesDisabled) return;

			EyesFactory.CreateRunner();
			EyesFactory.Configuration
				.AddBrowser(new DesktopBrowserInfo(_viewPortWidth, _viewPortHeight, BrowserType.CHROME))
				//.AddBrowser(new DesktopBrowserInfo(_viewPortWidth, _viewPortHeight, BrowserType.EDGE_CHROMIUM))
				//.AddBrowser(new DesktopBrowserInfo(_viewPortWidth, _viewPortHeight, BrowserType.EDGE_LEGACY))
				//.AddBrowser(new DesktopBrowserInfo(_viewPortWidth, _viewPortHeight, BrowserType.FIREFOX))
				//.AddBrowser(new IosDeviceInfo(IosDeviceName.iPad_Pro_3, ScreenOrientation.Landscape))
				//.AddBrowser(new ChromeEmulationInfo(DeviceName.Galaxy_Note_10_Plus, ScreenOrientation.Portrait))
				.SetBranchName(GetGitBranch())
				.SetAppName("TRO");

			// Local run
			if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BUILD_BUILDID")))
			{
				var user = Environment.GetEnvironmentVariable("USERNAME");
				var env = AppContext.GetData("env");
				var batchInfo = new BatchInfo($"EESQA / TRO {user} ({env})");
				batchInfo.SequenceName = batchInfo.Name;
				EyesFactory.Configuration.SetBatch(batchInfo);
			}
		}

		private static string GetGitBranch()
		{
			var startInfo = new ProcessStartInfo
			{
				FileName = "pwsh",
				Arguments = "-Command git branch --show-current",
				RedirectStandardOutput = true
			};
			var process = Process.Start(startInfo);
			var branch = process.StandardOutput.ReadToEnd().Trim();
			process.Close();
			return branch;
		}
	}
}