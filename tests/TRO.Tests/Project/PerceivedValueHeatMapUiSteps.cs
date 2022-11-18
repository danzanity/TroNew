using Applitools.Selenium;
using TechTalk.SpecFlow;
using TRO.PageObjects.Project;

namespace TRO.Tests.Project
{
	[Binding]
	[Scope(Tag = "ui")]
	public sealed class PerceivedValueHeatMapUiSteps
	{
		private readonly PerceivedValueHeatMapPage _page;
		private readonly Eyes _eyes;
		private readonly UiLogin _login;
		private readonly ScenarioContext _sceCtx;

		public PerceivedValueHeatMapUiSteps(
			PerceivedValueHeatMapPage page,
			Eyes eyes,
			UiLogin login,
			ScenarioContext sceCtx)
		{
			_page = page;
			_eyes = eyes;
			_login = login;
			_sceCtx = sceCtx;
		}

		[When(@"Connie views perceived value heat map")]
		public void WhenConnieViewsPerceivedValueHeatMap()
		{
			_login.AsConnie();
			_page.GoToUrl().WaitForPageToLoad();
		}

		[Then(@"the perceived value heat map is displayed")]
		public void ThenThePerceivedValueHeatMapIsDisplayed()
		{
			_eyes.Check(_sceCtx.StepContext.StepInfo.Text, Target.Region(_page.MainBy).Ignore(_page.Footer));
		}
	}
}