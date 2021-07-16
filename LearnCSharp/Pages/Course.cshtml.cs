using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LearnCSharp.Models;
using LearnCSharp.Models.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnCSharp.Pages
{
    public class CourseModel : PageModel
    {
        private readonly ApplicationContext _context;
        public List<Module> Modules { get; set; }
        public Course Course { get; set; }

        public CourseModel(ApplicationContext db)
        {
            _context = db;
        }
        
        public async Task<IActionResult> OnGetAsync(int? courseId)
        {
            if (courseId is null)
                return NotFound();

            Course = await _context.Courses.FindAsync(courseId);
 
            if (Course is null)
                return NotFound();
            
            Modules = _context.Modules.Where(m => m.CourseId == courseId).ToList();
            Modules.Sort((m1, m2) => m1.index < m2.index ? -1 : 1);
            
            return Page();
        }

        public IEnumerable<Chapter> GetChapters(int moduleId)
        {
            var chapters =_context.Chapters.Where(c => c.ModuleId == moduleId).ToList();
            chapters.Sort((c1, c2) => c1.index < c2.index ? -1 : 1);
            return chapters;
        }
    }
}