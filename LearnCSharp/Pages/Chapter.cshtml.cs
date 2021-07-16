using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using LearnCSharp.Models;
using LearnCSharp.Models.Course;
using LearnCSharp.Services;
using LearnCSharp.Services.Helpers;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LearnCSharp.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class ChapterModel : PageModel
    {
        public Chapter Chapter { get; private set; }
        private readonly ApplicationContext _context;
        private readonly SandboxHelper _sandbox;
        public Chapter NextChapter { get; private set; }
        public Chapter PreviousChapter { get; private set; }
        public bool IsContentOnly { get; private set; }

        public ChapterModel(ApplicationContext db, SandboxHelper sandbox)
        {
            _context = db;
            _sandbox = sandbox;
        }

        public async Task<IActionResult> OnGetAsync(int? chapterId)
        {
            if (chapterId is null)
                return NotFound();

            Chapter = await _context.Chapters.FindAsync(chapterId);
            await SetNextAndPreviousChaptersAsync();
            SetPageType();

            if (Chapter is null)
                return NotFound();

            return Page();
        }

        private void SetPageType()
        {
            if (string.IsNullOrWhiteSpace(Chapter.TestsCode))
            {
                IsContentOnly = true;
                return;
            }
            IsContentOnly = false;
        }
        
        private async Task SetNextAndPreviousChaptersAsync()
        {
            var chapters = await GetSortedLinkedListAsync();
            var chapter = chapters.Find(Chapter);

            if (chapter is null)
                return;
            
            if (chapter.Next is not null)
            {
                NextChapter = chapter.Next.Value;
            }

            if (chapter.Previous is not null)
            {
                PreviousChapter = chapter.Previous.Value;
            }
        }

        private async Task<LinkedList<Chapter>> GetSortedLinkedListAsync()
        {
            var chapters = await _context.Chapters
                .Where(c => c.ModuleId == Chapter.ModuleId)
                .ToListAsync();
            chapters.Sort((c1, c2) => c1.index < c2.index ? -1 : 1);

            return new LinkedList<Chapter>(chapters);
        }

        public async Task<IActionResult> OnPostGetResultsAsync(string userCode, int id)
        {
            Chapter = await _context.Chapters.FindAsync(id);
            return new JsonResult(new {code = SandboxHelper.GetResults(userCode, Chapter)});
        }
    }
}