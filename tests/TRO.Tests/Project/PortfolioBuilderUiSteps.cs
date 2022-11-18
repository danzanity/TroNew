using Applitools.Selenium;
using TechTalk.SpecFlow;
using TRO.PageObjects.Project;

namespace TRO.Tests.Project
{
	[Binding]
	[Scope(Tag = "ui")]
	public sealed class PortfolioBuilderUiSteps
	{
		private readonly PortfolioBuilderPage _page;
		private readonly Eyes _eyes;
		private readonly UiLogin _login;
		private readonly ScenarioContext _sceCtx;

		public PortfolioBuilderUiSteps(
			PortfolioBuilderPage page,
			Eyes eyes,
			UiLogin login,
			ScenarioContext sceCtx)
		{
			_page = page;
			_eyes = eyes;
			_login = login;
			_sceCtx = sceCtx;
		}

		[When(@"Connie views portfolio builder")]
		public void WhenConnieViewsPortfolioBuilder()
		{
			_login.AsConnie();
			_page.GoToUrl().WaitForPageToLoad();
		}

		[Then(@"the current state of the portfolio is displayed")]
		public void ThenTheCurrentStateOfThePortfolioIsDisplayed()
		{
			_eyes.Check(_sceCtx.StepContext.StepInfo.Text, Target.Region(_page.MainBy).Ignore(_page.Footer));
			_eyes.Check(_sceCtx.StepContext.StepInfo.Text, Target.Frame(_page.DundasBy).Ignore(_page.TestPurposesOnlyBy));
		}
	}
}