using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;

namespace School_Management_System.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly SchoolDbContext db;

        public SubjectsController(SchoolDbContext db)
        {
            this.db = db;
        }


        public async Task<IActionResult> Index(int page = 1)
        {
            ViewBag.totalPages = (int)Math.Ceiling((double)db.Subjects.Count() / 5);
            ViewBag.currentPage = page;
            return View(await db.Subjects.OrderBy(x => x.SubjectId).Skip((page - 1) * 5).Take(5).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subject subject)
        {
            if (ModelState.IsValid)
            {
                db.Add(subject);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Subjects == null)
            {
                return NotFound();
            }

            var subject = await db.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Subject subject)
        {
            if (id != subject.SubjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                    db.Update(subject);
                    await db.SaveChangesAsync();
               
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Subjects == null)
            {
                return NotFound();
            }

            var subject = await db.Subjects
                .FirstOrDefaultAsync(m => m.SubjectId == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.Subjects == null)
            {
                return Problem("Entity set 'SchoolDbContext.Spots'  is null.");
            }
            var subject = await db.Subjects.FindAsync(id);
            if (subject != null)
            {
                db.Subjects.Remove(subject);
            }

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
