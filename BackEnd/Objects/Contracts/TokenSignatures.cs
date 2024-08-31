namespace ReserveiAPI.Objects.Contracts
{
	public class TokenSignatures
	{
		public string Issuer { get; } = "Reservei Api";
		public string Audience { get; } = "Reservei Api WebSite";

		public string Key { get; } = "Reservei_Barrament_API_Autentication";

	}
}
