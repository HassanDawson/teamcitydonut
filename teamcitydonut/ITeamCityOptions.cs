namespace teamcitydonut
{
	public interface ITeamCityOptions
	{
		string TeamCityUri { get; set; }
		string Password { get; set; }
		string UserName { get; set; }
	}
}