using System.Collections.Generic;

namespace LearnCSharp.Models.Course
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public ICollection<Module>? Modules { get; set; }
    }
}