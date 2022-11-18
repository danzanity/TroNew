namespace Core.Dto
{
	public class PersonaDto
	{
		public string Username { get; set; }
		public string AuthToken { get; set; }

		// TODO: CHECK IF THESE ARE NEEDED IN THE API
		public string AuthId { get; set; }
		public string AuthClaims { get; set; }
		public string AuthRefreshToken { get; set; }
	}
}