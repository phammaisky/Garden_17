using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;


namespace _IQwinwin
{
    public static class ams
    {
        public static void Creat_Cong_Ty_ddl(DropDownList ddl)
        {
            ddl.Items.Clear();
            ai query = "SELECT ID, NameVn FROM [PDSBitexco].[dbo].[Company] ORDER BY ID";

            asql sql = query.asql();
            new asqlReader().ToDDL(ddl, "NameVn", "ID", query);

            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Tất cả Công ty", "0"));
        }
        public static void Creat_Phong_Ban_ddl(ai Cong_Ty_ID, DropDownList ddl)
        {
            ddl.Items.Clear();
            ai sqlWhere = a.e;

            if (Cong_Ty_ID.IsID && Cong_Ty_ID.NoZero)
                sqlWhere = " AND (CompanyId = '" + Cong_Ty_ID + "')";

            ai query =
                "SELECT ID, DeptName FROM [PDSBitexco].[dbo].[Departments]"
                + " WHERE (" + sqlWhere + ")"
                + " ORDER BY ID";

            asql sql = query.asql();
            new asqlReader().ToDDL(ddl, "DeptName", "ID", query);

            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Tất cả Phòng ban", "0"));
        }
        public static void Creat_Loai_Thiet_Bi_ddl(ai Cong_Ty_ID, ai Phong_Ban_ID, DropDownList ddl)
        {
            ddl.Items.Clear();
            ai sqlWhere = a.e;

            if (Phong_Ban_ID.IsID && Phong_Ban_ID.NoZero)
                sqlWhere = " WHERE Id IN (SELECT DeviceCatId FROM [PDSBitexco].[dbo].[DeviceAndTool] WHERE Id IN (SELECT DeviceToolId FROM [PDSBitexco].[dbo].[HistoryUse] WHERE (DeptId = '" + Phong_Ban_ID + "')))";

            if ((sqlWhere == a.e) && Cong_Ty_ID.IsID && Cong_Ty_ID.NoZero)
                sqlWhere = " WHERE Id IN (SELECT DeviceCatId FROM [PDSBitexco].[dbo].[DeviceAndTool] WHERE CompanyId = '" + Cong_Ty_ID + "')";

            ai query =
                "SELECT ID, DeviceCatName FROM [PDSBitexco].[dbo].[DeviceCategory]"
                + sqlWhere
                + " ORDER BY DeviceCatName";

            asql sql = query.asql();
            new asqlReader().ToDDL(ddl, "DeviceCatName", "ID", query);

            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Tất cả Loại tài sản", "0"));
        }

        public static Bitmap CreatBarcode(ai value)
        {
            QrCodeEncodingOptions option = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                PureBarcode = true,

                Width = 100,
                Height = 20,
                Margin = 0,
            };

            option.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);

            BarcodeWriter writer = new BarcodeWriter();
            writer.Options = option;
            writer.Format = BarcodeFormat.CODE_128;

            return writer.Write(value);
        }
        public static void CreatPdfFromMultiFile(aray allPicture, ai pdfFilePath, float Scale)
        {
            FileStream fileStream = new FileStream(pdfFilePath, FileMode.Create, FileAccess.Write, FileShare.None);

            Document pdfDocument = new Document();
            pdfDocument.SetPageSize(new iTextSharp.text.Rectangle(0, 0, 416, 416));
            pdfDocument.SetMargins(10, 10, 10, 10);

            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDocument, fileStream);
            pdfDocument.Open();

            for (int i = 0; i < allPicture.Length; i++)
            {
                iTextSharp.text.Image Picture = iTextSharp.text.Image.GetInstance(allPicture[i].aS());
                Picture.ScalePercent(Scale);

                pdfDocument.NewPage();
                pdfDocument.Add(Picture);

                //allPicture[i].afile().Delete();
            }

            pdfDocument.Close();
        }

        public static void RotateImage(ai filePath, float angle)
        {
            //using (System.Drawing.Image image = System.Drawing.Image.FromFile(filePath))
            //{
            //    using (Bitmap bitmap = new Bitmap(image))
            //    {
            //        using (Graphics graphic = Graphics.FromImage(bitmap))
            //        {
            //            graphic.SmoothingMode = SmoothingMode.AntiAlias;
            //            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //            graphic.TranslateTransform((float)bitmap.Width / 2, (float)bitmap.Height / 2);
            //            graphic.RotateTransform(angle);

            //            //graphic.TranslateTransform(-(float)bitmap.Width / 2, -(float)bitmap.Height / 2);
            //            graphic.DrawImage(bitmap, new Point(0, 0));

            //            graphic.Save();

            //            ai newFile = filePath.New.aReplace(".bmp", "_ok.bmp");
            //            bitmap.Save(newFile, ImageFormat.Bmp);
            //        }
            //    }
            //}

            var image = System.Drawing.Image.FromFile(filePath);
            image.RotateFlip(RotateFlipType.Rotate90FlipNone);

            ai newFile = filePath.New.aReplace(".bmp", "_ok.bmp");
            image.Save(newFile, ImageFormat.Bmp);

            //Delete
            filePath.afile.Delete();
        }
        public static void AmsUpload()
        {
            ai Return_Message = "Uploaded";
            HttpPostedFile postedFile = sys.Request.Files.Get(0);

            if (postedFile.ContentLength > 0)
            {
                ai folderName = a.Guid;
                afolder folder = sys.Context.Server.MapPath("~/File/Upload/" + folderName + "/");
                folder.Create();

                ai fileName = "Barcode.xlsx";
                afile file = apath.Add(folder, fileName);
                postedFile.SaveAs(file);

                ExcelPackage excel = new ExcelPackage(file);
                var sheet = excel.Workbook.Worksheets[1];

                //sqlWhere
                bool Have_sqlWhere = false;
                ai sqlWhere = " AND (ID IN (";

                //Read Data
                int i1 = 1;

                while (sheet.Cells[i1, 1].Value != null)
                {
                    ai ID = sheet.Cells[i1, 1].Value.ai();

                    if (ID.IsID)
                    {
                        Have_sqlWhere = true;
                        sqlWhere += ID + ", ";
                    }

                    i1++;
                }

                sqlWhere -= ",";
                sqlWhere += "))";

                if (!Have_sqlWhere)
                    sqlWhere = a.e;

                ai query =

                   " UPDATE [PDSBitexco].[dbo].[DeviceAndTool]"
                   + " SET Checked = NULL"

                   + " UPDATE [PDSBitexco].[dbo].[DeviceAndTool]"
                   + " SET Checked = 1"

                   + " WHERE (" + sqlWhere + ")"
                   ;

                query.asql().NonQuery();

                folder.Delete_All();
                Return_Message = "ok";
            }

            sys.Context.Response.Write(Return_Message.aS());
        }
        //31/10/2019 - for pageload    
    }
   
 }