using OpenQA.Selenium;
using System;

namespace TRO.PageObjects
{
	public class TroExpectedConditions
	{
		private TroExpectedConditions()
		{
		}

		// TODO: PROMOTE THIS TO AUTO.CORE.PLUS
		/// <summary>
		/// An expectation for svg to be stable.
		/// </summary>
		/// <returns><see langword="true"/> when svg are stable; otherwise, <see langword="false"/>.</returns>
		public static Func<IWebDriver, bool> SvgAreStable()
		{
			const string script =
				@"const trace = msg => console.log(`${new Date().toISOString()} ${msg}`);
				  const callback = arguments[arguments.length - 1];
				  let prev = [];
				  let curr = [];
				  const xpath = ""//*[name()='path']/@d"";
				  let paths = document.evaluate(xpath, document, null, XPathResult.UNORDERED_NODE_ITERATOR_TYPE, null);
				  while (path = paths.iterateNext())
				  {
					prev.push(path.value);
				  }
				  trace(`prev: ${prev}`);
				  setTimeout(() => {
					paths = document.evaluate(xpath, document, null, XPathResult.UNORDERED_NODE_ITERATOR_TYPE, null);
					while (path = paths.iterateNext())
					{
					  curr.push(path.value);
					}
					trace(`curr: ${curr}`);
					const stable = prev.length === curr.length && prev.every((v, i) => v === curr[i]);
					trace(`svg ${stable ? 'stable' : 'not yet stable'}`);
					callback(stable);
				  }, 500);";
			return driver => (bool)((IJavaScriptExecutor)driver).ExecuteAsyncScript(script);
		}

		//// TODO: PROMOTE THIS TO AUTO.CORE.PLUS
		///// <summary>
		///// An expectation that a local storage key is present.
		///// </summary>
		/// <param name="key">They local storage key.</param>
		///// <returns><see langword="true"/> when key is present; otherwise, <see langword="false"/>.</returns>
		public static Func<IWebDriver, bool> LocalStorageItemToBePresent(string key)
		{
			var script =
				$@"const callback = arguments[arguments.length - 1];
				   if (window.localStorage.hasOwnProperty('{key}')) {{
				     callback(true);
				  }} else {{
				     callback(false);
				  }}";
			return driver => (bool)((IJavaScriptExecutor)driver).ExecuteAsyncScript(script);
		}
	}
}