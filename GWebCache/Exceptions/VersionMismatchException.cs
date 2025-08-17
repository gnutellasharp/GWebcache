using System;

namespace GWebCache.Exceptions {
	/// <summary>
	/// This exception is thrown when a V1 method is called on a V2 GWebCache object or vice versa.
	/// </summary>
	public class VersionMismatchException : Exception {
		public VersionMismatchException() : base(
			"Version mismatch: This method is not supported by the current GWebCache version.") {
		}
	}
}