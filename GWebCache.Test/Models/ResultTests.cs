using GWebCache.ResponseProcessing;
using GWebCache.Responses;

namespace GWebCache.Test.Models;
[TestClass]
public class ResultTests {
	[TestMethod]
	public void VerifyErrorReflectedCorrectly() {
		const string errorMessage = "Something went wrong";
		Result<GetResponse> result = new ();
		result.Execute(new HttpResponseMessage() { Content = new StringContent(errorMessage)});
		Assert.IsFalse(result.WasSuccessful);
		Assert.AreEqual(errorMessage, result.ErrorMessage);
	}

	[TestMethod]
	public void TestShorthandConstructor() {
		const string errorMessage = "Something went wrong";
		Result<GetResponse> result = new ();
		result.WithException(errorMessage);
		
		Assert.IsFalse(result.WasSuccessful);
		Assert.AreEqual(errorMessage, result.ErrorMessage);
	}
}