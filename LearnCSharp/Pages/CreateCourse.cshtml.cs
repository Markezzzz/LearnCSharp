using System.Threading.Tasks;
using LearnCSharp.Models;
using LearnCSharp.Models.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnCSharp.Pages
{
    public class CreateCourseModel : PageModel
    {
        private readonly ApplicationContext _context;
        [BindProperty]
        public Course Course { get; set; }
        
        public CreateCourseModel(ApplicationContext db)
        {
            _context = db;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            
            await _context.Courses.AddAsync(Course);
            await _context.SaveChangesAsync();
            return RedirectToPage("EditCourse", new {id = Course.Id});
        }
    }
}