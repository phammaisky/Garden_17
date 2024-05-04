using System;
using System.Drawing;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Web;
using System.IO;
using System.Diagnostics;
using OfficeOpenXml;

using AMS.Models;

namespace AMS
{
    public class MSExcelReaderHD
    {
        public string pathFile { get; set; }

        public MSExcelReaderHD(string path)
        {
            this.pathFile = path;
        }
        private AMSEntities db;

        public void WriteError(ExcelWorksheet ws, int row, int col, string errorMessage)
        {
            ws.Cells[row, col + 1].Value = errorMessage;
            ws.Cells[row, col + 1].Style.Font.Color.SetColor(Color.FromArgb(158, 0, 0));
        }

        public void UploadKiemKe()
        {
            FileInfo file = new FileInfo(this.pathFile);
            using (ExcelPackage pck = new ExcelPackage(file))
            {
                ExcelWorksheet HopDongWS = pck.Workbook.Worksheets[1];
                var rowCount = HopDongWS.Dimension.End.Row + 1;
                var colCount = HopDongWS.Dimension.End.Column;
                for (int i = 2; i < rowCount; i++)
                {
                    string errorMessage = "";
                    errorMessage = UpdateKiemKe
                         (
                             HopDongWS.GetValue(i, 1) == null ? "" : HopDongWS.GetValue(i, 1).ToString()
                         );
                    WriteError(HopDongWS, i, colCount, errorMessage);
                }

                pck.Save();
            }

        }
        protected string UpdateKiemKe(string maTaiSan)
        {
            string errorMessage = "";

            if (!String.IsNullOrEmpty(maTaiSan))
            {
                  using (db = new AMSEntities())
                  {
                      var deviceAndTool = db.DeviceAndTools.Where(dv => dv.AssetsCode.Trim().ToLower() == maTaiSan.Trim().ToLower()).FirstOrDefault();
                      if(deviceAndTool != null)
                      {
                          deviceAndTool.CheckedDate = DateTime.Now;
                          try
                          {
                              db.SaveChanges();
                          }
                          catch
                          {
                              errorMessage += "Không cập nhật được ngày kiểm kê cho mã tài sản " + maTaiSan;
                          }
                      }
                      else
                      {
                          errorMessage += "Không có tài sản tương ứng với mã " + maTaiSan;
                      }
                  }
            }
            else
            {                
                errorMessage += "Không có mã tài sản";
            }
            return errorMessage;
        }

        public void AddContractDataFromExcel()
        {
            FileInfo file = new FileInfo(this.pathFile);
            using (ExcelPackage pck = new ExcelPackage(file))
            {
                ExcelWorksheet HopDongWS = pck.Workbook.Worksheets["TB"];
                var rowCount = HopDongWS.Dimension.End.Row + 1;
                var colCount = HopDongWS.Dimension.End.Column;
                for (int i = 3; i < rowCount; i++)
                {
                    string errorMessage = "";
                    errorMessage = AddDevice
                         (
                             HopDongWS.GetValue(i, 1) == null ? "" : HopDongWS.GetValue(i, 1).ToString(),
                             HopDongWS.GetValue(i, 3) == null ? "" : HopDongWS.GetValue(i, 3).ToString(),
                             HopDongWS.GetValue(i, 5) == null ? "" : HopDongWS.GetValue(i, 5).ToString(),
                             HopDongWS.GetValue(i, 6) == null ? "" : HopDongWS.GetValue(i, 6).ToString(),
                             HopDongWS.GetValue(i, 8) == null ? "" : HopDongWS.GetValue(i, 8).ToString(),
                             HopDongWS.GetValue(i, 9) == null ? "" : HopDongWS.GetValue(i, 9).ToString(),
                             HopDongWS.GetValue(i, 10) == null ? "" : HopDongWS.GetValue(i, 10).ToString(),
                             HopDongWS.GetValue(i, 12) == null ? "" : HopDongWS.GetValue(i, 12).ToString(),
                             HopDongWS.GetValue(i, 14) == null ? "" : HopDongWS.GetValue(i, 14).ToString(),
                             HopDongWS.GetValue(i, 16) == null ? "" : HopDongWS.GetValue(i, 16).ToString(),
                             HopDongWS.GetValue(i, 17) == null ? "" : HopDongWS.GetValue(i, 17).ToString(),
                             HopDongWS.GetValue(i, 19) == null ? "" : HopDongWS.GetValue(i, 19).ToString()
                         );
                    WriteError(HopDongWS, i, colCount, errorMessage);
                }

                pck.Save();
            }

        }

