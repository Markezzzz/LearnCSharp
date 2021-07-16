using System.Threading.Tasks;
using LearnCSharp.Models;
using LearnCSharp.Models.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnCSharp.Pages
{
    public class CreateModuleModel : PageModel
    {
        private readonly ApplicationContext _context;
        [BindProperty]
        public Module Module { get; set; }
        
        public CreateModuleModel(ApplicationContext db)
        {
            _context = db;
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid) return Page();

            if (id is null)
                return NotFound();
            
            Module.CourseId = (int) id;
            await _context.Modules.AddAsync(Module);
            await _context.SaveChangesAsync();
            return RedirectToPage("EditCourse", new {id = Module.CourseId});
        }
    }
}