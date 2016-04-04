using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace teamcitydonut
{
	internal class BrokenBuildByUser
	{
		private readonly BrokenBuild.Factory _brokenBuildFactory;
		private readonly IList<BrokenBuild> _brokenBuilds;

		public string UserName { get; private set; }

		public IEnumerable<BrokenBuild> BrokenBuilds => _brokenBuilds;

		public BrokenBuildByUser(BrokenBuild.Factory brokenBuildFactory, string userName)
		{
			_brokenBuildFactory = brokenBuildFactory;
			UserName = userName;
			_brokenBuilds = new List<BrokenBuild>();
		}

		public void AddBrokenBuild(string buildId, string buildTypeId, string buildNumber, string failure)
		{
			if (_brokenBuilds.Any(b => b.BuildNumber == buildNumber))
				return;
			_brokenBuilds.Add(_brokenBuildFactory(buildId, buildTypeId, buildNumber, failure));
		}

		public override string ToString()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"======{UserName} Total:{BrokenBuilds.Count()}======");
			foreach (var brokenBuild in BrokenBuilds)
			{
				stringBuilder.AppendLine($"Build configuration: {brokenBuild.BuildTypeId} Build number:{brokenBuild.BuildNumber}");
			}
			stringBuilder.AppendLine();
			return stringBuilder.ToString();
		}
	}
}