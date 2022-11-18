using System;
using Microsoft.Extensions.Configuration;

namespace TRO.Clients
{
	public interface IRestClient : RestSharp.IRestClient { }

	public class RestClient : RestSharp.RestClient, IRestClient
	{
		public RestClient(IConfiguration cfg)
		{
			BaseUrl = new Uri(cfg.GetValue<string>("apiBaseUrl"));
		}
	}
}