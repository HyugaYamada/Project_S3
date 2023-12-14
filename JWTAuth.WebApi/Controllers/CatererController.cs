
using JWTAuth.WebApi.Models;
using khiemnguyen.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography.Xml;

namespace khiemnguyen.WebApi.Controllers
{

    //[Route("api/caterer")]
    [ApiController]
    public class CatererController : Controller
    {
        private DatabaseContext _db;
        public CatererController(DatabaseContext db)
        {
            _db = db;   
        }

   

        [HttpGet("GetCaterers")]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GetCaterers(int id)
        {
            return await Task.FromResult(_db.UserInfos.Where(x=>x.Role=="Caterer").ToList());
        }

		[HttpGet("GetMenuID")]
		public async Task<ActionResult<IEnumerable<Menu>>> GetMenuID()
		{
			return await Task.FromResult(_db.Menues.OrderByDescending(x=>x.id).ToList());
		}

		[HttpGet("GetCatererDetailsbyEmail/{Email}")]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GetCatererDetailsbyEmail(string Email)
        {
            return await Task.FromResult(_db.UserInfos.Where(x => x.Email == Email).ToList());
        }

        [HttpGet("GetMenubyID/{id}")]
        public async Task<ActionResult<IEnumerable<Menu>>> GetMenubyID(int id)
        {

            return await Task.FromResult(_db.Menues.Where(x => x.id == id).ToList());
        }

		[HttpGet("GetMenu")]
		public async Task<ActionResult<IEnumerable<Menu>>> GetMenu()
		{

			return await Task.FromResult(_db.Menues.OrderByDescending(x=>x.id).ToList());
		}
		[HttpGet("GetMenubyCatererCategoury/{Cateere}/{Categoury}")]
		public async Task<ActionResult<IEnumerable<Menu>>> GetMenubyCatererCategoury(int Cateere,string Categoury )
		{
			var data = (from Mnu in _db.Menues
					
						where Mnu.CaterID==Cateere && Mnu.Category==Categoury
						select Mnu).ToList();

			return await Task.FromResult(data);
		}

		[HttpGet("GetMenubyCatererFood/{food}")]
		public async Task<ActionResult<IEnumerable<Menu>>> GetMenubyCatererCatererFood(string food)
		{
			var data = (from fd in _db.Foods
			  join fim in _db.Food_In_Menus on fd.id equals fim.Foodid
												  
			 where fd.Name.Contains(food)
		select fim.Menuid).Distinct().ToList();

			var data2 = _db.Menues.Where(x => data.Contains(x.id)).ToList();
			return await Task.FromResult(data2);
		}





		[HttpGet("GetMenubyCater/{id}")]
        public async Task<ActionResult<IEnumerable<Menu>>> GetMenubyUser(int id)
        {

			var data = (from items in _db.Food_In_Menus
						join Food in _db.Foods on items.Foodid equals Food.id
						where items.Menuid == id 
						select new Food_in_Menu { Menuid = items.Menuid, Name = Food.Name, Foodid = Food.id, id = items.id }).ToList();

			return await Task.FromResult(_db.Menues.Where(x => x.CaterID == id).ToList());
        }

		[HttpGet("GetMenuFoodsbyMenuID/{id}")]
		public async Task<ActionResult<IEnumerable<Food_in_Menu>>> GetMenuFoodsbyMenuID(int id)
		{
			var data = (from items in _db.Food_In_Menus
						join Food in _db.Foods on items.Foodid equals Food.id where items.Menuid==id
						select new Food_in_Menu { Menuid = items.Menuid, Name=Food.Name,Foodid=Food.id,id=items.id,image=Food.Image }).ToList();
	

			return await Task.FromResult(data);
		}

		[HttpGet("GetFoodsList")]
		public async Task<ActionResult<IEnumerable<Food>>> GetFoodsList()
		{
			return await Task.FromResult(_db.Foods.ToList());
		}

		[HttpGet("GetfavouriteList/{cusid}")]
		public async Task<ActionResult<IEnumerable<Favor_Cater>>> GefavouriteList(int cusid)
		{

			var data = (from fc in _db.Favor_Caters
						join usr in _db.UserInfos on fc.Caterid equals usr.UserId
						where fc.Custid==cusid
						select new Favor_Cater {id=fc.id,name=usr.FullName,Caterid=fc.Caterid,Custid=fc.Custid }).ToList();


			return await Task.FromResult(data);
		}

		[HttpGet("Deletefavourite/{id}")]
		public async Task<ActionResult<IEnumerable<Favor_Cater>>> Deletefavourite(int id)
		{
			var fav = new Favor_Cater { id = id };
			_db.Favor_Caters.Remove(fav);
			_db.SaveChanges();
			return await Task.FromResult(_db.Favor_Caters.ToList());
		}

