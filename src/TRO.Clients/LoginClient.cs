using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Dto;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using Shouldly;

namespace TRO.Clients
{
	public interface ILoginClient
	{
		/// <summary>
		/// Login as Administrator
		/// </summary>
		Task AsAndie(CancellationToken token = default);

		/// <summary>
		/// Login as Project Administrator using this convention auto.gina.{tenant_id}@wtwco.com
		/// if tenant id is supplied
		/// </summary>
		Task AsGina(CancellationToken token = default);

		/// <summary>
		/// Login as Project Analyst using this convention auto.connie.{tenant_id}@wtwco.com
		/// if tenant id is supplied
		/// </summary>
		Task AsConnie(CancellationToken token = default);
	}

	public sealed class LoginClient : ILoginClient
	{
		private readonly PersonaOptions _personaOpts;
		private readonly PersonaDto _persona;
		private readonly TestDataDto _testData;
		private readonly IRestClient _restClient;

		public LoginClient(
			IOptions<PersonaOptions> personaOpts,
			PersonaDto persona,
			TestDataDto testData,
			IRestClient restClient)
		{
			_personaOpts = personaOpts.Value;
			_persona = persona;
			_testData = testData;
			_restClient = restClient;
		}

		public async Task AsAndie(CancellationToken token = default)
		{
			await As(_personaOpts.AndieUser, _personaOpts.AndiePswd, token);
		}

		public async Task AsGina(CancellationToken token = default)
		{
			var email = _testData.TenantId == 0 ? _personaOpts.GinaUser : $"auto.gina.{_testData.TenantId}@wtwco.com";
			await As(email, _personaOpts.GinaPswd, token);
		}

		public async Task AsConnie(CancellationToken token = default)
		{
			var email = _testData.TenantId == 0 ? _personaOpts.ConnieUser : $"auto.connie.{_testData.TenantId}@wtwco.com";
			await As(email, _personaOpts.ConniePswd, token);
		}

		public async Task As(string userName, string password, CancellationToken token = default)
		{
			_persona.Username = userName;
			_restClient.CookieContainer = new CookieContainer();

			var req = new RestRequest("api/account/login", Method.POST)
				.AddParameter("application/json", JsonConvert.SerializeObject(new { userName, password }), ParameterType.RequestBody);
			var res = await _restClient.ExecuteAsync(req, token);
			res.StatusCode.ShouldBe(HttpStatusCode.OK, $"Login as {userName} failed!\nContent:\n{res.Content}");
			dynamic obj = JsonConvert.DeserializeObject(res.Content.Trim('"').Replace("\\r\\n", "").Replace("\\", ""));
			_persona.AuthToken = obj.auth_token;
			_persona.AuthToken.ShouldNotBeNullOrEmpty();
			_persona.AuthRefreshToken = obj.auth_refreshToken;
			_persona.AuthId = obj.id;
			_persona.AuthClaims = JsonConvert.SerializeObject(obj.claims);
		}
	}
}