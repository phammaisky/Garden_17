using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BtcKpi.Web.CustomActionFilter;

namespace BtcKpi.Web.Controllers
{
    [CustomAuthorize]
    [CustomActionFilterAttribute]
    public class BaseController : Controller
    {
        public virtual CustomPrincipal CurrentUser
        {
            get { return this.ControllerContext.HttpContext.User as CustomPrincipal; }
        }

        public static List<SelectListItem> Years
        {
            get
            {
                
                List<SelectListItem> years = new List<SelectListItem>() {
                    new SelectListItem {
                        Text = "2020", Value = "2020"
                    },
                    new SelectListItem {
                        Text = "2021", Value = "2021"
                    },
                    new SelectListItem {
                        Text = "2022", Value = "2022"
                    },
                    new SelectListItem {
                        Text = "2023", Value = "2023"
                    },
                    new SelectListItem {
                        Text = "2024", Value = "2024"
                    },
                    new SelectListItem {
                        Text = "2025", Value = "2025"
                    },
                    new SelectListItem {
                        Text = "2026", Value = "2026"
                    },
                    new SelectListItem {
                        Text = "2027", Value = "2027"
                    },
                    new SelectListItem {
                        Text = "2028", Value = "2028"
                    },
                    new SelectListItem {
                        Text = "2029", Value = "2029"
                    },
                    new SelectListItem {
                        Text = "2030", Value = "2030"
                    },
                };
                return years;
            }
        }

        public static List<SelectListItem> Months
        {
            get
            {

                List<SelectListItem> months = new List<SelectListItem>() {
                    new SelectListItem {
                        Text = "Tháng 1", Value = "1"
                    },
                    new SelectListItem {
                        Text = "Tháng 2", Value = "2"
                    },
                    new SelectListItem {
                        Text = "Tháng 3", Value = "3"
                    },
                    new SelectListItem {
                        Text = "Tháng 4", Value = "4"
                    },
                    new SelectListItem {
                        Text = "Tháng 5", Value = "5"
                    },
                    new SelectListItem {
                        Text = "Tháng 6", Value = "6"
                    },
                    new SelectListItem {
                        Text = "Tháng 7", Value = "7"
                    },
                    new SelectListItem {
                        Text = "Tháng 8", Value = "8"
                    },
                    new SelectListItem {
                        Text = "Tháng 9", Value = "9"
                    },
                    new SelectListItem {
                        Text = "Tháng 10", Value = "10"
                    },
                    new SelectListItem {
                        Text = "Tháng 11", Value = "11"
                    },
                    new SelectListItem {
                        Text = "Tháng 12", Value = "12"
                    }
                };
                return months;
            }
        }


    }
}