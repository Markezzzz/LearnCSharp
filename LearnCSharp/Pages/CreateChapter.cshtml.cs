using System.Linq;
using System.Threading.Tasks;
using LearnCSharp.Models;
using LearnCSharp.Models.Course;
using LearnCSharp.Services.Helpers;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LearnCSharp.Pages
{
    public class CreateChapterModel : PageModel
    {
        private readonly ApplicationContext _context;
        private readonly ChapterHelper _chapterHelper;
        [BindProperty]
        public Chapter Chapter { get; set; }
 
        public CreateChapterModel(ApplicationContext db, ChapterHelper ch)
        {
            _context = db;
            _chapterHelper = ch;
        }

        public async Task<IActionResult> OnPostAsync(int? moduleId, int? courseId)
        {
            if (!ModelState.IsValid) return Page();

            if (moduleId is null || courseId is null)
                return NotFound();
            
            Chapter.ModuleId = moduleId;
            Chapter.CourseId = courseId;
            Chapter.Content = _chapterHelper.ParseContent(Chapter);
            
            await _context.Chapters.AddAsync(Chapter);
            await _context.SaveChangesAsync();
            return RedirectToPage("EditModule", new {id = Chapter.ModuleId});
        }
    }
}