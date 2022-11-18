using System.Reflection;
using Xunit.Abstractions;

namespace TRO.Tests
{
	/// <summary>
	/// TestContext workaround for xUnit v2 until v3 is out.
	/// </summary>
	public static class TestOutputHelperExtensions
	{
		public static string DisplayName(this ITestOutputHelper testOutputHelper)
		{
			var type = testOutputHelper.GetType();
			var testMember = type.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
			var test = (ITest)testMember.GetValue(testOutputHelper);
			return test.DisplayName;
		}
	}
}