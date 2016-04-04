using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using NLogInjector;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;

namespace teamcitydonut
{
	internal class MyApplication : IApplication
	{
		[InjectLogger]
		private readonly ILogger _logger = NullLogger.Instance;

		private readonly ITeamCityOptions _teamCityOptions;
		private readonly Func<string, BrokenBuildByUser> _brokenBuildByUserFactory;

		public MyApplication(ITeamCityOptions teamCityOptions, Func<string, BrokenBuildByUser> brokenBuildByUserFactory)
		{
			if (teamCityOptions == null)
				throw new InvalidOperationException("Options must be specified");
			_teamCityOptions = teamCityOptions;
			_brokenBuildByUserFactory = brokenBuildByUserFactory;
		}

		public bool Run()
		{
			_logger.Info($"Running application against <{_teamCityOptions.TeamCityUri}>");

			var client = new TeamCityClient(_teamCityOptions.TeamCityUri);
			client.Connect(_teamCityOptions.UserName, _teamCityOptions.Password);
			
			var buildLocatorFailure = BuildLocator.WithDimensions(status: BuildStatus.FAILURE, maxResults: 1000, sinceDate: _teamCityOptions.StartOfStats, untilDate: new DateTime(2016, 3, 31, 23, 59, 59));
			var buildLocatorError = BuildLocator.WithDimensions(status: BuildStatus.ERROR, maxResults: 1000, sinceDate: _teamCityOptions.StartOfStats, untilDate: new DateTime(2016, 3, 31, 23, 59, 59));
			List<Build> failedBuilds = client.Builds.ByBuildLocator(buildLocatorFailure);
			List<Build> errorBuilds = client.Builds.ByBuildLocator(buildLocatorError);

			IEnumerable<Build> buildsOfInterest = errorBuilds.Union(failedBuilds).Where(b => _teamCityOptions.BuildsOfInterest.Contains(b.BuildTypeId));

			Dictionary<string, BrokenBuildByUser> brokenBuildsByUsers = new Dictionary<string, BrokenBuildByUser>();
			foreach (var build in buildsOfInterest)
			{
				var changeLocator = ChangeLocator.WithDimensions(buildType: build.BuildTypeId, build: build.Id);
				List<Change> changes = client.Changes.ByChangeLocator(changeLocator);

				foreach (var change in changes)
				{
					BrokenBuildByUser brokenBuildByUser;
					if (brokenBuildsByUsers.ContainsKey(change.Username))
					{
						brokenBuildByUser = brokenBuildsByUsers[change.Username];
					}
					else
					{
						brokenBuildByUser = _brokenBuildByUserFactory(change.Username);
						brokenBuildsByUsers.Add(change.Username, brokenBuildByUser);
					}
					brokenBuildByUser.AddBrokenBuild(build.Id, build.BuildTypeId, build.Number, build.StatusText);
				}
			}

			foreach (var brokenBuildsByUser in brokenBuildsByUsers.Values.OrderByDescending(b => b.BrokenBuilds.Count()))
			{
				_logger.Info(brokenBuildsByUser.ToString());
			}

			return true;
		}
	}
}