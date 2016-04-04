using System;
using System.Collections.Generic;

namespace teamcitydonut
{
	public interface ITeamCityOptions
	{
		string TeamCityUri { get; set; }
		string Password { get; set; }
		string UserName { get; set; }
		DateTime StartOfStats { get; set; }
		DateTime EndOfStats { get; set; }
		List<string> BuildsOfInterest { get; set; }
	}
}