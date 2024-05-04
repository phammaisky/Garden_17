using _IQwinwin;
using System;

public partial class Insert_Card : System.Web.UI.Page
{
    ai message = a.e;
    ai onPageLoad = " Insert_Card_Onload();";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!a.url.IsPageMethods_Undefined)
        {
            ai UserName = lib.Read_UserName();

            ai Ho_Va_Ten = a.e;
            ai Phong_Ban = a.e;

            bool Duoc_Xem_Bao_Cao = false;
            bool Duoc_Tich_Diem = false;
            bool Duoc_Tru_Diem = false;
            bool Duoc_Doi_Diem = false;

            crm.Read_User_Phan_Quyen(UserName, out Ho_Va_Ten, out Phong_Ban, out Duoc_Xem_Bao_Cao, out Duoc_Tich_Diem, out Duoc_Tru_Diem, out Duoc_Doi_Diem);

            if (Phong_Ban.ToLower() != "IT".ToLower())
                Response.Redirect(a.url.DomainHttp);
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!a.url.IsPageMethods_Undefined)
        {
            lib.Add_All_JavaScript_AND_CSS_File_To_Header_Basic(true);
            lib.Add_All_JavaScript_AND_CSS_File_To_Header();

            if (!IsPostBack)
                Page_Body.Attributes.Add("onload", onPageLoad);
            else
                lib.Run_JavaScript(onPageLoad);
        }
    }

    protected void OK_btn_Click(object sender, EventArgs e)
    {
        ai UserName = lib.Read_UserName();

        ai From_Card = From_Card_tbx.Text.ai().Get_ValidInput;
        ai To_Card = To_Card_tbx.Text.ai().Get_ValidInput;

        From_Card_tbx.Text = From_Card;
        To_Card_tbx.Text = To_Card;

        long From_Card_int = lib.Convert_String_To_BigInt(From_Card, 0);
        long To_Card_int = lib.Convert_String_To_BigInt(To_Card, 0);

        bool Valid = true;

        if ((From_Card.Length != 11) || (To_Card.Length != 11) || (From_Card_int.ToString().Length != 10) || (To_Card_int.ToString().Length != 10) || (From_Card_int == 0) || (To_Card_int == 0))
        {
            Valid = false;
            message += "<br/><br/> - Số thẻ nhập ko hợp lệ, phải nhập 11 chữ số khác 0 !";
        }

        if (From_Card_int > To_Card_int)
        {
            Valid = false;
            message += "<br/><br/> - Phải nhập số thẻ TỪ <= số thẻ ĐẾN !";
        }

        //
        if (!Valid)
        {
            Message_lbl.Text = "LỖI: " + message;
        }
        else
        {
            ai query = a.e;

            if (From_Card.StartsWith("0104"))
            {
                query =

                    " DECLARE @New_Card NVARCHAR(MAX)"

                    + " DECLARE @Inserted_Card BIGINT"
                    + " SET @Inserted_Card = 0"

                    + " WHILE (@From_Card <= @To_Card)"

                    + " BEGIN"

                    + "     SET @New_Card = '0' + CONVERT(NVARCHAR(MAX), @From_Card)"

                    + "     IF NOT EXISTS (SELECT * FROM [Server-001-1].TOPOS_DB.DBO.TTT_DanhMuc WHERE MaFoodCourt LIKE @New_Card)"
                    + "     BEGIN"
                    + "         INSERT INTO [Server-001-1].TOPOS_DB.DBO.TTT_DanhMuc"
                    + "         (MaFoodCourt, SoTien, TrangThai, NgayTao, MaNVTao)"
                    + "         VALUES"
                    + "         (@New_Card, 0, 0, GETDATE(), @UserName)"

                    + "         SET @Inserted_Card = @Inserted_Card + 1"
                    + "     END"

                    + "     SET @From_Card = @From_Card + 1"

                    + " END"

                    + " SELECT @Inserted_Card"
                    ;
            }
            else
            {
                query =
                    " USE GARDEN_CRM"

                    + " DECLARE @New_Card NVARCHAR(MAX)"

                    + " DECLARE @Inserted_Card BIGINT"
                    + " SET @Inserted_Card = 0"

                    + " WHILE (@From_Card <= @To_Card)"

                    + " BEGIN"

                    + "     SET @New_Card = '0' + CONVERT(NVARCHAR(MAX), @From_Card)"

                    + "     IF NOT EXISTS (SELECT * FROM T_MEM_MST WHERE Mem_Card LIKE @New_Card)"
                    + "     BEGIN"
                    + "         INSERT INTO T_MEM_MST"
                    + "         (MEM_CARD, MEM_NM, REG_ID, REG_DT, MOD_ID, MOD_DT, GRADE_CD, NAT_CD, CERTI_NO, MEM_BIRTHDAY, AGE_CD, SEX_CD, MAJOR_AREA, MINOR_AREA, MEM_ADDR, TEL_NO, MOBILE_NO, E_MAIL, COMPY_NM, COMPY_POS, COMPY_ADDR, COMPY_TEL_NO, JOB_CD, WEDDING_CD, WEDDING_DAY, CHILD_CNT, CAR_YN, AUTOBY_YN, INCOME_CD, USE_YN, LEAVE_DT)"
                    + "         VALUES"
                    + "         (@New_Card, 'X', 'IT', GETDATE(), @UserName, GETDATE(), 'GRD001', 'NAT001', '', '1990/01/01', 'AGE002', 'F', 'ARE100', 'ARE101', 'X', '0-0-0', '0-0-0', '', '', '', '', '0-0-0', '', 'N', '', 0, 'N', 'N', '', 'Y', '')"

                    + "         SET @Inserted_Card = @Inserted_Card + 1"
                    + "     END"

                    + "     SET @From_Card = @From_Card + 1"

                    + " END"

                    + " SELECT @Inserted_Card"
                    ;
            }

            int Inserted_Card = query.asql(
                new nv("UserName", UserName),
                new nv("From_Card", From_Card_int),
                new nv("To_Card", To_Card_int)
                ).Scalar;

            int Total_Card_Input = Convert.ToInt32(To_Card_int - From_Card_int) + 1;

            if (Inserted_Card == 0)
            {
                Message_lbl.Text = "CHÚ Ý ! Không có thẻ nào được kích hoạt. Vì tất cả đã có trong hệ thống.<br/><br/>"
                        + "Hãy báo cho Admin kiểm tra lại ngay !";
            }
            else
            {
                if (Inserted_Card < Total_Card_Input)
                {
                    Message_lbl.Text =
                        "CHÚ Ý ! Mới chỉ kích hoạt thành công: " + Inserted_Card + " / " + Total_Card_Input + " thẻ.<br/><br/>"
                        + "Hãy báo cho Admin kiểm tra lại ngay !";
                }
                else
                    if (Inserted_Card == Total_Card_Input)
                {
                    Message_lbl.Text =
                        "OK ! Đã kích hoạt thành công đầy đủ: " + Inserted_Card + " / " + Total_Card_Input + " thẻ !<br/><br/>"
                        + "Có thể báo Kiểm phẩm bàn giao thẻ cho Thu ngân được rồi !";
                }
            }
        }
    }
}