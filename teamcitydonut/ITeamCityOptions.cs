using System;
using System.Security;

namespace teamcitydonut
{
	public interface ITeamCityOptions
	{
		Uri TeamCityUri { get; set; }
		SecureString Password { get; set; }
		string UserName { get; set; }
	}
}