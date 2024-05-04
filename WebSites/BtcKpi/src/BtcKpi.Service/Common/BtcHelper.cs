using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtcKpi.Service.Common
{
    public static class BtcHelper
    {
        public static string ToRoman(int? number)
        {
            if (number == null) return "0";
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("Value must be between 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900); //EDIT: i've typed 400 instead 900
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            throw new ArgumentOutOfRangeException("Value must be between 1 and 3999");
        }

        public static string TermToString(byte? term)
        {
            if (term == 2) return "Ngắn hạn / Short term (2 năm/ year)";
            if (term == 5) return "Dài hạn / Long term (3-5 năm/ year)";
            return "";
        }

        public static string ConvertIpfScheduleType(byte? sheduleType)
        {
            if (sheduleType == 0) return "Năm";
            if (sheduleType == 1) return "Định kỳ";
            return "";
        }

        public static string RemoveComman(string input)
        {
            string removeBegin = input;
            string removeEnd;
            if (!string.IsNullOrEmpty(input))
            {
                if (input[0] == ',')
                {
                    removeBegin = input.Substring(1);
                }
                
                if (!string.IsNullOrEmpty(removeBegin) && removeBegin[removeBegin.Length-1] == ',')
                {
                    removeEnd = removeBegin.Substring(0, removeBegin.Length - 1);
                    return removeEnd;
                }

                return removeBegin;
            }
            return input;
        }

        public static string ConvertKpiStatus(int? status)
        {
            string statusName = "";
            if (status == null || status == 0)
            {
                statusName = "Lưu nháp";
            }
            else if (status == 1)
            {
                statusName = "Hoàn thành";
            }
            else if (status == 2)
            {
                statusName = "Đã duyệt";
            }
            return statusName;
        }

        public static string convertStatus(string status)
        {
            string statusName = "";
            if (status == "NotApproved")
            {
                statusName = "Chưa duyệt";
            }
            else if (status == "Approved")
            {
                statusName = "QL Đã duyệt";
            }
            else if (status == "BODApproved")
            {
                statusName = "BOD Đã duyệt";
            }
            else if (status == "DraftApproved")
            {
                statusName = "Lưu nháp";
            }
            else if (status == "Refuse")
            {
                statusName = "Từ chối";
            }
            return statusName;
        }
        public static string ConvertUpfCrossStatus(int? status)
        {
            if (status == 0) return "Chưa phản hồi";
            if (status == 1) return "Đã phản hồi";
            return "";
        }

        public static string ConvertSroreToRank(decimal? score)
        {
            if (score >= 5) return "A+";
            if (score < 5 & score >= 4) return "A";
            if (score < 4 & score >= 3) return "B+";
            if (score < 3 & score >= 2) return "B";
            if (score <= 2) return "C";
            return "";
        }

        public static string ConvertScoreToRankScheme10(decimal? score)
        {
            if (score >= 10) return "A+";
            if (score < 10 & score >= 8) return "A";
            if (score < 8 & score >= 6) return "B+";
            if (score < 6 & score >= 4) return "B";
            if (score < 4 & score >= 2) return "B-";
            if (score < 2) return "C";
            return "";
        }

        public static string ConvertIdToCode(int? id)
        {
            if (id == null)
                return "";
            else
            {
                return ((int)id).ToString("0###");
            }
            
        }

        public static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        public static string ConvertSeniority(DateTime? startDate)
        {
            if (startDate == null)
            {
                return "";
            }
            else
            {
                DateTime endDate = DateTime.Today;
                var totalDays = (decimal)(endDate.Day - ((DateTime)startDate).Day);
                var totalYears = Math.Truncate(totalDays / 365);
                var totalMonths = Math.Truncate((totalDays % 365) / 30);
                var remainingDays = Math.Truncate((totalDays % 365) % 30);
                return string.Format("{0} năm, {1} tháng, {2} ngày", totalYears, totalMonths, remainingDays);
            }
        }
        public static string convertQuarter(string status)
        {
            string statusName = "";
            if (status == "Quarter1")
            {
                statusName = "Quý 1";
            }
            else if (status == "Quarter2")
            {
                statusName = "Quý 2";
            }
            else if (status == "Quarter3")
            {
                statusName = "Quý 3";
            }
            else if (status == "Quarter4")
            {
                statusName = "Quý 4";
            }
            else if (status == "Refuse")
            {
                statusName = "Từ chối";
            }
            return statusName;
        }
    }
}
