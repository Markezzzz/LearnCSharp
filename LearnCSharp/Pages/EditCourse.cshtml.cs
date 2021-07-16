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
    // TODO: Find out why i need to ignore AF token (and why does EditChapter work like it should)
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class EditCourseModel : PageModel
    {
        private readonly ApplicationContext _context;
        [BindProperty]
        public Course Course { get; set; }

        public IEnumerable<Module> Modules
        {
            get
            {
                var modules = _context.Modules
                    .Where(m => m.CourseId == Course.Id)
                    .ToList();
                modules.Sort((m1, m2) => m1.index < m2.index ? -1 : 1);
                return modules;
            }
        }

        public EditCourseModel(ApplicationContext db)
        {
            _context = db;
        }
        
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
                return NotFound();

            Course = await _context.Courses.FindAsync(id);
 
            if (Course is null)
                return NotFound();

            return Page();
        }
        
        public async Task<IActionResult> OnPostDeleteAsync(int moduleId)
        {
            var module = await _context.Modules.FindAsync(moduleId);

            if (module is null) return RedirectToPage();
            
            _context.Modules.Remove(module);
            await _context.SaveChangesAsync();

            return RedirectToPage("EditCourse", new {id = Course.Id});
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Attach(Course).State = EntityState.Modified;
 
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
 
                if (!_context.Courses.Any(course => course.Id == Course.Id))
                    return NotFound();
                throw;
            }
            
            return RedirectToPage("Courses");
        }
    }
}