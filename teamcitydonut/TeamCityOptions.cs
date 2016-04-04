using System;
using System.Collections.Generic;
using System.Security;

namespace teamcitydonut
{
	internal class TeamCityOptions : ITeamCityOptions
	{
		public string TeamCityUri { get; set; }

		public string Password { get; set; }

		public string UserName { get; set; }

		public DateTime StartOfStats { get; set; }

		public DateTime EndOfStats { get; set; }

		public List<string> BuildsOfInterest { get; set; }
	}
}