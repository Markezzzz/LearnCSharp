using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnCSharp.Models.Course
{
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public uint index { get; set; }
        
        public int? CourseId { get; set; }
        public Course? Course { get; set; }
        
        public ICollection<Chapter>? Chapters { get; set; }
    }
}