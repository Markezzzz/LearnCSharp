using LearnCSharp.Models.Course;

namespace LearnCSharp.Services.Helpers
{
    public class ChapterHelper
    {
        private readonly MarkdownHelper _markdown;
        
        public ChapterHelper()
        {
            _markdown = new MarkdownHelper();
        }

        public string ParseContent(Chapter chapter)
        {
            return _markdown.ToHtml(chapter.RawContent);
        }
    }
}