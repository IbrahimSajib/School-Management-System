using Microsoft.AspNetCore.Mvc;
using School_Management_System.Models;

namespace School_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly SchoolDbContext _context;

        public HomeController(SchoolDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.totalStudent = _context.Students.Count();
            ViewBag.totalTeacher = _context.Teachers.Count();
            ViewBag.totalSubject = _context.Subjects.Count();
            return View();
        }
    }
}
