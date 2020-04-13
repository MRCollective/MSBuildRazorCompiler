using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using RazorLight;

namespace RazorRenderer
{
    public static class TemplateFileExtensions
    {
        public static IHtmlContent Render(this ITemplatePage template, ExpandoObject viewBag = null)
        {
            return RenderAsync(template, (object) null, viewBag).GetAwaiter().GetResult();
        }

        public static IHtmlContent Render<TModel>(this ITemplatePage template, TModel model, ExpandoObject viewBag = null)
        {
            return RenderAsync(template, model, viewBag).GetAwaiter().GetResult();
        }

        public static async Task<IHtmlContent> RenderAsync<TModel>(this ITemplatePage template, TModel model, ExpandoObject viewBag = null)
        {
            var content = await TemplateFileRenderer.RenderTemplateAsync(template, model, viewBag ?? new ExpandoObject()).ConfigureAwait(false);
            return new HtmlString(content);
        }
    }
}
