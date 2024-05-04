using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Models
{
    public class Pagging<T>
    {
        #region prop
        public IEnumerable<T> model;
        public List<int> pageItems { get; set; }
        public int pageSize { get; set; }
        public int currentPage { get; set; }

        public int totalPage { get; set; }
        public int totalItem { get; set; }

        public string href{ get; set; }
        public string param { get; set; }

        #endregion

        #region contractor
        public Pagging(List<T> Model, int TotalItems, int CurrentPage, int PageSize, string Href, string Param)
        {
            this.model = Model;
            this.totalItem = TotalItems;
            this.currentPage = CurrentPage;
            this.totalPage = totalItem % PageSize > 0 ? (totalItem / PageSize) + 1 : totalItem / PageSize;
            PopulatePage(CurrentPage);

            this.href = Href;
            this.param = Param;
        }

        public Pagging(List<T> Model, int TotalItems, int CurrentPage, int PageSize)
        {
            this.model = Model;
            this.totalItem = TotalItems;
            this.currentPage = CurrentPage;
            this.totalPage = totalItem % PageSize > 0 ? (totalItem / PageSize) + 1 : totalItem / PageSize;
            PopulatePage(CurrentPage);
        }
        public void PopulatePage(int repeterStart)
        {
            pageItems = new List<int>();
            while (repeterStart % 10 != 0)
            {
                repeterStart--;
            }
            repeterStart = repeterStart == 0 ? 1 : repeterStart;
            if (repeterStart >= 9)
            {
                for (int i = repeterStart - 1; i < repeterStart + 11; i++)
                {
                    if (i > totalPage)
                        break;
                    else
                    {
                        pageItems.Add(i);
                    }
                }
            }
            else
            {
                for (int i = repeterStart; i < repeterStart + 11; i++)
                {
                    pageItems.Add(i);
                    if (i >= totalPage)
                        break;
                }
            }
        }
        #endregion


        #region method
        public string GenerateHtmlPagger()
        {
            StringBuilder paggerHtml = new StringBuilder();
            paggerHtml.Append("<div class='pagination pagination-small pagination-centered '>");
            paggerHtml.Append("<ul>");

            if (currentPage > 1)
            {
                paggerHtml.Append("<li>");
                paggerHtml.Append("<a data-ajax='true' data-ajax-begin='beginPaging' data-ajax-failure='failurePaging' data-ajax-mode='replace' data-ajax-success='successPaging' data-ajax-update='#grid-list' href='"+href+"?page=1&"+param+"' title=''>");
                paggerHtml.Append("<span> ");
                paggerHtml.Append("<i class='icon-backward'></i>Trang đầu ");
                paggerHtml.Append("</span> ");
                paggerHtml.Append("</a>");
                paggerHtml.Append("</li> ");
                paggerHtml.Append("<li> ");
                paggerHtml.Append("<a data-ajax='true' data-ajax-begin='beginPaging' data-ajax-failure='failurePaging' data-ajax-mode='replace' data-ajax-success='successPaging' data-ajax-update='#grid-list' href='"+href+"?page=" + (currentPage - 1).ToString() + "&"+param+"' title=''> ");
                paggerHtml.Append("<span> ");
                paggerHtml.Append("<i class='icon-backward'></i>Trước");
                paggerHtml.Append("</span> ");
                paggerHtml.Append("</a>");
                paggerHtml.Append("</li> ");
            }
            else
            {
                paggerHtml.Append("<li class='active first'> ");
                paggerHtml.Append("<span><i class='icon-backward'></i> Trang đầu </span>");
                paggerHtml.Append("</li> ");
                paggerHtml.Append("<li class='active previous'>");
                paggerHtml.Append("<span><i class='icon-chevron-left'></i>Trước </span>");
                paggerHtml.Append("</li> ");
            }
            foreach (var item in pageItems)
            {
                if (item == currentPage)
                {
                    paggerHtml.Append(" <li class='active'><span>" + item.ToString() + "</span></li>");
                }
                else
                {
                    paggerHtml.Append(" <li><a data-ajax='true' data-ajax-begin='beginPaging' data-ajax-failure='failurePaging' data-ajax-mode='replace' data-ajax-success='successPaging' data-ajax-update='#grid-list' href='" + href + "?page=" + item.ToString() + "&"+param+"' title=''>" + item.ToString() + "</a></li>");
                }
            }

            if (currentPage < totalPage)
            {
                paggerHtml.Append("<li> ");
                paggerHtml.Append(" <a data-ajax='true' data-ajax-begin='beginPaging' data-ajax-failure='failurePaging' data-ajax-mode='replace' data-ajax-success='successPaging' data-ajax-update='#grid-list' href='" + href + "?page=" + (currentPage + 1).ToString() + "&" + param + "' title=''>");
                paggerHtml.Append(" <span><i class='icon-backward'></i> Tiếp  </span>");
                paggerHtml.Append("   </a>");
                paggerHtml.Append(" </li> ");
                paggerHtml.Append("  <li>");
                paggerHtml.Append("   <a data-ajax='true' data-ajax-begin='beginPaging' data-ajax-failure='failurePaging' data-ajax-mode='replace' data-ajax-success='successPaging' data-ajax-update='#grid-list' href='" + href + "?page=" + totalPage.ToString() + "&" + param + "' title=''>");
                paggerHtml.Append(" <span> <i class='icon-backward'></i> Trang cuối");
                paggerHtml.Append("</a> ");
                paggerHtml.Append("</li> ");

            }
            else
            {
                paggerHtml.Append(" <li class='active next'><span>Tiếp <i class='icon-chevron-right'></i></span></li> ");
                paggerHtml.Append(" <li class='active last'><span>Trang cuối <i class='icon-forward'></i></span></li> ");


            }

            paggerHtml.Append("</ul>");
            paggerHtml.Append("</div>");          

            return paggerHtml.ToString();
        }
        public string GenerateHtmlPaggerSubmit(string formIdSubmit, string PageParamName)
        {
            StringBuilder paggerHtml = new StringBuilder();
            paggerHtml.Append(" <input type='hidden' name='" + PageParamName + "' id='" + PageParamName + "' value='" + currentPage + "' />");
            paggerHtml.Append("<div class='pagination pagination-small pagination-centered '>");
            paggerHtml.Append("<ul>");

            if (currentPage > 1)
            {
                paggerHtml.Append("<li>");
                paggerHtml.Append("<a href='#' onclick='pageSubmit(1)'>");
                paggerHtml.Append("<span> ");
                paggerHtml.Append("<i class='icon-backward'></i>Trang đầu ");
                paggerHtml.Append("</span> ");
                paggerHtml.Append("</a>");
                paggerHtml.Append("</li> ");
                paggerHtml.Append("<li> ");
                paggerHtml.Append("<a href='#' onclick='pageSubmit(" + (currentPage - 1) + ");'> ");
                paggerHtml.Append("<span> ");
                paggerHtml.Append("<i class='icon-backward'></i>Trước");
                paggerHtml.Append("</span> ");
                paggerHtml.Append("</a>");
                paggerHtml.Append("</li> ");
            }
            else
            {
                paggerHtml.Append("<li class='active first'> ");
                paggerHtml.Append("<span><i class='icon-backward'></i> Trang đầu </span>");
                paggerHtml.Append("</li> ");
                paggerHtml.Append("<li class='active previous'>");
                paggerHtml.Append("<span><i class='icon-chevron-left'></i>Trước </span>");
                paggerHtml.Append("</li> ");
            }
            foreach (var item in pageItems)
            {
                if (item == currentPage)
                {
                    paggerHtml.Append(" <li class='active'><span>" + item.ToString() + "</span></li>");
                }
                else
                {
                    paggerHtml.Append(" <li><a href='#' onclick='pageSubmit(" + item.ToString() + ")'>" + item.ToString() + "</a></li>");
                }
            }

            if (currentPage < totalPage)
            {
                paggerHtml.Append("<li> ");
                paggerHtml.Append(" <a href='#' onclick='pageSubmit(" + (currentPage + 1) + ");'>");
                paggerHtml.Append(" <span><i class='icon-backward'></i> Tiếp  </span>");
                paggerHtml.Append("   </a>");
                paggerHtml.Append(" </li> ");
                paggerHtml.Append("  <li>");
                paggerHtml.Append("   <a href='#' onclick='pageSubmit(" + totalPage.ToString() + ");'>");
                paggerHtml.Append(" <span> <i class='icon-backward'></i> Trang cuối");
                paggerHtml.Append("</a> ");
                paggerHtml.Append("</li> ");

            }
            else
            {
                paggerHtml.Append(" <li class='active next'><span>Tiếp <i class='icon-chevron-right'></i></span></li> ");
                paggerHtml.Append(" <li class='active last'><span>Trang cuối <i class='icon-forward'></i></span></li> ");


            }

            paggerHtml.Append("</ul>");
            paggerHtml.Append("</div>" + "\n");
            paggerHtml.Append("<script type='text/javascript'>" + "\n");
            paggerHtml.Append("function pageSubmit(pageNum) \n {   \n $('#" + PageParamName + "').val(pageNum); \n $('#" + formIdSubmit + "').submit();\n}" + "\n");

            paggerHtml.Append(" function formSubmit() \n {  \n $('#" + PageParamName + "').val(1); \n $('#" + formIdSubmit + "').submit(); \n }" + "\n");

            paggerHtml.Append("</script>" + "\n");

            return paggerHtml.ToString();
        }

        public string GenerateHtmlPaggerAjax()
        {
            StringBuilder paggerHtml = new StringBuilder();
            paggerHtml.Append(" <input type='hidden' name='PageParam' id='PageParam' value='" + currentPage + "' />");
            paggerHtml.Append("<div class='pagination pagination-small pagination-centered '>");
            paggerHtml.Append("<ul>");

            if (currentPage > 1)
            {
                paggerHtml.Append("<li>");
                paggerHtml.Append("<a href='#' onclick='formSubmit(1)'>");
                paggerHtml.Append("<span> ");
                paggerHtml.Append("<i class='icon-backward'></i>Trang đầu ");
                paggerHtml.Append("</span> ");
                paggerHtml.Append("</a>");
                paggerHtml.Append("</li> ");
                paggerHtml.Append("<li> ");
                paggerHtml.Append("<a href='#' onclick='formSubmit(" + (currentPage - 1) + ");'> ");
                paggerHtml.Append("<span> ");
                paggerHtml.Append("<i class='icon-backward'></i>Trước");
                paggerHtml.Append("</span> ");
                paggerHtml.Append("</a>");
                paggerHtml.Append("</li> ");
            }
            else
            {
                paggerHtml.Append("<li class='active first'> ");
                paggerHtml.Append("<span><i class='icon-backward'></i> Trang đầu </span>");
                paggerHtml.Append("</li> ");
                paggerHtml.Append("<li class='active previous'>");
                paggerHtml.Append("<span><i class='icon-chevron-left'></i>Trước </span>");
                paggerHtml.Append("</li> ");
            }
            foreach (var item in pageItems)
            {
                if (item == currentPage)
                {
                    paggerHtml.Append(" <li class='active'><span>" + item.ToString() + "</span></li>");
                }
                else
                {
                    paggerHtml.Append(" <li><a href='#' onclick='formSubmit(" + item.ToString() + ")'>" + item.ToString() + "</a></li>");
                }
            }

            if (currentPage < totalPage)
            {
                paggerHtml.Append("<li> ");
                paggerHtml.Append(" <a href='#' onclick='formSubmit(" + (currentPage + 1) + ");'>");
                paggerHtml.Append(" <span><i class='icon-backward'></i> Tiếp  </span>");
                paggerHtml.Append("   </a>");
                paggerHtml.Append(" </li> ");
                paggerHtml.Append("  <li>");
                paggerHtml.Append("   <a href='#' onclick='formSubmit(" + totalPage.ToString() + ");'>");
                paggerHtml.Append(" <span> <i class='icon-backward'></i> Trang cuối");
                paggerHtml.Append("</a> ");
                paggerHtml.Append("</li> ");

            }
            else
            {
                paggerHtml.Append(" <li class='active next'><span>Tiếp <i class='icon-chevron-right'></i></span></li> ");
                paggerHtml.Append(" <li class='active last'><span>Trang cuối <i class='icon-forward'></i></span></li> ");


            }

            paggerHtml.Append("</ul>");
            paggerHtml.Append("</div>" + "\n");
          

            return paggerHtml.ToString();
        }
        public string GenerateHtmlPaggerAjaxEn()
        {
            StringBuilder paggerHtml = new StringBuilder();
            paggerHtml.Append(" <input type='hidden' name='PageParam' id='PageParam' value='" + currentPage + "' />");
            paggerHtml.Append("<div class='pagination pagination-small pagination-centered '>");
            paggerHtml.Append("<ul>");

            if (currentPage > 1)
            {
                paggerHtml.Append("<li>");
                paggerHtml.Append("<a href='#' onclick='formSubmit(1)'>");
                paggerHtml.Append("<span> ");
                paggerHtml.Append("<i class='icon-backward'></i>First ");
                paggerHtml.Append("</span> ");
                paggerHtml.Append("</a>");
                paggerHtml.Append("</li> ");
                paggerHtml.Append("<li> ");
                paggerHtml.Append("<a href='#' onclick='formSubmit(" + (currentPage - 1) + ");'> ");
                paggerHtml.Append("<span> ");
                paggerHtml.Append("<i class='icon-backward'></i>Previous");
                paggerHtml.Append("</span> ");
                paggerHtml.Append("</a>");
                paggerHtml.Append("</li> ");
            }
            else
            {
                paggerHtml.Append("<li class='active first'> ");
                paggerHtml.Append("<span><i class='icon-backward'></i> First</span>");
                paggerHtml.Append("</li> ");
                paggerHtml.Append("<li class='active previous'>");
                paggerHtml.Append("<span><i class='icon-chevron-left'></i>Previous</span>");
                paggerHtml.Append("</li> ");
            }
            foreach (var item in pageItems)
            {
                if (item == currentPage)
                {
                    paggerHtml.Append(" <li class='active'><span>" + item.ToString() + "</span></li>");
                }
                else
                {
                    paggerHtml.Append(" <li><a href='#' onclick='formSubmit(" + item.ToString() + ")'>" + item.ToString() + "</a></li>");
                }
            }

            if (currentPage < totalPage)
            {
                paggerHtml.Append("<li> ");
                paggerHtml.Append(" <a href='#' onclick='formSubmit(" + (currentPage + 1) + ");'>");
                paggerHtml.Append(" <span><i class='icon-backward'></i>Next</span>");
                paggerHtml.Append("   </a>");
                paggerHtml.Append(" </li> ");
                paggerHtml.Append("  <li>");
                paggerHtml.Append("   <a href='#' onclick='formSubmit(" + totalPage.ToString() + ");'>");
                paggerHtml.Append(" <span> <i class='icon-backward'></i>Last");
                paggerHtml.Append("</a> ");
                paggerHtml.Append("</li> ");

            }
            else
            {
                paggerHtml.Append(" <li class='active next'><span>Next<i class='icon-chevron-right'></i></span></li> ");
                paggerHtml.Append(" <li class='active last'><span>Last<i class='icon-forward'></i></span></li> ");


            }

            paggerHtml.Append("</ul>");
            paggerHtml.Append("</div>" + "\n");


            return paggerHtml.ToString();
        }
        #endregion

    }
}