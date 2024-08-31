using System.ComponentModel.DataAnnotations;

namespace ReserveiAPI.Objects.Contracts
{
	public class Login
	{
		[Required(ErrorMessage = "O e-mail é requirido!")]
		public string Email { get; set; }
		[Required(ErrorMessage = "A senha é requirido!")]
		public string Password { get; set; }

	}
}
