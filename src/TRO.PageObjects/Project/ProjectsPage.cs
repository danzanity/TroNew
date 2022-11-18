using Auto.Core.Plus.UI.WaitHelpers;
using OpenQA.Selenium;
using IWebDriver = Auto.Core.Plus.UI.IWebDriver;

namespace TRO.PageObjects.Project
{
	public sealed class ProjectsPage : BasePage<ProjectsPage>
	{
		private readonly IWebDriver _driver;

		public ProjectsPage(IWebDriver driver) : base(driver) => _driver = driver;

		public override ProjectsPage WaitForPageToLoad()
		{
			base.WaitForPageToLoad();
			_driver.Wait().Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("mat-list-item")));
			return this;
		}
	}
}