        protected string AddTool(string companyId, string ngayMua, string maTS, string loaiCCId, string tenTaiSan, string moTaChiTietTaiSan, string nguoiSuDungId, string phongBanSuDungId, string diaDiemId, string ngayBanGiao, string tinhTrangId, string tinhTrangLucBanGiao)
        {
            bool error = false;
            string errorMessage = "";

            DeviceToolAndHistory deviceToolAndHistory = new DeviceToolAndHistory();

            if (!String.IsNullOrEmpty(tenTaiSan))
                deviceToolAndHistory.DeviceName = tenTaiSan;
            else
            {
                error = true;
                errorMessage += "Khong co ten tai san";
            }

            if (!String.IsNullOrEmpty(moTaChiTietTaiSan))
                deviceToolAndHistory.DescriptionDevice = moTaChiTietTaiSan;
            if (!String.IsNullOrEmpty(tinhTrangLucBanGiao))
                deviceToolAndHistory.StatusDescription = tinhTrangLucBanGiao;

            if (String.IsNullOrEmpty(ngayMua))
                ngayMua = "31/08/2015";
            if (String.IsNullOrEmpty(ngayBanGiao))
                ngayBanGiao = "31/08/2015";

            CheckCompanyId(companyId, ref error, deviceToolAndHistory, ref errorMessage);
            CheckNgayMua(ngayMua, ref error, deviceToolAndHistory, ref errorMessage);
            
            CheckLoaiCCId(loaiCCId, ref error, deviceToolAndHistory, ref errorMessage);

            if (!String.IsNullOrEmpty(nguoiSuDungId) || !String.IsNullOrEmpty(phongBanSuDungId) || !String.IsNullOrEmpty(diaDiemId))
            {
                if (!String.IsNullOrEmpty(nguoiSuDungId))
                CheckUserId(nguoiSuDungId, ref error, deviceToolAndHistory, ref errorMessage);
                if (!String.IsNullOrEmpty(phongBanSuDungId))
                CheckDeptId(phongBanSuDungId, ref error, deviceToolAndHistory, ref errorMessage);
                if (!String.IsNullOrEmpty(diaDiemId))
                CheckLocationId(diaDiemId, ref error, deviceToolAndHistory, ref errorMessage);
            }
            else
            {
                error = true;
                errorMessage += "Khong co nguoi su dung, phong ban su dung, dia diem";
            }


            CheckNgayBanGiao(ngayBanGiao, ref error, deviceToolAndHistory, ref errorMessage);
            CheckStatusCategory(tinhTrangId, ref error, deviceToolAndHistory, ref errorMessage);

            //if (!error)
            //{
            //    if (String.IsNullOrEmpty(maTS))
            //        maTS = AMS.Controllers.FunctionsGeneral.generateDeviceAndToolCode();

            //    CheckCode(maTS, ref error, deviceToolAndHistory, ref errorMessage);

            //    if (!error)
            //    {
            //        using (db = new AMSEntities())
            //        {
            //            AMS.Repositories.DeviceAndToolReposity.CreateDeviceAndTool(deviceToolAndHistory, 3096);
            //        }
            //    }
            //}

            return errorMessage;
        }

