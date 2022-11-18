using Applitools.Selenium;
using TechTalk.SpecFlow;
using TRO.PageObjects.Authentication;

namespace TRO.Tests.Authentication
{
	[Binding]
	[Scope(Tag = "ui")]
	public sealed class RegisterNowUiSteps
	{
		private readonly RegisterNowPage _page;
		private readonly Eyes _eyes;
		private readonly ScenarioContext _sceCtx;

		public RegisterNowUiSteps(RegisterNowPage page, Eyes eyes, ScenarioContext sceCtx)
		{
			_page = page;
			_eyes = eyes;
			_sceCtx = sceCtx;
		}

		[Then(@"a register now form is displayed")]
		public void ThenARegisterNowFormIsDisplayed()
		{
			_page.WaitForPageToLoad();
			_eyes.Check(_sceCtx.StepContext.StepInfo.Text, Target.Region(_page.MainBy).Ignore(_page.Header, _page.Footer));
		}
	}
}