using Auto.Core.Plus.UI.WaitHelpers;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using IWebDriver = Auto.Core.Plus.UI.IWebDriver;

namespace TRO.PageObjects.Project
{
	public sealed partial class PerceivedValueHeatMapPage : BasePage<PerceivedValueHeatMapPage>
	{
		private readonly IWebDriver _driver;
		private readonly string _baseUrl;

		public PerceivedValueHeatMapPage(IWebDriver driver, IConfiguration cfg) : base(driver)
		{
			_driver = driver;
			_baseUrl = cfg.GetValue<string>("baseUrl");
		}

		public PerceivedValueHeatMapPage GoToUrl()
		{
			_driver.Wait().Until(TroExpectedConditions.LocalStorageItemToBePresent("Data Imported"));
			base.GoToUrl($"{_baseUrl}/surveyresults/perceivedvalueheatmap");
			return this;
		}

		public override PerceivedValueHeatMapPage WaitForPageToLoad()
		{
			base.WaitForPageToLoad();
			_driver.Wait().Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//div[contains(@class,'chart-legend')]")));
			return this;
		}
	}
}