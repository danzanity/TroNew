using System;
using Auto.Core.Plus.UI.WaitHelpers;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using IWebDriver = Auto.Core.Plus.UI.IWebDriver;

namespace TRO.PageObjects.Project
{
	public sealed partial class IsolatedRewardImpactsPage : BasePage<IsolatedRewardImpactsPage>
	{
		private readonly IWebDriver _driver;
		private readonly string _baseUrl;

		public IsolatedRewardImpactsPage(IWebDriver driver, IConfiguration cfg) : base(driver)
		{
			_driver = driver;
			_baseUrl = cfg.GetValue<string>("baseUrl");
		}

		public By DundasBy => By.XPath("//div[@id='dundasElement']/iframe");

		public By TestPurposesOnlyBy => By.CssSelector(".the-message.with-icon");

		public IsolatedRewardImpactsPage GoToUrl()
		{
			_driver.Wait().Until(TroExpectedConditions.LocalStorageItemToBePresent("Data Imported"));
			base.GoToUrl($"{_baseUrl}/surveyresults/isolatedrewardimpacts");
			return this;
		}

		public override IsolatedRewardImpactsPage WaitForPageToLoad()
		{
			base.WaitForPageToLoad();
			_driver.Wait(TimeSpan.FromMinutes(1)).Until(ExpectedConditions.ElementIsVisible(By.TagName("spinner")));
			_driver.Wait(TimeSpan.FromMinutes(1)).Until(ExpectedConditions.InvisibilityOfElementLocated(By.TagName("spinner")));
			_driver.Wait(TimeSpan.FromMinutes(1)).Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(DundasBy));
			_driver.Wait().Until(ExpectedConditions.JQueryIsStable());
			_driver.Wait().Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[text()='Isolated Reward Impacts']")));
			_driver.Wait().Until(TroExpectedConditions.SvgAreStable());
			_driver.SwitchTo().DefaultContent();
			return this;
		}
	}
}