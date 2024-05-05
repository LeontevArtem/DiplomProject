using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WorkplacesAccounting.Models
{
	public class AuthModel
	{
		[Required(ErrorMessage = "Не указан Логин")]
		public string Login {  get; set; }

		[Required(ErrorMessage = "Не указан пароль")]
		public string Password { get; set; }
	}
}
