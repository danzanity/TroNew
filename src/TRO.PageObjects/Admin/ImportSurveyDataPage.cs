using Auto.Core.Plus.UI.WaitHelpers;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using IWebDriver = Auto.Core.Plus.UI.IWebDriver;

namespace TRO.PageObjects.Admin
{
	public sealed class ImportSurveyDataPage : BasePage<ImportSurveyDataPage>
	{
		private readonly IWebDriver _driver;
		private readonly string _baseUrl;

		public ImportSurveyDataPage(IWebDriver driver, IConfiguration cfg) : base(driver)
		{
			_driver = driver;
			_baseUrl = cfg.GetValue<string>("baseUrl");
		}

		public ImportSurveyDataPage GoToUrl()
		{
			_driver.Wait().Until(TroExpectedConditions.LocalStorageItemToBePresent("Data Imported"));
			base.GoToUrl($"{_baseUrl}/importsurvey");
			return this;
		}

		public override ImportSurveyDataPage WaitForPageToLoad()
		{
			base.WaitForPageToLoad();
			_driver.Wait().Until(ExpectedConditions.ElementIsVisible(By.Id("customFile")));
			return this;
		}
	}
}