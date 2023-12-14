using khiemnguyen_FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace khiemnguyen_FrontEnd.Controllers
{
    public class CatererController : Controller
    {

        APIControl Control=new APIControl();
        public IActionResult Index()
        {
			
			ViewBag.Cid = APIControl.UserID;
		

			IEnumerable<Menu> Mnu = Control.GetEndPoint<Menu>("/GetMenubyCater/"+APIControl.UserID);
                return View(Mnu);
        }


		public IActionResult ViewItems()
		{
			IEnumerable<Category> Cate = Control.GetEndPoint<Category>("/GetCategoryList/" );
			ViewBag.cate = Cate;
			IEnumerable<Foods> Food = Control.GetEndPoint<Foods>("/GetFoodsList/");
			ViewBag.food = Food;
			Foods fd = new Foods();
			return View(fd);
		}

		public IActionResult editprofile()
		{
			IEnumerable<UserInfo> Mnu = Control.GetEndPoint<UserInfo>("/GetUserbyUserName/" + APIControl.UserName);

			return View(Mnu.FirstOrDefault());
		}

		[HttpPost]
		public IActionResult EditUsersPOST(UserInfo model)
		{
			model.CreatedDate = DateTime.Now;

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

			IEnumerable<Order> ss = Control.PostEndPoint<Order>("/UpdateUser/", model);
			IEnumerable<UserInfo> Mnu = Control.GetEndPoint<UserInfo>("/GetUserbyUserName/" + APIControl.UserName);

			return View("editprofile", Mnu.FirstOrDefault());
		}

		public IActionResult DeleteFood(int id)
		{
			
			IEnumerable<Foods> food = Control.GetEndPoint<Foods>("/DeleteItems/"+id);

			IEnumerable<Category> Cate = Control.GetEndPoint<Category>("/GetCategoryList/");
			ViewBag.cate = Cate;
			IEnumerable<Foods> Food = Control.GetEndPoint<Foods>("/GetFoodsList/");
			ViewBag.food = Food;
		
			return RedirectToAction("AddFoods");
		}

		[HttpPost]
		public IActionResult AddItemPOST(Foods model, IFormFile postedFile)
		{

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

			model.Caterid = APIControl.UserID;
			model.Category = model.Name;
			IEnumerable<Foods> ss = Control.PostEndPoint<Foods>("/Addfood/", model);

			return RedirectToAction("ViewItems");
		}
		public IActionResult CreateMenu(int Cid)
		{
			IEnumerable<Category> category = Control.GetEndPoint<Category>("/GetcategoryList/");
					ViewBag.category = category;


			IEnumerable<Foods> Items = Control.GetEndPoint<Foods>("/GetFoodsList/");
			IEnumerable<Foods> appertise = Control.GetEndPoint<Foods>("/GetFoodsbyCategory/Appetizer");
			IEnumerable<Foods> Starter = Control.GetEndPoint<Foods>("/GetFoodsbyCategory/Starter");
			IEnumerable<Foods> Maincourse = Control.GetEndPoint<Foods>("/GetFoodsbyCategory/Main course");
			IEnumerable<Foods> Drink = Control.GetEndPoint<Foods>("/GetFoodsbyCategory/Drink");
			IEnumerable<Foods> Desert = Control.GetEndPoint<Foods>("/GetFoodsbyCategory/Desert");


			ViewBag.Foods = Items;
			var Mnu = new Menu { CaterID=Cid};

			ViewBag.appertise = appertise;
			ViewBag.Stater = Starter;
			ViewBag.MainSourse= Maincourse;
			ViewBag.Drink= Drink;
			ViewBag.Desert = Desert;



			return View(Mnu);
		}

		[HttpPost]

		public IActionResult UpdateStatus()
		{
			var id = Request.Form["id"].ToString();
			var status = Request.Form["status"].ToString();

			StatusUpdate data = new StatusUpdate{ id=int.Parse(id),Status=status };


			IEnumerable<StatusUpdate> ss = Control.PostEndPoint<StatusUpdate>("/UpdateOrderStatus/", data);

			return RedirectToAction("UpdateOrders");
		}

		public IActionResult SaveFeedReply()
		{
			var menuid = Request.Form["menuid"];

			var id = Request.Form["id"];
			var ordid = Request.Form["ordid"];
			var Reply = Request.Form["replyto"];

			FeedBack data = new FeedBack {
				Menuid = int.Parse(menuid),
				id=int.Parse(id),
		         Userid=0,
				Message=Reply,
			 orderid=int.Parse(ordid),
			 rate=0,
			 Replyto=Reply,
			 Timestamp=DateTime.Now,
			 UserName=""

				
			};
			IEnumerable<FeedBack> ss = Control.PostEndPoint<FeedBack>("/UpdateFeedReply/", data);

		
			return RedirectToAction("updateorders");
		}
		public IActionResult RatingReply(int ordid, int menuid)
		{
			IEnumerable<FeedBack> Mnu = Control.GetEndPoint<FeedBack>("/GetFeedBackbyOrderID/" + ordid);

			return View(Mnu);
		}


		public IActionResult UpdateOrders()
		{
			IEnumerable<Order> Mnu = Control.GetEndPoint<Order>("/GetOrdersbyCater/" + APIControl.UserID);

			return View(Mnu);
		}

		[HttpPost]
		public IActionResult CreateMenuPost(Menu model)
		{
			

			if (Request.Form.Files.Count > 0)
			{
				IFormFile image = Request.Form.Files[0];

				var fileName = Path.GetFileName(image.FileName);

				model.israted = 0;

				using (var target = new MemoryStream())
				{
					image.CopyTo(target);
					model.Image = target.ToArray();

				}
			}

			

			IEnumerable<Menu> ss = Control.PostEndPoint<Menu>("/Addmenu/", model);

			IEnumerable<Menu> Mnu = Control.GetEndPoint<Menu>("/GetMenuID/");

			int Menuid = Mnu.FirstOrDefault().id;
			int Appertise = int.Parse(Request.Form["Appertise"].ToString());
			int Starter = int.Parse(Request.Form["Starter"].ToString());
			int Maincourse = int.Parse(Request.Form["Maincourse"].ToString());
			int Drink = int.Parse(Request.Form["Drink"].ToString());
			int Desert = int.Parse(Request.Form["Desert"].ToString());

			List<Food_in_Menu> foods = new List<Food_in_Menu> {
				new Food_in_Menu {Foodid=Appertise,Menuid=Menuid},
				new Food_in_Menu {Foodid=Starter,Menuid=Menuid},
				new Food_in_Menu {Foodid=Maincourse,Menuid=Menuid},
				new Food_in_Menu {Foodid=Drink,Menuid=Menuid},
				new Food_in_Menu {Foodid=Desert,Menuid=Menuid}

			};
			
			IEnumerable<Menu> ss2 = Control.PostEndPoint<Menu>("/AddItemstoMenuRange/", foods);
		


			return RedirectToAction("Index");
		}


		[HttpPost]
		public IActionResult EditMenuPOST(Menu model, IFormFile postedFile)
		{
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
            

           IEnumerable<Menu>  ss= Control.PostEndPoint<Menu>("/Updatemenu/", model);
           

            return RedirectToAction("Editmenu", new {id=model.id });
		}

	

		public IActionResult EditMenu(int id)
		{
			//category
			IEnumerable<Menu> Mnu = Control.GetEndPoint<Menu>("/GetMenubyID/" + id);
			IEnumerable<Category> category = Control.GetEndPoint<Category>("/GetcategoryList/");
			IEnumerable<Foods> Items = Control.GetEndPoint<Foods>("/GetFoodsList/");
			IEnumerable<Food_in_Menu> MnuItems = Control.GetEndPoint<Food_in_Menu>("/GetMenuFoodsbyMenuID/" + id);
            ViewBag.MenuFoods = MnuItems;
			ViewBag.category = category;

			ViewBag.Foods = Items;
			return View(Mnu.FirstOrDefault());
		}

		[HttpPost]
		public IActionResult LoadFoods()
		{

			var cat = Request.Form["Description"].ToString();
			var id = Request.Form["menuid"].ToString();
			IEnumerable<Menu> Mnu = Control.GetEndPoint<Menu>("/GetMenubyID/" + id);
			IEnumerable<Foods> items = Control.GetEndPoint<Foods>("/GetFoodsbyCategory/"+ cat);
			IEnumerable<Category> category = Control.GetEndPoint<Category>("/GetcategoryList/");
			IEnumerable<Food_in_Menu> MnuItems = Control.GetEndPoint<Food_in_Menu>("/GetMenuFoodsbyMenuID/" + id);
			ViewBag.MenuFoods = MnuItems;
			ViewBag.Foods = items;
			ViewBag.category = category;
			return View("EditMenu",Mnu.FirstOrDefault());
		}

	

		public IActionResult AddFoods()
		{
			IEnumerable<Category> category = Control.GetEndPoint<Category>("/GetcategoryList/");
			IEnumerable<Foods> foods = Control.GetEndPoint<Foods>("/GetFoodsListbyCaterer/" + APIControl.UserID);
			ViewBag.category = category;
			ViewBag.foods = foods;
			return View();
		}

		[HttpPost]
		public IActionResult AddFoodsPOST(Foods model)
		{
			IEnumerable<Category> category = Control.GetEndPoint<Category>("/GetcategoryList/");
			model.Caterid = APIControl.UserID;

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

			IEnumerable<Foods> ss = Control.PostEndPoint<Foods>("/AddFoods/", model);
			//AddFoods

			ViewBag.category = category;


			return RedirectToAction("AddFoods");
		}


		public IActionResult additemstoMenu()
        {
            var Menuid = Request.Form["menuid"].ToString();
			var itemid = Request.Form["id"].ToString();
            
            var MenuItems = new Food_in_Menu { 
               
            Menuid=int.Parse(Menuid),
            Foodid=int.Parse(itemid)
            };
			
            IEnumerable<Food_in_Menu> ss = Control.PostEndPoint<Food_in_Menu>("/AddItemstoMenu/", MenuItems);

			return RedirectToAction("EditMenu", new { id = Menuid });
        }
		public IActionResult DeleteMenuItem(int id,int mid)
		{
			Food_in_Menu FiM = new Food_in_Menu { id = id };

			IEnumerable<Food_in_Menu> ss = Control.PostEndPoint<Food_in_Menu>("/DeleteMenuItems/", FiM);

			return RedirectToAction("EditMenu", new { id = mid });
		}
		
	}
}
