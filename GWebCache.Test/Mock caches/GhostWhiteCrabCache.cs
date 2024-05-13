using GWebCache.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWebCache.Test.Mock_caches;
internal class GhostWhiteCrabCache : IMockCache {
	public string GetGetResponse(GnutellaNetwork? net) {
		throw new NotImplementedException();
	}

	public string GetHostfileResponse() {
		return 
			"127.193.147.7:45710\r\n" +
			"127.33.201.187:19218\r\n" +
			"127.195.122.147:4190\r\n" +
			"127.191.191.250:36594\r\n" +
			"127.202.152.57:5786\r\n";
	}

	public string GetPongRespone() {
		return "PONG GhostWhiteCrab/0.9.7\r\n";
	}

	public string GetStatFileResponse() {
		return "";
	}

	public string GetUpdateReponse() {
		throw new NotImplementedException();
	}

	public string GetUrlfileResponse() {
		return
			"http://test.net/skulls.php\n" +
			"http://test.net:3558/\r\n" +
			"http://test.net/skulls.php\r\n" +
			"http://test.net/g2/bazooka.php\r\n" +
	}
}