		[HttpGet("GetFoodsListbyCaterer/{id}")]
		public async Task<ActionResult<IEnumerable<Food>>> GetFoodsListbyCaterer(int id)
		{
			return await Task.FromResult(_db.Foods.Where(x=>x.Caterid==id).ToList());
		}


		[HttpGet("GetFoodsbyCategory/{name}")]
		public async Task<ActionResult<IEnumerable<Food>>> GetFoodsbyCategory(string name)
		{
			return await Task.FromResult(_db.Foods.Where(x=>x.Category==name).ToList());
		}


		[HttpGet("GetOrdersbyUser/{id}")]
		public async Task<ActionResult<IEnumerable<Order>>> GetOrdersbyUser(int id)
		{
			var data = (from Ord in _db.Orders
						join Mnu in _db.Menues on Ord.Menuid equals Mnu.id where Ord.Custid==id &&Ord.IsCanceled==0
						select new Order {Menuid=Ord.Menuid,MenuName=Mnu.Name,Caterid=Ord.Caterid,Custid=Ord.Custid,DelivDate=Ord.DelivDate,Description=Ord.Description,GuestNum=Ord.GuestNum,id=Ord.id,israted=Ord.israted,Name=Ord.Name,OrdDate=Ord.OrdDate,Quantity=Ord.Quantity,Status=Ord.Status,TotalPrice=Ord.TotalPrice}).OrderByDescending(x=>x.OrdDate).ToList();


			return await Task.FromResult(data);
		}

		[HttpGet("GetOrdersbyCater/{id}")]
		public async Task<ActionResult<IEnumerable<Order>>> GetOrdersbyCater(int id)
		{
			var data = (from Ord in _db.Orders
						join Mnu in _db.Menues on Ord.Menuid equals Mnu.id
						join usr in _db.UserInfos on Ord.Custid equals usr.UserId
						where Ord.Caterid == id && Ord.IsCanceled==0
						select new Order { Menuid = Ord.Menuid, MenuName = Mnu.Name, Caterid = Ord.Caterid, Custid = Ord.Custid, DelivDate = Ord.DelivDate, Description = Ord.Description, GuestNum = Ord.GuestNum, id = Ord.id, israted = Ord.israted, Name = Ord.Name, OrdDate = Ord.OrdDate, Quantity = Ord.Quantity, Status = Ord.Status, TotalPrice = Ord.TotalPrice,CustomerName=usr.FullName }).OrderByDescending(x => x.OrdDate).ToList();


			return await Task.FromResult(data);
		}


		[HttpGet("GetOrdersbyid2/{id}")]
		public async Task<ActionResult<IEnumerable<Order>>> GetOrdersbyid2(int id)
		{
			var data = (from Ord in _db.Orders
						join Mnu in _db.Menues on Ord.Menuid equals Mnu.id
						where Ord.id == id && Ord.IsCanceled == 0
						select new Order { Menuid = Ord.Menuid, MenuName = Mnu.Name, Caterid = Ord.Caterid, Custid = Ord.Custid, DelivDate = Ord.DelivDate, Description = Ord.Description, GuestNum = Ord.GuestNum, id = Ord.id, israted = Ord.israted, Name = Ord.Name, OrdDate = Ord.OrdDate, Quantity = Ord.Quantity, Status = Ord.Status, TotalPrice = Ord.TotalPrice,Image=Mnu.Image,Venue=Ord.Venue,IsCanceled=Ord.IsCanceled }).OrderByDescending(x => x.OrdDate).ToList();


			return await Task.FromResult(data);
		}

		//[HttpGet("GetOrdersbyID/{id}")]
		//public async Task<ActionResult<IEnumerable<Order>>> GetOrdersbyID(int id)
		//{
		//	return await Task.FromResult(_db.Orders.Where(x => x.id == id).ToList());
		//}

		[HttpGet("GetOrdersbyID/{id}")]
		public async Task<ActionResult<IEnumerable<Order>>> GetOrdersbyID(int id)
		{
			return await Task.FromResult(_db.Orders.Where(x => x.id == id).ToList());
		}

		[HttpGet("GetCategoryList")]
		public async Task<ActionResult<IEnumerable<Category>>> GetCategoryList()
		{
			return await Task.FromResult(_db.Categories.ToList());
		}

		[HttpGet("GetMailInbox/{user}")]
		public async Task<ActionResult<IEnumerable<Message>>> GetMailInbox(int user)
		{
			return await Task.FromResult(_db.Messages.Where(x=>x.User_received==user).ToList());
		}


