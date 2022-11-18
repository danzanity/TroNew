using Auto.Core.Plus.UI.WaitHelpers;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using System;
using IWebDriver = Auto.Core.Plus.UI.IWebDriver;

namespace TRO.PageObjects.Project
{
	public sealed partial class PortfolioBuilderPage : BasePage<PortfolioBuilderPage>
	{
		private readonly IWebDriver _driver;
		private readonly string _baseUrl;

		public PortfolioBuilderPage(IWebDriver driver, IConfiguration cfg) : base(driver)
		{
			_driver = driver;
			_baseUrl = cfg.GetValue<string>("baseUrl");
		}

		public By DundasBy => By.XPath("//div[@id='dundasElement']/iframe");

		public By TestPurposesOnlyBy => By.CssSelector(".the-message.with-icon");

		public PortfolioBuilderPage GoToUrl()
		{
			_driver.Wait().Until(TroExpectedConditions.LocalStorageItemToBePresent("Data Imported"));
			base.GoToUrl($"{_baseUrl}/portfoliobuilder");
			return this;
		}

		public override PortfolioBuilderPage WaitForPageToLoad()
		{
			base.WaitForPageToLoad();
			var dundasEl = _driver.Wait().Until(ExpectedConditions.ElementIsVisible(DundasBy));
			_driver.Wait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@id='segment-filter']/div/button")));
			_driver.SwitchTo().Frame(dundasEl);
			_driver.Wait().Until(ExpectedConditions.JQueryIsStable());
			_driver.Wait().Until(TroExpectedConditions.SvgAreStable());
			_driver.SwitchTo().DefaultContent();
			return this;
		}
	}
}