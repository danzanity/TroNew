using Applitools.Selenium;
using TechTalk.SpecFlow;
using TRO.PageObjects.Project;

namespace TRO.Tests.Project
{
	[Binding]
	[Scope(Tag = "ui")]
	public sealed class IsolatedRewardImpactsUiSteps
	{
		private readonly IsolatedRewardImpactsPage _page;
		private readonly Eyes _eyes;
		private readonly UiLogin _login;
		private readonly ScenarioContext _sceCtx;

		public IsolatedRewardImpactsUiSteps(
			IsolatedRewardImpactsPage page,
			Eyes eyes,
			UiLogin login,
			ScenarioContext sceCtx)
		{
			_page = page;
			_eyes = eyes;
			_login = login;
			_sceCtx = sceCtx;
		}

		[When(@"Connie views isolated rewards impacts")]
		public void WhenConnieViewsIsolatedRewardsImpacts()
		{
			_login.AsConnie();
			_page.GoToUrl().WaitForPageToLoad();
		}

		[Then(@"the isolated rewards impacts is displayed")]
		public void ThenTheIsolatedRewardImpactsIsDisplayed()
		{
			_eyes.Check(_sceCtx.StepContext.StepInfo.Text, Target.Region(_page.MainBy).Ignore(_page.Footer));
			_eyes.Check(_sceCtx.StepContext.StepInfo.Text, Target.Frame(_page.DundasBy).Ignore(_page.TestPurposesOnlyBy));
		}
	}
}