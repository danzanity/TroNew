using Core.Dto;
using Microsoft.Extensions.Options;
using Shouldly;
using TechTalk.SpecFlow;

namespace OSA.Tests
{
	[Binding]
	public sealed class TestDataSteps
	{
		private readonly TestDataOptions _testDataOpts;
		private readonly TestDataDto _testData;

		public TestDataSteps(IOptions<TestDataOptions> opts, TestDataDto testData)
		{
			_testDataOpts = opts.Value;
			_testData = testData;
		}

		[Given(@"test data ""(.+)""")]
		public void GivenTestData(string name)
		{
			_testDataOpts.TestData.ContainsKey(name)
				.ShouldBeTrue($"Static data \"{name}\" not found! Please add entry in testdata.yaml");

			var testData = _testDataOpts.TestData[name];
			testData.TenantId.ShouldBeGreaterThan(0);

			_testData.ProjectName = testData.ProjectName;
			_testData.TenantId = testData.TenantId;
		}
	}
}