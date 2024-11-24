using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;

namespace Main.Helpers
{
    public static class BreadcrumbHelper
    {
        public static string GenerateBreadcrumb(this IHtmlHelper htmlHelper, ViewContext viewContext)
        {
            var controller = viewContext.RouteData.Values["controller"]?.ToString();
            var action = viewContext.RouteData.Values["action"]?.ToString();
            var breadcrumb = new StringBuilder();

            breadcrumb.Append("<nav aria-label='breadcrumb'><ol class='breadcrumb'>");

            if (!string.IsNullOrEmpty(controller))
            {
                breadcrumb.Append($"<li class='breadcrumb-item'><a href='/{controller}'>{controller}</a></li>");
            }

            if (!string.IsNullOrEmpty(action) && !action.Equals("index", StringComparison.CurrentCultureIgnoreCase))
            {
                breadcrumb.Append($"<li class='breadcrumb-item active' aria-current='page'>{action}</li>");
            }

            breadcrumb.Append("</ol></nav>");

            return breadcrumb.ToString();
        }

    }
}
