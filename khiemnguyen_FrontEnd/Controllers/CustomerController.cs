using khiemnguyen.WebApi.Models;
using khiemnguyen_FrontEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using BraintreeHttp;
using PayPal.Core;
using PayPal.v1.Payments;
using static System.Net.Mime.MediaTypeNames;
using Order = khiemnguyen_FrontEnd.Models.Order;

namespace khiemnguyen_FrontEnd.Controllers
{
	public class CustomerController : Controller
	{
		private readonly string _clientId;
		private readonly string _secretKey;

		APIControl Control = new APIControl();
		public CustomerController(IConfiguration configuration)
		{
			_clientId = configuration["PaypalSettings:ClientId"];
			_secretKey = configuration["PaypalSettings:SecretKey"];
		}
		public IActionResult Index()
		{
			ViewBag.Cid = APIControl.UserID;
			
			IEnumerable<Category> category = Control.GetEndPoint<Category>("/GetcategoryList/");
			IEnumerable<UserInfo> caterers = Control.GetEndPoint<UserInfo>("/GetCaterers/");
			IEnumerable<Menu> Mnu = Control.GetEndPoint<Menu>("/GetMenu/");
			ViewBag.category = category;
			ViewBag.caterers = caterers;

			return View(Mnu);
			
		}

		[HttpPost]
		public IActionResult searchbyfoods()
		{

			IEnumerable<Category> category = Control.GetEndPoint<Category>("/GetcategoryList/");
			IEnumerable<UserInfo> caterers = Control.GetEndPoint<UserInfo>("/GetCaterers/");
		
			ViewBag.category = category;
			ViewBag.caterers = caterers;


			string search = Request.Form["search-field"].ToString();
			IEnumerable<Menu> Mnu = Control.GetEndPoint<Menu>("/GetMenubyCatererFood/" + search);

			return View("Index",Mnu);
		}

