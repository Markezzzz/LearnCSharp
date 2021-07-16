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
    public class EditModuleModel : PageModel
    {
        private readonly ApplicationContext _context;
        [BindProperty]
        public Module Module { get; set; }
        public IEnumerable<Chapter> Chapters
        {
            get
            {
                var chapters = _context.Chapters
                    .Where(c => c.ModuleId == Module.Id)
                    .ToList();
                chapters.Sort((c1, c2) => c1.index < c2.index ? -1 : 1);
                return chapters;
            }
        }

        public Dictionary<string, string> CreateChapterRouteData;
 
        public EditModuleModel(ApplicationContext db)
        {
            _context = db;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
                return NotFound();

            Module = await _context.Modules.FindAsync(id);
            
            CreateChapterRouteData = new Dictionary<string, string>
            {
                {"moduleId", Module.Id.ToString()},
                {"courseId", Module.CourseId.ToString()}
            };
 
            if (Module is null)
                return NotFound();
            
            return Page();
        }
        
        public async Task<IActionResult> OnPostDeleteAsync(int chapterId)
        {
            var chapter = await _context.Chapters.FindAsync(chapterId);

            if (chapter is null) return RedirectToPage();
            
            _context.Chapters.Remove(chapter);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Attach(Module).State = EntityState.Modified;
 
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
 
                if (!_context.Modules.Any(module => module.Id == Module.Id))
                    return NotFound();
                throw;
            }

            return RedirectToPage("EditCourse", new {id = Module.CourseId});
        }
    }
}