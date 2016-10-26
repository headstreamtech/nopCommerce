using Nop.Core.Infrastructure;
using Nop.Services.Catalog;
using System;
using System.Web.Mvc;

namespace Nop.Web.Themes.FaTech
{
    public static class FaTechExtensions
    {
        public static MvcHtmlString CategoryBodyClasss(this HtmlHelper html, UrlHelper url)
        {
            string currentCategorySlug = null;

            var categoryService = EngineContext.Current.Resolve<ICategoryService>();
            if (url.RequestContext.RouteData.Values["categoryId"] != null)
            {
                var id = Convert.ToInt32(url.RequestContext.RouteData.Values["categoryId"].ToString());
                currentCategorySlug = categoryService.GetCategoryById(id).Name.ToLower();
            }

            if (url.RequestContext.RouteData.Values["productId"] != null)
            {
                var productId = int.Parse(url.RequestContext.RouteData.Values["productId"].ToString());


                var productCategories = categoryService.GetProductCategoriesByProductId(productId);
                if (productCategories.Count > 0)
                    currentCategorySlug = productCategories[0].Category.Name.ToLower();
            }

            return new MvcHtmlString(string.IsNullOrEmpty(currentCategorySlug) ? string.Empty : "category-" + currentCategorySlug);
        }
    }
}