using System.Linq;
using System.Threading.Tasks;
using LearnCSharp.Models;
using LearnCSharp.Models.Course;
using LearnCSharp.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LearnCSharp.Pages
{
    public class EditChapterModel : PageModel
    {
        private readonly ApplicationContext _context;
        private readonly ChapterHelper _chapterHelper;
        [BindProperty]
        public Chapter Chapter { get; set; }
 
        public EditChapterModel(ApplicationContext db, ChapterHelper ch)
        {
            _context = db;
            _chapterHelper = ch;
        }
        
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
                return NotFound();

            Chapter = await _context.Chapters.FindAsync(id);
 
            if (Chapter is null)
                return NotFound();
            
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Attach(Chapter).State = EntityState.Modified;
            Chapter.Content = _chapterHelper.ParseContent(Chapter);
 
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
 
                if (!_context.Chapters.Any(chapter => chapter.Id == Chapter.Id))
                    return NotFound();
                throw;
            }

            return RedirectToPage("EditModule", new {id = Chapter.ModuleId});
        }
    }
}