using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnCSharp.Models;
using LearnCSharp.Models.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LearnCSharp.Pages
{
    public class CoursesModel : PageModel
    {
        private readonly ApplicationContext _context;
        public List<Course> Courses { get; set; }
        
        public CoursesModel(ApplicationContext db)
        {
            _context = db;
        }
        
        public void OnGet()
        {
            Courses = _context.Courses.AsNoTracking().ToList();
        }
        
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course is null) return RedirectToPage();
            
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}