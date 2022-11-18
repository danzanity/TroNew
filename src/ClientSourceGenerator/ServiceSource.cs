using System;
using System.Collections.Generic;
using System.Text;

namespace ClientSourceGenerator
{
	public sealed class ServiceSource
	{
		public static string Generate(IEnumerable<(string, string)> types)
		{
			var services = new StringBuilder();
			services.Append(@"using Microsoft.Extensions.DependencyInjection;

namespace ClientSourceGenerator
{
	public static class Services
	{
		public static IServiceCollection AddAutoGeneratedClients(this IServiceCollection services) => services");

			foreach (var (@interface, type) in types)
			{
				services.Append($"{Environment.NewLine}\t\t\t.AddTransient<{@interface}, {type}>()");
			}

			services.Append(@";
	}
}");

			return services.ToString();
		}
	}
}