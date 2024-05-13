using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWebCache.Test.Mock_caches;
public static class MockCacheDataSource {
	public static IEnumerable<object[]> Caches {
		get {
			return new[] {
				[new GhostWhiteCrabCache()],
				[new BazookaCache()],
				[new DKACCache()],
				[new SkullsCache()],
				new object[]{new BeaconCache()},
			};
		}
	}
}
