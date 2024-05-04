using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;

namespace _IQwinwin
{
    public static class crm
    {
        #region CRM
        public static bool Check_Exists_Data_Base_POS(ai Data_Base)
        {
            ai query =
                "IF EXISTS (SELECT DB_ID(N'" + Data_Base + "'))"
                + " SELECT 'true'"

                + " ELSE"
                + " SELECT 'false'"
                ;

            return query.asql.Scalar;
        }
        public static bool Check_Exists_Table_POS(ai Data_Base, ai Data_Table)
        {
            bool result = false;

            if (Check_Exists_Data_Base_POS(Data_Base))
            {
                ai query =
                    "IF EXISTS (SELECT * FROM " + Data_Base + ".INFORMATION_SCHEMA.TABLES WHERE (TABLE_NAME = N'" + Data_Table + "'))"
                    + " SELECT 'true'"

                    + " ELSE"
                    + " SELECT 'false'"
                    ;

                result = query.asql.Scalar;
            }

            return result;
        }

        public static void Read_User_Phan_Quyen(ai UserName, out ai Ho_Va_Ten, out ai Phong_Ban, out bool Result_Duoc_Xem_Bao_Cao, out bool Result_Duoc_Tich_Diem, out bool Result_Duoc_Tru_Diem, out bool Result_Duoc_Doi_Diem)
        {
            a.run(UserName.Lower);

            Ho_Va_Ten = "Garden";
            Phong_Ban = a.e;

            Result_Duoc_Xem_Bao_Cao = false;
            Result_Duoc_Tich_Diem = false;
            Result_Duoc_Tru_Diem = false;
            Result_Duoc_Doi_Diem = false;

            //Test
            if (sys.Request.IsLocal)
            {
                Ho_Va_Ten = "Ngô Vương Quyền";
                Phong_Ban = "IT";

                Result_Duoc_Xem_Bao_Cao = true;
                Result_Duoc_Tich_Diem = true;
                Result_Duoc_Tru_Diem = true;
                Result_Duoc_Doi_Diem = true;
            }
            else
            {
                bool mustUpdate = false;
                afile file = apath.Add(sys.Root_Folder, @"File\Excel\Crm\User-Phan-Quyen.xlsx");

                DateTime LastTime = a.Time;
                if (sys.VarApp["LastTime_Of_User-Phan-Quyen"] != null)
                {
                    LastTime = (DateTime)sys.VarApp["LastTime_Of_User-Phan-Quyen"];
                }

                if (file.Modified.Diff(LastTime))
                {
                    mustUpdate = true;
                    sys.VarApp["LastTime_Of_User-Phan-Quyen"] = LastTime;
                }

                if (
                    !mustUpdate
                    && (sys.VarApp["Ho_Va_Ten_Of_" + UserName] != null)
                    && (sys.VarApp["Phong_Ban_Of_" + UserName] != null)

                    && (sys.VarApp["Duoc_Xem_Bao_Cao_Of_" + UserName] != null)
                    && (sys.VarApp["Duoc_Tich_Diem_Of_" + UserName] != null)
                    && (sys.VarApp["Duoc_Tru_Diem_Of_" + UserName] != null)
                    && (sys.VarApp["Duoc_Doi_Diem_Of_" + UserName] != null)
                    )
                {
                    Ho_Va_Ten = sys.VarApp["Ho_Va_Ten_Of_" + UserName].aS();
                    Phong_Ban = sys.VarApp["Phong_Ban_Of_" + UserName].aS();

                    Result_Duoc_Xem_Bao_Cao = (bool)sys.VarApp["Duoc_Xem_Bao_Cao_Of_" + UserName];
                    Result_Duoc_Tich_Diem = (bool)sys.VarApp["Duoc_Tich_Diem_Of_" + UserName];
                    Result_Duoc_Tru_Diem = (bool)sys.VarApp["Duoc_Tru_Diem_Of_" + UserName];
                    Result_Duoc_Doi_Diem = (bool)sys.VarApp["Duoc_Doi_Diem_Of_" + UserName];
                }
                else
                {
                    ExcelPackage excel = new ExcelPackage(file);
                    var sheet = excel.Workbook.Worksheets[1];

                    for (int i1 = 8; i1 <= sheet.Dimension.End.Row; i1++)
                    {
                        ai Ten_Dang_Nhap = a.aS(sheet.Cells[i1, 3].Value);

                        if (Ten_Dang_Nhap == UserName)
                        {
                            Ho_Va_Ten = a.aS(sheet.Cells[i1, 2].Value);
                            Phong_Ban = a.aS(sheet.Cells[i1, 1].Value);

                            ai Duoc_Xem_Bao_Cao = a.aS(sheet.Cells[i1, 4].Value);
                            ai Duoc_Tich_Diem = a.aS(sheet.Cells[i1, 5].Value);
                            ai Duoc_Tru_Diem = a.aS(sheet.Cells[i1, 6].Value);
                            ai Duoc_Doi_Diem = a.aS(sheet.Cells[i1, 7].Value);

                            Ho_Va_Ten.aRemSpace();
                            sys.VarApp["Ho_Va_Ten_Of_" + UserName] = Ho_Va_Ten;

                            Duoc_Xem_Bao_Cao.aRemSpace();
                            if (Duoc_Xem_Bao_Cao == "x")
                            {
                                Result_Duoc_Xem_Bao_Cao = true;
                                sys.VarApp["Duoc_Xem_Bao_Cao_Of_" + UserName] = true;
                            }
                            else
                                sys.VarApp["Duoc_Xem_Bao_Cao_Of_" + UserName] = false;

                            Duoc_Tich_Diem.aRemSpace();
                            if (Duoc_Tich_Diem == "x")
                            {
                                Result_Duoc_Tich_Diem = true;
                                sys.VarApp["Duoc_Tich_Diem_Of_" + UserName] = true;
                            }
                            else
                                sys.VarApp["Duoc_Tich_Diem_Of_" + UserName] = false;

                            Duoc_Tru_Diem.aRemSpace();
                            if (Duoc_Tru_Diem == "x")
                            {
                                Result_Duoc_Tru_Diem = true;
                                sys.VarApp["Duoc_Tru_Diem_Of_" + UserName] = true;
                            }
                            else
                                sys.VarApp["Duoc_Tru_Diem_Of_" + UserName] = false;

                            Duoc_Doi_Diem.aRemSpace();
                            if (Duoc_Doi_Diem == "x")
                            {
                                Result_Duoc_Doi_Diem = true;
                                sys.VarApp["Duoc_Doi_Diem_Of_" + UserName] = true;
                            }
                            else
                                sys.VarApp["Duoc_Doi_Diem_Of_" + UserName] = false;

                            break;
                        }
                    }
                }
            }
        }
        public static void Read_Iam(int Try_Times, out ai Computer, out ai Domain, out ai UserName, out ai QuayBan, out ai CaBan, out ai NVBan)
        {
            Try_Times++;

            Computer = a.e;
            Domain = a.e;
            UserName = a.e;

            QuayBan = a.e;
            CaBan = a.e;
            NVBan = a.e;

            ai IP = sys.IP;

            if (
                (sys.VarApp["Computer_Of_" + IP] != null)
                && (sys.VarApp["Domain_Of_" + IP] != null)
                && (sys.VarApp["UserName_Of_" + IP] != null)
                && (sys.VarApp["QuayBan_Of_" + IP] != null)
                && (sys.VarApp["CaBan_Of_" + IP] != null)
                && (sys.VarApp["NVBan_Of_" + IP] != null)
                )
            {
                Computer = sys.VarApp["Computer_Of_" + IP].aS();
                Domain = sys.VarApp["Domain_Of_" + IP].aS();
                UserName = sys.VarApp["UserName_Of_" + IP].aS();

                QuayBan = sys.VarApp["QuayBan_Of_" + IP].aS();
                CaBan = sys.VarApp["CaBan_Of_" + IP].aS();
                NVBan = sys.VarApp["NVBan_Of_" + IP].aS();
            }
            else
            {
                if (Try_Times <= 30)
                {
                    Thread.Sleep(1000 * 1);
                    Read_Iam(Try_Times, out Computer, out Domain, out UserName, out QuayBan, out CaBan, out NVBan);
                }
            }
        }

