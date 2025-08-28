using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    public class MembershipTierController : Controller
    {
        public ApplicationDbContext context;

        public MembershipTierController(ApplicationDbContext _context)
        {
            context = _context;
        }


        public IActionResult Index()
        {
            var tiers = context.MembershipTiers.Include(t => t.Users).ToList();
            return View(tiers);
        }

        [HttpGet]
        public IActionResult EditTier(int id)
        {
            MembershipTier getTier = context.MembershipTiers.Find(id);
            if (getTier == null) return NotFound();

            return View("EditTier", getTier);
        }

        [HttpPost]
        public IActionResult EditTier(int id, MembershipTier tierFormRequest)
        {
            if (id != tierFormRequest.TierId) return BadRequest();

            if (ModelState.IsValid)
            {
                MembershipTier getTier = context.MembershipTiers.Find(id);
                if (getTier == null) return NotFound();
                getTier.ExtraBooks = tierFormRequest.ExtraBooks;
                getTier.ExtraDays = tierFormRequest.ExtraDays;
                getTier.ExtraPenalty = tierFormRequest.ExtraPenalty;

                context.MembershipTiers.Update(getTier);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("EditTier", tierFormRequest);
        }

    }
}
