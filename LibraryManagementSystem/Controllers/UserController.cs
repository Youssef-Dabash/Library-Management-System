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
        public async Task<IActionResult> Index()
        {
            var userList = await context.Users.Include(e => e.MembershipTier).ToListAsync();
            return View("Index", userList);
        }

        [HttpGet]
        public async Task<IActionResult> AddUser()
        {
            ViewData["MembershipTier"] = await context.MembershipTiers.ToListAsync();
            return View("AddUser");
        }

        [HttpPost]
        public async Task<IActionResult> SaveUser(User userFromRequest)
        {
            if(ModelState.IsValid)
            {
                if (userFromRequest.TierId == 0) userFromRequest.TierId = 1; 
                await context.Users.AddAsync(userFromRequest);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["MembershipTier"] = await context.MembershipTiers.ToListAsync();
            return View("AddUser", userFromRequest);
        }

        public async Task<IActionResult> DetailsUser(int id)
        {
            var user = await context.Users
                .Include(b => b.MembershipTier)
                .FirstOrDefaultAsync(b => b.UserId == id);
            ViewBag.TierNames = user?.MembershipTier.TierName;
            if (user == null) return NotFound();
            return View("DetailsUser", user);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var getUser = await context.Users.FindAsync(id);
            if(getUser == null) return NotFound();

            ViewData["MembershipTier"] = await context.MembershipTiers.ToListAsync();
            return View("EditUser", getUser);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(int id, User userFromRequest)
        {
            if(id != userFromRequest.UserId) return BadRequest();

            if (ModelState.IsValid)
            {
                var getUser = await context.Users.FindAsync(id);
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
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["MembershipTier"] = await context.MembershipTiers.ToListAsync();
            return View("EditUser", userFromRequest);
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            var getUser = await context.Users.FindAsync(id);  
            if (getUser != null)
            {
                context.Users.Remove(getUser);
                await context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

    }
}
