using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;
using School_Management_System.ViewModels;

namespace School_Management_System.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolDbContext _context;
        private readonly IWebHostEnvironment _he;
        public StudentsController(SchoolDbContext context, IWebHostEnvironment he)
        {
            this._context = context;
            this._he = he;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            var student = await _context.Students.Include(x => x.StudentSubjects).ThenInclude(y => y.Subject).ToListAsync();
            ViewBag.totalPages = (int)Math.Ceiling((double)_context.Students.Count() / 5);
            ViewBag.currentPage = page;
            return View(await _context.Students.Include(x => x.StudentSubjects).ThenInclude(y => y.Subject).Skip((page - 1) * 5).Take(5).ToListAsync());
        }

        public ActionResult Details(int? id)
        {
            Student student = _context.Students.Include(x => x.StudentSubjects).ThenInclude(y => y.Subject).Single(x => x.StudentId == id);
            return View(student);
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
        public async Task<IActionResult> Create(StudentVM studentVM, int[] subjectId)
        {
            if (ModelState.IsValid)
            {
                Student student = new Student()
                {
                    StudentName=studentVM.StudentName,
                    DateofBirth=studentVM.DateofBirth,
                    Class=studentVM.Class,
                    IsRegular=studentVM.IsRegular,
                };
                //Image
                var file = studentVM.ImagePath;
                string webroot = _he.WebRootPath;
                string folder = "Images";
                string imgFileName = Path.GetFileName(studentVM.ImagePath.FileName);
                string fileToSave = Path.Combine(webroot, folder, imgFileName);

                if (file != null)
                {
                    using (var stream = new FileStream(fileToSave, FileMode.Create))
                    {
                        studentVM.ImagePath.CopyTo(stream);
                        student.Image = "/" + folder + "/" + imgFileName;
                    }
                }

                foreach (var item in subjectId)
                {
                    StudentSubject studentSubject = new StudentSubject()
                    {
                        Student=student,
                        StudentId=student.StudentId,
                        SubjectId=item
                    };
                    _context.StudentSubjects.Add(studentSubject);                 
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task<IActionResult> Edit(int? id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(x => x.StudentId == id);
            StudentVM studentVM = new StudentVM()
            {
                StudentId=student.StudentId,
                StudentName=student.StudentName,
                DateofBirth=student.DateofBirth,
                Class=student.Class,
                Image=student.Image,
                IsRegular=student.IsRegular
            };
            
            var existSubject = _context.StudentSubjects.Where(x => x.StudentId == id).ToList();
            foreach (var item in existSubject)
            {
                studentVM.SubjectList.Add(item.SubjectId);
            }
            return View(studentVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(StudentVM studentVM, int[] subjectId)
        {
            if (ModelState.IsValid)
            {
                Student student = new Student()
                {
                    StudentId=studentVM.StudentId,
                    StudentName= studentVM.StudentName,
                    DateofBirth=studentVM.DateofBirth,
                    Class=studentVM.Class,
                    Image=studentVM.Image,
                    IsRegular = studentVM.IsRegular
                };
                var file = studentVM.ImagePath;
                if (file != null)
                {
                    string webroot = _he.WebRootPath;
                    string folder = "Images";
                    string imgFileName = Path.GetFileName(studentVM.ImagePath.FileName);
                    string fileToSave = Path.Combine(webroot, folder, imgFileName);
                    using (var stream = new FileStream(fileToSave, FileMode.Create))
                    {
                        studentVM.ImagePath.CopyTo(stream);
                        student.Image = "/" + folder + "/" + imgFileName;
                    }
                }

                var existSubject = _context.StudentSubjects.Where(x => x.StudentId == student.StudentId).ToList();
                foreach (var item in existSubject)
                {
                    _context.StudentSubjects.Remove(item);
                }
                foreach (var item in subjectId)
                {
                    StudentSubject studentSubject = new StudentSubject()
                    {
                        StudentId=student.StudentId,
                        SubjectId=item
                    };                    
                    _context.StudentSubjects.Add(studentSubject);
                }
                _context.Update(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students.Include(x => x.StudentSubjects).ThenInclude(y => y.Subject)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Students == null)
            {
                return Problem("Entity set 'SchoolDbContext.Students'  is null.");
            }
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
