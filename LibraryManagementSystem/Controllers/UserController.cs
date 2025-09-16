using LibraryManagementSystem.Data;
using LibraryManagementSystem.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    public class UserController : Controller
    {
        UnitOfWork unitOfWork;
        public UserController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        
        public async Task<IActionResult> Index()
        {
            var userList = await unitOfWork.Users.GetAllWithMembershipAsync();
            return View("Index", userList);
        }

        public async Task<IActionResult> AddUser()
        {
            ViewData["MembershipTier"] = await unitOfWork.MembershipTiers.ToListAsync();
            return View("AddUser");
        }

        [HttpPost]
        public async Task<IActionResult> SaveUser(User userFromRequest)
        {
            var tier = await unitOfWork.MembershipTiers
            .FirstOrDefaultAsync(userFromRequest.TierId);

            if (ModelState.IsValid)
            {
                if (userFromRequest.TierId == 0) userFromRequest.TierId = 1;
                userFromRequest.NumBooksAvailable = tier.ExtraBooks;

                await unitOfWork.Users.AddAsync(userFromRequest);
                await unitOfWork.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["MembershipTier"] = await unitOfWork.MembershipTiers.ToListAsync();
            return View("AddUser", userFromRequest);
        }

        public async Task<IActionResult> DetailsUser(int id)
        {
            var user = await unitOfWork.Users.GetUserWithDetailsAsync(id);

            if (user == null)
                return NotFound();

            ViewBag.TierNames = user.MembershipTier?.TierName.ToString() ?? "No Membership";

            return View("DetailsUser", user);
        }

        public async Task<IActionResult> EditUser(int id)
        {
            var getUser = await unitOfWork.Users.FindAsync(id);

            if(getUser == null) return NotFound();

            ViewData["MembershipTier"] = await unitOfWork.MembershipTiers.ToListAsync();
            return View("EditUser", getUser);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(int id, User userFromRequest)
        {
            if(id != userFromRequest.UserId) return BadRequest();

            if (ModelState.IsValid)
            {
                var getUser = await unitOfWork.Users.FindAsync(id);
                if (getUser == null ) return NotFound();
                if (userFromRequest.TierId == 0) userFromRequest.TierId = 1;

                var tier = await unitOfWork.MembershipTiers
                           .FirstOrDefaultAsync(getUser.TierId);

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

                unitOfWork.Users.Update(getUser);
                await unitOfWork.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["MembershipTier"] = await unitOfWork.MembershipTiers.ToListAsync();
            return View("EditUser", userFromRequest);
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            var getUser = await unitOfWork.Users.FindAsync(id);  
            if (getUser != null)
            {
                unitOfWork.Users.Delete(getUser);
                await unitOfWork.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

    }
}
