using Auto.Core.Plus.UI.WaitHelpers;
using OpenQA.Selenium;
using PageObjectSourceGenerator;
using IWebDriver = Auto.Core.Plus.UI.IWebDriver;

namespace TRO.PageObjects
{
	public sealed partial class NavbarPage
	{
		private readonly IWebDriver _driver;

		public By LoginBy => By.XPath("//button[@type='submit']");

		public NavbarPage(IWebDriver driver) => _driver = driver;

		[PageObject]
		private IWebElement ClientEl => _driver.FindElementOrDefault(By.TagName("change-tenant"));

		[PageObject]
		private IWebElement AdminEl =>
			_driver.Wait().Until(ExpectedConditions.ElementExists(By.Id("navigationAdmin")));

		[PageObject]
		private IWebElement SurveyResultsEl =>
			_driver.Wait().Until(ExpectedConditions.ElementExists(By.Id("navbarDropdownMenuSurveyLink")));

		public AdminPage ClickAdmin()
		{
			AdminEl.Click();
			return new AdminPage(_driver);
		}

		public SurveyResultsPage ClickSurveyResults()
		{
			SurveyResultsEl.Click();
			return new SurveyResultsPage(_driver);
		}

		[PageObject]
		private IWebElement PortfolioPathfinderEl => _driver.FindElementOrDefault(By.LinkText("Portfolio Pathfinder"));

		[PageObject]
		private IWebElement PortfolioBuilderEl => _driver.FindElementOrDefault(By.LinkText("Portfolio Builder"));

		[PageObject]
		private IWebElement RewardCostsEl => _driver.FindElementOrDefault(By.LinkText("Reward Costs"));
	}

	public sealed partial class AdminPage
	{
		private readonly IWebDriver _driver;

		public AdminPage(IWebDriver driver) => _driver = driver;

		[PageObject]
		private IWebElement ManageUsersEl => _driver.FindElementOrDefault(By.LinkText("Manage Users"));

		[PageObject]
		private IWebElement ManageProjectEl => _driver.FindElementOrDefault(By.LinkText("Manage Project"));

		[PageObject]
		private IWebElement ImportSurveyDataEl => _driver.FindElementOrDefault(By.LinkText("Import Survey Data"));

		[PageObject]
		private IWebElement DynamicTextReplacementEl => _driver.FindElementOrDefault(By.LinkText("Dynamic Text Replacement"));
	}

	public sealed partial class SurveyResultsPage
	{
		private readonly IWebDriver _driver;

		public SurveyResultsPage(IWebDriver driver) => _driver = driver;

		[PageObject]
		private IWebElement PerceivedValueHeatMapEl => _driver.FindElementOrDefault(By.LinkText("Perceived Value Heat Map"));

		[PageObject]
		private IWebElement IsolatedRewardImpactsEl => _driver.FindElementOrDefault(By.LinkText("Isolated Reward Impacts"));
	}
}