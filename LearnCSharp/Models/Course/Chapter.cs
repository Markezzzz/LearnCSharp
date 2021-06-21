using System.Reflection.Metadata.Ecma335;

namespace LearnCSharp.Models.Course
{
    public class Chapter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RawContent { get; set; }
        public string? Content { get; set; }   
        public string? AssemblyName { get; set; }
        public string? TestsCode { get; set; }
        public string? SecretTestsCode { get; set; }
        public string? TeacherCode { get; set; }
        public uint index { get; set; }

        public int? CourseId { get; set; }
        public int? ModuleId { get; set; }
        public Module? Module { get; set; }
    }
}