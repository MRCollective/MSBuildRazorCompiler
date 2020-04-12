using System;
using System.Dynamic;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using RazorLight;
using RazorLight.Internal;

namespace RazorRenderer
{
    // todo: see if there is a cleaner way to do this class
    public static class TemplateFileRenderer
    {
        private readonly static EngineHandler Handler = new EngineHandler(new RazorLightOptions(), new NullRazorTemplateCompiler(), new NullTemplateFactoryProvider(), null);

        public async static Task<string> RenderTemplateAsync<T>(ITemplatePage templatePage, T model, ExpandoObject viewBag = null)
        {
            using (var writer = new StringWriter())
            {
                await RenderTemplateAsync(templatePage, model, writer, viewBag);

                return writer.ToString();
            }
        }

        private async static Task RenderTemplateAsync<T>(
            ITemplatePage templatePage,
            T model,
            TextWriter textWriter,
            ExpandoObject viewBag = null)
        {
            SetModelContext(templatePage, textWriter, model, viewBag);

            using (var scope = new MemoryPoolViewBufferScope())
            {
                var renderer = new TemplateRenderer(Handler, HtmlEncoder.Default, scope);

                await renderer.RenderAsync(templatePage).ConfigureAwait(false);
            }
        }

        private static void SetModelContext<T>(
            ITemplatePage templatePage,
            TextWriter textWriter,
            T model,
            ExpandoObject viewBag)
        {
            if (textWriter == null)
            {
                throw new ArgumentNullException(nameof(textWriter));
            }

            var pageContext = new PageContext(viewBag)
            {
                ExecutingPageKey = templatePage.Key,
                Writer = textWriter
            };

            if (model != null)
            {
                pageContext.ModelTypeInfo = new ModelTypeInfo(model.GetType());

                object pageModel = pageContext.ModelTypeInfo.CreateTemplateModel(model);
                templatePage.SetModel(pageModel);

                pageContext.Model = pageModel;
            }

            templatePage.PageContext = pageContext;
        }
    }
}