        protected string AddDevice(string companyId, string ngayMua, string maTS, string loaiTBId, string tenTaiSan, string moTaChiTietTaiSan, string nguoiSuDungId, string phongBanSuDungId, string diaDiemId, string ngayBanGiao, string tinhTrangId, string tinhTrangLucBanGiao)
        {
            bool error = false;
            string errorMessage = "";

            DeviceToolAndHistory deviceToolAndHistory = new DeviceToolAndHistory();

            if (!String.IsNullOrEmpty(tenTaiSan))
                deviceToolAndHistory.DeviceName = tenTaiSan;
            else
            {
                error = true;
                errorMessage += "Khong co ten tai san";
            }

            if (!String.IsNullOrEmpty(moTaChiTietTaiSan))
                deviceToolAndHistory.DescriptionDevice = moTaChiTietTaiSan;
            if (!String.IsNullOrEmpty(tinhTrangLucBanGiao))
                deviceToolAndHistory.StatusDescription = tinhTrangLucBanGiao;

            if (String.IsNullOrEmpty(ngayMua))
                ngayMua = "31/08/2015";
            if (String.IsNullOrEmpty(ngayBanGiao))
                ngayBanGiao = "31/08/2015";

            CheckCompanyId(companyId, ref error, deviceToolAndHistory, ref errorMessage);
            CheckNgayMua(ngayMua, ref error, deviceToolAndHistory, ref errorMessage);

            CheckLoaiDeviceId(loaiTBId, ref error, deviceToolAndHistory, ref errorMessage);

            if (!String.IsNullOrEmpty(nguoiSuDungId) || !String.IsNullOrEmpty(phongBanSuDungId) || !String.IsNullOrEmpty(diaDiemId))
            {
                if (!String.IsNullOrEmpty(nguoiSuDungId))
                    CheckUserId(nguoiSuDungId, ref error, deviceToolAndHistory, ref errorMessage);
                if (!String.IsNullOrEmpty(phongBanSuDungId))
                    CheckDeptId(phongBanSuDungId, ref error, deviceToolAndHistory, ref errorMessage);
                if (!String.IsNullOrEmpty(diaDiemId))
                    CheckLocationId(diaDiemId, ref error, deviceToolAndHistory, ref errorMessage);
            }
            else
            {
                error = true;
                errorMessage += "Khong co nguoi su dung, phong ban su dung, dia diem";
            }


            CheckNgayBanGiao(ngayBanGiao, ref error, deviceToolAndHistory, ref errorMessage);
            CheckStatusCategory(tinhTrangId, ref error, deviceToolAndHistory, ref errorMessage);

            //if (!error)
            //{
            //    if (String.IsNullOrEmpty(maTS))
            //        maTS = AMS.Controllers.FunctionsGeneral.generateDeviceAndToolCode();

            //    CheckCode(maTS, ref error, deviceToolAndHistory, ref errorMessage);

            //    if (!error)
            //    {
            //        using (db = new AMSEntities())
            //        {
            //            AMS.Repositories.DeviceAndToolReposity.CreateDeviceAndTool(deviceToolAndHistory, 3096);
            //        }
            //    }
            //}

            return errorMessage;
        }

        private void CheckCompanyId(string companyId, ref bool error, DeviceToolAndHistory deviceToolAndHistory, ref string errorMessage)
        {
            int _companyId;
            if (int.TryParse(companyId, out _companyId))
            {
                using (db = new AMSEntities())
                {
                    var company = db.Companies.Find(_companyId);
                    if (company != null)
                    {
                        deviceToolAndHistory.CompanyId = _companyId;
                    }
                    else
                    {
                        error = true;
                        errorMessage += " CompanyId không có trong dữ liệu hệ thống.";
                    }
                }
            }
            else
            {
                error = true;
                errorMessage += " CompanyId không hợp lệ.";
            }
        }

