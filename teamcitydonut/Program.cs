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
			fluentCommandLineParser.Setup(f => f.BuildsOfInterest).As('b', "builds").Required().WithDescription("The configuration ID of the builds we are interested in");
			fluentCommandLineParser.Setup(f => f.StartOfStats).As('s', "start").WithDescription("Start date for statistics gathering").SetDefault(DateTime.Today.AddDays(-1*(DateTime.Today.Day - 1))); // default is start of month
			fluentCommandLineParser.Setup(f => f.EndOfStats).As('e', "end").WithDescription("End date for statistics gathering").SetDefault(DateTime.Today.AddDays(DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month) - DateTime.Today.Day));

			ICommandLineParserResult commandLineParserResult = fluentCommandLineParser.Parse(args);

			if (!commandLineParserResult.HasErrors)
			{
				var teamCityOptions = fluentCommandLineParser.Object;

				var containerBuilder = new ContainerBuilder();
				containerBuilder.RegisterModule<NLogModule>();
				containerBuilder.RegisterInstance(teamCityOptions).As<ITeamCityOptions>().SingleInstance();
				containerBuilder.RegisterType<MyApplication>().As<IApplication>().SingleInstance();
				containerBuilder.RegisterType<BrokenBuildByUser>();
				containerBuilder.RegisterType<BrokenBuild>();

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
