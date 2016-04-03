using System;
using System.Security;

namespace teamcitydonut
{
	internal class TeamCityOptions : ITeamCityOptions
	{
		public Uri TeamCityUri { get; set; }

		public SecureString Password { get; set; }

		public string UserName { get; set; }
	}
}