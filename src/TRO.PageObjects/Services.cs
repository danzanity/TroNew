using Microsoft.Extensions.DependencyInjection;
using PageObjectSourceGenerator;

namespace TRO.PageObjects
{
	public static class Services
	{
		public static IServiceCollection AddPageObjects(this IServiceCollection services) => services
			.AddAutoGeneratedPageObjects();
	}
}