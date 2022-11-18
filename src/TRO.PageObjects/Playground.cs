using System;
using Microsoft.Extensions.Configuration;
using Auto.Core.Plus.UI;
using Core.Dto;
using TRO.PageObjects.Authentication;

namespace TRO.PageObjects
{
	public sealed class Playground
	{
		private readonly IWebDriver _driver;
		private readonly IConfiguration _cfg;
		private readonly AuthenticationPage _authenticationPage;

		// Add page object under test

		public Playground(
			IWebDriver driver,
			IConfiguration cfg,
			AuthenticationPage authenticationPage)
		{
			_driver = driver;
			_cfg = cfg;
			_authenticationPage = authenticationPage;

			// Add page object under test

		}

		public void Run()
		{
			try
			{
				Login();
				TestYourPageObjectHere();
				_driver.JavaScriptExecutor.ExecuteScript("alert('✅ Done playing around!');");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				_driver.JavaScriptExecutor
					.ExecuteScript($@"alert(`❌ Something is messing around the playground.\n\nException:\n{e.Message}`);");
			}
			finally
			{
				Console.WriteLine(Environment.NewLine);
				Console.WriteLine("Press any key to exit.");
				Console.ReadKey();
				_driver?.Quit();
			}
		}

		private void Login()
		{
			var baseUrl = $"{_cfg.GetValue<string>("baseUrl")}/account/login";
			_driver.Navigate().GoToUrl(baseUrl);

			var personaOpts = _cfg.GetSection(nameof(PersonaOptions)).Get<PersonaOptions>();
			_authenticationPage
				.SetEmail(personaOpts.AndieUser)
				.SetPassword(personaOpts.AndiePswd)
				.ClickLogin();
		}

		private void TestYourPageObjectHere()
		{
		}
	}
}