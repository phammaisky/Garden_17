using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMS
{
    public class CustomDateModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var displayFormat = bindingContext.ModelMetadata.DisplayFormatString;
            var namePropertie = bindingContext.ModelMetadata.DisplayName;
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (!string.IsNullOrEmpty(displayFormat) && value != null)
            {
                DateTime date;
                displayFormat = displayFormat.Replace("{0:", string.Empty).Replace("}", string.Empty);
                if (DateTime.TryParseExact(value.AttemptedValue, displayFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    return date;
                }
                else
                {
                    if (value.AttemptedValue == "")
                    {
                        if(bindingContext.ModelMetadata.IsNullableValueType)
                        {
                            return null;
                        }
                        bindingContext.ModelState.AddModelError(
                        bindingContext.ModelName,
                        string.Format("{0} là bắt buộc phải nhập ", namePropertie)
                    );
                    }
                    else
                    {
                        bindingContext.ModelState.AddModelError(
                        bindingContext.ModelName,
                        string.Format("{0} nhập không đúng định dạng ngày/tháng/năm", namePropertie)
                    );
                    }

                }
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }
}