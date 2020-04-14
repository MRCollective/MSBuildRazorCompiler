using RazorLight;

namespace RazorRenderer
{
    public abstract class BasePage<TModel> : TemplatePage<TModel>
    {
        protected void Include<TIncludedPage, TIncludedPageModel>(TIncludedPageModel model)
            where TIncludedPage : BasePage<TIncludedPageModel>, new()
        {
            var newPage = new TIncludedPage();
            TemplateFileRenderer.RenderIncludedTemplateAsync(this, newPage, model).GetAwaiter().GetResult();
        }
    }
}
