using Applitools.Selenium;
using TechTalk.SpecFlow;
using TRO.PageObjects.Admin;

namespace TRO.Tests.Admin
{
	[Binding]
	[Scope(Tag = "ui")]
	public sealed class ImportSurveyDataUiSteps
	{
		private readonly ImportSurveyDataPage _page;
		private readonly Eyes _eyes;
		private readonly UiLogin _login;
		private readonly ScenarioContext _sceCtx;

		public ImportSurveyDataUiSteps(
			ImportSurveyDataPage page,
			Eyes eyes,
			UiLogin login,
			ScenarioContext sceCtx)
		{
			_page = page;
			_eyes = eyes;
			_login = login;
			_sceCtx = sceCtx;
		}

		[When(@"Gina wants to import a survey data")]
		public void WhenGinaWantsToImportASurveyData()
		{
			_login.AsGina();
			_page.GoToUrl().WaitForPageToLoad();
		}

		[Then(@"an import survey data form is displayed")]
		public void ThenAnImportSurveyDataFormIsDisplayed()
		{
			_eyes.Check(_sceCtx.StepContext.StepInfo.Text, Target.Region(_page.MainBy).Ignore(_page.Header, _page.Footer));
		}
	}
}