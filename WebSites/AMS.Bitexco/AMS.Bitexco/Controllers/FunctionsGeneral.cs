using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.Models;

namespace AMS.Controllers
{
    public class FunctionsGeneral
    {     
        public static DateTime? EnUserDate(int assertId, DateTime dateEnd)
        {
            using(AMSEntities db = new AMSEntities())
            {
                var historyUse = db.HistoryUses.Where(hu => hu.DeviceToolId == assertId && hu.HandedDate > dateEnd).OrderByDescending(o=>o.HandedDate).FirstOrDefault();
                if(historyUse != null)
                {
                    return historyUse.HandedDate;
                }
            }
            return null;
        }
        public static string ConvertCoAcToHref(Controller ctl)
        {
            string _control = ctl.ControllerContext.RouteData.Values["controller"].ToString();
            string _action = ctl.ControllerContext.RouteData.Values["action"].ToString();

            return "/" + _control + "/" + _action;
        }
       
        public static string generateContractCode()
        {
            string code = string.Empty;
            string yearNow = DateTime.Now.Year.ToString();
            using (AMSEntities db = new AMSEntities())
            {
                var genCode = db.GenerateCodes.Find(1);
                if (genCode != null)
                {
                    code = (genCode.CodeNext).ToString("D4") + "-" + genCode.YearCode;
                    genCode.CodeNext = genCode.CodeNext + genCode.IncreNumber;
                    if (Convert.ToInt32(yearNow) > Convert.ToInt32(genCode.YearCode))
                    {
                        genCode.CodeNext = 1;
                        genCode.YearCode = yearNow;
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {
                        code = null;
                    }
                }
            }
            return code;
        }
        public static string generatePaymentCode()
        {
            string code = string.Empty;
            string yearNow = DateTime.Now.Year.ToString();
            using (AMSEntities db = new AMSEntities())
            {
                var genCode = db.GenerateCodes.Find(2);
                if (genCode != null)
                {
                    code = (genCode.CodeNext).ToString("D4") + "-" + genCode.YearCode;
                    genCode.CodeNext = genCode.CodeNext + genCode.IncreNumber;
                    if (Convert.ToInt32(yearNow) > Convert.ToInt32(genCode.YearCode))
                    {
                        genCode.CodeNext = 1;
                        genCode.YearCode = yearNow;
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {
                        code = null;
                    }
                }
            }
            return code;
        }

        public static string generateDeviceAndToolCode()
        {
            string code = string.Empty;
            string yearNow = DateTime.Now.Year.ToString();
            using (AMSEntities db = new AMSEntities())
            {
                var genCode = db.GenerateCodes.Find(3);
                if (genCode != null)
                {
                    code = (genCode.CodeNext).ToString("D4") + "-" + genCode.YearCode;
                    genCode.CodeNext = genCode.CodeNext + genCode.IncreNumber;
                    if (Convert.ToInt32(yearNow) > Convert.ToInt32(genCode.YearCode))
                    {
                        genCode.CodeNext = 1;
                        genCode.YearCode = yearNow;
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {
                        code = null;
                    }
                }
            }
            return code;
        }

        public static string generateLevelPaymentRequest(Int16 level, string content)
        {
            if (String.IsNullOrEmpty(content))
                content = "";

            if(level==1)
            {
                return "<label data-toggle='tooltip' data-placement='top' style='color:red' title='" + content + "' style='color:green'>" + level.ToString() + "</label>";
            }
            else
            {
                return "<label data-toggle='tooltip' data-placement='top' style='color:green'>" + level.ToString() + "</label>";
            }          
        }

        public static string generateSourceAMS(int level)
        {
            switch (level)
            {
                case 1:
                    return "Vốn tự có";
                case 2:
                    return "Vốn vay";
                case 3:
                    return "Trái phiếu";
                default:
                    return "Vốn tự có";

            }
        }

        public static string generateStatusPaymentRequest(int status)
        {
            switch (status)
            {
                case 0:
                    return "Chờ duyệt";

                case 1:
                    return "Đang xử lý";

                case 2:
                    return "Từ chối";

                case 3:
                    return "Đã duyệt";

                default:
                    return "Lỗi!";

            }
        }

        public static string generateLocation(int? locationId, string text, int leverl = 0)
        {
            string textReturn = "";

            Location location = LocationLevel(locationId, leverl);
            while(location != null && location.ParentId != null )
            {
                if (leverl == 0)
                    textReturn = location.ShortName + "|" + location.LocationName;
                else
                    textReturn = location.ShortName + " / " + textReturn;
                location = LocationLevel(location.ParentId, leverl);
                leverl++;
            }
           return textReturn;
        }
       

        private static Location LocationLevel(int? locationId, int leverl)
        {
            using (var db = new AMSEntities())
            {
                if (locationId != null)
                {
                    var location = db.Locations.Find(locationId);
                    if (location != null)
                    {
                        return location;
                    }
                }
            }
            return null;
        }
     
        public static void DateThisWeek(ref DateTime startDate, ref DateTime endDate)
        {
            DateTime now = DateTime.Now.Date;
            startDate = now.AddDays(-(int)now.DayOfWeek);
            endDate = startDate.AddDays(5);
        }
        public static void DateNextWeek(ref DateTime startDate, ref DateTime endDate)
        {
            DateTime now = DateTime.Now.Date;
            int dayOfWeek = (int)now.DayOfWeek - 1;
            DateTime nowAdd = DateTime.Now.AddDays(7);
            startDate = nowAdd.AddDays(-dayOfWeek).Date;
            endDate = startDate.AddDays(5);
        }
        public static void DateLastWeek(ref DateTime startDate, ref DateTime endDate)
        {
            DateTime now = DateTime.Now.Date;
            int dayOfWeek = (int)now.DayOfWeek - 1;
            DateTime nowAdd = DateTime.Now.AddDays(-7);
            startDate = nowAdd.AddDays(-dayOfWeek).Date;
            endDate = startDate.AddDays(5);
        }
        public static void DateThisMonth(ref DateTime startDate, ref DateTime endDate)
        {
            DateTime now = DateTime.Now.Date;
            startDate = new DateTime(now.Year, now.Month, 1);
            endDate = startDate.AddDays(DateTime.DaysInMonth(now.Year, now.Month));
        }
        public static void DateNextMonth(ref DateTime startDate, ref DateTime endDate)
        {
            DateTime now = DateTime.Now.Date.AddMonths(1);
            startDate = new DateTime(now.Year, now.Month, 1);
            endDate = startDate.AddDays(DateTime.DaysInMonth(now.Year, now.Month) - 1);
        }

        public static void DateLastMonth(ref DateTime startDate, ref DateTime endDate)
        {
            DateTime now = DateTime.Now.Date.AddMonths(-1);
            startDate = new DateTime(now.Year, now.Month, 1);
            endDate = startDate.AddDays(DateTime.DaysInMonth(now.Year, now.Month) - 1);
        }

        public static IEnumerable<SelectListItem> AddDefaultOption(IEnumerable<SelectListItem> list, string dataTextField, string selectedValue)
        {            
            var items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = dataTextField, Value = selectedValue });
             if(list != null)
              items.AddRange(list);
            return items;
        }

      

        public static List<DateTime> AllDateOfNextMonth()
        {
            DateTime now = DateTime.Now.Date;
            now = now.AddMonths(1);

            var dates = new List<DateTime>();
            for (var date = new DateTime(now.Year, now.Month, 1); date.Month == now.Month; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Sunday)
                    dates.Add(date);
            }
            return dates;


        }
        public static List<DateTime> AllDateOfNextWeek()
        {
            DateTime starDate = new DateTime();
            DateTime endDate = new DateTime();
            DateNextWeek(ref starDate, ref endDate);

            var dates = new List<DateTime>();
            for (var date = starDate; date <= endDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Sunday)
                    dates.Add(date);
            }
            return dates;
        }

