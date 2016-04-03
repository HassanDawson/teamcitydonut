namespace teamcitydonut
{
	internal class MyApplication : IApplication
	{
		private readonly ITeamCityOptions _teamCityOptions;

		public MyApplication(ITeamCityOptions teamCityOptions)
		{
			_teamCityOptions = teamCityOptions;
		}

		public bool Run()
		{
			return true;
		}
	}
}