		[HttpGet("DeleteMessage/{id}")]
		public async Task<ActionResult<IEnumerable<Message>>> DeleteMessage(int id)
		{
			var Msg = _db.Messages.Where(x => x.id == id).FirstOrDefault();
			Msg.isCanceled = 1;
			
			_db.Messages.Update(Msg);
			_db.SaveChanges();

			return await Task.FromResult(_db.Messages.ToList());
		}

		[HttpGet("GetMailInbox2/{user}")]
		public async Task<ActionResult<IEnumerable<Message2>>> GetMailInbox2(int user)
		{
			var data = (from msg in _db.Messages
						join usr in _db.UserInfos on msg.User_received equals usr.UserId where( msg.User_received==user|| msg.User_sent==user) && msg.isCanceled==0
						select new Message2 {id=msg.id,Message_note=msg.Message_note,Image=msg.Image,SenderName=usr.FullName,Timestamp=msg.Timestamp,User_received=msg.User_received,User_sent=msg.User_sent  }).OrderByDescending(x=>x.Timestamp ).ToList();

			return await Task.FromResult(data);
		}

		[HttpGet("GetMailInbox3/{user}")]
		public async Task<ActionResult<IEnumerable<Message2>>> GetMailInbox3(int user)
		{
			var data = (from msg in _db.Messages
						join usr in _db.UserInfos on msg.User_received equals usr.UserId
						where (msg.User_received == user || msg.User_sent == user) 
						select new Message2 { id = msg.id, Message_note = msg.Message_note, Image = msg.Image, SenderName = usr.FullName, Timestamp = msg.Timestamp, User_received = msg.User_received, User_sent = msg.User_sent }).OrderByDescending(x => x.Timestamp).ToList();

			return await Task.FromResult(data);
		}


		[HttpGet("GetAllMails/{user}")]
		public async Task<ActionResult<IEnumerable<Message2>>> GetAllMails(int user)
		{
			var data = (from msg in _db.Messages
						join usr in _db.UserInfos on msg.User_received equals usr.UserId
						where msg.User_received == user 
						select new Message2 { id = msg.id, Message_note = msg.Message_note, Image = msg.Image, SenderName = usr.FullName, Timestamp = msg.Timestamp, User_received = msg.User_received, User_sent = msg.User_sent }).OrderByDescending(x => x.Timestamp).ToList();

			return await Task.FromResult(data);
		}


		[HttpGet("GetMessagebyID/{ID}")]
		public async Task<ActionResult<IEnumerable<Message>>> GetMessagebyID(int ID)
		{
			return await Task.FromResult(_db.Messages.Where(x => x.id == ID).ToList());
		}

		[HttpGet("GetMessagebyUser/{Userid}/{Senderid}")]
		public async Task<ActionResult<IEnumerable<Message>>> GetMessagebyUser(int Userid,int Senderid)
		{
			return await Task.FromResult(_db.Messages.Where(x => (x.User_received == Userid && x.User_sent==Senderid && x.isCanceled==0) || (x.User_sent == Userid && x.User_received == Senderid && x.isCanceled==0)).OrderBy(x=>x.Timestamp).ToList());
		}

		[HttpGet("GetAllMessagebyUser/{Userid}/{Senderid}")]
		public async Task<ActionResult<IEnumerable<Message>>> GetAllMessagebyUser(int Userid, int Senderid)
		{
			return await Task.FromResult(_db.Messages.Where(x => (x.User_received == Userid && x.User_sent == Senderid ) || (x.User_sent == Userid && x.User_received == Senderid )).OrderBy(x => x.Timestamp).ToList());
		}




		[HttpGet("GetUserbyID/{id}")]
		public async Task<ActionResult<IEnumerable<UserInfo>>> GetUserbyID(int id)
		{
			return await Task.FromResult(_db.UserInfos.Where(x=>x.UserId==id).ToList());
		}

		[HttpGet("GetUsers")]
		public async Task<ActionResult<IEnumerable<UserInfo>>> GetUsers()
		{
			return await Task.FromResult(_db.UserInfos.Where(x=>x.FullName!="API").ToList());
		}

		[HttpGet("GetUserbyUserName/{name}")]
		public async Task<ActionResult<IEnumerable<UserInfo>>> GetUserbyUserName(string name)
		{
			return await Task.FromResult(_db.UserInfos.Where(x => x.Email == name).ToList());
		}



		[HttpGet("GetFeedBackbyID/{id}")]
		public async Task<ActionResult<IEnumerable<FeedBack>>> GetFeedBackbyID(int id)
		{
			return await Task.FromResult(_db.FeedBacks.Where(x=>x.Menuid==id ).ToList());
		}