        public static ai Read_Cashier_Code(ai Cashier_UserName)
        {
            ai query =
                " SELECT TOP 1 MaNV"
                + " FROM TOPOS_DB.DBO.NhanVien"
                + " WHERE (TenDangNhap LIKE @Cashier_UserName)"
                ;

            return query.asql(new nv("Cashier_UserName", Cashier_UserName)).Scalar;
        }

        public static ai Submit_Switch_Point(ai Card_1, ai Card_2)
        {
            ai Sucessfull = "0";

            bool Valid = true;

            Card_1.aRemSpace();
            Card_2.aRemSpace();

            if (
                !Card_1.CheckLength(11, 11)
                || !Card_2.CheckLength(11, 11)

                || Card_1.NoID
                || Card_2.NoID
                )
            {
                Valid = false;
            }

            if (Valid)
            {
                ai IP = sys.IP;

                ai UserName = lib.Read_UserName();

                ai Ho_Va_Ten = a.e;
                ai Phong_Ban = a.e;

                bool Duoc_Xem_Bao_Cao = false;
                bool Duoc_Tich_Diem = false;
                bool Duoc_Tru_Diem = false;
                bool Duoc_Doi_Diem = false;

                Read_User_Phan_Quyen(UserName, out Ho_Va_Ten, out Phong_Ban, out Duoc_Xem_Bao_Cao, out Duoc_Tich_Diem, out Duoc_Tru_Diem, out Duoc_Doi_Diem);

                ai Computer = a.e;
                ai Domain = a.e;
                ai UserName_Services = a.e;

                ai QuayBan = a.e;
                ai CaBan = a.e;
                ai NVBan = a.e;

                Read_Iam(31, out Computer, out Domain, out UserName_Services, out QuayBan, out CaBan, out NVBan);

                if (QuayBan == a.e)
                {
                    if (Computer != a.e)
                    {
                        QuayBan = Computer;
                    }
                    else
                    {
                        QuayBan = IP;
                    }
                }

                NVBan = UserName;

                ai Add_At = QuayBan;
                ai Add_By = NVBan;
                ai Add_Time = a.Time;

                ai Card_1_NEW = "-" + Card_1;

                ai query =

                    " DECLARE @Mem_SEQ_1 NVARCHAR(MAX)"
                    + " DECLARE @Mem_SEQ_2 NVARCHAR(MAX)"

                    + " SET @Mem_SEQ_1 = (SELECT TOP 1 Mem_SEQ FROM GARDEN_CRM.DBO.T_MEM_MST WHERE (Mem_Card = @Card_1))"
                    + " SET @Mem_SEQ_2 = (SELECT TOP 1 Mem_SEQ FROM GARDEN_CRM.DBO.T_MEM_MST WHERE (Mem_Card = @Card_2))"

                    //Sale
                    + " UPDATE GARDEN_CRM.DBO.T_SALE_INFO"
                    + " SET Mem_SEQ_Old = @Mem_SEQ_1, Mem_SEQ = @Mem_SEQ_2, Mem_SEQ_Edit_At = @Add_At, Mem_SEQ_Edit_By = @Add_By, Mem_SEQ_Edit_Time = @Add_Time"
                    + " WHERE (Mem_SEQ = @Mem_SEQ_1)"

                    //Point
                    + " UPDATE GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                    + " SET Mem_SEQ_Old = @Mem_SEQ_1, Mem_SEQ = @Mem_SEQ_2, Mem_SEQ_Edit_At = @Add_At, Mem_SEQ_Edit_By = @Add_By, Mem_SEQ_Edit_Time = @Add_Time"
                    + " WHERE (Mem_SEQ = @Mem_SEQ_1)"

                    //Mem-Card
                    + " UPDATE GARDEN_CRM.DBO.T_MEM_MST"
                    + " SET Mem_Card_Old = @Card_1, Mem_Card = @Card_1_NEW, Mem_Card_Edit_At = @Add_At, Mem_Card_Edit_By = @Add_By, Mem_Card_Edit_Time = @Add_Time"
                    + " WHERE (Mem_SEQ = @Mem_SEQ_1)"

                    //1
                    + " DELETE FROM GARDEN_CRM.DBO.T_MEM_NOW_PNT WHERE (MEM_SEQ = @Mem_SEQ_1)"

                    //2
                    + " DECLARE @Total_Point_2 NVARCHAR(MAX)"
                    + " SET @Total_Point_2 = (SELECT SUM(CHG_PNT) FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO WHERE (MEM_SEQ = @Mem_SEQ_2))"

                    + " IF EXISTS (SELECT * FROM GARDEN_CRM.DBO.T_MEM_NOW_PNT WHERE (MEM_SEQ = @Mem_SEQ_2))"
                    + "     UPDATE GARDEN_CRM.DBO.T_MEM_NOW_PNT SET NOW_TOT_PNT = @Total_Point_2 WHERE (MEM_SEQ = @Mem_SEQ_2)"
                    + " ELSE"
                    + "     INSERT INTO GARDEN_CRM.DBO.T_MEM_NOW_PNT (MEM_SEQ, NOW_TOT_PNT) VALUES (@Mem_SEQ_2, @Total_Point_2)"
                    ;

                query.asql(
                    new nv("Card_1", Card_1),
                    new nv("Card_2", Card_2),
                    new nv("Card_1_NEW", Card_1_NEW),
                    new nv("Add_At", Add_At),
                    new nv("Add_By", Add_By),
                    new nv("Add_Time", Add_Time)
                    ).NonQuery();

                Sucessfull = "1";
            }

            //
            return Sucessfull;
        }
        public static ai Submit_Switch_Multi_Point(ai Card_1_List, ai Card_2)
        {
            ai Sucessfull = "1";

            foreach (ai card1 in Card_1_List.aray)
            {
                if (card1 != Card_2)
                    Submit_Switch_Point(card1, Card_2);
            }

            return Sucessfull;
        }

