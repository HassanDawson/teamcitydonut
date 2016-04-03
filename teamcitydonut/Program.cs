using System;
using Autofac;
using Fclp;

namespace teamcitydonut
{
	class Program
	{
		static void Main(string[] args)
		{
			// create a generic parser for the ApplicationArguments type
			var fluentCommandLineParser = new FluentCommandLineParser<TeamCityOptions>();
			fluentCommandLineParser.Setup(f => f.TeamCityUri).As('u', "uri").Required().WithDescription("Url to access TC");
			fluentCommandLineParser.Setup(f => f.Password).As('p', "password").Required().WithDescription("Password to access TC");
			fluentCommandLineParser.Setup(f => f.UserName).As('n', "username").Required().WithDescription("Username to access TC");

			ICommandLineParserResult commandLineParserResult = fluentCommandLineParser.Parse(args);

			if (!commandLineParserResult.HasErrors)
			{
				var teamCityOptions = fluentCommandLineParser.Object;

				var containerBuilder = new ContainerBuilder();
				containerBuilder.RegisterInstance(teamCityOptions).As<ITeamCityOptions>().SingleInstance();
				containerBuilder.RegisterType<MyApplication>().As<IApplication>().SingleInstance();

				var container = containerBuilder.Build();
				var application = container.Resolve<IApplication>();
				bool success = application.Run();
				if (!success)
					Environment.Exit(-1);
			}
			else
			{
				Environment.Exit(-1);
			}
		}
	}
}
