using Applitools.Selenium;
using TechTalk.SpecFlow;
using TRO.PageObjects.Authentication;

namespace TRO.Tests.Authentication
{
	[Binding]
	[Scope(Tag = "ui")]
	public sealed class ForgotPasswordUiSteps
	{
		private readonly ForgotPasswordPage _page;
		private readonly Eyes _eyes;
		private readonly ScenarioContext _sceCtx;

		public ForgotPasswordUiSteps(ForgotPasswordPage page, Eyes eyes, ScenarioContext sceCtx)
		{
			_page = page;
			_eyes = eyes;
			_sceCtx = sceCtx;
		}

		[Then(@"a forgot password form is displayed")]
		public void ThenAForgotPasswordFormIsDisplayed()
		{
			_page.WaitForPageToLoad();
			_eyes.Check(_sceCtx.StepContext.StepInfo.Text, Target.Region(_page.MainBy).Ignore(_page.Header, _page.Footer));
		}
	}
}