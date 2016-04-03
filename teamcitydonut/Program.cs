using System;
using Autofac;
using Fclp;
using NLogInjector;

namespace teamcitydonut
{
	class Program
	{
		static void Main(string[] args)
		{
			// create a generic parser for the ApplicationArguments type
			var fluentCommandLineParser = new FluentCommandLineParser<TeamCityOptions>();
			fluentCommandLineParser.Setup(f => f.TeamCityUri).As('t', "uri").Required().WithDescription("Url to access TC");
			fluentCommandLineParser.Setup(f => f.Password).As('p', "password").Required().WithDescription("Password to access TC");
			fluentCommandLineParser.Setup(f => f.UserName).As('u', "username").Required().WithDescription("Username to access TC");

			ICommandLineParserResult commandLineParserResult = fluentCommandLineParser.Parse(args);

			if (!commandLineParserResult.HasErrors)
			{
				var teamCityOptions = fluentCommandLineParser.Object;

				var containerBuilder = new ContainerBuilder();
				containerBuilder.RegisterModule<NLogModule>();
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
				Console.WriteLine($"Usage: teamcitydonut -[t|url] http://teamcity -[u|username] admin -[p|password] XXX");
				foreach (var commandLineParserError in commandLineParserResult.Errors)
				{
					Console.WriteLine($"{commandLineParserError.Option.Description} is a required parameter");
				;
				}
				Environment.Exit(-1);
			}
		}
	}
}