        private static void CheckNgayMua(string ngayMua, ref bool error, DeviceToolAndHistory deviceToolAndHistory, ref string errorMessage)
        {
            DateTime dt;
            if (DateTime.TryParseExact(ngayMua, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
            {
                deviceToolAndHistory.BuyDate = dt.Date;
            }
            else
            {
                error = true;
                errorMessage += " NgayMua không đúng định dạng dd/MM/yyyy.";
            }
        }
        private void CheckCode(string code, ref bool error, DeviceToolAndHistory deviceToolAndHistory, ref string errorMessage)
        {
            int countExit = 0;
            using (db = new AMSEntities())
            {
                countExit = db.DeviceAndTools.Where(tc => tc.AssetsCode == code).Count();
                if (countExit == 0)
                {
                    deviceToolAndHistory.AssetsCode = code;
                }
                else
                {
                    error = true;
                    errorMessage += " AssetsCode da có trong dữ liệu hệ thống.";
                }
            }
          
        }
        private void CheckLoaiCCId(string toolId, ref bool error, DeviceToolAndHistory deviceToolAndHistory, ref string errorMessage)
        {
            int _toolId;
            if (int.TryParse(toolId, out _toolId))
            {
                using(db = new AMSEntities())
                {
                    var bod = db.ToolCategories.Where(b => b.Id == _toolId).FirstOrDefault();
                    if (bod != null)
                    {
                        deviceToolAndHistory.ToolCatId = _toolId;
                    }
                    else
                    {
                        error = true;
                        errorMessage += " ToolId không có trong dữ liệu.";
                    }
                }
                
            }
            else
            {
                error = true;
                errorMessage += " ToolId không đúng định dạng.";
            }
        }
        private void CheckLoaiDeviceId(string deviceId, ref bool error, DeviceToolAndHistory deviceToolAndHistory, ref string errorMessage)
        {
            int _toolId;
            if (int.TryParse(deviceId, out _toolId))
            {
                using (db = new AMSEntities())
                {
                    var bod = db.DeviceCategories.Where(b => b.Id == _toolId).FirstOrDefault();
                    if (bod != null)
                    {
                        deviceToolAndHistory.DeviceCatId = _toolId;
                    }
                    else
                    {
                        error = true;
                        errorMessage += " ToolId không có trong dữ liệu.";
                    }
                }

            }
            else
            {
                error = true;
                errorMessage += " ToolId không đúng định dạng.";
            }
        }
        private void CheckUserId(string nguoiSuDungId, ref bool error, DeviceToolAndHistory deviceToolAndHistory, ref string errorMessage)
        {
            int _userId;
            if (int.TryParse(nguoiSuDungId, out _userId))
            {
                using (db = new AMSEntities())
                {
                    var bod = db.UserInfoes.Where(b => b.Id == _userId).FirstOrDefault();
                    if (bod != null)
                    {
                        deviceToolAndHistory.StaffId = _userId;
                    }
                    else
                    {
                        error = true;
                        errorMessage += " userId không có trong dữ liệu.";
                    }
                }

            }
            else
            {
                error = true;
                errorMessage += " userId không đúng định dạng.";
            }
        }
        private void CheckDeptId(string phongBanSuDungId, ref bool error, DeviceToolAndHistory deviceToolAndHistory, ref string errorMessage)
        {
            int _deptId;
            if (int.TryParse(phongBanSuDungId, out _deptId))
            {
                using (db = new AMSEntities())
                {
                    var bod = db.Departments.Where(b => b.Id == _deptId).FirstOrDefault();
                    if (bod != null)
                    {
                        deviceToolAndHistory.DeptId = _deptId;
                    }
                    else
                    {
                        error = true;
                        errorMessage += " deptId không có trong dữ liệu.";
                    }
                }

            }
            else
            {
                error = true;
                errorMessage += " deptId không đúng định dạng.";
            }
        }
        private void CheckLocationId(string diaDiemId, ref bool error, DeviceToolAndHistory deviceToolAndHistory, ref string errorMessage)
        {
            int _locationId;
            if (int.TryParse(diaDiemId, out _locationId))
            {
                using (db = new AMSEntities())
                {
                    var bod = db.Locations.Where(b => b.Id == _locationId).FirstOrDefault();
                    if (bod != null)
                    {
                        deviceToolAndHistory.LocationId = _locationId;
                    }
                    else
                    {
                        error = true;
                        errorMessage += " diaDiemId không có trong dữ liệu.";
                    }
                }

            }
            else
            {
                error = true;
                errorMessage += " diaDiemId không đúng định dạng.";
            }
        }

        private static void CheckNgayBanGiao(string ngayBanGiao, ref bool error, DeviceToolAndHistory deviceToolAndHistory, ref string errorMessage)
        {
            DateTime dt;
            if (DateTime.TryParseExact(ngayBanGiao, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
            {
                deviceToolAndHistory.HandedDate = dt.Date;
            }
            else
            {
                error = true;
                errorMessage += " ngayBanGiao không đúng định dạng dd/MM/yyyy.";
            }
        }

        private void CheckStatusCategory(string statusId, ref bool error, DeviceToolAndHistory deviceToolAndHistory, ref string errorMessage)
        {
            int _statusId;
            if (int.TryParse(statusId, out _statusId))
            {
                using (db = new AMSEntities())
                {
                    var bod = db.StatusCategories.Where(b => b.Id == _statusId).FirstOrDefault();
                    if (bod != null)
                    {
                        deviceToolAndHistory.StatusId = _statusId;
                    }
                    else
                    {
                        error = true;
                        errorMessage += " StatusId không có trong dữ liệu.";
                    }
                }

            }
            else
            {
                error = true;
                errorMessage += " StatusId không đúng định dạng.";
            }
        }
    }
}