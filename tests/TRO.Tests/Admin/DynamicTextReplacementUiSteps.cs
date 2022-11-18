using Applitools.Selenium;
using TechTalk.SpecFlow;
using TRO.PageObjects.Admin;

namespace TRO.Tests.Admin
{
	[Binding]
	[Scope(Tag = "ui")]
	public sealed class DynamicTextReplacementUiSteps
	{
		private readonly DynamicTextReplacementPage _page;
		private readonly Eyes _eyes;
		private readonly UiLogin _login;
		private readonly ScenarioContext _sceCtx;

		public DynamicTextReplacementUiSteps(
			DynamicTextReplacementPage page,
			Eyes eyes,
			UiLogin login,
			ScenarioContext sceCtx)
		{
			_page = page;
			_eyes = eyes;
			_login = login;
			_sceCtx = sceCtx;
		}

		[When(@"Gina views dynamic text replacement")]
		public void WhenGinaViewsDynamicTextReplacement()
		{
			_login.AsGina();
			_page.GoToUrl().WaitForPageToLoad();
		}

		[Then(@"a list of default text with a corresponding custom text is displayed")]
		public void ThenAListOfDefaultTextWithACorrespondingCustomTextIsDisplayed()
		{
			_eyes.Check(_sceCtx.StepContext.StepInfo.Text, Target.Region(_page.MainBy).Ignore(_page.Header, _page.Footer));
		}
	}
}