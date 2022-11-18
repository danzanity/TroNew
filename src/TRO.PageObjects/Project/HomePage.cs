using Auto.Core.Plus.UI.WaitHelpers;
using OpenQA.Selenium;
using IWebDriver = Auto.Core.Plus.UI.IWebDriver;

namespace TRO.PageObjects.Project
{
	public sealed class HomePage : BasePage<HomePage>
	{
		private readonly IWebDriver _driver;
		public By SvgPathBy => By.XPath("//*[name()='path']");

		public HomePage(IWebDriver driver) : base(driver) => _driver = driver;

		public override HomePage WaitForPageToLoad()
		{
			base.WaitForPageToLoad();
			_driver.Wait().Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//div[@class='card home-card-item']")));
			_driver.Wait().Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(SvgPathBy));
			_driver.Wait().Until(TroExpectedConditions.SvgAreStable());
			return this;
		}
	}
}