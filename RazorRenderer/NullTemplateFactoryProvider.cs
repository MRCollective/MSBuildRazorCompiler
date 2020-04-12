using System;
using RazorLight;
using RazorLight.Compilation;

namespace RazorRenderer
{
    internal class NullTemplateFactoryProvider : ITemplateFactoryProvider
    {
        public Func<ITemplatePage> CreateFactory(CompiledTemplateDescriptor templateDescriptor)
        {
            throw new NotImplementedException();
        }
    }
}
