using System;
using System.Threading.Tasks;
using RazorLight.Compilation;

namespace RazorRenderer
{
    internal class NullRazorTemplateCompiler : IRazorTemplateCompiler
    {
        public Task<CompiledTemplateDescriptor> CompileAsync(string templateKey)
        {
            throw new NotImplementedException();
        }

        public ICompilationService CompilationService { get; }
    }
}
