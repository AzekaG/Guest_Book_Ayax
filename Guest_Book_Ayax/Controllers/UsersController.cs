using Guest_Book_Ayax.Models;
using Guest_Book_Ayax.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Guest_Book_Ayax.Controllers
{
    public class UsersController : Controller
    {
        IRepository repository;
        


        public UsersController(IRepository rep)
        {
            repository = rep;
        }
        [HttpGet]
        public ActionResult Login()
        {
            return PartialView("Login");
        }

       

        [HttpGet]
        public async Task<IActionResult> IsLoggedIn()
        {
            string response = String.Empty;
            if (HttpContext.Session.GetString("Id") != null)
            {
               Users? activeUs =  repository.GetUsersList().Where(x => x.Id == int.Parse(HttpContext.Session.GetString("Id"))).FirstOrDefault();
               response = JsonConvert.SerializeObject(activeUs);
               return Json(response);
            }
            return NotFound();
        }

        [HttpGet]
		public async Task<IActionResult> GetMessages()
		{
			List<Message> res = repository.GetMessageList();
			List<MessageModelForView> tempMessage = new List<MessageModelForView>();
            foreach(Message message in res) 
            {
                tempMessage.Add(new MessageModelForView { Date = message.Date.Value.ToShortDateString(), 
                    UserFirstName = message.User.FirstName,
                    UserLastName = message.User.LastName,
                    Msg = message.Msg,
                    id = message.id
                });
            }




			string response = JsonConvert.SerializeObject(tempMessage);
			return Json(response);
		}

		public ActionResult Registration()
        {
            return PartialView("Registration");
        }
		[HttpGet]
		public ActionResult EnterMessage()
		{
			return PartialView("MessageView");
		}
	

		[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registration(RegistrationModel reg)
        {
            if (ModelState.IsValid)
            {
                Users user = new Users();
                user.FirstName = reg.FirstName;
                user.LastName = reg.LastName;
                user.Email = reg.Email;

                byte[] saltbuf = new byte[16];

                RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
                randomNumberGenerator.GetBytes(saltbuf);

                StringBuilder sb = new StringBuilder(16);
                for (int i = 0; i < 16; i++)
                    sb.Append(string.Format("{0:X2}", saltbuf[i]));
                string salt = sb.ToString();

                //переводим пароль в байт-массив  
                byte[] password = Encoding.Unicode.GetBytes(salt + reg.Password);

                //создаем объект для получения средств шифрования  
                var md5 = MD5.Create();

                //вычисляем хеш-представление в байтах  
                byte[] byteHash = md5.ComputeHash(password);

                StringBuilder hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                user.Password = hash.ToString();
                user.Salt = salt;
                repository.CreateUser(user);
                repository.Save();
                return Ok();
            }

            return NotFound(reg);
		}


 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EnterMessage(Message mes)
        {
            if (mes.Msg.Count() > 0)
            {
                int id = int.Parse(HttpContext.Session.GetString("Id"));

                mes.Date = DateTime.Now;

                mes.User = repository.GetUsersList().Where(x => x.Id == id).First();

                repository.AddMessage(mes);
                repository.Save();
				return Ok();
			}
            return BadRequest();


        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Login(LoginModel logon)
		{
			if (ModelState.IsValid)
			{

				if (repository.GetUsersList().Count == 0)
				{
					ModelState.AddModelError("", "Неверный логин или пароль");
					return NotFound("Неверный логин или пароль");
				}
				var users = repository.GetUsersList().Where(a => a.Email == logon.Email);

				if (users.ToList().Count == 0)
				{
					ModelState.AddModelError("", "Неверный логин или пароль");
					return NotFound("Неверный логин или пароль");
				}
				var user = users.First();
				string? salt = user.Salt;

				//переводим пароль в байт-массив  
				byte[] password = Encoding.Unicode.GetBytes(salt + logon.Password);

				//создаем объект для получения средств шифрования  
				var md5 = MD5.Create();

				//вычисляем хеш-представление в байтах  
				byte[] byteHash = md5.ComputeHash(password);

				StringBuilder hash = new StringBuilder(byteHash.Length);
				for (int i = 0; i < byteHash.Length; i++)
					hash.Append(string.Format("{0:X2}", byteHash[i]));

				if (user.Password != hash.ToString())
				{
					ModelState.AddModelError("", "Неверный логин или пароль");
					return NotFound("Неверный логин или пароль");
				}

				HttpContext.Session.SetString("FirstName", user.FirstName);
				HttpContext.Session.SetString("LastName", user.LastName);
				HttpContext.Session.SetString("Id", user.Id.ToString());
				return Ok("Здравствуйте, " + user.FirstName);
			}

			return NotFound("-1");
		}


	}
}
