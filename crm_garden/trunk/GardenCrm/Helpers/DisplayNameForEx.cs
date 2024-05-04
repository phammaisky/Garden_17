using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace GardenCrm.Helpers
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

        public static string TruncateAtWord(this string value, int length, bool withEtc)
        {
            if (value == null || value.Length < length || value.IndexOf(" ", length) == -1)
                return value;

            return withEtc ? value.Substring(0, value.IndexOf(" ", length)) + "..." : value.Substring(0, value.IndexOf(" ", length));
        }

        public static IEnumerable<List<T>> SplitList<T>(List<T> locations, int nSize = 30)
        {
            for (int i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }
    }

}