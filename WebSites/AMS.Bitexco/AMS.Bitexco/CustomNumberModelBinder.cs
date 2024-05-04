using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace AMS
{
    public class CustomDoubleModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var displayFormat = bindingContext.ModelMetadata.DisplayFormatString;
            var namePropertie = bindingContext.ModelMetadata.DisplayName;
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (!string.IsNullOrEmpty(displayFormat) && value != null)
            {
                double number;

                if (Double.TryParse(value.AttemptedValue.Replace(",",""), out number))
                {
                    return number;
                }
                else
                {
                    if (value.AttemptedValue == "")
                    {
                        bindingContext.ModelState.AddModelError(
                        bindingContext.ModelName,
                        string.Format("{0} là bắt buộc phải nhập ", namePropertie)
                    );
                    }
                    else
                    {
                        bindingContext.ModelState.AddModelError(
                        bindingContext.ModelName,
                        string.Format("{0} nhập không đúng định dạng số ", namePropertie)
                    );
                    }

                }
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }
}