namespace teamcitydonut
{
	internal class BrokenBuild
	{
		public string BuildId { get; private set; }
		public string BuildTypeId { get; private set; }
		public string Failure { get; private set; }
		public string BuildNumber { get; private set; }

		public delegate BrokenBuild Factory(string buildId, string buildTypeId, string buildNumber, string failure);

		public BrokenBuild(string buildId, string buildTypeId, string buildNumber, string failure)
		{
			BuildId = buildId;
			BuildTypeId = buildTypeId;
			BuildNumber = buildNumber;
			Failure = failure;
		}
	}
}