		[HttpPost]
		public IActionResult SearchMenue()
		{
			var Caterer = Request.Form["id"].ToString();
			var Category = Request.Form["Name"].ToString();
			ViewBag.Cid = APIControl.UserID;
			//GetMenubyCatererCategoury

			IEnumerable<Category> category = Control.GetEndPoint<Category>("/GetcategoryList/");
			IEnumerable<UserInfo> caterers = Control.GetEndPoint<UserInfo>("/GetCaterers/");
			IEnumerable<Menu> Mnu = Control.GetEndPoint<Menu>("/GetMenubyCatererCategoury/"+ Caterer+ "/" + Category);
			ViewBag.category = category;
			ViewBag.caterers = caterers;

			return View("Index",Mnu);

		
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult EditUsersPOST(UserInfo model)
		{
			model.CreatedDate = DateTime.Now;
			IEnumerable<Order> ss = Control.PostEndPoint<Order>("/UpdateUser/", model);
			IEnumerable<UserInfo> Mnu = Control.GetEndPoint<UserInfo>("/GetUserbyUserName/" + APIControl.UserName);

			return View("editprofile",Mnu.FirstOrDefault());
		}

		[HttpPost]

		public IActionResult Changepassword()
		{
			IEnumerable<UserInfo> Mnu = Control.GetEndPoint<UserInfo>("/GetUserbyUserName/" + APIControl.UserName);
			string Msg = "";
			var oldpwd = Request.Form["oldpwd"].ToString();
			var newpwd = Request.Form["newpwd"].ToString();
			var conpwd = Request.Form["conpwd"].ToString();

			if (Mnu.FirstOrDefault().Password != oldpwd)
			{
				Msg = "Old Password Mismatched!";
			}
			else if (newpwd != conpwd)
			{
				Msg = "New Password Mismatched!";
			}
			ViewBag.msg = Msg;

			if (Msg.Trim() == "")
			{
				Mnu.FirstOrDefault().Password = newpwd;
				Mnu.FirstOrDefault().CreatedDate = DateTime.Now;
				IEnumerable<Order> ss = Control.PostEndPoint<Order>("/UpdateUser/", Mnu.FirstOrDefault());
			}

			return View("editprofile", Mnu.FirstOrDefault());
		}
		public IActionResult editprofile()
		{
			IEnumerable<UserInfo> Mnu = Control.GetEndPoint<UserInfo>("/GetUserbyUserName/" + APIControl.UserName);

			return View(Mnu.FirstOrDefault());
		}
		public IActionResult RegistorUsers()
		{
		
			return View();
		}

		public IActionResult ResetPassword()
		{
			return View();
		}

		[HttpPost]
		public IActionResult ResetPasswordPOST(UserInfo model)
		{
			string Msg = "";
			IEnumerable<UserInfo> Mnu = Control.GetEndPoint<UserInfo>("/GetUserbyUserName/" + model.Email);
		
			var newpwd = Request.Form["newpwd"].ToString();
			var conpwd = Request.Form["conpwd"].ToString();
			var QuationNo = Request.Form["QuationNo"].ToString();


			if (Mnu.FirstOrDefault().quationNo!=int.Parse(QuationNo))
			{
				Msg = "Inavlid Security Quation or Awnswer!";
			}
			if (Mnu.FirstOrDefault().Answer !=model.Answer)
			{
				Msg = "Inavlid Security Quation or Awnswer!";
			}
			if (Mnu.Count()<=0)
			{
				Msg = "Email Address not found!";
			}

			if (newpwd != conpwd)
			{
				Msg = "New Password Mismatched!";
			}

			ViewBag.msg = Msg;

			if (Msg.Trim() == "")
			{
				Mnu.FirstOrDefault().Password = newpwd;
				Mnu.FirstOrDefault().CreatedDate = DateTime.Now;
				IEnumerable<Order> ss = Control.PostEndPoint<Order>("/UpdateUser/", Mnu.FirstOrDefault());
			}

			return View("ResetPassword");
		}
		[HttpPost]

		public IActionResult RegistorUsersPOST(UserInfo model)
		{
			model.CreatedDate = DateTime.Now;
			model.Role = "Customer";
			IFormFile image = Request.Form.Files[0];
			int QuationNo =int.Parse( Request.Form["QuationNo"].ToString());
			model.quationNo = QuationNo;
			var fileName = Path.GetFileName(image.FileName);



			using (var target = new MemoryStream())
			{
				image.CopyTo(target);
				model.Image = target.ToArray();

			}

			IEnumerable<UserInfo> ss = Control.PostEndPoint<UserInfo>("/Adduser/", model);


			return RedirectToAction("Login");
		
		}
		[HttpPost]
		public IActionResult LoginPOST()
		{
			var user = Request.Form["username"].ToString();
			var password = Request.Form["password"].ToString();
			IEnumerable<UserInfo> Mnu = Control.GetEndPoint<UserInfo>("/GetUserbyUserName/" + user);

			if (Mnu.Count() > 0 && password == Mnu.FirstOrDefault().Password)
			{
				if (Mnu.FirstOrDefault().StatusAccount)
				{
					TempData["ErrorMessage"] = "You have been banned. Please contact the admin.";
					return RedirectToAction("Login", "Customer");
				}

				// Set APIControl properties
				APIControl.UserName = Mnu.FirstOrDefault().Email;
				APIControl.UserID = Mnu.FirstOrDefault().UserId;
				APIControl.FullName = Mnu.FirstOrDefault().FullName;
				APIControl.Role = Mnu.FirstOrDefault().Role;

				// Redirect based on role
				if (APIControl.Role == "Caterer")
				{
					return RedirectToAction("Index", "Caterer");
				}
				else if (APIControl.Role == "Customer")
				{
					return RedirectToAction("Index", "Customer");
				}
				else if (APIControl.Role == "Admin")
				{
					return RedirectToAction("Index", "Admin");
				}
			}
			else
			{
				APIControl.UserName = "";
				APIControl.UserID = 0;
				APIControl.FullName = "";
			}

			return RedirectToAction("Login", "Customer");
		}

		[HttpPost]
		public IActionResult SendEmailToAdmin()
		{
			try
			{
				string userName = Request.Form["userName"].ToString();
				string senderEmail = Request.Form["senderEmail"].ToString();
				string subject = Request.Form["subject"].ToString();
				string message = Request.Form["message"].ToString();

				SendEmailToAdmin(userName, senderEmail, subject, message);

				TempData["InfoMessage"] = "Mail sent to admin. We will answer your mail soon. Thanks.";

				return RedirectToAction("contactus");
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "An error occurred while sending the email.";
				return RedirectToAction("contactus");
			}
		}


		private void SendEmailToAdmin(string userName, string senderEmail, string subject, string body)
		{
			var mimeMessage = new MimeMessage();
			mimeMessage.From.Add(new MailboxAddress(userName, senderEmail));
			mimeMessage.To.Add(new MailboxAddress("Admin", "lazybee9711@gmail.com"));
			mimeMessage.Subject = subject;
			mimeMessage.Body = new TextPart("plain") { Text = senderEmail+" "+ "Dear Admin: " + body };
			using (MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient())
			{
				smtpClient.Connect("smtp.gmail.com", 587, false);
				smtpClient.Authenticate("phamanhtuan11197@gmail.com", "yzwojvbcppukmrom");

				// Send the email
				smtpClient.Send(mimeMessage);

				// Disconnect from the server
				smtpClient.Disconnect(true);
			}
		}

		public IActionResult Logout()
		{
			APIControl.UserID = 0;
			APIControl.FullName = "";
			APIControl.UserName = "";
			return RedirectToAction("Login","Customer");
		}


		public IActionResult ViewOrderDetails(int id)
		{
		



					IEnumerable<Order> Ord = Control.GetEndPoint<Order>("/GetOrdersbyid2/" + id);
			IEnumerable<Food_in_Menu> MnuItems = Control.GetEndPoint<Food_in_Menu>("/GetMenuFoodsbyMenuID/" + Ord.FirstOrDefault().Menuid);
			IEnumerable<Menu> Mnu = Control.GetEndPoint<Menu>("/GetMenubyID/" + MnuItems.FirstOrDefault().Menuid);


			IEnumerable<UserInfo> Us = Control.GetEndPoint<UserInfo>("/GetUserbyID/" + Mnu.FirstOrDefault().CaterID);

			ViewBag.chef = Us.FirstOrDefault().FullName;
			ViewBag.MenuFoods = MnuItems;
			return View(Ord.FirstOrDefault());
		}
		public IActionResult addFavorite(int id,int mid)
		{
			var data = new Favor_Cater {Caterid=id,Custid=APIControl.UserID,name="" };
			


			IEnumerable<Order> ss = Control.PostEndPoint<Order>("/AddFavorit/", data);

			return RedirectToAction("singleshop",new {id=mid});
		}
		public IActionResult CheckOrders()
		{
			IEnumerable<Order> Mnu = Control.GetEndPoint<Order>("/GetOrdersbyUser/" + APIControl.UserID);

			return View(Mnu);
		}

		public IActionResult Ordercancel(int id)
		{
			
		
			var Pr = new Para { id = id };

			IEnumerable<UserInfo> ss = Control.PostEndPoint<UserInfo>("/CancelOrder/",Pr);

			IEnumerable<Order> Mnu = Control.GetEndPoint<Order>("/GetOrdersbyUser/" + APIControl.UserID);

			return RedirectToAction("CheckOrders");
	
		}

		public IActionResult PayNow()
		{


			List<Order> Ord = new List<Order>();

			foreach (var obj in CartControl.MyCart)
			{
				Order data = new Order
				{
					id = 0,
					Name = APIControl.UserName,
					GuestNum = 1,
					Caterid = obj.caterid,
					Custid = APIControl.UserID,
					Description = "Order",
					Menuid = obj.menuid,
					Quantity = obj.Quantity,
					TotalPrice = obj.Price,
					OrdDate = obj.dob,
					DelivDate = DateTime.Now,
					Status = "P",
					israted = 0,
					MenuName=obj.MenuName,
					IsCanceled=0,
					Venue=obj.Venue
					
					
					
				};
				Ord.Add(data);
			}
	
			IEnumerable<Order> ss = Control.PostEndPoint<Order>("/AddOrder/", Ord);
			CartControl.MyCart.Clear();

			return RedirectToAction("CheckOrders");
		}

		public IActionResult DeleteCartItem(int id)
		{
			CartControl.MyCart.RemoveAt(id);
			return RedirectToAction("ViewCart");
		}
		public IActionResult Message(int id)
		{

			IEnumerable<UserInfo> Mnu = Control.GetEndPoint<UserInfo>("/GetUserbyID/"+id);
			ViewBag.caterer = Mnu.FirstOrDefault().FullName;
			ViewBag.catererid = Mnu.FirstOrDefault().UserId;
			
			return View();
		}


		public IActionResult deleteMessage(int id,int recvid)
		{
			IEnumerable<Message> Mnu = Control.GetEndPoint<Message>("/DeleteMessage/" + id);

			return RedirectToAction("ViewMail", new { id = recvid });
		}

		public IActionResult ViewCaterer(int id)
		{
			

						IEnumerable<UserInfo> Mnu = Control.GetEndPoint<UserInfo>("/GetUserbyID/" + id);




			return View(Mnu.FirstOrDefault());
		}
		public IActionResult Viewfavourite()
		{
			
					IEnumerable<Favor_Cater> Mnu = Control.GetEndPoint<Favor_Cater>("/GetfavouriteList/" + APIControl.UserID);

			return View(Mnu);
		}

		public IActionResult Deletefavourite(int id)
		{

			IEnumerable<Favor_Cater> Mnu = Control.GetEndPoint<Favor_Cater>("/deletefavourite/" + id);

			return RedirectToAction("Viewfavourite");
		}
		public IActionResult ShowAllMessages(int senderid)
		{
			IEnumerable<Message> messages = Control.GetEndPoint<Message>("/GetAllMessagebyUser/4/2/");

			//IEnumerable<Message> messages = Control.GetEndPoint<Message>("/GetMessagebyUser/" + "4/2/");
			ViewBag.messages = messages;

			IEnumerable<Message2> Mnu = Control.GetEndPoint<Message2>("/GetMailInbox3/" + APIControl.UserID);

			IEnumerable<Message> Mnu2 = Control.GetEndPoint<Message>("/GetMessagebyID/" + senderid);
			IEnumerable<UserInfo> Usr = Control.GetEndPoint<UserInfo>("/GetUserbyID/" + Mnu.FirstOrDefault().User_sent);
			List<Message2> tt = Mnu.Where(x => x.User_sent != APIControl.UserID).DistinctBy(x => x.User_sent).ToList();

			ViewBag.caterer = Usr.FirstOrDefault().FullName;
			ViewBag.mesgid = senderid;
			ViewBag.catererid = Usr.FirstOrDefault().UserId;
			ViewBag.smessage = Mnu2.FirstOrDefault().Message_note;



			return View("MailBox", tt);
		}
		public IActionResult MailBox()
		{
			
					IEnumerable<Message2> Mnu = Control.GetEndPoint<Message2>("/GetMailInbox3/" +APIControl.UserID);
			List<Message2> tt=Mnu.Where(x=>x.User_sent==APIControl.UserID || x.User_received==APIControl.UserID).DistinctBy(x => x.User_sent).ToList();
			return View(tt);
		}


		public IActionResult ViewMail(int id)
		{
			//{ Userid}/{ Senderid}
			IEnumerable<Message> messages = Control.GetEndPoint<Message>("/GetMessagebyUser/" +"4/2/");
			ViewBag.messages = messages;

			IEnumerable<Message2> Mnu = Control.GetEndPoint<Message2>("/GetMailInbox3/" + APIControl.UserID);

			IEnumerable<Message> Mnu2 = Control.GetEndPoint<Message>("/GetMessagebyID/" +id);
			IEnumerable<UserInfo> Usr = Control.GetEndPoint<UserInfo>("/GetUserbyID/" + Mnu.FirstOrDefault().User_sent);
			List<Message2> tt = Mnu.Where(x => x.User_sent == APIControl.UserID || x.User_received == APIControl.UserID).DistinctBy(x => x.User_sent).ToList();

			ViewBag.caterer = Usr.FirstOrDefault().FullName;
			ViewBag.mesgid = id;
			ViewBag.catererid = Usr.FirstOrDefault().UserId;
			if (Mnu2.Count()>0)
			{
				ViewBag.smessage = Mnu2.FirstOrDefault().Message_note;
			}
			else
			{
				ViewBag.smessage = null;
			}
			return View("MailBox",tt);
		}

		public IActionResult SendMessage(Message model)
		{
			var id = Request.Form["catererid"].ToString();
			if (id!="")
			{
				model.User_received = int.Parse(Request.Form["catererid"].ToString());
				model.User_sent = APIControl.UserID;
				model.Timestamp = DateTime.Now;
				model.Image = "";

				IEnumerable<Message> ss = Control.PostEndPoint<Message>("/AddMessageto/", model);
			}

			return RedirectToAction("ViewMail",new { id= model.User_received });
		}



		[HttpPost]
		public IActionResult SaveRate()
		{
			byte[]? image2=null;
			IFormFile image;
			var s1 = ((Request.Form["rate1"].ToString() == "") ? 0 : int.Parse(Request.Form["rate1"].ToString())) ;
			var s2 = ((Request.Form["rate2"].ToString() == "") ? 0 : int.Parse(Request.Form["rate2"].ToString()));
			var s3 = ((Request.Form["rate3"].ToString() == "") ? 0 : int.Parse(Request.Form["rate3"].ToString()));
			var s4 = ((Request.Form["rate4"].ToString() == "") ? 0 : int.Parse(Request.Form["rate4"].ToString()));
			var s5 = ((Request.Form["rate5"].ToString() == "") ? 0 : int.Parse(Request.Form["rate5"].ToString()));
			int rate = 0;


			if (s5 >0)
			{
				rate = 5;
			}else if (s4 > 0)
			{
				rate = 4;
			}
			else if (s3 > 0)
			{
				rate = 3;
			}
			else if (s2 > 0)
			{
				rate = 2;
			}
			else if (s1 > 0)
			{
				rate = 1;
			}
			var Message = Request.Form["Message"].ToString();
			var Menuid = Request.Form["Menuid"].ToString();
			var orderid = Request.Form["id"].ToString();

			if (Request.Form.Files.Count > 0)
			{
				 image = Request.Form.Files[0];
				using (var target = new MemoryStream())
				{
					image.CopyTo(target);
					image2 = target.ToArray();

				}
			}

			var RateData = new FeedBack { 
			Menuid=int.Parse(Menuid),
			Userid=APIControl.UserID,
			Message=Message,
			image=image2,
			rate=rate,
			Timestamp=DateTime.Now		,
			Replyto="",
			orderid=int.Parse(orderid),
			UserName=""
			};

			IEnumerable<Menu> ss = Control.PostEndPoint<Menu>("/AddFeedBack/", RateData);

			return RedirectToAction("checkorders");
		}

		public IActionResult Rating(int id,int ordid)
		{
			Order ord = new Order();
			ord.Menuid = id;
			ord.id = ordid;
			return View(ord);
		}
		public IActionResult ViewCart()
		{
			List<Order> Ord = new List<Order>();

			foreach(var obj in CartControl.MyCart)
			{
				var data = new Order
				{
					 Caterid = APIControl.UserID,
					  Custid=obj.custid,
					   Menuid=obj.menuid,
					    Quantity=obj.Quantity,
						TotalPrice=obj.Price,
						Image=obj.Image,
						MenuName=obj.MenuName
						
				};
				Ord.Add(data);
			}

			IEnumerable<UserInfo> Mnu = Control.GetEndPoint<UserInfo>("/GetUserbyUserName/" + APIControl.UserName);
			ViewBag.name = Mnu.FirstOrDefault().FullName;
			ViewBag.address = Mnu.FirstOrDefault().Address;
			ViewBag.phoneno = Mnu.FirstOrDefault().PhoneNo;


			return View(Ord);

		}
		[HttpPost]
		public IActionResult AddtoCart()
		{
			
			string menuid = Request.Form["id"].ToString();
			string qty = Request.Form["quantity"].ToString();
			string price = Request.Form["Price"].ToString();
			string Venue = Request.Form["Venue"].ToString();
			string dob = Request.Form["dob"].ToString();

			IEnumerable<Menu> Mnu = Control.GetEndPoint<Menu>("/GetMenubyID/" + menuid);
			var cartdata = new Cart { caterid=Mnu.FirstOrDefault().CaterID, custid = APIControl.UserID, Image=Mnu.FirstOrDefault().Image, menuid=int.Parse(menuid),Quantity=int.Parse(qty),Price=int.Parse(price) ,MenuName=Mnu.FirstOrDefault().Name,dob=DateTime.Parse( dob),Venue=Venue};

			CartControl.MyCart.Add(cartdata);
			return RedirectToAction("Index");

		}

		public IActionResult About()
		{
			return View();
		}

		public IActionResult contactus()
		{
			return View();	
		}

		public IActionResult SingleShop(int id)
		{
			

			IEnumerable<Menu> Mnu = Control.GetEndPoint<Menu>("/GetMenubyID/" + id);
			IEnumerable<FeedBack> Backf = Control.GetEndPoint<FeedBack>("/GetFeedBackbyID2/" + id);
			IEnumerable<UserInfo> chefData =  Control.GetEndPoint<UserInfo>("/GetUserbyID/" + Mnu.FirstOrDefault().CaterID);

			ViewBag.Chef = chefData.FirstOrDefault().FullName;

			IEnumerable<FeedBack> feed = Control.GetEndPoint<FeedBack>("/GetFeedBackbyID/" + id);

			if (feed.Count() > 0)
			{
				ViewBag.Rate = feed.Sum(x => x.rate) / feed.Count();
			}
			else
			{
				ViewBag.Rate = 0;
			}

			IEnumerable<Food_in_Menu> MnuItems = Control.GetEndPoint<Food_in_Menu>("/GetMenuFoodsbyMenuID/" + id);
			ViewBag.MenuFoods = MnuItems;
			ViewBag.Backf = Backf;
			ViewBag.rcount = Backf.Count();

			return View(Mnu.FirstOrDefault());
			
		}
		
		public async Task<IActionResult> PaypalCheckout()
    {
        var environment = new SandboxEnvironment(_clientId, _secretKey);
        var client = new PayPalHttpClient(environment);

        #region Create Paypal Order

        var itemList = new ItemList
        {
            Items = new List<Item>()
        };
        var total = Math.Round((double)CartControl.MyCart.Sum(p => p.Price * p.Quantity)*1.1);
        foreach (var obj in CartControl.MyCart)
            itemList.Items.Add(new Item
            {
                Name = obj.MenuName,
                Currency = "USD",
                Price = total.ToString(),
                Quantity = "1",
                Sku = "sku",
                Tax = "0"
            });

        #endregion

        var paypalOrderId = DateTime.Now.Ticks;
        var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
        var payment = new Payment
        {
            Intent = "sale",
            Transactions = new List<Transaction>
            {
                new()
                {
                    Amount = new Amount
                    {
                        Total = total.ToString(CultureInfo.CurrentCulture),
                        Currency = "USD",
                        Details = new AmountDetails
                        {
                            Tax = "0",
                            Shipping = "0",
                            Subtotal = total.ToString(CultureInfo.CurrentCulture)
                        }
                    },
                    ItemList = itemList,
                    Description = $"Invoice #{paypalOrderId}",
                    InvoiceNumber = paypalOrderId.ToString()
                }
            },
            RedirectUrls = new RedirectUrls
            {
                CancelUrl = $"{hostname}/Customer/Index",
                ReturnUrl = $"{hostname}/Customer/PayNow"
            },
            Payer = new Payer
            {
                PaymentMethod = "paypal"
            }
        };

        var request = new PaymentCreateRequest();
        request.RequestBody(payment);

        try
        {
            var response = await client.Execute(request);
            var statusCode = response.StatusCode;
            var result = response.Result<Payment>();

            var links = result.Links.GetEnumerator();
            string paypalRedirectUrl = null;
            while (links.MoveNext())
            {
                var lnk = links.Current;
                if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
                    //saving the payapalredirect URL to which user will be redirected for payment  
                    paypalRedirectUrl = lnk.Href;
            }

            return Redirect(paypalRedirectUrl);
        }
        catch (HttpException httpException)
        {
            var statusCode = httpException.StatusCode;
            var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();

            // Log detailed information about the exception
            Console.WriteLine($"PayPal Checkout Exception - Status Code: {statusCode}, Debug ID: {debugId}");

            // Log the exception message and response body
            Console.WriteLine($"PayPal Checkout Exception - Message: {httpException.Message}");

            // Process when Checkout with Paypal fails
            return Redirect("/Customer/Index");
        }

    }
	}
}
