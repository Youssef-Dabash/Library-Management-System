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

        public async Task<IActionResult> Index()
        {
            var userList = await context.Users.Include(e => e.MembershipTier).ToListAsync();
            return View("Index", userList);
        }

        public async Task<IActionResult> AddUser()
        {
            ViewData["MembershipTier"] = await context.MembershipTiers.ToListAsync();
            return View("AddUser");
        }

        [HttpPost]
        public async Task<IActionResult> SaveUser(User userFromRequest)
        {
            var tier = await context.MembershipTiers
            .FirstOrDefaultAsync(m => m.TierId == userFromRequest.TierId);
            if (ModelState.IsValid)
            {
                if (userFromRequest.TierId == 0) userFromRequest.TierId = 1;
                userFromRequest.NumBooksAvailable = tier.ExtraBooks;

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
               .Include(u => u.MembershipTier)        
               .Include(u => u.Borrowings)             
                .ThenInclude(b => b.Book)         
               .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
                return NotFound();

            ViewBag.TierNames = user.MembershipTier?.TierName.ToString() ?? "No Membership";

            return View("DetailsUser", user);
        }

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

                var tier = await context.MembershipTiers
                           .FirstOrDefaultAsync(m => m.TierId == getUser.TierId);

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
                getUser.NumBooksAvailable = tier.ExtraBooks;

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
