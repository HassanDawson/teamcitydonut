using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using NLogInjector;
using TeamCitySharp;
using TeamCitySharp.ActionTypes;
using TeamCitySharp.DomainEntities;

namespace teamcitydonut
{
	internal class MyApplication : IApplication
	{
		[InjectLogger]
		private readonly ILogger _logger = NullLogger.Instance;

		private readonly ITeamCityOptions _teamCityOptions;

		public MyApplication(ITeamCityOptions teamCityOptions)
		{
			if (teamCityOptions == null)
				throw new InvalidOperationException("Options must be specified");
			_teamCityOptions = teamCityOptions;
		}

		public bool Run()
		{
			_logger.Info($"Running application against <{_teamCityOptions.TeamCityUri}>");

			var client = new TeamCityClient(_teamCityOptions.TeamCityUri);
			client.Connect(_teamCityOptions.UserName, _teamCityOptions.Password);

			//Project extensionsProject = client.Projects.ById("Extensions");

			List<User> users = client.Users.All();

			//List<Build> nonSuccessfulBuilds = client.Builds.NonSuccessfulBuildsForUser("administrator");
			//nonSuccessfulBuilds.Where(b => b.)

			return true;
		}
	}
}