		[HttpGet("GetFeedBackbyID2/{id}")]
		public async Task<ActionResult<IEnumerable<FeedBack>>> GetFeedBackbyID2(int id)
		{
			var data = (from fed in _db.FeedBacks
						join usr in _db.UserInfos on fed.Userid equals usr.UserId
						where fed.Menuid== id
						select new FeedBack { id = fed.id,Menuid=fed.Menuid,Message=fed.Message,image=fed.image,orderid=fed.orderid,rate=fed.rate,Replyto=fed.Replyto,Timestamp=fed.Timestamp,Userid=fed.Userid,UserName=usr.FullName }).OrderByDescending(x => x.Timestamp).ToList();

			return await Task.FromResult(data);
		}

		[HttpGet("GetFeedBackbyOrderID/{id}")]
		public async Task<ActionResult<IEnumerable<FeedBack>>> GetFeedBackbyOrderID(int id)
		{
			return await Task.FromResult(_db.FeedBacks.Where(x => x.orderid == id).ToList());
		}





		[HttpPost("UpdateMenu")]
		public async Task<ActionResult<IEnumerable<Menu>>> UpdateMenu(Menu MenuData)
		{
          
            
            if (MenuData.Image == null)
            {
				var _db2 = _db.Menues.Where(x => x.id == MenuData.id).FirstOrDefault();
				MenuData.Image = _db2.Image;
            }

			try
			{
				_db.Menues.Update(MenuData);
				_db.SaveChanges();
			}
			catch (Exception ex)
			{
				return NotFound(ex.InnerException);
			}

			return Ok(MenuData);
		}


		[HttpPost("AddMenu")]
        public async Task<ActionResult<IEnumerable<Menu>>> AddMenu(Menu MenuData)
        {
          
            try
            {
                _db.Menues.Add(MenuData);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return NotFound(ex.InnerException);
            }

            return Ok(MenuData);
        }

        [NonAction]
		public void SaveRateonMenu1(int menuid,int orderid)
		{
            var data = _db.Menues.Where(x => x.id ==menuid).FirstOrDefault();
            var Orderdata = _db.Orders.Where(x => x.id == orderid).FirstOrDefault();

            if (_db.FeedBacks.Where(x => x.id == menuid).Count() > 0)
            {
                data.israted = _db.FeedBacks.Where(x => x.Menuid == menuid).Sum(x => x.rate) / _db.FeedBacks.Where(x => x.Menuid== menuid).Count();
            }
            else
            {
                data.israted = 0;
            }

            Orderdata.israted = data.israted;
			try
			{
                _db.Update(Orderdata);  
				_db.Menues.Update(data);
				_db.SaveChanges();
			}
			catch (Exception ex)
			{
				 NotFound(ex.InnerException);
			}

		
		}

		[HttpPost("AddFoods")]
		public async Task<ActionResult<IEnumerable<Menu>>> AddFoods(Food MenuData)
		{

			try
			{
				_db.Foods.Add(MenuData);
				_db.SaveChanges();
			}
			catch (Exception ex)
			{
				return NotFound(ex.InnerException);
			}

			return Ok(MenuData);
		}


		[HttpPost("AddFavorit")]
		public async Task<ActionResult<IEnumerable<Favor_Cater>>> AddFavorit(Favor_Cater Data)
		{

			try
			{
				_db.Favor_Caters.Add(Data);
				_db.SaveChanges();
			}
			catch (Exception ex)
			{
				return NotFound(ex.InnerException);
			}

			return Ok(Data);
		}

		[HttpPost("AddMessageto")]
		public async Task<ActionResult<IEnumerable<Message>>> AddMessageto(Message MenuData)
		{

			try
			{
				_db.Messages.Add(MenuData);
				_db.SaveChanges();
			}
			catch (Exception ex)
			{
				return NotFound(ex.InnerException);
			}

			return Ok(MenuData);
		}



		[HttpGet("DeleteItems/{id}")]
		public async Task<ActionResult<IEnumerable<Food>>> DeleteItems(int id)
		{

            var Data = new Food { id = id };
			try
			{
				_db.Foods.Remove(Data);
				_db.SaveChanges();
			}
			catch (Exception ex)
			{
				return NotFound(ex.InnerException);
			}

			return Ok(_db.Foods.ToList());
		}

