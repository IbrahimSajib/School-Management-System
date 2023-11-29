using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;
using School_Management_System.ViewModels;

namespace School_Management_System.Controllers
{
    public class TeachersController : Controller
    {
        private readonly SchoolDbContext _context;
        private readonly IWebHostEnvironment _he;
        public TeachersController(SchoolDbContext context, IWebHostEnvironment he)
        {
            this._context = context;
            this._he = he;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            var teacher = await _context.Teachers.Include(x => x.TeacherSubjects).ThenInclude(y => y.Subject).ToListAsync();
            ViewBag.totalPages = (int)Math.Ceiling((double)_context.Teachers.Count() / 5);
            ViewBag.currentPage = page;
            return View(await _context.Teachers.Include(x => x.TeacherSubjects).ThenInclude(y => y.Subject).Skip((page - 1) * 5).Take(5).ToListAsync());
        }

        public ActionResult Details(int? id)
        {
            Teacher teacher = _context.Teachers.Include(x => x.TeacherSubjects).ThenInclude(y => y.Subject).Single(x => x.TeacherId == id);
            return View(teacher);
        }

        public IActionResult AddNewSubjects(int? id)
        {
            ViewBag.subject = new SelectList(_context.Subjects, "SubjectId", "SubjectName", id.ToString() ?? "");
            return PartialView("_addNewSubjects");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherVM teacherVM, int[] subjectId)
        {
            if (ModelState.IsValid)
            {
                Teacher teacher = new Teacher()
                {
                    TeacherName = teacherVM.TeacherName,
                    JoinDate = teacherVM.JoinDate,
                    Salary = teacherVM.Salary,
                    Phone = teacherVM.Phone
                };

                foreach (var item in subjectId)
                {
                    TeacherSubject teacherSubject = new TeacherSubject()
                    {
                        Teacher=teacher,
                        TeacherId=teacher.TeacherId,
                        SubjectId=item
                    };
                    _context.TeacherSubjects.Add(teacherSubject);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task<IActionResult> Edit(int? id)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(x => x.TeacherId == id);
            TeacherVM teacherVM = new TeacherVM()
            {
                TeacherId=teacher.TeacherId,
                TeacherName=teacher.TeacherName,
                JoinDate=teacher.JoinDate,
                Salary=teacher.Salary,
                Phone=teacher.Phone
            };

            var existSubject = _context.TeacherSubjects.Where(x => x.TeacherId == id).ToList();
            foreach (var item in existSubject)
            {
                teacherVM.SubjectList.Add(item.SubjectId);
            }
            return View(teacherVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TeacherVM teacherVM, int[] subjectId)
        {
            if (ModelState.IsValid)
            {
                Teacher teacher = new Teacher()
                {
                    TeacherId=teacherVM.TeacherId,
                    TeacherName=teacherVM.TeacherName,
                    JoinDate=teacherVM.JoinDate,
                    Salary=teacherVM.Salary,
                    Phone=teacherVM.Phone
                };

                var existSubject = _context.TeacherSubjects.Where(x => x.TeacherId == teacher.TeacherId).ToList();
                foreach (var item in existSubject)
                {
                    _context.TeacherSubjects.Remove(item);
                }
                foreach (var item in subjectId)
                {
                    TeacherSubject teacherSubject = new TeacherSubject()
                    {
                        TeacherId = teacher.TeacherId,
                        SubjectId=item
                    };
                    _context.TeacherSubjects.Add(teacherSubject);
                }
                _context.Update(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.Include(x => x.TeacherSubjects).ThenInclude(y => y.Subject)
                .FirstOrDefaultAsync(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Teachers == null)
            {
                return Problem("Entity set 'SchoolDbContext.Teachers'  is null.");
            }
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
