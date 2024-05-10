using GWebCache.Extensions;

namespace GWebCache.Test;
[TestClass]
public class ExtensionsTest {

	public static IEnumerable<object[]> ValidUrlData {
		get {
			return new[]
			{
			[
				new Uri("https://www.test.com"),
				new Dictionary<string, object> { { "test", "yeah" } },
				"https://www.test.com/?test=yeah"
			],
			[
				new Uri("https://www.test.com?test=yeah"),
				new Dictionary<string, object> { { "test", "yeah" } },
				"https://www.test.com/?test=yeah"
			],
			[
				new Uri("https://www.test.com?test=yeah&tester=yeah"),
				new Dictionary<string, object> { { "test", "yeah" } },
				"https://www.test.com/?test=yeah&tester=yeah"
			],
			[
				new Uri("https://www.test.com"),
				new Dictionary<string, object> { { "page", 1}, { "filter", true} },
				"https://www.test.com/?page=1&filter=True"
			],
			new object[] {
				new Uri("https://www.test.com"),
				new Dictionary<string, object>(),
				"https://www.test.com/"
			},
		  };
		}
	}


	[TestMethod]
	[DataRow("",1)]
	[DataRow("I | test",2)]
	[DataRow("I||test",3)]
	[DataRow("I|test|",3)]
	public void SplitFieldsGivesCorrectNumberOfFields(string input, int expectedOutputAmount) {
		HttpResponseMessage response = new HttpResponseMessage() {
			Content = new StringContent(input)
		};
		
		Assert.AreEqual(response.SplitContentInFields().Length, expectedOutputAmount);
	}


	[TestMethod]
	[DynamicData(nameof(ValidUrlData))]
	public void AddingUrlParameters(Uri input, Dictionary<string,object> parameters, string expectedOutput) {
		Assert.AreEqual(input.GetUrlWithQuery(parameters), expectedOutput);
	}
}