		[HttpPost("DeleteMenuItems")]
		public async Task<ActionResult<IEnumerable<Menu>>> DeleteMenuItems(Food_in_Menu FiM)
		{
           
            

			try
			{
                _db.Food_In_Menus.Remove(FiM);
				_db.SaveChanges();
			}
			catch (Exception ex)
			{
				return NotFound(ex.InnerException);
			}

			return Ok();
		}


		[HttpPost("AddItemstoMenu")]
        public async Task<ActionResult<IEnumerable<Food_in_Menu>>> AddItemstoMenu(Food_in_Menu MenuItemsData)
        {

            try
            {
                _db.Add(MenuItemsData);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return NotFound(ex.InnerException);
            }

            return Ok(MenuItemsData);
        }

		[HttpPost("AddItemstoMenuRange")]
		public async Task<ActionResult<IEnumerable<Food_in_Menu>>> AddItemstoMenuRange(List<Food_in_Menu> MenuItemsData)
		{

			try
			{
				_db.Food_In_Menus.AddRange(MenuItemsData);
				_db.SaveChanges();
			}
			catch (Exception ex)
			{
				return NotFound(ex.InnerException);
			}

			return Ok(MenuItemsData);
		}

		[HttpPost("UpdateUserInfo")]
        public async Task<ActionResult<IEnumerable<UserInfo>>> UpdateUserInfo(UserInfo UserData)
        {

            try
            {
                _db.Update(UserData);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.ToString());
            }

            return Ok(UserData);
        }

		[HttpPost("UpdateOrderStatus")]
		public async Task<ActionResult<IEnumerable<StatusUpdate>>> UpdateOrderStatus(StatusUpdate Para)
		{
            var data = _db.Orders.Where(x => x.id == Para.id).FirstOrDefault();
            data.Status = Para.Status;
			try
			{
				_db.Update(data);
				_db.SaveChanges();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.InnerException.ToString());
			}

			return Ok(data);
		}

		[HttpPost("CancelOrder")]
		public async Task<ActionResult<IEnumerable<StatusUpdate>>> CancelOrder(Para Pr)
		{
			var data = _db.Orders.Where(x => x.id == Pr.id).FirstOrDefault();
			data.IsCanceled=1;
			try
			{
				_db.Update(data);
				_db.SaveChanges();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.InnerException.ToString());
			}

			return Ok(data);
		}

		[HttpPost("ResetPassowrd")]
        public async Task<ActionResult<IEnumerable<ResetPassowrdDTO>>> ResetPassowrd(ResetPassowrdDTO UserData)
        {

           var data=_db.UserInfos.Where(x=>x.UserId==UserData.Userid).FirstOrDefault();

            if (data == null)
            {
                return BadRequest("Invalid user ID");
            }else if (data.Password!=UserData.OldPassword)
            {
                return BadRequest("Old Password Mismatch");
            }else if (UserData.NewPassword.Trim()=="")
            {
                return BadRequest("New Password Error!");
            }
            else
            {
                data.Password= UserData.NewPassword;
            }

            try
            {

                _db.Update(data);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.ToString());
            }

            return Ok(UserData);
        }


   

			[HttpPost("AddOrder")]
        public async Task<ActionResult<IEnumerable<Order>>> AddOrder(List<Order> OrderData)
        {

            try
            {
                _db.Orders.AddRange(OrderData);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return NotFound(ex.InnerException);
            }

            return Ok(OrderData);
        }

        [HttpPost("AddFeedBack")]
        public async Task<ActionResult<IEnumerable<FeedBack>>> AddFeedBack(FeedBack FeedBackData)
        {
            
            try
            {
                _db.Add(FeedBackData);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return NotFound(ex.InnerException);
            }

            SaveRateonMenu1(FeedBackData.Menuid,FeedBackData.orderid);


			return Ok(FeedBackData);
        }

		[HttpPost("UpdateFeedReply")]
		public async Task<ActionResult<IEnumerable<FeedBack>>> UpdateFeedReply(FeedBack FeedBackData)
		{
            var data = _db.FeedBacks.Where(x => x.id == FeedBackData.id).FirstOrDefault();
            data.Replyto = FeedBackData.Replyto;
			try
			{
				_db.Update(data);
				_db.SaveChanges();
			}
			catch (Exception ex)
			{
				return NotFound(ex.InnerException);
			}

			SaveRateonMenu1(FeedBackData.Menuid, FeedBackData.orderid);


			return Ok(FeedBackData);
		}


		[HttpPost("AddTagInclude")]
        public async Task<ActionResult<IEnumerable<Message>>> AddTagInclude(Tag_Include TagIncludeData)
        {

            try
            {
                _db.Add(TagIncludeData);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return NotFound(ex.InnerException);
            }

            return Ok(TagIncludeData);
        }


    }
}
