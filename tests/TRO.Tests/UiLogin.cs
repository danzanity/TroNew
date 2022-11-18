using Core.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TRO.PageObjects.Authentication;
using IWebDriver = Auto.Core.Plus.UI.IWebDriver;

namespace TRO.Tests
{
	public sealed class UiLogin
	{
		private readonly string _baseUrl;
		private readonly PersonaOptions _personaOpts;
		private readonly TestDataDto _testData;
		private readonly IWebDriver _driver;
		private readonly AuthenticationPage _page;

		public UiLogin(
			IConfiguration cfg,
			IOptions<PersonaOptions> personaOpts,
			TestDataDto testData,
			IWebDriver driver,
			AuthenticationPage page)
		{
			_baseUrl = cfg.GetValue<string>("baseUrl");
			_personaOpts = personaOpts.Value;
			_testData = testData;
			_driver = driver;
			_page = page;
		}

		public void AsAndie()
		{
			As(_personaOpts.AndieUser, _personaOpts.AndiePswd);
		}
		public void AsGina()
		{
			var email = _testData.TenantId == 0 ? _personaOpts.GinaUser : $"auto.gina.{_testData.TenantId}@wtwco.com";
			As(email, _personaOpts.GinaPswd);
		}

		public void AsConnie()
		{
			var email = _testData.TenantId == 0 ? _personaOpts.ConnieUser : $"auto.connie.{_testData.TenantId}@wtwco.com";
			As(email, _personaOpts.ConniePswd);
		}

		private void As(string email, string password)
		{
			_driver.Navigate().GoToUrl(_baseUrl);
			_page
				.SetEmail(email)
				.SetPassword(password)
				.ClickLogin()
				.WaitForPageToUnload();
		}
	}
}