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


        public async Task<IActionResult> Index()
        {
            var tiers = await context.MembershipTiers.Include(t => t.Users).ToListAsync();
            return View(tiers);
        }

        [HttpGet]
        public async Task<IActionResult> EditTier(int id)
        {
            var getTier = await context.MembershipTiers.FindAsync(id);
            if (getTier == null) return NotFound();

            return View("EditTier", getTier);
        }

        [HttpPost]
        public async Task<IActionResult> EditTier(int id, MembershipTier tierFormRequest)
        {
            if (id != tierFormRequest.TierId) return BadRequest();

            if (ModelState.IsValid)
            {
                var getTier =  await context.MembershipTiers.FindAsync(id);
                if (getTier == null) return NotFound();
                getTier.ExtraBooks = tierFormRequest.ExtraBooks;
                getTier.ExtraDays = tierFormRequest.ExtraDays;
                getTier.ExtraPenalty = tierFormRequest.ExtraPenalty;

                context.MembershipTiers.Update(getTier);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("EditTier", tierFormRequest);
        }
    }
}
