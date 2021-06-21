using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace LearnCSharp.Services
{
    public class Sandbox
    {
        //Этот метод может уронить сервер...
        public static string CompileAndRun(string assemblyName, string code)
        {
            using var ms = new MemoryStream();
            var result = GetCompilationResult(code, ms);
            var output = "";

            try
            {
                output = !result.Success ? GetErrorOutput(result) : RunAndGetOutput(assemblyName, ms);
            }
            catch(Exception e)
            {
                output += e.Message; //temp solution
            }

            return output;
        }
        
        //TODO: Find a way to not include EVERY reference
        private static IEnumerable<MetadataReference> GetReferences()
        {
            var references = new List<MetadataReference>() { };
            references.AddRange(((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES"))
                .Split(Path.PathSeparator)
                .Select(r => MetadataReference.CreateFromFile(r)).Cast<MetadataReference>());
            return references;
        }

        private static EmitResult GetCompilationResult(string code, Stream stream)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var assemblyName = Path.GetRandomFileName();
            var references = GetReferences();

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] {syntaxTree},
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            EmitResult result = compilation.Emit(stream);
            return result;
        }

        private static string RunAndGetOutput(string assemblyName, Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            Assembly assembly = AssemblyLoadContext.Default.LoadFromStream(stream);
            var type = assembly.GetType(assemblyName);
            var instance = assembly.CreateInstance(assemblyName);
            var method = type?.GetMember("RunAllTests").First() as MethodInfo;
            method?.Invoke(instance, null);
            var output = method?.Invoke(instance, null) + "\r\n";
            return output;
        }

        private static string GetErrorOutput(EmitResult result)
        {
            var output = "";
            IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                diagnostic.IsWarningAsError ||
                diagnostic.Severity == DiagnosticSeverity.Error);

            failures.Cast<Diagnostic>().ToList()
                .ForEach(diagnostic => output += $"\t{diagnostic.Id}: {diagnostic.GetMessage()}" + "\r\n");
            return output;
        }
    }
}