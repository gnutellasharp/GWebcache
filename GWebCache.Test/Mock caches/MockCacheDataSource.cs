namespace GWebCache.Test.Mock_caches;
public static class MockCacheDataSource {
	public static IEnumerable<object[]> Caches {
		get {
			return new[] {
				[new GhostWhiteCrabCache()],
				[new BazookaCache()],
				[new DkacCache()],
				[new SkullsCache()],
				new object[]{new BeaconCache()},
			};
		}
	}
}
