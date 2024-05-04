using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace AMS.Helpers
{
    public static class DisplayNameForEx
    {
        public static MvcHtmlString DisplayColumnNameFor<TModel, TClass, TProperty>(this HtmlHelper<TModel> helper, IEnumerable<TClass> model, Expression<Func<TClass, TProperty>> expression)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            var metadata = ModelMetadataProviders.Current.GetMetadataForProperty(
                () => Activator.CreateInstance<TClass>(), typeof(TClass), name);

            var returnName = metadata.DisplayName;
            if (string.IsNullOrEmpty(returnName))
                returnName = metadata.PropertyName;
            return new MvcHtmlString(returnName);
        }
    }

}