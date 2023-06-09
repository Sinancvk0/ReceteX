using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ReceteX.Models;
using ReceteX.Repository.Shared.Abstract;
using System.Security.Claims;

namespace ReceteX.Web.Controllers
{
	public class UserController : Controller
	{
		private readonly IUnitOfWork unitOfWork;
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Login()
		{
			return View();

		}
		[HttpPost]

		public async Task<IActionResult> login (AppUser user)
		{
			if (user != null)
			{
				AppUser usr = unitOfWork.Users.GetFirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);

				if (usr != null)
				{
					List<Claim> claims = new List<Claim>();
					claims.Add(new Claim(ClaimTypes.Name, usr.Name));
					claims.Add(new Claim(ClaimTypes.NameIdentifier, usr.Id.ToString()));
					if (usr.isAdmin)
					{
						claims.Add(new Claim(ClaimTypes.Role, "Admin"));

					}
					else
					{
						claims.Add(new Claim(ClaimTypes.Role, "User"));
					}
					var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

					await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
					return RedirectToAction("Index", "Home");

				}
				else
				{
					return View();
				}
			

			}
			else
			{
				return View();
			}


		}

	}
}
