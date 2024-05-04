using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BtcKpi.Model;

namespace BtcKpi.Service.Common
{
    public static class BtcData
    {
        public static List<PersonalPlan> PersonalCareerDefault()
        {
            var items = new List<PersonalPlan>()
            {
                new PersonalPlan() {Type = 1, Seq = 1, Activity = "Ngắn hạn / Short term (2 năm/ year)", Term = 2, DeleteFlg = 0},
                new PersonalPlan() {Type = 1, Seq = 2, Activity = "Dài hạn / Long term (3-5 năm/ year)", Term = 5, DeleteFlg = 0}
            };
            return items;
        }

        public static List<IpfDetail> IpfCompetencyDefault()
        {
            var items = new List<IpfDetail>()
            {
                new IpfDetail() {WorkType = 1, WorkCompleteID = 1, Objective = "Luôn tuân thủ nội quy, giờ giấc", Seq = 1, Target = "Tuân thủ nội quy, giờ giấc", Result = "", DeleteFlg = 0},
                new IpfDetail() {WorkType = 1, WorkCompleteID = 2, Objective = "Thái độ đối với công việc / Tuân thủ yêu cầu của Lãnh đạo / giữ hình ảnh cho bản thân và cho công ty", Seq = 2, Target = "Chăm chỉ làm việc, tuân thủ chỉ đạp cấp trên, có thái độ tốt, giữ gìn hình ảnh bản thân và quảng bá hình ảnh công ty", Result = "", DeleteFlg = 0},
                new IpfDetail() {WorkType = 1, WorkCompleteID = 3, Objective = "Kỹ năng làm việc đồng đội, Hợp tác tốt với các phòng ban nhằm thực hiện tốt chiến lược kinh doanh của Công ty.", Seq = 3, Target = "Hợp tác, hỗ trợ đồng nghiệp, các phòng ban khác nếu cần", Result = "", DeleteFlg = 0},
                new IpfDetail() {WorkType = 1, WorkCompleteID = 4, Objective = "Cải thiện/Sáng tạo/Đóng góp mới cho tổ chức", Seq = 4, Target = "Có những sáng kiến để đạt hiệu quả làm việc tốt nhất", Result = "", DeleteFlg = 0},
                new IpfDetail() {WorkType = 1, WorkCompleteID = 5, Objective = "Kỹ năng lãnh đạo, Khả năng hoạch định và tổ chức công việc cho bản thân và nhân viên", Seq = 5, Target = "Thiết lập được mục tiêu và có kế hoạch hành động phù hợp với mục tiêu.", Result = "", DeleteFlg = 0},
                new IpfDetail() {WorkType = 1, WorkCompleteID = 6, Objective = "Kỹ năng giao tiếp / thuyết trình", Seq = 6, Target = "Tạo được môi trường giao tiếp chuyên nghiệp và sự hài hòa trong quan hệ đồng nghiệp.", Result = "", DeleteFlg = 0},
                new IpfDetail() {WorkType = 1, WorkCompleteID = 7, Objective = "Kỹ năng giải quyết vấn đề", Seq = 7, Target = "Hỗ trợ và hướng dẫn được người khác phân tích và giải quyết vấn đề đạt hiệu qủa công việc cao.", Result = "", DeleteFlg = 0},
                
            };
            return items;
        }

        public static List<IpfDetail> IpfCompetencyManagerDefault()
        {
            var items = new List<IpfDetail>()
            {
                new IpfDetail() {WorkType = 1, WorkCompleteID = 1, Objective = "Luôn tuân thủ nội quy, giờ giấc", Seq = 1, Target = "Tuân thủ nội quy, giờ giấc", Result = "", DeleteFlg = 0},
                new IpfDetail() {WorkType = 1, WorkCompleteID = 2, Objective = "Thái độ đối với công việc / Tuân thủ yêu cầu của Lãnh đạo / giữ hình ảnh cho bản thân và cho công ty", Seq = 2, Target = "Chăm chỉ làm việc, tuân thủ chỉ đạp cấp trên, có thái độ tốt, giữ gìn hình ảnh bản thân và quảng bá hình ảnh công ty", Result = "", DeleteFlg = 0},
                new IpfDetail() {WorkType = 1, WorkCompleteID = 3, Objective = "Kỹ năng làm việc đồng đội, Hợp tác tốt với các phòng ban nhằm thực hiện tốt chiến lược kinh doanh của Công ty.", Seq = 3, Target = "Hợp tác, hỗ trợ đồng nghiệp, các phòng ban khác nếu cần", Result = "", DeleteFlg = 0},
                new IpfDetail() {WorkType = 1, WorkCompleteID = 4, Objective = "Cải thiện/Sáng tạo/Đóng góp mới cho tổ chức", Seq = 4, Target = "Có những sáng kiến để đạt hiệu quả làm việc tốt nhất", Result = "", DeleteFlg = 0},
                new IpfDetail() {WorkType = 1, WorkCompleteID = 5, Objective = "Kiến thức chuyên môn; Đào tạo, hướng dẫn giúp nhân viên phát triển năng lực và khả năng của bản thân  trong công việc", Seq = 5, Target = "Có kiến thức chuyên môn trong công việc đồng thời hướng dẫn nhân viên nâng cao khả năng bản thân để hoàn thành tốt công việc", Result = "", DeleteFlg = 0},
                new IpfDetail() {WorkType = 1, WorkCompleteID = 6, Objective = "Kỹ năng lãnh đạo, Khả năng hoạch định và tổ chức công việc cho bản thân và nhân viên", Seq = 6, Target = "Thiết lập được mục tiêu và có kế hoạch hành động phù hợp với mục tiêu.", Result = "", DeleteFlg = 0},
                new IpfDetail() {WorkType = 1, WorkCompleteID = 7, Objective = "Kỹ năng giao tiếp / thuyết trình", Seq = 7, Target = "Tạo được môi trường giao tiếp chuyên nghiệp và sự hài hòa trong quan hệ đồng nghiệp.", Result = "", DeleteFlg = 0},
                new IpfDetail() {WorkType = 1, WorkCompleteID = 8, Objective = "Kỹ năng giải quyết vấn đề", Seq = 8, Target = "Hỗ trợ và hướng dẫn được người khác phân tích và giải quyết vấn đề đạt hiệu qủa công việc cao.", Result = "", DeleteFlg = 0},

            };
            return items;
        }

    }
}
