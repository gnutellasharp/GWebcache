using GWebCache.ReponseProcessing;
using GWebCache.Reponses;

namespace GWebCache.Test.Models;
[TestClass]
public class ResultTest {
	[TestMethod]
	public void VerifyErrorReflectedCorrectly() {
		string errorMessage = "Something went wrong";
		Result<MockFailResult> result = new();
		result.Execute(new HttpResponseMessage() { Content = new StringContent(errorMessage)});
		Assert.IsFalse(result.WasSuccessful);
		Assert.AreEqual(errorMessage, result.ErrorMessage);
	}

	[TestMethod]
	public void TestShorthandConstructor() {
		string errorMessage = "Something went wrong";
		Result<MockFailResult> result = new();
		result.WithException(errorMessage);
		Assert.IsFalse(result.WasSuccessful);
		Assert.AreEqual(errorMessage, result.ErrorMessage);
	}

	[TestMethod]
	public void VerifyV1ResponseIsParsedCorrectly() {
		Result<MockV1Result> result = new();
		result.Execute(new HttpResponseMessage());

		Assert.IsNotNull(result.ResultObject);
		Assert.IsTrue(result.WasSuccessful);
		Assert.IsTrue(string.IsNullOrEmpty(result.ErrorMessage));
		Assert.IsTrue(result.ResultObject.IsV1);
	}


	[TestMethod]
	public void VerifyV2ResponseIsParsedCorrectly() {
		Result<MockV2Result> result = new();
		result.Execute(new HttpResponseMessage());

		Assert.IsNotNull(result.ResultObject);
		Assert.IsTrue(result.WasSuccessful);
		Assert.IsTrue(string.IsNullOrEmpty(result.ErrorMessage));
		Assert.IsTrue(result.ResultObject.IsV2);
	}
}

internal class MockV1Result : GWebCacheResponse {
	public bool IsV2 => !IsV1;
	public bool IsV1 { get; set; }

	internal override bool IsValidResponse(HttpResponseMessage? responseMessage) {
		return true;
	}

	internal override bool IsValidV2Response(HttpResponseMessage? responseMessage) {
		return false;
	}
	
	internal override void Parse(HttpResponseMessage response) {
		IsV1 = true;
	}

	internal override void ParseV2(HttpResponseMessage response) {
		IsV1 = false;
	}
}

internal class MockV2Result : GWebCacheResponse {
	public bool IsV2 => !IsV1;
	public bool IsV1 { get; set; }

	internal override bool IsValidResponse(HttpResponseMessage? responseMessage) {
		return true;
	}

	internal override bool IsValidV2Response(HttpResponseMessage? responseMessage) {
		return true;
	}

	internal override void Parse(HttpResponseMessage response) {
		IsV1 = true;
	}

	internal override void ParseV2(HttpResponseMessage response) {
		IsV1 = false;
	}
}

internal class MockFailResult : GWebCacheResponse {
	internal override bool IsValidResponse(HttpResponseMessage? responseMessage) {
		return false;
	}

	//also checks here if the valid response is false this should also é
	internal override bool IsValidV2Response(HttpResponseMessage? responseMessage) {
		return true;
	}
	internal override void Parse(HttpResponseMessage response) {
		throw new NotImplementedException();
	}

	internal override void ParseV2(HttpResponseMessage response) {
		throw new NotImplementedException();
	}
}