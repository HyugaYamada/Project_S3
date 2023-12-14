using khiemnguyen_FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace khiemnguyen_FrontEnd.Controllers
{
	public class AdminController : Controller
	{
		APIControl Control = new APIControl();
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult AddCategories()
		{
			
			IEnumerable<Category> category = Control.GetEndPoint<Category>("/GetCategoryList/");

			ViewBag.category = category;

			
			return View();
		}

		[HttpPost]
		public IActionResult AddCategoriesPOST(Category model)
		{
			
					IEnumerable<Foods> ss = Control.PostEndPoint<Foods>("/AddCategory/", model);

			return RedirectToAction("AddCategories");
		}
		
		public IActionResult DeleteCategoury(int id)
		{
			var cate = new Category { id = id ,Name="delete"};
			IEnumerable<Foods> ss = Control.PostEndPoint<Foods>("/DeleteCategory/",cate);

			return RedirectToAction("AddCategories");
		}
		public IActionResult AddUsers()
		{

			IEnumerable<UserInfo> allUsers = Control.GetEndPoint<UserInfo>("/GetUsers/");
			IEnumerable<UserInfo> users = allUsers.Where(u => u.Role != "Admin").ToList();

			ViewBag.users = users;

			return View();
		}

		[HttpPost]
		public IActionResult AddUsersPOST(UserInfo model)
		{
			model.CreatedDate = DateTime.Now;

			int QuationNo = int.Parse(Request.Form["QuationNo"].ToString());
			model.quationNo = QuationNo;

			if (Request.Form.Files.Count > 0)
			{
				IFormFile image = Request.Form.Files[0];

				var fileName = Path.GetFileName(image.FileName);



				using (var target = new MemoryStream())
				{
					image.CopyTo(target);
					model.Image = target.ToArray();

				}
			}

			IEnumerable<UserInfo> ss = Control.PostEndPoint<UserInfo>("/Adduser/", model);


			return RedirectToAction("Addusers");
		}

		[HttpPost]
		public IActionResult EditUsersPOST(UserInfo model)
		{
			model.CreatedDate = DateTime.Now;
			IEnumerable<Order> ss = Control.PostEndPoint<Order>("/UpdateUser/", model);
			IEnumerable<UserInfo> Mnu = Control.GetEndPoint<UserInfo>("/GetUserbyUserName/" + APIControl.UserName);

			return View("editprofile", Mnu.FirstOrDefault());
		}



		public IActionResult editprofile()
		{
			IEnumerable<UserInfo> Mnu = Control.GetEndPoint<UserInfo>("/GetUserbyUserName/" + APIControl.UserName);

			return View(Mnu.FirstOrDefault());
		}
		public IActionResult DeleteUser(int id)
		{
			var Usr = new UserInfo { UserId = id };
			IEnumerable<Foods> ss = Control.PostEndPoint<Foods>("/Deleteuser/", Usr);

			return RedirectToAction("Addusers");
		}

		public IActionResult BanUser(int id)
		{
			var Usr = new UserInfo { UserId = id };
			IEnumerable<UserInfo> ss = Control.PostEndPoint<UserInfo>("/Banuser/", Usr);

			return RedirectToAction("Addusers");
		}
	}
}
