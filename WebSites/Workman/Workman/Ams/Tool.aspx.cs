using _IQwinwin;
using OfficeOpenXml;
using System;
using _4u4m;


public partial class Tool : System.Web.UI.Page

{
    ai onPageLoad = "Hide_Loading_Parent();";
    ai fileName = "TS_2019_DEC14.xlsx";

    protected void Page_Load(object sender, EventArgs e)
    {
        //totaltime
        totaltime totaltime = new totaltime();

        #region Lien
        //Gen
        //GenAmsByNumber("VP", 1, 4, 443);
        //GenAmsByNumber("Thietbile", 2, 4, 4);
        //GenAmsByNumber("TSCC", 3, 4, 59);
        //GenAmsByNumber("GoldproHC", 4, 4, 16);
        //GenAmsByNumber("GoldproKP", 5, 4, 29);
        //GenAmsByNumber("Dongphuc", 6, 4, 13);

        //Insert
        //InsertAmsVanPhong("VP", 855);

        //InsertAmsViTri("Thietbile", 6, 23);
        //InsertAmsViTri("TSCC", 1860, 24);
        //InsertAmsViTri("Dongphuc", 1668, 28);

        //InsertAmsViTri("GoldproKP", 441, 25);
        //InsertAmsViTri("GoldproHC", 38, 26);
        #endregion

        #region Thu
        //Gen
        //GenAmsByNumber("GD", 1, 6, 98);
        //GenAmsByNumber("Manor", 2, 6, 11);
        //GenAmsByNumber("TSD-GD", 3, 8, 93);
        //GenAmsByNumber("TSD-Manor", 4, 6, 9);

        //Insert
        //InsertAmsViTri("GD", 536, 30);
        //InsertAmsViTri("Manor", 197, 31);

        //InsertAmsViTri("TSD-GD", 1052, 32);
        //InsertAmsViTri("TSD-Manor", 118, 33);
        #endregion

        #region Lien : 02/10/2019
        //Gen
        //GenAmsByNumber("Thietbile", 1, 4, 5);
        //GenAmsByNumber("TSCC", 2, 4, 4);

        //Insert
        //InsertAmsViTri("Thietbile", 7, 23);
        //InsertAmsViTri("TSCC", 9, 24);
        #endregion

        #region Thu : 04/10/2019
        //Gen
        //GenAmsByNumber("GD", 1, 6, 11);

        //Insert
        //InsertAmsViTri("GD", 461, 30);
        #endregion

        #region Thu: 31/10/2019
        //Gen
        //GenAmsByNumber("GD",1, 6, 8);

        //Insert
        // InsertAmsViTri("GD", 14, 6);
        #endregion
        #region Thu: 14/12/2019
        //Gen
        //GenAmsByNumber("Manor", 2, 6, 12);
        // GenAmsByNumber("GD", 3, 6, 8);
        //GenAmsByNumber("TSD-GD", 1, 6, 18);
        //GenAmsByNumber("TSD-Manor", 2, 6, 12);

        //Insert
        // InsertAmsViTri("GD", 14, 6);
       // InsertAmsViTri("TSD-GD", 103, 1);
        //InsertAmsViTri("TSD-Manor", 201, 1);

        #endregion

        //totaltime
        totaltime.Done();
        Response.Write("OK !<br/>" + totaltime.Result);
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!a.url.IsPageMethods_Undefined)
        //if (!new _4e().Check_PageMethods_Is_undefined())
        {
            lib.Add_All_JavaScript_AND_CSS_File_To_Header_Basic(true);
            lib.Add_All_JavaScript_AND_CSS_File_To_Header();

            if (!IsPostBack)
                Page_Body.Attributes.Add("onload", onPageLoad);
            else
                lib.Run_JavaScript(onPageLoad);
        }
    }

    public ai GetExcelCell(ExcelWorksheet sheet, int row, int column)
    {
        return sheet.Cells[row, column].Value.ai().aRem_Space_Tab_Line();
    }
    protected void GenAmsByNumber(ai name, int sheetNumber, int from, int to)
    {
        afile file = apath.Add(sys.Root_Folder, @"File\Excel\Ams\KeKhai\" + fileName);
        ExcelPackage excel = new ExcelPackage(file);

        afile outputFile = apath.Add(sys.Root_Folder, @"File\Excel\Ams\KeKhai\_Mau-ke-khai-Tai-san.xlsx");
        ExcelPackage outputExcel = new ExcelPackage(outputFile);

        var sheet = excel.Workbook.Worksheets[sheetNumber];
        var outputSheet = outputExcel.Workbook.Worksheets[1];

        //output
        int outputRow = 5;

        //Read Data
        for (int i1 = from; i1 <= to; i1++)
        {
            ai tenTS = GetExcelCell(sheet, i1, 1);
            ai loaiTS = tenTS;

            ai maSap = GetExcelCell(sheet, i1, 2);
            ai maTS = GetExcelCell(sheet, i1, 3);

            int allsoluong = GetExcelCell(sheet, i1, 5);
            int mabatdau = GetExcelCell(sheet, i1, 6);

            ai allnguoidung = GetExcelCell(sheet, i1, 8);
            ai phongban = GetExcelCell(sheet, i1, 9);

            if (phongban.IsEmpty)
                phongban = allnguoidung;

            //
            aray array = allnguoidung.aray("+");

            for (int x = 0; x < array.Length; x++)
            {
                ai nguoidung = a.e;
                int soluong = 0;

                aray subArray = array[x].ai().aRemSpace().aray("=");

                if (subArray.Length == 1)
                {
                    nguoidung = subArray[0].ai().aRemSpace();
                    soluong = allnguoidung.aContains("+") ? 1 : allsoluong;
                }
                else if (subArray.Length > 1)
                {
                    nguoidung = subArray[0].ai().aRemSpace();
                    soluong = subArray[1].ai().aRemSpace();
                }

                for (int y = 1; y <= soluong; y++)
                {
                    ai maTSFull = maTS + " " + mabatdau.ai().Add0Before(3);
                    mabatdau++;

                    outputRow++;
                    outputSheet.Cells[outputRow, 1].Value = maTSFull;

                    outputSheet.Cells[outputRow, 3].Value = loaiTS;
                    outputSheet.Cells[outputRow, 4].Value = tenTS;

                    outputSheet.Cells[outputRow, 6].Value = nguoidung;
                    outputSheet.Cells[outputRow, 7].Value = phongban;
                    outputSheet.Cells[outputRow, 8].Value = nguoidung;
                    outputSheet.Cells[outputRow, 12].Value = maSap;
                }
            }
        }

        //Save
        afile resultFile = apath.Add(sys.Root_Folder, @"File\Excel\Ams\KeKhai\Mau-ke-khai-Tai-san_OK_" + name + ".xlsx");
        outputExcel.SaveAs(resultFile);
    }

    protected void InsertAmsVanPhong(ai name, int to)
    {
        afile file = apath.Add(sys.Root_Folder, @"File\Excel\Ams\KeKhai\Mau-ke-khai-Tai-san_OK_" + name + ".xlsx");
        ExcelPackage excel = new ExcelPackage(file);
        var sheet = excel.Workbook.Worksheets[1];

        //Read Data
        for (int i1 = 6; i1 <= to; i1++)
        {
            ai maTS = GetExcelCell(sheet, i1, 1);
            ai loaiTS = GetExcelCell(sheet, i1, 3);
            ai tenTS = GetExcelCell(sheet, i1, 4);

            ai nguoidung = GetExcelCell(sheet, i1, 6);
            ai phongban = GetExcelCell(sheet, i1, 7);

            ai query =
                " USE PDSBitexco"

                + " DECLARE @DeptId NVARCHAR(MAX)"
                + " DECLARE @UserId NVARCHAR(MAX)"

                + " DECLARE @DeviceCategoryId NVARCHAR(MAX)"
                + " DECLARE @DeviceAndToolId NVARCHAR(MAX)"

                //DeptId
                + " SET @DeptId = (SELECT TOP 1 ID FROM Departments WHERE (DeptName LIKE @Department))"

                + " IF @DeptId IS NULL"
                + " BEGIN"
                + "     INSERT INTO Departments"
                + "     (CompanyId, DeptCode, DeptName, DeptDesc, Active) VALUES (1, @Department, @Department, @Department, 1)"
                + "     SET @DeptId = (SELECT SCOPE_IDENTITY());"
                + " END"

                //UserId
                + " SET @UserId = (SELECT TOP 1 ID FROM UserInfo WHERE (DeptId = @DeptId) AND (UserName LIKE @User))"

                + " IF @UserId IS NULL"
                + " BEGIN"
                + "     INSERT INTO UserInfo"
                + "     (DeptId, UserName, FullName, Active) VALUES (@DeptId, @User, @User, 1)"
                + "     SET @UserId = (SELECT SCOPE_IDENTITY());"
                + " END"

                //GroupUser_User
                + " IF NOT EXISTS (SELECT * FROM GroupUser_User WHERE (UserId = @UserId))"
                + " BEGIN"
                + "     INSERT INTO GroupUser_User (UserId, GroupUserId) VALUES (@UserId, 2)"
                + "     INSERT INTO GroupUser_User (UserId, GroupUserId) VALUES (@UserId, 12)"
                + "     INSERT INTO GroupUser_User (UserId, GroupUserId) VALUES (@UserId, 1018)"
                + " END"

                //DeviceCategoryId
                + " SET @DeviceCategoryId = (SELECT TOP 1 ID FROM DeviceCategory WHERE (DeviceCatName LIKE @DeviceCategory))"

                + " IF @DeviceCategoryId IS NULL"
                + " BEGIN"
                + "     INSERT INTO DeviceCategory"
                + "     (DeviceCatName, Type, Active) VALUES (@DeviceCategory, 0, 1)"
                + "     SET @DeviceCategoryId = (SELECT SCOPE_IDENTITY());"
                + " END"

                //DeviceAndToolId - CreateById, CreateDate
                + " SET @DeviceAndToolId = (SELECT TOP 1 ID FROM DeviceAndTool WHERE (CompanyId = 1) AND (DeviceCatId = @DeviceCategoryId) AND (AssetsCode = @AssetsCode))"

                + " IF @DeviceAndToolId IS NULL"
                + " BEGIN"
                + "     INSERT INTO DeviceAndTool"
                + "     (CompanyId, DeviceCatId, AssetsCode, DeviceName, CreateById, CreateDate) VALUES (1, @DeviceCategoryId, @AssetsCode, @DeviceName, @UserId, GETDATE())"
                + "     SET @DeviceAndToolId = (SELECT SCOPE_IDENTITY());"
                + " END"

                //HistoryUse - DeviceToolId, HandedToStaffId, DeptId, HandedDate, StatusId
                + " IF NOT EXISTS (SELECT * FROM HistoryUse WHERE (DeviceToolId = @DeviceAndToolId))"
                + " INSERT INTO HistoryUse (DeviceToolId, HandedToStaffId, DeptId, HandedDate, StatusId) VALUES (@DeviceAndToolId, @UserId, @DeptId, GETDATE(), 2)"

                ;

            "SELECT ffff".asql().NonQuery();

            query.asql(
                new nv("User", nguoidung),
                new nv("Department", phongban),
                new nv("DeviceName", tenTS),
                new nv("AssetsCode", maTS),
                new nv("DeviceCategory", loaiTS)
                ).NonQuery();
        }
    }
    protected void InsertAmsViTri(ai name, int to, int companyId)
    {
        afile file = apath.Add(sys.Root_Folder, @"File\Excel\Ams\KeKhai\Mau-ke-khai-Tai-san_OK_" + name + ".xlsx");
        ExcelPackage excel = new ExcelPackage(file);
        var sheet = excel.Workbook.Worksheets[1];

        //Read Data
        for (int i1 = 6; i1 <= to; i1++)
        {
            ai maTS = GetExcelCell(sheet, i1, 1);
            ai loaiTS = GetExcelCell(sheet, i1, 3);
            ai tenTS = GetExcelCell(sheet, i1, 4);

            ai vitri = GetExcelCell(sheet, i1, 8);
            ai maSap = GetExcelCell(sheet, i1, 12);

            ai query =
                " USE PDSBitexco"

                + " DECLARE @CompanyId NVARCHAR(MAX)"
                + " SET @CompanyId = " + companyId

                + " DECLARE @LocationId NVARCHAR(MAX)"

                + " DECLARE @DeviceCategoryId NVARCHAR(MAX)"
                + " DECLARE @DeviceAndToolId NVARCHAR(MAX)"

                //LocationId, ShortName, LocationName  --- Location
                + " SET @LocationId = (SELECT TOP 1 ID FROM Location WHERE (LocationName LIKE @Location))"

                + " IF @LocationId IS NULL"
                + " BEGIN"
                + "     INSERT INTO Location"
                + "     (ShortName, LocationName) VALUES (@Location, @Location)"
                + "     SET @LocationId = (SELECT SCOPE_IDENTITY());"
                + " END"

                //DeviceCategoryId
                + " SET @DeviceCategoryId = (SELECT TOP 1 ID FROM DeviceCategory WHERE (DeviceCatName LIKE @DeviceCategory))"

                + " IF @DeviceCategoryId IS NULL"
                + " BEGIN"
                + "     INSERT INTO DeviceCategory"
                + "     (DeviceCatName, Type, Active) VALUES (@DeviceCategory, 0, 1)"
                + "     SET @DeviceCategoryId = (SELECT SCOPE_IDENTITY());"
                + " END"

                //DeviceAndToolId - CreateById, CreateDate
                + " SET @DeviceAndToolId = (SELECT TOP 1 ID FROM DeviceAndTool WHERE (CompanyId = @CompanyId) AND (DeviceCatId = @DeviceCategoryId) AND (AssetsCode = @AssetsCode))"

                + " IF @DeviceAndToolId IS NULL"
                + " BEGIN"
                + "     INSERT INTO DeviceAndTool"
                + "     (CompanyId, DeviceCatId, AssetsCode, SAPCode, DeviceName, CreateById, CreateDate) VALUES (@CompanyId, @DeviceCategoryId, @AssetsCode, @SAPCode, @DeviceName, 8, GETDATE())"
                + "     SET @DeviceAndToolId = (SELECT SCOPE_IDENTITY());"
                + " END"

                //HistoryUse - DeviceToolId, LocationId, HandedDate, StatusId
                + " IF NOT EXISTS (SELECT * FROM HistoryUse WHERE (DeviceToolId = @DeviceAndToolId))"
                + " INSERT INTO HistoryUse (DeviceToolId, LocationId, HandedDate, StatusId) VALUES (@DeviceAndToolId, @LocationId, GETDATE(), 2)"
                ;

            query.asql(
                new nv("Location", vitri),
                new nv("DeviceName", tenTS),
                new nv("AssetsCode", maTS),
                new nv("SAPCode", maSap),
                new nv("DeviceCategory", loaiTS)
                ).NonQuery();
        }
    }
}