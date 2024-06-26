﻿using GWebCache.Extensions;
namespace GWebCache.Reponses;

public class StatFileResponse : GWebCacheResponse {
	public int TotalNumberOfRequests { get; set; }
	public int RequestsInLastHour { get; set; }
	public int? UpdateRequestsInLastHour { get; set; }


	internal override bool IsValidResponse(HttpResponseMessage? responseMessage) {
		if (!base.IsValidResponse(responseMessage))
			return false;

		string[] lines = responseMessage!.ContentAsString().Split("\n").Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l)).ToArray();
		return lines.Length > 2 && lines.All(line => int.TryParse(line, out int _));
	}

	internal override bool IsValidV2Response(HttpResponseMessage? responseMessage) {
		return false;
	}

	internal override void Parse(HttpResponseMessage response) {
		string[] lines = response.ContentAsString().Split("\n").Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l)).ToArray();
		TotalNumberOfRequests = int.Parse(lines[0]);
		RequestsInLastHour = int.Parse(lines[1]);
		if (lines.Length > 2) {
			UpdateRequestsInLastHour = int.Parse(lines[2]);
		}
	}

	internal override void ParseV2(HttpResponseMessage response) {
		throw new NotImplementedException();
	}
}
