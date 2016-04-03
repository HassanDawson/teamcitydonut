using System;
using System.Security;

namespace teamcitydonut
{
	internal class TeamCityOptions : ITeamCityOptions
	{
		public string TeamCityUri { get; set; }

		public string Password { get; set; }

		public string UserName { get; set; }
	}
}