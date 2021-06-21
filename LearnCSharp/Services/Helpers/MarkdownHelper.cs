using Markdig;

namespace LearnCSharp.Services.Helpers
{
    public class MarkdownHelper
    {
        private readonly MarkdownPipeline _pipeline;
        
        public MarkdownHelper()
        {
            _pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseSmartyPants()
                .Build();
        }
        
        public string ToHtml(string rawMarkdownText)
        {
            return Markdown.ToHtml(rawMarkdownText, _pipeline);
        }
    }
}