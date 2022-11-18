using Auto.Core.Plus.UI.WaitHelpers;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using IWebDriver = Auto.Core.Plus.UI.IWebDriver;

namespace TRO.PageObjects.Project
{
	public sealed partial class PortfolioPathfinderPage : BasePage<PortfolioPathfinderPage>
	{
		private readonly IWebDriver _driver;
		private readonly string _baseUrl;

		public PortfolioPathfinderPage(IWebDriver driver, IConfiguration cfg) : base(driver)
		{
			_driver = driver;
			_baseUrl = cfg.GetValue<string>("baseUrl");
		}

		public PortfolioPathfinderPage GoToUrl()
		{
			_driver.Wait().Until(TroExpectedConditions.LocalStorageItemToBePresent("Data Imported"));
			base.GoToUrl($"{_baseUrl}/portfoliopathfinder");
			return this;
		}

		public override PortfolioPathfinderPage WaitForPageToLoad()
		{
			base.WaitForPageToLoad();
			_driver.Wait().Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//mat-icon[text()='info']")));
			return this;
		}
	}
}