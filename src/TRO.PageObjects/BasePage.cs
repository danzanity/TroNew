using OpenQA.Selenium;
using IWebDriver = Auto.Core.Plus.UI.IWebDriver;

namespace TRO.PageObjects
{
	public abstract class BasePage<T> where T : BasePage<T>
	{
		private readonly IWebDriver _driver;
		public readonly CookieNotificationPage CookieNotification;
		public readonly NavbarPage Navbar;
		public By Header => By.TagName("nav-menu");
		public By MainBy => By.XPath("html/body/app");
		public By Footer => By.TagName("footer-bar");

		public BasePage(IWebDriver driver)
		{
			_driver = driver;
			CookieNotification = new(_driver);
			Navbar = new(_driver);
		}

		public T GoToUrl(string url)
		{
			_driver.Navigate().GoToUrl(url);
			return (T)this;
		}

		public virtual T WaitForPageToLoad()
		{
			// TODO: UNCOMMENT THIS ONCE THE FOREVER PENDING MACRO TASK IS IDENTIFIED AND ADDRESSED IN ANGULAR
			//_driver.Wait().Until(ExpectedConditions.AngularIsStable());
			return (T)this;
		}
	}
}