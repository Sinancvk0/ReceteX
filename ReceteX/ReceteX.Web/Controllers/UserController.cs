using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ReceteX.Models;
using ReceteX.Repository.Shared.Abstract;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ReceteX.Web.Controllers
{
	public class UserController : Controller
	{
		private readonly IUnitOfWork unitOfWork;

		public UserController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		//admin olarak girildiğinde  tüm userları görüntüleyeceğimiz ekran 

		[Authorize(Roles = "Admin")]
		public IActionResult List()
		{
			//DataTables oluşabilmek için verilerin hepsinin DATA isimli bir obje içinde gelme kuralı koyuyor. O yuzden aşağıdaki gibi yeni
			//bir anonim nesne oluşturup içerisine data diye bir field açıyoruz ve bütün datayı onun içine koyuyoruz. Bunu sırf DataTables için yapıyoruz.
			return Json(new { data = unitOfWork.Users.GetAll() });

		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public IActionResult Create(AppUser appUser)
		{

			unitOfWork.Users.Add(appUser);
			unitOfWork.Save();
			return Ok();

		}
		[Authorize(Roles = "Admin")]
		[HttpPost]
		public IActionResult Update(AppUser appUser)
		{
			unitOfWork.Users.Update(appUser);
			unitOfWork.Save();
			return Ok();

		}
		[Authorize(Roles = "Admin")]
		[HttpPost]

		public IActionResult GetById(Guid guid)
		{
			AppUser usr = unitOfWork.Users.GetFirstOrDefault(u => u.Id == guid);

			return Json(usr);


		}


		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Login()
		{
			return View();

		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login", "User");

		}

		public IActionResult login()
		{
			return View();

		}

		[HttpPost]

		public async Task<IActionResult> login(AppUser user)
		{
			if (user != null)
			{
				AppUser usr = unitOfWork.Users.GetFirstOrDefault(u => u.Email == user.Email && u.Password == user.Password && u.isActive && !u.isDeleted);

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