        public static string ChuyenSotienSangChu(decimal? number)
        {
            string s = number.HasValue ? number.Value.ToString("#") : "";

            string[] so = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] hang = new string[] { "", "nghìn", "triệu", "tỷ" };
            int i, j, donvi, chuc, tram;
            string str = " ";
            bool booAm = false;
            decimal decS = 0;
            //Tung addnew
            try
            {
                decS = Convert.ToDecimal(s.ToString());
            }
            catch
            {
            }
            if (decS < 0)
            {
                decS = -decS;
                s = decS.ToString();
                booAm = true;
            }
            i = s.Length;
            if (i == 0)
                str = so[0] + str;
            else
            {
                j = 0;
                while (i > 0)
                {
                    donvi = Convert.ToInt32(s.Substring(i - 1, 1));
                    i--;
                    if (i > 0)
                        chuc = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        chuc = -1;
                    i--;
                    if (i > 0)
                        tram = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        tram = -1;
                    i--;
                    if ((donvi > 0) || (chuc > 0) || (tram > 0) || (j == 3))
                        str = hang[j] + str;
                    j++;
                    if (j > 3) j = 1;
                    if ((donvi == 1) && (chuc > 1))
                        str = "mốt " + str;
                    else
                    {
                        if ((donvi == 5) && (chuc > 0))
                            str = "lăm " + str;
                        else if (donvi > 0)
                            str = so[donvi] + " " + str;
                    }
                    if (chuc < 0)
                        break;
                    else
                    {
                        if ((chuc == 0) && (donvi > 0)) str = "lẻ " + str;
                        if (chuc == 1) str = "mười " + str;
                        if (chuc > 1) str = so[chuc] + " mươi " + str;
                    }
                    if (tram < 0) break;
                    else
                    {
                        if ((tram > 0) || (chuc > 0) || (donvi > 0)) str = so[tram] + " trăm " + str;
                    }
                    str = " " + str;
                }
            }
            if (booAm) str = "Âm " + str;
            else
            {
                string UperString = str.Substring(0, 1).ToUpper();
                string SubString = str.Substring(1);
                str = UperString + SubString;
            }
            return str + " đồng ";

        }

         public static string ChuyenSotienSangChu(double? number, string donViTT)
        {
            if (String.IsNullOrEmpty(donViTT))
                donViTT = "đồng";

            if (donViTT == "VND")
                donViTT = "đồng";

            string s = number.HasValue ? number.Value.ToString() : "";
            string sOut = "";
            if (!String.IsNullOrEmpty(s))
            {
                string s1="";
                string s2="";
                if (s.IndexOf(".") > 0)
                {
                    int p = s.IndexOf(".");
                    s1 = s.Substring(0, s.IndexOf("."));
                    s2 = s.Substring(s.IndexOf(".")+1, (s.Length -1) - s1.Length);

                    sOut = DocSo(s1) + " phẩy " + DocSoThapPhan(s2) + " " + donViTT;
                }
                else
                {
                    sOut = DocSo(s) + " "+ donViTT;
                }


                string UperString = sOut.Substring(0, 1).ToUpper();
                string SubString = sOut.Substring(1);
                sOut = UperString + SubString;
                return sOut;  
            }
            else
                return "";
        }
        public static string DocSoThapPhan(string number)
         {
             string[] so = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
             if (number.Length > 0)
             {
                 if(number.Length > 1 )
                 {
                     if(Convert.ToInt16(number.Substring(0,1)) == 0)
                     {
                         return "không " + DocSo(number.Substring(1, number.Length - 1));
                     }
                     else
                     {
                         return DocSo(number);
                     }
                 }
                 else
                 {
                     return so[Convert.ToInt16(number)];
                 }
             }
             else
                 return "";
         }
        public static string DocSo(string number)
        {          
            string s = number;
            string[] so = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] hang = new string[] { "", "nghìn", "triệu", "tỷ" };
            int i, j, donvi, chuc, tram;
            string str = " ";
            bool booAm = false;
            double decS = 0;
            //Tung addnew
            try
            {
                decS = Convert.ToDouble(s.ToString());
            }
            catch
            {
            }
            if (decS < 0)
            {
                decS = -decS;
                s = decS.ToString();
                booAm = true;
            }
            i = s.Length;
            if (i == 0)
                str = so[0] + str;
            else
            {
                j = 0;
                while (i > 0)
                {
                    donvi = Convert.ToInt32(s.Substring(i - 1, 1));
                    i--;
                    if (i > 0)
                        chuc = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        chuc = -1;
                    i--;
                    if (i > 0)
                        tram = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        tram = -1;
                    i--;
                    if ((donvi > 0) || (chuc > 0) || (tram > 0) || (j == 3))
                        str = hang[j] + str;
                    j++;
                    if (j > 3) j = 1;
                    if ((donvi == 1) && (chuc > 1))
                        str = "mốt " + str;
                    else
                    {
                        if ((donvi == 5) && (chuc > 0))
                            str = "lăm " + str;
                        else if (donvi > 0)
                            str = so[donvi] + " " + str;
                    }
                    if (chuc < 0)
                        break;
                    else
                    {
                        if ((chuc == 0) && (donvi > 0)) str = "lẻ " + str;
                        if (chuc == 1) str = "mười " + str;
                        if (chuc > 1) str = so[chuc] + " mươi " + str;
                    }
                    if (tram < 0) break;
                    else
                    {
                        if ((tram > 0) || (chuc > 0) || (donvi > 0)) str = so[tram] + " trăm " + str;
                    }
                    str = " " + str;
                }
            }
            if (booAm) str = "Âm " + str;
            else
            {
                str = str.Trim();               
            }

            return str ;
        }
    }
}