        public static void Read_allShopCode(aray allShopCode, aray allShopName)
        {
            ai query = " SELECT SoHopDong, TenGianHang FROM TOPOS_DB.dbo.ThueGianHang";

            asql sql = query.asql();
            sql.DataReader();

            try
            {
                while (sql.Data.Read())
                {
                    ai SoHopDong = sql.Data["SoHopDong"].aS();
                    ai TenGianHang = sql.Data["TenGianHang"].aS();

                    SoHopDong.aRemSpace().aRemFrom_Text_End(a.Space);
                    TenGianHang.aRemSpace();

                    if (TenGianHang.NoEmpty && SoHopDong.NoEmpty)
                    {
                        allShopCode.AddAny(SoHopDong);
                        allShopName.AddAny(TenGianHang);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
            }
            finally
            {
                sql.Close();
            }

            sys.VarApp["allShopCode"] = allShopCode;
            sys.VarApp["allShopName"] = allShopName;
        }

        public static ai Creat_Search_Name_List(ai NameOrPhone)
        {
            NameOrPhone.aRemSpace();

            ai query =
                " SELECT TOP 10 Mem_Nm AS Name, MOBILE_NO AS Phone, Mem_Card AS Card"
                + " FROM GARDEN_CRM.DBO.T_MEM_MST"

                + " WHERE ("
                + " (Mem_Card NOT LIKE '0107%') AND (Mem_Card NOT LIKE '07%') AND (Mem_Card NOT LIKE '0108%') AND (Mem_Card NOT LIKE '08%') AND (Mem_Card NOT LIKE '-%')"
                + " AND ((Mem_Nm LIKE '%' + @Search_Name_OR_Phone + '%') OR (REPLACE(MOBILE_NO, '-', '') LIKE '%' + @Search_Name_OR_Phone + '%') OR (REPLACE(CERTI_NO, '-', '') LIKE '%' + @Search_Name_OR_Phone + '%'))"
                + ")"
                ;

            asql sql = query.asql(new nv("Search_Name_OR_Phone", NameOrPhone));
            sql.DataReader();

            return sql.ToStringJson();
        }
        public static ai Creat_Shop_List(ai NameOrCode)
        {
            ai sqlWhere = a.e;

            NameOrCode.aRemSpace();
            if (NameOrCode.NoZero)
                sqlWhere = " AND ((SoHopDong LIKE '%' + @Shop_Name_OR_Code + '%') OR (TenGianHang LIKE '%' + @Shop_Name_OR_Code + '%'))";

            ai query =
                "SELECT TOP 10 MaThue, SoHopDong, TenGianHang, Tich_Diem"
                + " FROM TOPOS_DB.DBO.ThueGianHang"

                + " WHERE ((Hien_Thi = '1') AND (TenGianHang NOT IN ('0')) AND (TenGianHang IS NOT NULL) AND (TenGianHang NOT LIKE '') AND (NgayKetThuc >= GETDATE())" + sqlWhere + ")"// AND (NgayKetThuc >= GETDATE())
                + " ORDER BY TenGianHang";

            asql sql = query.asql(new nv("Shop_Name_OR_Code", NameOrCode));
            sql.DataReader();

            return sql.ToStringJson();
        }

        public static ai Read_Receipt_Info(ai Receipt)
        {
            bool Have_Result = false;
            ai Result = a.e;

            //
            ai MaQuay = a.e;
            ai MaNV = a.e;

            ai MaThue = a.e;
            ai SoHopDong = a.e;
            ai TenGianHang = a.e;
            ai Tich_Diem = a.e;

            ai NgayBatDau = a.e;
            int GioBatDau = 0;

            Int64 Money = 0;
            int Point = 0;

            ai Buy_Time_Day = a.e;
            ai Buy_Time_Month = a.e;
            ai Buy_Time_Year = a.e;

            //
            Receipt.aRemSpace();

            if (Receipt.Length == 16)
            {
                char[] Receipt_Array = Receipt.ToCharArray();

                //
                ai Data_Table_Hoa_Don = "HoaDon" + Receipt_Array[8].aS() + Receipt_Array[9].aS() + "20" + Receipt_Array[10].aS() + Receipt_Array[11].aS();

                //
                if (Check_Exists_Table_POS("TOPOS_DB", Data_Table_Hoa_Don))
                {
                    ai query =
                        "SELECT MaQuay, MaNV, Shop_Alias.MaThue, SoHopDong, TenGianHang, Tich_Diem, NgayBatDau, GioBatDau, SUM(ThanhTienBan) AS Money"
                        + " FROM TOPOS_DB.DBO.ThueGianHang Shop_Alias"

                        + " JOIN"
                        + " ("

                        + " SELECT MaQuay, MaNV, MaThue, NgayBatDau, GioBatDau, TTHD_Alias.ThanhTien AS ThanhTienBan"
                        + " FROM TOPOS_DB.DBO." + Data_Table_Hoa_Don + " HD_Alias"

                        + " JOIN TOPOS_DB.DBO.ThanhToan" + Data_Table_Hoa_Don + " TTHD_Alias"
                        + " ON HD_Alias.MaHD = TTHD_Alias.MaHD"

                        + " WHERE (ID = @Receipt) AND (TTHD_Alias.ThanhTien > 0) AND ((MaHinhThuc = '0001') OR (MaHinhThuc = '0013') OR (MaHinhThuc = '0031') OR (MaNhomThanhToan = '003') OR ((MaHinhThuc IS NULL) AND (MaNhomThanhToan IS NULL)))"

                        + " ) AS Hoa_Don_Alias"
                        + " ON Hoa_Don_Alias.MaThue = Shop_Alias.MaThue"

                        + " WHERE ((Shop_Alias.Hien_Thi = '1') AND (Shop_Alias.TenGianHang NOT IN ('0')) AND (Shop_Alias.TenGianHang IS NOT NULL) AND (Shop_Alias.TenGianHang NOT LIKE ''))"

                        + " GROUP BY MaQuay, MaNV, Shop_Alias.MaThue, SoHopDong, TenGianHang, Tich_Diem, NgayBatDau, GioBatDau"
                        ;

                    //
                    asql sql = query.asql(new nv("Receipt", Receipt));
                    sql.DataReader();

                    try
                    {
                        if (sql.Data.Read())
                        {
                            Have_Result = true;

                            MaQuay = sql.Data["MaQuay"].aS();
                            MaNV = sql.Data["MaNV"].aS();

                            MaThue = sql.Data["MaThue"].aS();
                            SoHopDong = sql.Data["SoHopDong"].aS();
                            TenGianHang = sql.Data["TenGianHang"].aS();
                            Tich_Diem = sql.Data["Tich_Diem"].abool().ToBit;

                            NgayBatDau = sql.Data["NgayBatDau"].aS();
                            //GioBatDau = Convert_String_To_Int(sql.Data["GioBatDau"].aS(), 0);

                            ai Money_Temp = sql.Data["Money"].ai().aRemFrom_Text_End(".");
                            Money = Convert_String_To_BigInt(Money_Temp, 0);
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                    }
                    finally
                    {
                        sql.Close();
                    }

                    //
                    NgayBatDau.aRemFrom_Text_End(a.Space);

                    string[] NgayBatDau_Array = NgayBatDau.Split('/');

                    if (NgayBatDau_Array.Length == 3)
                    {
                        Buy_Time_Day = NgayBatDau_Array[0];
                        Buy_Time_Month = NgayBatDau_Array[1];
                        Buy_Time_Year = NgayBatDau_Array[2];
                    }

                    //
                    if (Tich_Diem == "1")
                    {
                        Point = Convert.ToInt32(Money / 15000);
                    }
                }
            }

            //
            if (Have_Result)
            {
                string[] MaQuay_Array = new string[] { MaQuay };
                string[] MaNV_Array = new string[] { MaNV };

                string[] MaThue_Array = new string[] { MaThue };
                string[] Shop_AND_Code_Array = new string[] { TenGianHang + " (" + SoHopDong + ")" };
                string[] Tich_Diem_Array = new string[] { Tich_Diem };

                string[] Buy_Time_Day_Array = new string[] { Buy_Time_Day };
                string[] Buy_Time_Month_Array = new string[] { Buy_Time_Month };
                string[] Buy_Time_Year_Array = new string[] { Buy_Time_Year };

                string[] Money_Array = new string[] { Split_Thousand(Money.aS()) };
                string[] Point_Array = new string[] { Point.aS() };

                //
                Result = Creat_JSON_From_List_10_Item(MaQuay_Array, MaNV_Array,
                    MaThue_Array, Shop_AND_Code_Array, Tich_Diem_Array,
                    Buy_Time_Day_Array, Buy_Time_Month_Array, Buy_Time_Year_Array,
                    Money_Array, Point_Array
                    );
            }

            //
            return Result;
        }
        public static ai Read_Card_Info(ai Card)
        {
            ai Result = a.e;
            ai Card_Info = a.e;
            ai Current_Point = a.e;

            //
            Card.aRemSpace();

            //
            if ((Card.Length == 11) && Card.IsID && (!Card.StartsWith("0107")))
            {
                ai query =

                    " DECLARE @MEM_SEQ NVARCHAR(MAX)"
                    + " SET @MEM_SEQ = (SELECT TOP 1 MEM_SEQ FROM GARDEN_CRM.DBO.T_MEM_MST WHERE (Mem_Card = @Card))"

                    + " IF @MEM_SEQ IS NOT NULL"
                    + " BEGIN"

                    + "     DECLARE @Total_Point BIGINT"
                    + "     SET @Total_Point = (SELECT SUM(CHG_PNT) FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO WHERE (MEM_SEQ = @MEM_SEQ))"

                    //
                    + "     IF EXISTS (SELECT * FROM GARDEN_CRM.DBO.T_MEM_NOW_PNT WHERE (MEM_SEQ = @MEM_SEQ))"
                    + "         UPDATE GARDEN_CRM.DBO.T_MEM_NOW_PNT SET NOW_TOT_PNT = @Total_Point WHERE (MEM_SEQ = @MEM_SEQ)"
                    + "     ELSE"
                    + "         INSERT INTO GARDEN_CRM.DBO.T_MEM_NOW_PNT (MEM_SEQ, NOW_TOT_PNT) VALUES (@MEM_SEQ, @Total_Point)"

                    //
                    + "     SELECT Mem_Nm AS Name, MEM_BIRTHDAY AS Birthday, YEAR(GETDATE()) - YEAR(Mem_birthday) AS Age, Sex_CD AS Sex, MOBILE_NO AS Phone, E_MAIL AS Email, CERTI_NO as ID, MEM_ADDR AS Address, NOW_TOT_PNT AS Current_Point"
                    + "     FROM GARDEN_CRM.DBO.T_MEM_MST Member_Alias"

                    + "     LEFT JOIN GARDEN_CRM.DBO.T_MEM_NOW_PNT Member_Point_NOW_Alias"
                    + "     ON Member_Alias.MEM_SEQ = Member_Point_NOW_Alias.MEM_SEQ"

                    + "     WHERE (Member_Alias.MEM_SEQ = @MEM_SEQ)"
                    + " END"
                    ;

                asql sql = query.asql(new nv("Card", Card));
                sql.DataReader();

                try
                {
                    if (sql.Data.Read())
                    {
                        ai Name = sql.Data["Name"].aS();
                        ai Birthday = sql.Data["Birthday"].aS();
                        ai Age = sql.Data["Age"].aS();
                        ai Sex = sql.Data["Sex"].aS();
                        ai Phone = sql.Data["Phone"].aS();
                        ai Email = sql.Data["Email"].aS();
                        ai ID = sql.Data["ID"].aS();
                        ai Address = sql.Data["Address"].aS();
                        Current_Point = sql.Data["Current_Point"].ai().SplitThousand;

                        if (Current_Point == a.e)
                        {
                            Current_Point = "0";
                        }

                        string[] Birthday_Array = Birthday.Split('/');

                        if (Birthday_Array.Length == 3)
                        {
                            Birthday = Birthday_Array[2] + "-" + Birthday_Array[1] + "-" + Birthday_Array[0];
                        }

                        if (Sex.ToLower() == "f")
                        {
                            Sex = "Nữ";
                        }
                        else
                        {
                            Sex = "Nam";
                        }

                        Card_Info =
                            " - Khách hàng: <span class='Bold_Red_Text_css'>" + Name + "</span><br/>"
                            + " - Tổng điểm hiện tại: <span class='Bold_Red_Text_css'>" + Current_Point + "</span><br/>"
                            + " - Ngày sinh: <span class='Bold_Red_Text_css'>" + Birthday + "</span><br/>"
                            + " - Tuổi: <span class='Bold_Red_Text_css'>" + Age + "</span>"
                            + " - Giới tính: <span class='Bold_Red_Text_css'>" + Sex + "</span><br/>"
                            + " - Phone: <span class='Bold_Red_Text_css'>" + Phone + "</span><br/>"
                            + " - Email: <span class='Bold_Red_Text_css'>" + Email + "</span><br/>"
                            + " - CMND/Hộ chiếu: <span class='Bold_Red_Text_css'>" + ID + "</span><br/>"
                            + " - Đ/C: <span class='Bold_Red_Text_css'>" + Address + "</span>"
                            ;
                    }
                }
                catch (SqlException sqlEx)
                {
                }
                finally
                {
                    sql.Close();
                }
            }

            //
            if (Card_Info != a.e)
            {
                string[] Card_Info_Array = new string[] { Card_Info };
                string[] Current_Point_Array = new string[] { Current_Point };

                //
                Result = Creat_JSON_From_List_2_Item(Card_Info_Array, Current_Point_Array);
            }

            //
            return Result;
        }

        public static ai Submit_Add_Point(ai Submit_Add_Point_JSON)
        {
            //ai UserId = Read_UserId(a.e);

            ai Sucessfull = "0";

            try
            {
                //
                //if (Check_GUID(UserId))
                {
                    bool Valid = true;

                    //
                    List_7_Item Submit_Add_Point_obj = Convert_JSON_7(Submit_Add_Point_JSON);

                    //            
                    ai Card = Get_Input_String_From_Encoded_HTML_To_SQL(Submit_Add_Point_obj.Item_1, false);
                    ai Receipt = Get_Input_String_From_Encoded_HTML_To_SQL(Submit_Add_Point_obj.Item_2, false);
                    ai MaThue = Get_Input_String_From_Encoded_HTML_To_SQL(Submit_Add_Point_obj.Item_3, false);
                    ai Buy_Time = Get_Input_String_From_Encoded_HTML_To_SQL(Submit_Add_Point_obj.Item_4, false);
                    ai Money = Get_Input_String_From_Encoded_HTML_To_SQL(Submit_Add_Point_obj.Item_5, false);
                    ai Point = Get_Input_String_From_Encoded_HTML_To_SQL(Submit_Add_Point_obj.Item_6, false);
                    ai Reason = Get_Input_String_From_Encoded_HTML_To_SQL(Submit_Add_Point_obj.Item_7, false);

                    Card.aRemSpace();
                    Receipt = Remove_Space_String(Receipt);
                    MaThue = Remove_Space_String(MaThue);
                    Buy_Time = Remove_Space_String(Buy_Time);
                    Money = Remove_Space_String(Money);
                    Point = Remove_Space_String(Point);
                    Reason = Remove_Space_String(Reason);

                    Money = Money.Replace(".", a.e).Replace(",", a.e);
                    Point = Point.Replace(".", a.e).Replace(",", a.e);

                    //
                    if (
                        (!Card.CheckLength(11, 11))
                        || (!Card.IsID)
                        || (Card.StartsWith("0107"))

                        || (!Money.IsID)
                        || (!Point.IsID)
                        )
                    {
                        Valid = false;
                    }

                    //
                    if (Reason == "Add")
                    {
                        if (
                            (Receipt.IsEmpty)
                            || (MaThue.IsEmpty)
                            || (Buy_Time.IsEmpty)
                           )
                        {
                            Valid = false;
                        }
                    }

                    //
                    if (Valid)
                    {
                        ai IP = sys.Context.Request.UserHostAddress;

                        //UserName
                        ai UserName = lib.Read_UserName();

                        ai Ho_Va_Ten = a.e;
                        ai Phong_Ban = a.e;

                        bool Duoc_Xem_Bao_Cao = false;
                        bool Duoc_Tich_Diem = false;
                        bool Duoc_Tru_Diem = false;
                        bool Duoc_Doi_Diem = false;

                        Read_User_Phan_Quyen(UserName, out Ho_Va_Ten, out Phong_Ban, out Duoc_Xem_Bao_Cao, out Duoc_Tich_Diem, out Duoc_Tru_Diem, out Duoc_Doi_Diem);

                        //
                        ai Computer = a.e;
                        ai Domain = a.e;
                        ai UserName_Services = a.e;

                        ai QuayBan = a.e;
                        ai CaBan = a.e;
                        ai NVBan = a.e;

                        Read_Iam(31, out Computer, out Domain, out UserName_Services, out QuayBan, out CaBan, out NVBan);

                        if (QuayBan == a.e)
                        {
                            if (Computer != a.e)
                            {
                                QuayBan = Computer;
                            }
                            else
                            {
                                QuayBan = IP;
                            }
                        }

                        if ((UserName.ToLower() == "pos2") || (UserName_Services.ToLower() == "pos2"))
                        {
                            if (NVBan == a.e)
                            {
                                if (UserName_Services != a.e)
                                {
                                    NVBan = UserName_Services;
                                }
                                else
                                {
                                    NVBan = UserName;
                                }
                            }
                        }
                        else
                        {
                            NVBan = UserName;
                        }

                        //
                        ai Add_At = QuayBan;
                        ai Add_By = NVBan;
                        ai Add_Time = a.Time;

                        ai Name = a.e;
                        ai Total_Point = a.e;

                        //
                        if ((UserName.ToLower() == "pos2") || (UserName_Services.ToLower() == "pos2"))
                        {
                            ai Cashier_Code = Read_Cashier_Code(NVBan);

                            if (Cashier_Code != a.e)
                            {
                                Add_By = Cashier_Code;
                            }
                        }

                        //
                        ai query =

                            " DECLARE @Sale_SEQ NVARCHAR(MAX)"

                            + " DECLARE @Mem_SEQ NVARCHAR(MAX)"
                            + " SET @Mem_SEQ = (SELECT TOP 1 Mem_SEQ FROM GARDEN_CRM.DBO.T_MEM_MST WHERE (Mem_Card = @Card))"

                            + " DECLARE @Add_Time NVARCHAR(MAX)"
                            + " SET @Add_Time = CONVERT(DATETIME, @Add_Time_Temp, 103)"

                            + " DECLARE @Name NVARCHAR(MAX)"
                            + " DECLARE @Total_Point BIGINT"

                            //
                            //+ " DELETE FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                            //+ " WHERE SALE_SEQ IN (SELECT SALE_SEQ FROM GARDEN_CRM.DBO.T_SALE_INFO WHERE (MEM_SEQ IS NULL) OR (MEM_SEQ = '0'))"

                            //+ " DELETE FROM GARDEN_CRM.DBO.T_SALE_INFO"
                            //+ " WHERE (MEM_SEQ IS NULL) OR (MEM_SEQ = '0')"

                            + " UPDATE GARDEN_CRM.DBO.T_SALE_INFO"
                            + " SET RECP_NO_Old = RECP_NO, RECP_NO = NULL"
                            + " WHERE (MEM_SEQ IS NULL) OR (MEM_SEQ = '0')"
                            ;

                        //
                        if (Reason == "Add")
                        {
                            query +=

                                " DECLARE @Buy_Time NVARCHAR(MAX)"
                                + " SET @Buy_Time = CONVERT(DATETIME, @Buy_Time_Temp, 103)"

                                + " DECLARE @Buy_Date NVARCHAR(MAX)"
                                + " SET @Buy_Date = CONVERT(DATE, @Buy_Time_Temp, 103)"
                                + " SET @Buy_Date = REPLACE(@Buy_Date, '-', '/')"

                                + " IF NOT EXISTS (SELECT * FROM GARDEN_CRM.DBO.T_SALE_INFO WHERE ((RECP_NO = @Receipt) AND (BIZ_DAY = @Buy_Date) AND ((MaThue = @MaThue) OR (BRD_CD IS NOT NULL))))"
                                + " BEGIN"

                                + "     INSERT INTO GARDEN_CRM.DBO.T_SALE_INFO"
                                + "     (MEM_SEQ, RECP_NO, MaThue, BIZ_DAY, SALE_DT, PAY_AMT, ADD_PNT, POS_NO, CASHER_NO, REG_ID, REG_DT, MOD_ID, MOD_DT, Is_Manual_Add_Point)"
                                + "     VALUES"
                                + "     (@MEM_SEQ, @Receipt, @MaThue, @Buy_Date, @Buy_Time, @Money, @Point, @Add_At, @Add_By, @Add_By, @Add_Time, @Add_By, @Add_Time, @Is_Manual_Add_Point)"

                                + "     SET @Sale_SEQ = (SELECT SCOPE_IDENTITY());"

                                //
                                + "     INSERT INTO GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                                + "     (MEM_SEQ, CHG_PNT, PNT_RSN, SALE_SEQ, Add_At, Add_By_Group, REG_ID, REG_DT, MOD_ID, MOD_DT, Is_Manual_Add_Point)"
                                + "     VALUES"
                                + "     (@MEM_SEQ, @Point, @Reason, @Sale_SEQ, @Add_At, @Add_By_Group, @Add_By, @Add_Time, @Add_By, @Add_Time, @Is_Manual_Add_Point)"

                                + " END"
                                ;
                        }
                        else
                            if (Reason == "Mistake")
                        {
                            query +=

                                " SET @Total_Point = (SELECT SUM(CHG_PNT) FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO WHERE (MEM_SEQ = @MEM_SEQ))"
                                + " IF (@Point <= @Total_Point)"
                                + " BEGIN"
                                + "     DECLARE @Buy_Time NVARCHAR(MAX)"
                                + "     SET @Buy_Time = CONVERT(DATETIME, @Buy_Time_Temp, 103)"

                                + "     DECLARE @Buy_Date NVARCHAR(MAX)"
                                + "     SET @Buy_Date = CONVERT(DATE, @Buy_Time_Temp, 103)"
                                + "     SET @Buy_Date = REPLACE(@Buy_Date, '-', '/')"

                                + "     SET @Sale_SEQ = (SELECT TOP 1 Sale_SEQ FROM GARDEN_CRM.DBO.T_SALE_INFO WHERE ((RECP_NO = @Receipt) AND (BIZ_DAY = @Buy_Date) AND ((MaThue = @MaThue) OR (BRD_CD IS NOT NULL))))"

                                + "     IF @Sale_SEQ IS NULL"
                                + "     BEGIN"
                                + "         INSERT INTO GARDEN_CRM.DBO.T_SALE_INFO"
                                + "         (MEM_SEQ, RECP_NO, MaThue, BIZ_DAY, SALE_DT, PAY_AMT, ADD_PNT, POS_NO, CASHER_NO, REG_ID, REG_DT, MOD_ID, MOD_DT, Is_Manual_Add_Point)"
                                + "         VALUES"
                                + "         (@MEM_SEQ, @Receipt, @MaThue, @Buy_Date, @Buy_Time, @Money, @Point, @Add_At, @Add_By, @Add_By, @Add_Time, @Add_By, @Add_Time, @Is_Manual_Add_Point)"

                                + "         SET @Sale_SEQ = (SELECT SCOPE_IDENTITY());"
                                + "     END"

                                + "     INSERT INTO GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                                + "     (MEM_SEQ, CHG_PNT, PNT_RSN, SALE_SEQ, Add_At, Add_By_Group, REG_ID, REG_DT, MOD_ID, MOD_DT, Is_Manual_Add_Point)"
                                + "     VALUES"
                                + "     (@MEM_SEQ, '-' + CONVERT(NVARCHAR(MAX), @Point), @Reason, @Sale_SEQ, @Add_At, @Add_By_Group, @Add_By, @Add_Time, @Add_By, @Add_Time, @Is_Manual_Add_Point)"
                                + " END"
                                ;
                        }
                        else
                        if (Reason.StartsWith("Minus"))
                        {
                            query +=
                                " SET @Total_Point = (SELECT SUM(CHG_PNT) FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO WHERE (MEM_SEQ = @MEM_SEQ))"

                                + " IF (@Point <= @Total_Point)"
                                + "     INSERT INTO GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                                + "     (MEM_SEQ, CHG_PNT, PNT_RSN, SALE_SEQ, Add_At, Add_By_Group, REG_ID, REG_DT, MOD_ID, MOD_DT, Is_Manual_Add_Point)"
                                + "     VALUES"
                                + "     (@MEM_SEQ, '-' + CONVERT(NVARCHAR(MAX), @Point), @Reason, @Sale_SEQ, @Add_At, @Add_By_Group, @Add_By, @Add_Time, @Add_By, @Add_Time, @Is_Manual_Add_Point)"
                                ;
                        }
                        else
                        if (Reason.StartsWith("Reward"))
                        {
                            query +=
                                "     INSERT INTO GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                                + "     (MEM_SEQ, CHG_PNT, PNT_RSN, SALE_SEQ, Add_At, Add_By_Group, REG_ID, REG_DT, MOD_ID, MOD_DT, Is_Manual_Add_Point)"
                                + "     VALUES"
                                + "     (@MEM_SEQ, @Point, @Reason, @Sale_SEQ, @Add_At, @Add_By_Group, @Add_By, @Add_Time, @Add_By, @Add_Time, @Is_Manual_Add_Point)"
                                ;
                        }
                        else
                        if (Reason == "Redeem")
                        {
                            query +=
                                " SET @Total_Point = (SELECT SUM(CHG_PNT) FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO WHERE (MEM_SEQ = @MEM_SEQ))"

                                + " IF (@Point <= @Total_Point)"
                                + "     INSERT INTO GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                                + "     (MEM_SEQ, CHG_PNT, PNT_RSN, SALE_SEQ, Add_At, Add_By_Group, REG_ID, REG_DT, MOD_ID, MOD_DT, Is_Manual_Add_Point)"
                                + "     VALUES"
                                + "     (@MEM_SEQ, '-' + CONVERT(NVARCHAR(MAX), @Point), @Reason, @Sale_SEQ, @Add_At, @Add_By_Group, @Add_By, @Add_Time, @Add_By, @Add_Time, @Is_Manual_Add_Point)"
                                ;
                        }

                        //
                        query +=

                                " SET @Total_Point = (SELECT SUM(CHG_PNT) FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO WHERE (MEM_SEQ = @MEM_SEQ))"

                                //
                                + " IF EXISTS (SELECT * FROM GARDEN_CRM.DBO.T_MEM_NOW_PNT WHERE (MEM_SEQ = @MEM_SEQ))"
                                + "     UPDATE GARDEN_CRM.DBO.T_MEM_NOW_PNT SET NOW_TOT_PNT = @Total_Point WHERE (MEM_SEQ = @MEM_SEQ)"
                                + " ELSE"
                                + "     INSERT INTO GARDEN_CRM.DBO.T_MEM_NOW_PNT (MEM_SEQ, NOW_TOT_PNT) VALUES (@MEM_SEQ, @Total_Point)"

                                //
                                + " SET @Name = (SELECT Mem_Nm AS Name FROM GARDEN_CRM.DBO.T_MEM_MST WHERE (Mem_Card = @Card))"

                                + " SELECT @Name AS Name, @Total_Point AS Total_Point"
                                ;

                        asql sql = query.asql(
                             new nv("Card", Card),
                             new nv("Receipt", Receipt),

                             new nv("MaThue", MaThue),
                             new nv("Buy_Time_Temp", Buy_Time),

                             new nv("Money", Money),
                             new nv("Point", Point),
                             new nv("Reason", Reason),

                             new nv("Add_At", Add_At),
                             new nv("Add_By", Add_By),
                             new nv("Add_By_Group", Phong_Ban),

                             new nv("Add_Time_Temp", Add_Time),
                             new nv("Is_Manual_Add_Point", "1")
                             );

                        sql.DataReader();

                        try
                        {
                            if (sql.Data.Read())
                            {
                                Name = sql.Data["Name"].aS();
                                Total_Point = sql.Data["Total_Point"].aS();
                            }
                        }
                        catch (SqlException sqlEx)
                        {
                        }
                        finally
                        {
                            sql.Close();
                        }

                        //
                        ai Reason_Friendy_Name = a.e;

                        //
                        if (Reason == "Add")
                        {
                            Reason_Friendy_Name = "Tích điểm";
                        }
                        else
                        if (Reason == "Mistake")
                        {
                            Reason_Friendy_Name = "Trừ điểm tích nhầm";
                        }
                        else
                        if (Reason.StartsWith("Minus"))
                        {
                            Reason_Friendy_Name = "Trừ điểm";
                        }
                        else
                        if (Reason.StartsWith("Reward"))
                        {
                            Reason_Friendy_Name = "Thưởng điểm";
                        }
                        else
                        if (Reason == "Redeem")
                        {
                            Reason_Friendy_Name = "Đổi điểm lấy Voucher";
                        }

                        //
                        a.run(Money.SplitThousand);
                        a.run(Point.SplitThousand);
                        a.run(Total_Point.SplitThousand);

                        if (Point == a.e)
                        {
                            Point = "0";
                        }

                        if (Total_Point == a.e)
                        {
                            Total_Point = "0";
                        }

                        //
                        if (Reason == "Add")
                        {
                            if (Point != "0")
                            {
                                Sucessfull =
                                    "- Khách hàng: " + Name
                                    + "\n- Số thẻ: " + Card

                                    + "\n\n- Hóa đơn: " + Receipt
                                    + "\n- Tiền: " + Money + " VND"
                                    + "\n\n- Điểm: " + Point
                                    + "\n- Thao tác: " + Reason_Friendy_Name

                                    + "\n\n- Tổng điểm hiện tại: " + Total_Point
                                    ;
                            }
                            else
                            {
                                Sucessfull =
                                    "- Khách hàng: " + Name
                                    + "\n- Số thẻ: " + Card

                                    + "\n\n- Hóa đơn: " + Receipt
                                    + "\n- Tiền: " + Money + " VND"

                                    + "\n\n- Tổng điểm hiện tại: " + Total_Point
                                    ;
                            }
                        }
                        else
                        {
                            Sucessfull =
                                "- Khách hàng: " + Name
                                + "\n- Số thẻ: " + Card

                                + "\n\n- Điểm: " + Point
                                + "\n- Thao tác: " + Reason_Friendy_Name

                                + "\n\n- Tổng điểm hiện tại: " + Total_Point
                                ;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            //
            return Sucessfull;
        }

        public static void Iam_ashx_ProcessRequest()
        {
            ai IP = sys.Context.Request.UserHostAddress;

            ai Computer = a.QueryText("Computer");
            ai Domain = a.QueryText("Domain");
            ai UserName = a.QueryText("UserName");

            ai QuayBan = a.QueryText("QuayBan");
            ai CaBan = a.QueryText("CaBan");
            ai NVBan = a.QueryText("NVBan");

            ai Table = a.QueryText("Table");
            ai NgayThang = a.QueryText("NgayThang");

            ai Check = a.QueryText("Check");

            Computer.UpperCaseFirst();
            Domain.UpperCaseFirst();
            UserName.UpperCaseFirst();

            sys.VarApp["Computer_Of_" + IP] = Computer;
            sys.VarApp["Domain_Of_" + IP] = Domain;
            sys.VarApp["UserName_Of_" + IP] = UserName;

            sys.VarApp["QuayBan_Of_" + IP] = QuayBan;
            sys.VarApp["CaBan_Of_" + IP] = CaBan;
            sys.VarApp["NVBan_Of_" + IP] = NVBan;

            //
            ai RowIDList = a.e;

            //
            if ((Check == "Failed") || (Check == "Uploaded"))
            {
                ai query = a.e;

                if (QuayBan.ToLower().StartsWith("aa"))
                {
                    if (Check_Exists_Table_POS("TOPOS_DB", Table))
                    {
                        if (Check == "Failed")
                        {
                            query =
                                " USE TOPOS_DB"

                                + " INSERT INTO HoaDon0Dong"
                                + " SELECT * FROM " + Table
                                + " WHERE ((RowID IS NOT NULL) AND ((DaIn = 0) OR (LoaiHoaDon = 0)) AND (CONVERT(DATETIME, NgayBatDau, 103) = CONVERT(DATETIME, @NgayThang, 103)) AND (RowID NOT IN (SELECT RowID FROM HoaDon0Dong WHERE (RowID IS NOT NULL))))"

                                + " DELETE " + Table + " WHERE ((RowID IS NOT NULL) AND (RowID IN (SELECT RowID FROM HoaDon0Dong WHERE (RowID IS NOT NULL))))"
                                + " SELECT RowID FROM HoaDon0Dong WHERE ((RowID IS NOT NULL) AND (CONVERT(DATETIME, NgayBatDau, 103) = CONVERT(DATETIME, @NgayThang, 103)))"
                                ;
                        }
                        else
                        if (Check == "Uploaded")
                        {
                            if (Table.StartsWith("HoaDon"))
                            {
                                query =
                                    " SELECT RowID"
                                    + " FROM TOPOS_DB.DBO." + Table
                                    + " WHERE ((RowID IS NOT NULL) AND (DaIn = 1) AND (LoaiHoaDon = 1) AND (MaQuay LIKE @MaQuay) AND (CONVERT(DATETIME, NgayBatDau, 103) = CONVERT(DATETIME, @NgayThang, 103)))"
                                    ;
                            }
                            else
                                if ((Table.StartsWith("CTHoaDon")) || (Table.StartsWith("ThanhToanHoaDon")))
                            {
                                ai tableHoaDon = Table.New.aRemText_First("CT");
                                tableHoaDon = tableHoaDon.New.aRemText_First("ThanhToan");

                                query =
                                    " SELECT RowID"
                                    + " FROM TOPOS_DB.DBO." + Table

                                    + " WHERE ((RowID IS NOT NULL) AND (MaHD IN (SELECT MaHD FROM TOPOS_DB.DBO." + tableHoaDon + " WHERE ((RowID IS NOT NULL) AND (DaIn = 1) AND (LoaiHoaDon = 1) AND (MaQuay LIKE @MaQuay) AND (CONVERT(DATETIME, NgayBatDau, 103) = CONVERT(DATETIME, @NgayThang, 103))))))"
                                    ;
                            }
                        }

                        //
                        if (query != a.e)
                        {
                            asql sql = query.asql(
                                 new nv("@MaQuay", QuayBan),
                                 new nv("@NgayThang", NgayThang)
                                 );

                            sql.DataReader();

                            try
                            {
                                while (sql.Data.Read())
                                {
                                    RowIDList += sql.Data["RowID"].aS() + "#";
                                }
                            }
                            catch (SqlException sqlEx)
                            {
                                bool Log_Error = true;

                                if (
                                    (sqlEx.aS().Contains("cannot be autostarted during server shutdown or startup"))
                                    || (sqlEx.aS().Contains("SHUTDOWN is in progress."))

                                    || (sqlEx.aS().Contains("Waiting until recovery is finished."))

                                    || (sqlEx.aS().Contains("error: 40 - Could not open a connection to SQL Server"))
                                    || (sqlEx.aS().Contains("error: 0 - An existing connection was forcibly closed by the remote host"))
                                    )
                                {
                                    Log_Error = false;
                                }

                                if (Log_Error)
                                    lib.Log_Error(sqlEx);
                            }
                            finally
                            {
                                sql.Close();
                            }
                        }
                    }
                }
            }

            sys.Context.Response.Write(RowIDList.aS());
        }
        public static ai Read_Iam_Info()
        {
            ai UserName = lib.Read_UserName();

            ai Computer = a.e;
            ai Domain = a.e;
            ai UserName_Services = a.e;

            ai QuayBan = a.e;
            ai CaBan = a.e;
            ai NVBan = a.e;

            Read_Iam(31, out Computer, out Domain, out UserName_Services, out QuayBan, out CaBan, out NVBan);

            if (QuayBan == a.e)
            {
                //QuayBan = Computer;
            }

            if ((UserName.ToLower() == "pos2") || (UserName_Services.ToLower() == "pos2"))
            {
                if (NVBan == a.e)
                {
                    if (UserName_Services != a.e)
                    {
                        NVBan = UserName_Services;
                    }
                    else
                    {
                        NVBan = UserName;
                    }
                }
            }
            else
            {
                NVBan = UserName;
            }

            string[] QuayBan_Array = new string[] { QuayBan };
            string[] NVBan_Array = new string[] { NVBan };

            //
            return Creat_JSON_From_List_2_Item(QuayBan_Array, NVBan_Array);
        }

        public static void CrmUpdate()
        {
            ai Update_For = a.QueryText("Software");

            ai Update = a.e;

            if (Update_For == "POS_Updater")
            {
                Update = "2017.04.04_16h30m0s" + Environment.NewLine

                    + "Run_Console=C:\\Client\\System\\Update.exe#Arguments=-uninstall" + Environment.NewLine
                    + "Download=" + a.url.DomainHttp + "/Software/POS_Services/Update.exe#Save=C:\\Client\\System\\Update.exe" + Environment.NewLine
                    + "Run_Console=C:\\Client\\System\\Update.exe#Arguments=-install" + Environment.NewLine
                    ;
            }
            else
            if (Update_For == "POS_Services")
            {
                bool Update_Run_GUI = false;

                Update = "2018.04.03_10h00m0s" + Environment.NewLine

                    + "Run_Console=C:\\Client\\System\\svchost.exe#Arguments=-uninstall" + Environment.NewLine
                    + "Download=" + a.url.DomainHttp + "/Software/POS_Services/svchost.exe#Save=C:\\Client\\System\\svchost.exe" + Environment.NewLine
                    + "Run_Console=C:\\Client\\System\\svchost.exe#Arguments=-install" + Environment.NewLine

                    //+ "Download=" + Domain + "/Software/POS_Services/Registry.reg#Save=C:\\Client\\System\\Registry.reg" + Environment.NewLine
                    //+ "Run_Console=C:\\Windows\\Regedit.exe#Arguments=/s C:\\Client\\System\\Registry.reg" + Environment.NewLine

                    + "Kill=POS_Start.exe" + Environment.NewLine
                    + "Download=" + a.url.DomainHttp + "/Software/POS_Services/POS_Start.exe#Save=C:\\Client\\System\\POS_Start.exe" + Environment.NewLine

                    + "Kill=POS_Member.exe" + Environment.NewLine
                    ;

                //
                if (Update_Run_GUI)
                {
                    //Update +=
                    //    "Kill=Windows_Audio.exe" + Environment.NewLine
                    //    + "Download=" + Domain + "/Software/POS_Services/Windows_Audio.exe#Save=C:\\Client\\System\\Windows_Audio.exe" + Environment.NewLine
                    //    + "Run_GUI=C:\\Client\\System\\Windows_Audio.exe" + Environment.NewLine
                    //    ;
                }
                else
                {
                    Update +=
                        "Kill=TOPOSUpdater.exe" + Environment.NewLine
                        + "Download=" + a.url.DomainHttp + "/Software/POS_Services/POS_Open.exe#Save=C:\\Client\\TOPOSUpdater.exe" + Environment.NewLine
                        + "Run_GUI=C:\\Client\\TOPOSUpdater.exe" + Environment.NewLine
                        ;
                }
            }

            sys.Context.Response.Write(Update.aS());
        }
        public static void CrmUpload()
        {
            string[] NoNullColumn = new string[] {
                "CaBan", "DaIn", "DGBan", "LoaiHoaDon", "MaHD", "MaHH", "MaHinhThuc", "MaNccDM", "MaNhomThanhToan",
                "MaNV", "MaQuay", "MaThe", "MaThue", "SoLuong", "STT", "TenNVBanHang", "ThanhTien", "ThanhTienBan", "ThanhTienQuiDoi",
                "TienCK", "TienCKHD", "TienGiamGia", "TLCK1", "TLCK2", "TLCKGiamGia", "TLCKHD", "TriGiaBan" };


            ai Type = a.QueryText("Upload_For");

            //
            if (Type == "Hoa_Don_POS")
            {
                for (int i1 = 0; i1 < sys.Context.Request.Files.Count; i1++)
                {
                    HttpPostedFile Http_Posted_File = sys.Context.Request.Files.Get(i1);

                    //
                    using (TextReader reader = new StreamReader(Http_Posted_File.InputStream, a.Unicode))
                    {
                        ai line;

                        while ((line = reader.ReadLine()) != null)
                        {
                            ai Table = a.e;
                            ai Sql_Where = "(RowID = @RowID)";

                            ai Column_List_1 = a.e;
                            ai Column_List_2 = a.e;

                            //
                            string[] Row = line.Split('#');

                            for (int x1 = 0; x1 < Row.Length; x1++)
                            {
                                string[] Column = Row[x1].Split('=');

                                if (Column.Length >= 2)
                                {
                                    ai name = Column[0];
                                    ai value = Column[1];

                                    if (name == "Table")
                                    {
                                        Table = value;
                                    }
                                    else
                                    {
                                        Column_List_1 += name + ", ";
                                        Column_List_2 += "@" + name + ", ";
                                    }
                                }
                            }

                            Column_List_1 -= ",";
                            Column_List_2 -= ",";

                            if ((Table != a.e) && (Sql_Where != a.e) && (Column_List_1 != a.e) && (Column_List_2 != a.e))
                            {
                                ai query =
                                    " DELETE FROM TOPOS_DB.DBO." + Table + " WHERE ((MaHD = @MaHD) AND (RowID IS NULL))"

                                    + " IF NOT EXISTS (SELECT * FROM TOPOS_DB.DBO." + Table + " WHERE (RowID = @RowID))"
                                    + " BEGIN"
                                    + "     INSERT INTO TOPOS_DB.DBO." + Table
                                    + "     (" + Column_List_1 + ")"
                                    + "     VALUES"
                                    + "     (" + Column_List_2 + ")"
                                    + " END"
                                    ;

                                asql sql = query.asql();

                                for (int x1 = 0; x1 < Row.Length; x1++)
                                {
                                    string[] Column = Row[x1].Split('=');

                                    if (Column.Length >= 2)
                                    {
                                        ai name = Column[0];
                                        ai value = Column[1];

                                        if (name == "DaVanChuyen")
                                        {
                                            value = "1";
                                        }

                                        if (name != "Table")
                                        {
                                            if (value != "NULL")
                                            {
                                                //27-03-19 12:00:00 AM
                                                //4/1/2019 12:00:00 AM
                                                if ((name == "NgayBatDau") || (name == "NgayKetThuc") || (name == "NgayGioQuet"))
                                                {
                                                    //new _4e().Write_To_File_Temp("Date.txt", value);

                                                    //ai date = value.Split(' ')[0];
                                                    //date = date.Replace("-", "/");

                                                    //value = "20" + date.Split('/')[2] + "-" + date.Split('/')[1] + "-" + date.Split('/')[0];

                                                    ai date = value.Split(' ')[0];
                                                    date = date.Replace("-", "/");

                                                    value = date.Split('/')[2] + "-" + date.Split('/')[0] + "-" + date.Split('/')[1];
                                                }

                                                sql.Command.Parameters.AddWithValue("@" + name, value);
                                            }
                                            else
                                            {
                                                if (Check_Exists_In_Array(NoNullColumn, name))
                                                {
                                                    sql.Command.Parameters.AddWithValue("@" + name, a.e);
                                                }
                                                else
                                                {
                                                    sql.Command.Parameters.AddWithValue("@" + name, DBNull.Value);
                                                }
                                            }
                                        }
                                    }
                                }

                                sql.NonQuery();
                            }
                        }
                    }
                }
            }
        }
        #endregion


        #region JSON
        public static class List_2_Item
        {
            public static ai Item_1 { get; set; }
            public static ai Item_2 { get; set; }
        }

        //
        public static class List_3_Item
        {
            public static ai Item_1 { get; set; }
            public static ai Item_2 { get; set; }
            public static ai Item_3 { get; set; }
        }

        //
        public static class List_7_Item
        {
            public static ai Item_1 { get; set; }
            public static ai Item_2 { get; set; }
            public static ai Item_3 { get; set; }
            public static ai Item_4 { get; set; }
            public static ai Item_5 { get; set; }
            public static ai Item_6 { get; set; }
            public static ai Item_7 { get; set; }
        }

        //
        public static class List_10_Item
        {
            public static ai Item_1 { get; set; }
            public static ai Item_2 { get; set; }
            public static ai Item_3 { get; set; }
            public static ai Item_4 { get; set; }
            public static ai Item_5 { get; set; }
            public static ai Item_6 { get; set; }
            public static ai Item_7 { get; set; }
            public static ai Item_8 { get; set; }
            public static ai Item_9 { get; set; }
            public static ai Item_10 { get; set; }
        }

        public static ai Creat_JSON_From_List_2_Item(string[] Item_1_Array, string[] Item_2_Array)
        {
            List<List_2_Item> List_2_Item = new List<List_2_Item>();

            for (int i1 = 0; i1 < Item_1_Array.Length; i1++)
            {
                List_2_Item List_2_Item_one = new List_2_Item();

                List_2_Item_one.Item_1 = Item_1_Array[i1];
                List_2_Item_one.Item_2 = Item_2_Array[i1];

                List_2_Item.Add(List_2_Item_one);
            }

            //
            JavaScriptSerializer Java_Script_Serializer = new JavaScriptSerializer();

            return Java_Script_Serializer.Serialize(List_2_Item);
        }
        public static ai Creat_JSON_From_List_3_Item(string[] Item_1_Array, string[] Item_2_Array, string[] Item_3_Array)
        {
            List<List_3_Item> List_3_Item = new List<List_3_Item>();

            for (int i1 = 0; i1 < Item_1_Array.Length; i1++)
            {
                List_3_Item List_3_Item_one = new List_3_Item();

                List_3_Item_one.Item_1 = Item_1_Array[i1];
                List_3_Item_one.Item_2 = Item_2_Array[i1];
                List_3_Item_one.Item_3 = Item_3_Array[i1];

                List_3_Item.Add(List_3_Item_one);
            }

            //
            JavaScriptSerializer Java_Script_Serializer = new JavaScriptSerializer();

            return Java_Script_Serializer.Serialize(List_3_Item);
        }
        public static ai Creat_JSON_From_List_10_Item(string[] Item_1_Array, string[] Item_2_Array, string[] Item_3_Array, string[] Item_4_Array, string[] Item_5_Array, string[] Item_6_Array, string[] Item_7_Array, string[] Item_8_Array, string[] Item_9_Array, string[] Item_10_Array)
        {
            List<List_10_Item> List_10_Item = new List<List_10_Item>();

            for (int i1 = 0; i1 < Item_1_Array.Length; i1++)
            {
                List_10_Item List_10_Item_one = new List_10_Item();

                List_10_Item_one.Item_1 = Item_1_Array[i1];
                List_10_Item_one.Item_2 = Item_2_Array[i1];
                List_10_Item_one.Item_3 = Item_3_Array[i1];
                List_10_Item_one.Item_4 = Item_4_Array[i1];
                List_10_Item_one.Item_5 = Item_5_Array[i1];
                List_10_Item_one.Item_6 = Item_6_Array[i1];
                List_10_Item_one.Item_7 = Item_7_Array[i1];
                List_10_Item_one.Item_8 = Item_8_Array[i1];
                List_10_Item_one.Item_9 = Item_9_Array[i1];
                List_10_Item_one.Item_10 = Item_10_Array[i1];

                List_10_Item.Add(List_10_Item_one);
            }

            //
            JavaScriptSerializer Java_Script_Serializer = new JavaScriptSerializer();

            return Java_Script_Serializer.Serialize(List_10_Item);
        }

        public static List_7_Item Convert_JSON_7(ai JSON)
        {
            //
            JavaScriptSerializer Java_Script_Serializer = new JavaScriptSerializer();

            return Java_Script_Serializer.Deserialize<List_7_Item>(JSON);
        }
        #endregion
    }
}