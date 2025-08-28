using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    public class UserController : Controller
    {
        ApplicationDbContext  context;
        public UserController(ApplicationDbContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userList = context.Users.Include(e => e.MembershipTier).ToList();
            return View("Index", userList);
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            ViewData["MembershipTier"] = context.MembershipTiers.ToList();
            return View("AddUser");
        }

        [HttpPost]
        public IActionResult SaveUser(User userFromRequest)
        {
            if(ModelState.IsValid)
            {
                if (userFromRequest.TierId == 0) userFromRequest.TierId = 1; 
                context.Users.Add(userFromRequest);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["MembershipTier"] = context.MembershipTiers.ToList();
            return View("AddUser", userFromRequest);
        }

        public IActionResult DetailsUser(int id)
        {
            var user = context.Users
                .Include(b => b.MembershipTier)
                .FirstOrDefault(b => b.UserId == id);
            ViewBag.TierNames = user?.MembershipTier.TierName;
            if (user == null) return NotFound();
            return View("DetailsUser", user);
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            User getUser = context.Users.Find(id);
            if(getUser == null) return NotFound();

            ViewData["MembershipTier"] = context.MembershipTiers.ToList();
            return View("EditUser", getUser);
        }

        [HttpPost]
        public IActionResult EditUser(int id, User userFromRequest)
        {
            if(id != userFromRequest.UserId) return BadRequest();

            if (ModelState.IsValid)
            {
                User getUser = context.Users.Find(id);
                if (getUser == null ) return NotFound();
                if (userFromRequest.TierId == 0) userFromRequest.TierId = 1;

                getUser.FullName = userFromRequest.FullName;
                getUser.UserName = userFromRequest.UserName;
                getUser.Password = userFromRequest.Password;
                getUser.ConfirmPassword = userFromRequest.ConfirmPassword;
                getUser.Address = userFromRequest.Address;
                getUser.Phone = userFromRequest.Phone;
                getUser.Email = userFromRequest.Email;
                getUser.Status = userFromRequest.Status;
                getUser.BirthOfDate = userFromRequest.BirthOfDate;
                getUser.TierId = userFromRequest.TierId;

                context.Users.Update(getUser);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["MembershipTier"] = context.MembershipTiers.ToList();
            return View("EditUser", userFromRequest);
        }

        public IActionResult DeleteUser(int id)
        {
            User getUser = context.Users.Find(id);  
            if (getUser != null)
            {
                context.Users.Remove(getUser);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}
