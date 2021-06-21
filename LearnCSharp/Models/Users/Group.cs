using System.Collections;
using System.Collections.Generic;

namespace LearnCSharp.Models.Users
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Teacher> Teachers { get; set; }
    }
}