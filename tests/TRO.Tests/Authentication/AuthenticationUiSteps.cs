using Applitools.Selenium;
using Core.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TechTalk.SpecFlow;
using TRO.PageObjects.Authentication;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace TRO.Tests.Authentication
{
	[Binding]
	[Scope(Tag = "ui")]
	public sealed class AuthenticationUiSteps
	{
		private readonly string _baseUrl;
		private readonly PersonaOptions _personaOpts;
		private readonly TestDataDto _testData;
		private readonly AuthenticationPage _page;
		private readonly Eyes _eyes;
		private readonly ScenarioContext _sceCtx;

		public AuthenticationUiSteps(
			IConfiguration cfg,
			IOptions<PersonaOptions> personaOpts,
			TestDataDto testData,
			AuthenticationPage page,
			Eyes eyes,
			ScenarioContext sceCtx)
		{
			_baseUrl = cfg.GetValue<string>("baseUrl");
			_personaOpts = personaOpts.Value;
			_testData = testData;
			_page = page;
			_eyes = eyes;
			_sceCtx = sceCtx;
		}

		[When(@"a user visits the total rewards optimization page")]
		public void WhenAUserVisitsTheTotalRewardsOptimizationPage()
		{
			_page.GoToUrl(_baseUrl).WaitForPageToLoad();
		}

		[Then(@"the user gets redirects to the login page")]
		public void ThenTheUserGetsRedirectsToTheLoginPage()
		{
			_eyes.Check(_sceCtx.StepContext.StepInfo.Text, Target.Window());
		}

		[When(@"a user wants to register now")]
		public void WhenAUserWantsToRegisterNow()
		{
			_page.GoToUrl(_baseUrl).ClickRegisterNow().WaitForPageToUnload();
		}

		[When(@"a user forgots their password")]
		public void WhenAUserForgotsTheirPassword()
		{
			_page.GoToUrl(_baseUrl).ClickForgotPassword().WaitForPageToUnload();
		}

		[When(@"Andie logs in")]
		public void WhenAndieLogsIn()
		{
			Login(_personaOpts.AndieUser, _personaOpts.AndiePswd);
		}

		[When(@"Gina logs in")]
		public void WhenGinaLogsIn()
		{
			var email = _testData.TenantId == 0 ? _personaOpts.GinaUser : $"auto.gina.{_testData.TenantId}@wtwco.com";
			Login(email, _personaOpts.GinaPswd);
		}

		[When(@"Connie logs in")]
		public void WhenConnieLogsIn()
		{
			var email = _testData.TenantId == 0 ? _personaOpts.ConnieUser : $"auto.connie.{_testData.TenantId}@wtwco.com";
			Login(email, _personaOpts.ConniePswd);
		}

		private void Login(string email, string password)
		{
			_page
				.GoToUrl(_baseUrl)
				.SetEmail(email)
				.SetPassword(password)
				.ClickLogin()
				.WaitForPageToUnload();
		}
	}
}