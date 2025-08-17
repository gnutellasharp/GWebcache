using System;

namespace GWebCache.Models{
	/// <summary>
	/// A model representing a GWebCache
	/// </summary>
	public class GWebCacheNode {
		/// <summary>
		/// The url of the GWebCache
		/// </summary>
		public Uri Url { get; }

		/// <summary>
		/// When did the GWebCache first gain knowledge about the other GWebCache.
		/// </summary>
		/// <remarks>This is exclusively provided by V2 GWebCache so always check if this is not filled in</remarks>
		public TimeSpan ActiveSince { get; set; }

		/// <summary>
		///  Constructs a new GWebCacheNode
		/// </summary>
		/// <param name="url">string representation of the url</param>
		/// <exception cref="ArgumentException">If the url could not be parsed to an absolute URI</exception>
		public GWebCacheNode(string url) {
			if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
				throw new ArgumentException($"{url} is not a valid url");

			this.Url = uri;
		}


		/// <summary>
		///  Constructs a new GWebCacheNode
		/// </summary>
		public GWebCacheNode(Uri uri) {
			this.Url = uri;
		}

		/// <summary>
		/// Converts the GWebCacheNode to a string representation
		/// </summary>
		/// <returns>UrlEncoded Url of the GWebCache</returns>
		public override string ToString() {
			return Url?.ToString() ?? "";
		}

		/// <summary>
		/// Two GWebCaches are equal if the url is the same.
		/// </summary>
		/// <returns>A boolean indicating if two GWebCaches are the same</returns>
		public override bool Equals(object obj) {
			if (obj is GWebCacheNode node)
				return node.Url == Url;

			return false;
		}

		/// <summary>
		/// Not relevant since the Equals method doesn't need it
		/// </summary>
		/// <exception cref="NotImplementedException"></exception>
		public override int GetHashCode() {
			throw new NotImplementedException();
		}
	}
}