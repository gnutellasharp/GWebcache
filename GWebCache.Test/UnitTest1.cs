using GWebCache.Extensions;

namespace GWebCache.Test;
[TestClass]
public class ExtensionsTest {

	public static IEnumerable<object[]> ValidUrlData {
		get {
			return new[]
			{
			[ 
				new Uri("www.test.com"), 
				new Dictionary<string, object> { { "test", "yeah" } },
				"www.test.com?test=yeah"
			],
			[
				new Uri("www.test.com?test=yeah"),
				new Dictionary<string, object> { { "test", "yeah" } },
				"www.test.com?test=yeah"
			],
			[
				new Uri("www.test.com?test=yeah&test=yeah"),
				new Dictionary<string, object> { { "test", "yeah" } },
				"www.test.com?test=yeah&test=yeah"
			],
			[
				new Uri("www.test.com"),
				new Dictionary<string, object> { { "page", 1}, { "filter", true} },
				"www.test.com?page=1&filter=true"
			],
			new object[] {
				new Uri("www.test.com"),
				new Dictionary<string, object>(),
				"www.test.com"
			},
		  };
		}
	}

	public static IEnumerable<object[]> InvalidUrlData {
		get {
			return new[]
			{
			new object[] {
				new Uri("www.test.com"),
				new Dictionary<string, object> { { "test", null } }
			},
			[
				new Uri("www.test.com"),
				new Dictionary<string, object> { { null, "test"} }
			],
			[
				null,
				new Dictionary<string, object> { { "ttest", "yeah"} }
			],
			[
				new Uri("www.test.com"),
				null
			],
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