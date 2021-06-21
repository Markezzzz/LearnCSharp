using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnCSharp.Models;
using LearnCSharp.Models.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearnCSharp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationContext _context;
        [BindProperty]
        public List<Course> Courses { get; set; }
        
        public IndexModel(ApplicationContext db)
        {
            _context = db;
        }
        
        public void OnGet()
        {
            Courses = _context.Courses.AsNoTracking().ToList();
        }
    }
}