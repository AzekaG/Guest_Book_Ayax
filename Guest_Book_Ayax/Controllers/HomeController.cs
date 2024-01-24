using Guest_Book_Ayax.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Guest_Book_Ayax.Controllers
{
    public class HomeController : Controller
    {

        private readonly UsersContext _context;

        public HomeController(UsersContext context)
        {
            _context = context;
        }




        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("Id") != null) 
            {  int id = int.Parse(HttpContext.Session.GetString("Id"));
                Users user = _context.Users.Where(x=>x.Id == id).FirstOrDefault();
                return View(user);
            }
             return View();
        }


        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok();
        }
    }
}
