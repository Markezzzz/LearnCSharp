using LearnCSharp.Models.Course;

namespace LearnCSharp.Services.Helpers
{
    public class SandboxHelper
    {
        private readonly Sandbox _sandbox;

        public SandboxHelper()
        {
            _sandbox = new Sandbox();
        }
        
        public string GetResults(string solutionCode, Chapter chapter)
        {
            var code = chapter.TestsCode + "\n" + chapter.SecretTestsCode + "\n" + solutionCode;
            return Sandbox.CompileAndRun(chapter.AssemblyName, code);
        }
    }
}