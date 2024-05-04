using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GardenCrm.Helpers
{
    public class BizTx
    {
        public static int SP_PIF_TEST(
          string ARG1,
          string ARG2,
          string ARG3,
          string ARG4,
          string ARG5,
          string ARG6,
          string ARG7,
          string ARG8,
          string ARG9,
          string ARG10,
          string ARG11,
          string ARG12,
          string ARG13,
          string ARG14,
          string ARG15,
          string ARG16,
          string ARG17,
          string ARG18,
          string ARG19,
          string ARG20,
          string ARG21,
          string ARG22,
          string ARG23,
          string ARG24,
          string ARG25,
          string ARG26,
          string ARG27,
          string ARG28,
          string ARG29,
          string ARG30,
          string ARG31,
          string ARG32,
          string ARG33,
          string ARG34,
          string ARG35)
        {
            return new DataHandler().ExcuteSp(DacTx.SP_PIF_TEST, new List<SqlParameter>()
      {
        BizTx.SetParam("@ARG1", SqlDbType.VarChar, ParameterDirection.Input, ARG1),
        BizTx.SetParam("@ARG2", SqlDbType.VarChar, ParameterDirection.Input, ARG2),
        BizTx.SetParam("@ARG3", SqlDbType.VarChar, ParameterDirection.Input, ARG3),
        BizTx.SetParam("@ARG4", SqlDbType.VarChar, ParameterDirection.Input, ARG4),
        BizTx.SetParam("@ARG5", SqlDbType.VarChar, ParameterDirection.Input, ARG5),
        BizTx.SetParam("@ARG6", SqlDbType.VarChar, ParameterDirection.Input, ARG6),
        BizTx.SetParam("@ARG7", SqlDbType.VarChar, ParameterDirection.Input, ARG7),
        BizTx.SetParam("@ARG8", SqlDbType.VarChar, ParameterDirection.Input, ARG8),
        BizTx.SetParam("@ARG9", SqlDbType.VarChar, ParameterDirection.Input, ARG9),
        BizTx.SetParam("@ARG10", SqlDbType.VarChar, ParameterDirection.Input, ARG10),
        BizTx.SetParam("@ARG11", SqlDbType.VarChar, ParameterDirection.Input, ARG11),
        BizTx.SetParam("@ARG12", SqlDbType.VarChar, ParameterDirection.Input, ARG12),
        BizTx.SetParam("@ARG13", SqlDbType.VarChar, ParameterDirection.Input, ARG13),
        BizTx.SetParam("@ARG14", SqlDbType.VarChar, ParameterDirection.Input, ARG14),
        BizTx.SetParam("@ARG15", SqlDbType.VarChar, ParameterDirection.Input, ARG15),
        BizTx.SetParam("@ARG16", SqlDbType.VarChar, ParameterDirection.Input, ARG16),
        BizTx.SetParam("@ARG17", SqlDbType.VarChar, ParameterDirection.Input, ARG17),
        BizTx.SetParam("@ARG18", SqlDbType.VarChar, ParameterDirection.Input, ARG18),
        BizTx.SetParam("@ARG19", SqlDbType.VarChar, ParameterDirection.Input, ARG19),
        BizTx.SetParam("@ARG20", SqlDbType.VarChar, ParameterDirection.Input, ARG20),
        BizTx.SetParam("@ARG21", SqlDbType.VarChar, ParameterDirection.Input, ARG21),
        BizTx.SetParam("@ARG22", SqlDbType.VarChar, ParameterDirection.Input, ARG22),
        BizTx.SetParam("@ARG23", SqlDbType.VarChar, ParameterDirection.Input, ARG23),
        BizTx.SetParam("@ARG24", SqlDbType.VarChar, ParameterDirection.Input, ARG24),
        BizTx.SetParam("@ARG25", SqlDbType.VarChar, ParameterDirection.Input, ARG25),
        BizTx.SetParam("@ARG26", SqlDbType.VarChar, ParameterDirection.Input, ARG26),
        BizTx.SetParam("@ARG27", SqlDbType.VarChar, ParameterDirection.Input, ARG27),
        BizTx.SetParam("@ARG28", SqlDbType.VarChar, ParameterDirection.Input, ARG28),
        BizTx.SetParam("@ARG29", SqlDbType.VarChar, ParameterDirection.Input, ARG29),
        BizTx.SetParam("@ARG30", SqlDbType.VarChar, ParameterDirection.Input, ARG30),
        BizTx.SetParam("@ARG31", SqlDbType.VarChar, ParameterDirection.Input, ARG31),
        BizTx.SetParam("@ARG32", SqlDbType.VarChar, ParameterDirection.Input, ARG32),
        BizTx.SetParam("@ARG33", SqlDbType.VarChar, ParameterDirection.Input, ARG33),
        BizTx.SetParam("@ARG34", SqlDbType.VarChar, ParameterDirection.Input, ARG34),
        BizTx.SetParam("@ARG35", SqlDbType.VarChar, ParameterDirection.Input, ARG35),
        BizTx.SetParam("@IsSuccess", SqlDbType.Int, ParameterDirection.Output, "-1")
      }, "@IsSuccess");
        }

        public static int UptAdmGrdSetList(
          string GRADE_CD,
          string sSales,
          string eSales,
          string sDate,
          string eDate,
          string OrderBy,
          string OrderByMed,
          string limit,
          string SET_GRADE)
        {
            return new DataHandler().ExcuteSp(DacTx.USP_SAVE_ADMIN_GRD_APPLY, new List<SqlParameter>()
      {
        BizTx.SetParam("@GRADE_CD2", SqlDbType.VarChar, ParameterDirection.Input, GRADE_CD),
        BizTx.SetParam("@sSales2", SqlDbType.VarChar, ParameterDirection.Input, sSales),
        BizTx.SetParam("@eSales2", SqlDbType.VarChar, ParameterDirection.Input, eSales),
        BizTx.SetParam("@sDate2", SqlDbType.VarChar, ParameterDirection.Input, sDate),
        BizTx.SetParam("@eDate2", SqlDbType.VarChar, ParameterDirection.Input, eDate),
        BizTx.SetParam("@OrderBy2", SqlDbType.VarChar, ParameterDirection.Input, OrderBy),
        BizTx.SetParam("@OrderByMed2", SqlDbType.VarChar, ParameterDirection.Input, OrderByMed),
        BizTx.SetParam("@limit2", SqlDbType.VarChar, ParameterDirection.Input, limit),
        BizTx.SetParam("@SET_GRADE", SqlDbType.VarChar, ParameterDirection.Input, SET_GRADE),
        BizTx.SetParam("@IsSuccess", SqlDbType.Int, ParameterDirection.Output, "-1")
      }, "@IsSuccess");
        }

        public static int UptAdmChg(string ADMIN_ID, string ADMIN_PWD)
        {
            return new DataHandler().ExcuteSp(DacTx.USP_MOD_USER_PWD, new List<SqlParameter>()
      {
        BizTx.SetParam("@ADMIN_ID", SqlDbType.VarChar, ParameterDirection.Input, ADMIN_ID),
        BizTx.SetParam("@ADMIN_PWD", SqlDbType.VarChar, ParameterDirection.Input, ADMIN_PWD),
        BizTx.SetParam("@IsSuccess", SqlDbType.Int, ParameterDirection.Output, "-1")
      }, "@IsSuccess");
        }

        public static int UptCampRegList(
          string CPAIN_SEQ,
          string GRADE_CD,
          string SEX_CD,
          string AGE_CD,
          string JOB_CD,
          string MAJOR_AREA,
          string MINOR_AREA,
          string FLR_CD,
          string DPT_CD,
          string BRAND_CD,
          string sBirthDate,
          string eBirthDate,
          string sDate,
          string eDate,
          string OrderBy,
          string OrderByMed,
          string limit,
          string DEL_FG)
        {
            return new DataHandler().ExcuteSp(DacTx.USP_SAVE_CPAIN_TGET_LIST, new List<SqlParameter>()
      {
        BizTx.SetParam("@CPAIN_SEQ", SqlDbType.VarChar, ParameterDirection.Input, CPAIN_SEQ),
        BizTx.SetParam("@GRADE_CD2", SqlDbType.VarChar, ParameterDirection.Input, GRADE_CD),
        BizTx.SetParam("@SEX_CD2", SqlDbType.VarChar, ParameterDirection.Input, SEX_CD),
        BizTx.SetParam("@AGE_CD2", SqlDbType.VarChar, ParameterDirection.Input, AGE_CD),
        BizTx.SetParam("@JOB_CD2", SqlDbType.VarChar, ParameterDirection.Input, JOB_CD),
        BizTx.SetParam("@MAJOR_AREA2", SqlDbType.VarChar, ParameterDirection.Input, MAJOR_AREA),
        BizTx.SetParam("@MINOR_AREA2", SqlDbType.VarChar, ParameterDirection.Input, MINOR_AREA),
        BizTx.SetParam("@FLR_CD2", SqlDbType.VarChar, ParameterDirection.Input, FLR_CD),
        BizTx.SetParam("@DPT_CD2", SqlDbType.VarChar, ParameterDirection.Input, DPT_CD),
        BizTx.SetParam("@BRD_CD2", SqlDbType.VarChar, ParameterDirection.Input, BRAND_CD),
        BizTx.SetParam("@sBirthDate2", SqlDbType.VarChar, ParameterDirection.Input, sBirthDate == "" ? "" : sBirthDate),
        BizTx.SetParam("@eBirthDate2", SqlDbType.VarChar, ParameterDirection.Input, eBirthDate == "" ? "" : eBirthDate),
        BizTx.SetParam("@sDate2", SqlDbType.VarChar, ParameterDirection.Input, sDate),
        BizTx.SetParam("@eDate2", SqlDbType.VarChar, ParameterDirection.Input, eDate),
        BizTx.SetParam("@OrderBy2", SqlDbType.VarChar, ParameterDirection.Input, OrderBy),
        BizTx.SetParam("@OrderByMed2", SqlDbType.VarChar, ParameterDirection.Input, OrderByMed),
        BizTx.SetParam("@limit2", SqlDbType.VarChar, ParameterDirection.Input, limit),
        BizTx.SetParam("@DEL_FG2", SqlDbType.VarChar, ParameterDirection.Input, DEL_FG),
        BizTx.SetParam("@IsSuccess", SqlDbType.Int, ParameterDirection.Output, "-1")
      }, "@IsSuccess");
        }

        public static int InsCampSpPnt(
          string PNT_SUBJ,
          string PNT_FG,
          string APY_S_DAY,
          string APY_E_DAY,
          string GRADE_1_BY_AMT,
          string GRADE_1_PNT,
          string GRADE_1_ADD,
          string GRADE_2_BY_AMT,
          string GRADE_2_PNT,
          string GRADE_2_ADD,
          string GRADE_3_BY_AMT,
          string GRADE_3_PNT,
          string GRADE_3_ADD,
          string PNT_PAY,
          string PNT_DESC,
          string REG_ID)
        {
            return new DataHandler().ExcuteSp(DacTx.USP_SAVE_CPAIN_SP_INFO, new List<SqlParameter>()
      {
        BizTx.SetParam("@PNT_SUBJ", SqlDbType.VarChar, ParameterDirection.Input, PNT_SUBJ),
        BizTx.SetParam("@PNT_FG", SqlDbType.VarChar, ParameterDirection.Input, PNT_FG),
        BizTx.SetParam("@APY_S_DAY", SqlDbType.VarChar, ParameterDirection.Input, APY_S_DAY),
        BizTx.SetParam("@APY_E_DAY", SqlDbType.VarChar, ParameterDirection.Input, APY_E_DAY),
        BizTx.SetParam("@GRADE_1_BY_AMT", SqlDbType.VarChar, ParameterDirection.Input, GRADE_1_BY_AMT),
        BizTx.SetParam("@GRADE_1_PNT", SqlDbType.VarChar, ParameterDirection.Input, GRADE_1_PNT),
        BizTx.SetParam("@GRADE_1_ADD", SqlDbType.VarChar, ParameterDirection.Input, GRADE_1_ADD),
        BizTx.SetParam("@GRADE_2_BY_AMT", SqlDbType.VarChar, ParameterDirection.Input, GRADE_2_BY_AMT),
        BizTx.SetParam("@GRADE_2_PNT", SqlDbType.VarChar, ParameterDirection.Input, GRADE_2_PNT),
        BizTx.SetParam("@GRADE_2_ADD", SqlDbType.VarChar, ParameterDirection.Input, GRADE_2_ADD),
        BizTx.SetParam("@GRADE_3_BY_AMT", SqlDbType.VarChar, ParameterDirection.Input, GRADE_3_BY_AMT),
        BizTx.SetParam("@GRADE_3_PNT", SqlDbType.VarChar, ParameterDirection.Input, GRADE_3_PNT),
        BizTx.SetParam("@GRADE_3_ADD", SqlDbType.VarChar, ParameterDirection.Input, GRADE_3_ADD),
        BizTx.SetParam("@PNT_PAY", SqlDbType.VarChar, ParameterDirection.Input, PNT_PAY),
        BizTx.SetParam("@PNT_DESC", SqlDbType.VarChar, ParameterDirection.Input, PNT_DESC),
        BizTx.SetParam("@REG_ID", SqlDbType.VarChar, ParameterDirection.Input, REG_ID),
        BizTx.SetParam("@IsSuccess", SqlDbType.Int, ParameterDirection.Output, "-1")
      }, "@IsSuccess");
        }

        public static int UptCampSpPnt(
          string PNT_SEQ,
          string PNT_SUBJ,
          string PNT_FG,
          string APY_S_DAY,
          string APY_E_DAY,
          string GRADE_1_BY_AMT,
          string GRADE_1_PNT,
          string GRADE_1_ADD,
          string GRADE_2_BY_AMT,
          string GRADE_2_PNT,
          string GRADE_2_ADD,
          string GRADE_3_BY_AMT,
          string GRADE_3_PNT,
          string GRADE_3_ADD,
          string PNT_PAY,
          string PNT_DESC,
          string USE_YN,
          string REG_ID)
        {
            return new DataHandler().ExcuteSp(DacTx.USP_MOD_CPAIN_SP_INFO, new List<SqlParameter>()
      {
        BizTx.SetParam("@PNT_SEQ", SqlDbType.VarChar, ParameterDirection.Input, PNT_SEQ),
        BizTx.SetParam("@PNT_SUBJ", SqlDbType.VarChar, ParameterDirection.Input, PNT_SUBJ),
        BizTx.SetParam("@PNT_FG", SqlDbType.VarChar, ParameterDirection.Input, PNT_FG),
        BizTx.SetParam("@APY_S_DAY", SqlDbType.VarChar, ParameterDirection.Input, APY_S_DAY),
        BizTx.SetParam("@APY_E_DAY", SqlDbType.VarChar, ParameterDirection.Input, APY_E_DAY),
        BizTx.SetParam("@GRADE_1_BY_AMT", SqlDbType.VarChar, ParameterDirection.Input, GRADE_1_BY_AMT),
        BizTx.SetParam("@GRADE_1_PNT", SqlDbType.VarChar, ParameterDirection.Input, GRADE_1_PNT),
        BizTx.SetParam("@GRADE_1_ADD", SqlDbType.VarChar, ParameterDirection.Input, GRADE_1_ADD),
        BizTx.SetParam("@GRADE_2_BY_AMT", SqlDbType.VarChar, ParameterDirection.Input, GRADE_2_BY_AMT),
        BizTx.SetParam("@GRADE_2_PNT", SqlDbType.VarChar, ParameterDirection.Input, GRADE_2_PNT),
        BizTx.SetParam("@GRADE_2_ADD", SqlDbType.VarChar, ParameterDirection.Input, GRADE_2_ADD),
        BizTx.SetParam("@GRADE_3_BY_AMT", SqlDbType.VarChar, ParameterDirection.Input, GRADE_3_BY_AMT),
        BizTx.SetParam("@GRADE_3_PNT", SqlDbType.VarChar, ParameterDirection.Input, GRADE_3_PNT),
        BizTx.SetParam("@GRADE_3_ADD", SqlDbType.VarChar, ParameterDirection.Input, GRADE_3_ADD),
        BizTx.SetParam("@PNT_PAY", SqlDbType.VarChar, ParameterDirection.Input, PNT_PAY),
        BizTx.SetParam("@PNT_DESC", SqlDbType.VarChar, ParameterDirection.Input, PNT_DESC),
        BizTx.SetParam("@USE_YN", SqlDbType.VarChar, ParameterDirection.Input, USE_YN),
        BizTx.SetParam("@REG_ID", SqlDbType.VarChar, ParameterDirection.Input, REG_ID),
        BizTx.SetParam("@IsSuccess", SqlDbType.Int, ParameterDirection.Output, "-1")
      }, "@IsSuccess");
        }

        public static int UptGrdInfo(
          string PNT_SEQ,
          string GRADE_1_BY_AMT,
          string GRADE_1_PNT,
          string GRADE_2_BY_AMT,
          string GRADE_2_PNT,
          string GRADE_3_BY_AMT,
          string GRADE_3_PNT,
          string REG_ID)
        {
            return new DataHandler().ExcuteSp(DacTx.USP_SAVE_ADMIN_GRD_INFO, new List<SqlParameter>()
      {
        BizTx.SetParam("@PNT_SEQ", SqlDbType.VarChar, ParameterDirection.Input, PNT_SEQ),
        BizTx.SetParam("@GRADE_1_BY_AMT", SqlDbType.VarChar, ParameterDirection.Input, GRADE_1_BY_AMT),
        BizTx.SetParam("@GRADE_1_PNT", SqlDbType.VarChar, ParameterDirection.Input, GRADE_1_PNT),
        BizTx.SetParam("@GRADE_2_BY_AMT", SqlDbType.VarChar, ParameterDirection.Input, GRADE_2_BY_AMT),
        BizTx.SetParam("@GRADE_2_PNT", SqlDbType.VarChar, ParameterDirection.Input, GRADE_2_PNT),
        BizTx.SetParam("@GRADE_3_BY_AMT", SqlDbType.VarChar, ParameterDirection.Input, GRADE_3_BY_AMT),
        BizTx.SetParam("@GRADE_3_PNT", SqlDbType.VarChar, ParameterDirection.Input, GRADE_3_PNT),
        BizTx.SetParam("@REG_ID", SqlDbType.VarChar, ParameterDirection.Input, REG_ID),
        BizTx.SetParam("@IsSuccess", SqlDbType.Int, ParameterDirection.Output, "-1")
      }, "@IsSuccess");
        }

        public static int InsCampReg(
          string CPAIN_SUBJ,
          string APY_S_DAY,
          string APY_E_DAY,
          string TGET_CNT,
          string CPAIN_DESC,
          string REG_ID)
        {
            return new DataHandler().ExcuteSp(DacTx.USP_SAVE_CPAIN_INFO, new List<SqlParameter>()
      {
        BizTx.SetParam("@CPAIN_SUBJ", SqlDbType.VarChar, ParameterDirection.Input, CPAIN_SUBJ),
        BizTx.SetParam("@APY_S_DAY", SqlDbType.VarChar, ParameterDirection.Input, APY_S_DAY),
        BizTx.SetParam("@APY_E_DAY", SqlDbType.VarChar, ParameterDirection.Input, APY_E_DAY),
        BizTx.SetParam("@TGET_CNT", SqlDbType.VarChar, ParameterDirection.Input, TGET_CNT),
        BizTx.SetParam("@CPAIN_DESC", SqlDbType.VarChar, ParameterDirection.Input, CPAIN_DESC),
        BizTx.SetParam("@REG_ID", SqlDbType.VarChar, ParameterDirection.Input, REG_ID),
        BizTx.SetParam("@IsSuccess", SqlDbType.Int, ParameterDirection.Output, "-1")
      }, "@IsSuccess");
        }

        public static int UptCamp(
          string CPAIN_SEQ,
          string CPAIN_SUBJ,
          string APY_S_DAY,
          string APY_E_DAY,
          string CPAIN_DESC,
          string USE_YN,
          string REG_ID)
        {
            return new DataHandler().ExcuteSp(DacTx.USP_MOD_CPAIN_INFO, new List<SqlParameter>()
      {
        BizTx.SetParam("@CPAIN_SEQ", SqlDbType.VarChar, ParameterDirection.Input, CPAIN_SEQ),
        BizTx.SetParam("@CPAIN_SUBJ", SqlDbType.VarChar, ParameterDirection.Input, CPAIN_SUBJ),
        BizTx.SetParam("@APY_S_DAY", SqlDbType.VarChar, ParameterDirection.Input, APY_S_DAY),
        BizTx.SetParam("@APY_E_DAY", SqlDbType.VarChar, ParameterDirection.Input, APY_E_DAY),
        BizTx.SetParam("@CPAIN_DESC", SqlDbType.VarChar, ParameterDirection.Input, CPAIN_DESC),
        BizTx.SetParam("@USE_YN", SqlDbType.VarChar, ParameterDirection.Input, USE_YN),
        BizTx.SetParam("@REG_ID", SqlDbType.VarChar, ParameterDirection.Input, REG_ID),
        BizTx.SetParam("@IsSuccess", SqlDbType.Int, ParameterDirection.Output, "-1")
      }, "@IsSuccess");
        }

        public static int UptCampUse(string CPAIN_SEQ, string USE_YN, string REG_ID)
        {
            return new DataHandler().ExcuteSp(DacTx.USP_DEL_CPAIN_INFO, new List<SqlParameter>()
      {
        BizTx.SetParam("@CPAIN_SEQ", SqlDbType.VarChar, ParameterDirection.Input, CPAIN_SEQ),
        BizTx.SetParam("@USE_YN", SqlDbType.VarChar, ParameterDirection.Input, USE_YN),
        BizTx.SetParam("@REG_ID", SqlDbType.VarChar, ParameterDirection.Input, REG_ID),
        BizTx.SetParam("@IsSuccess", SqlDbType.Int, ParameterDirection.Output, "-1")
      }, "@IsSuccess");
        }

        public static int UptSpPntUse(string PNT_SEQ, string USE_YN, string REG_ID)
        {
            return new DataHandler().ExcuteSp(DacTx.USP_DEL_CPAIN_SP_INFO, new List<SqlParameter>()
      {
        BizTx.SetParam("@PNT_SEQ", SqlDbType.VarChar, ParameterDirection.Input, PNT_SEQ),
        BizTx.SetParam("@USE_YN", SqlDbType.VarChar, ParameterDirection.Input, USE_YN),
        BizTx.SetParam("@REG_ID", SqlDbType.VarChar, ParameterDirection.Input, REG_ID),
        BizTx.SetParam("@IsSuccess", SqlDbType.Int, ParameterDirection.Output, "-1")
      }, "@IsSuccess");
        }

        public static int UptAdmUse(string ADMIN_ID, string USE_YN, string REG_ID)
        {
            return new DataHandler().ExcuteSp(DacTx.USP_DEL_ADMIN_USER, new List<SqlParameter>()
      {
        BizTx.SetParam("@ADMIN_ID", SqlDbType.VarChar, ParameterDirection.Input, ADMIN_ID),
        BizTx.SetParam("@USE_YN", SqlDbType.VarChar, ParameterDirection.Input, USE_YN),
        BizTx.SetParam("@REG_ID", SqlDbType.VarChar, ParameterDirection.Input, REG_ID),
        BizTx.SetParam("@IsSuccess", SqlDbType.Int, ParameterDirection.Output, "-1")
      }, "@IsSuccess");
        }

        public static int InsAdmReg(
          string ADMIN_ID,
          string ADMIN_PWD,
          string GRP_ID,
          string ADMIN_NM,
          string DEPT_NM,
          string STAFF_NO,
          string MOBILE_NO,
          string TEL_NO,
          string REG_ID)
        {
            return new DataHandler().ExcuteSp(DacTx.USP_SAVE_ADMIN_USER_INFO, new List<SqlParameter>()
      {
        BizTx.SetParam("@ADMIN_ID", SqlDbType.VarChar, ParameterDirection.Input, ADMIN_ID),
        BizTx.SetParam("@ADMIN_PWD", SqlDbType.VarChar, ParameterDirection.Input, ADMIN_PWD),
        BizTx.SetParam("@GRP_ID", SqlDbType.VarChar, ParameterDirection.Input, GRP_ID),
        BizTx.SetParam("@ADMIN_NM", SqlDbType.VarChar, ParameterDirection.Input, ADMIN_NM),
        BizTx.SetParam("@DEPT_NM", SqlDbType.VarChar, ParameterDirection.Input, DEPT_NM),
        BizTx.SetParam("@STAFF_NO", SqlDbType.VarChar, ParameterDirection.Input, STAFF_NO),
        BizTx.SetParam("@MOBILE_NO", SqlDbType.VarChar, ParameterDirection.Input, MOBILE_NO),
        BizTx.SetParam("@TEL_NO", SqlDbType.VarChar, ParameterDirection.Input, TEL_NO),
        BizTx.SetParam("@REG_ID", SqlDbType.VarChar, ParameterDirection.Input, REG_ID),
        BizTx.SetParam("@IsSuccess", SqlDbType.Int, ParameterDirection.Output, "-1")
      }, "@IsSuccess");
        }

        public static int InsAdmUpt(
          string ADMIN_ID,
          string ADMIN_PWD,
          string GRP_ID,
          string ADMIN_NM,
          string DEPT_NM,
          string STAFF_NO,
          string MOBILE_NO,
          string TEL_NO,
          string REG_ID,
          string PWD_CHG_YN,
          string USE_YN)
        {
            return new DataHandler().ExcuteSp(DacTx.USP_MOD_ADMIN_USER_INFO, new List<SqlParameter>()
      {
        BizTx.SetParam("@ADMIN_ID", SqlDbType.VarChar, ParameterDirection.Input, ADMIN_ID),
        BizTx.SetParam("@ADMIN_PWD", SqlDbType.VarChar, ParameterDirection.Input, ADMIN_PWD),
        BizTx.SetParam("@GRP_ID", SqlDbType.VarChar, ParameterDirection.Input, GRP_ID),
        BizTx.SetParam("@ADMIN_NM", SqlDbType.VarChar, ParameterDirection.Input, ADMIN_NM),
        BizTx.SetParam("@DEPT_NM", SqlDbType.VarChar, ParameterDirection.Input, DEPT_NM),
        BizTx.SetParam("@STAFF_NO", SqlDbType.VarChar, ParameterDirection.Input, STAFF_NO),
        BizTx.SetParam("@MOBILE_NO", SqlDbType.VarChar, ParameterDirection.Input, MOBILE_NO),
        BizTx.SetParam("@TEL_NO", SqlDbType.VarChar, ParameterDirection.Input, TEL_NO),
        BizTx.SetParam("@REG_ID", SqlDbType.VarChar, ParameterDirection.Input, REG_ID),
        BizTx.SetParam("@PWD_CHG_YN", SqlDbType.VarChar, ParameterDirection.Input, PWD_CHG_YN),
        BizTx.SetParam("@USE_YN", SqlDbType.VarChar, ParameterDirection.Input, USE_YN),
        BizTx.SetParam("@IsSuccess", SqlDbType.Int, ParameterDirection.Output, "-1")
      }, "@IsSuccess");
        }

        public static int InsMemReg(
          string MEM_CARD,
          string MEM_NM,
          string GRADE_CD,
          string NAT_CD,
          string CERTI_NO,
          string MEM_BIRTHDAY,
          string AGE_CD,
          string SEX_CD,
          string MAJOR_AREA,
          string MINOR_AREA,
          string MEM_ADDR,
          string TEL_NO,
          string MOBILE_NO,
          string E_MAIL,
          string COMPY_NM,
          string COMPY_POS,
          string COMPY_ADDR,
          string COMPY_TEL_NO,
          string JOB_CD,
          string WEDDING_CD,
          string WEDDING_DAY,
          string CHILD_CNT,
          string CAR_YN,
          string AUTOBY_YN,
          string INCOME_CD,
          string REG_ID,
          string HOBI_CD,
          string ADD_PNT,
          string PNT_RSN,
          string ETC_HOBI)
        {
            return new DataHandler().ExcuteSp(DacTx.USP_SAVE_MEM_INFO, new List<SqlParameter>()
      {
        BizTx.SetParam("@MEM_CARD", SqlDbType.VarChar, ParameterDirection.Input, MEM_CARD),
        BizTx.SetParam("@MEM_NM", SqlDbType.VarChar, ParameterDirection.Input, MEM_NM),
        BizTx.SetParam("@GRADE_CD", SqlDbType.VarChar, ParameterDirection.Input, GRADE_CD),
        BizTx.SetParam("@NAT_CD", SqlDbType.VarChar, ParameterDirection.Input, NAT_CD),
        BizTx.SetParam("@CERTI_NO", SqlDbType.VarChar, ParameterDirection.Input, CERTI_NO),
        BizTx.SetParam("@MEM_BIRTHDAY", SqlDbType.VarChar, ParameterDirection.Input, MEM_BIRTHDAY),
        BizTx.SetParam("@AGE_CD", SqlDbType.VarChar, ParameterDirection.Input, AGE_CD),
        BizTx.SetParam("@SEX_CD", SqlDbType.VarChar, ParameterDirection.Input, SEX_CD),
        BizTx.SetParam("@MAJOR_AREA", SqlDbType.VarChar, ParameterDirection.Input, MAJOR_AREA),
        BizTx.SetParam("@MINOR_AREA", SqlDbType.VarChar, ParameterDirection.Input, MINOR_AREA),
        BizTx.SetParam("@MEM_ADDR", SqlDbType.VarChar, ParameterDirection.Input, MEM_ADDR),
        BizTx.SetParam("@TEL_NO", SqlDbType.VarChar, ParameterDirection.Input, TEL_NO),
        BizTx.SetParam("@MOBILE_NO", SqlDbType.VarChar, ParameterDirection.Input, MOBILE_NO),
        BizTx.SetParam("@E_MAIL", SqlDbType.VarChar, ParameterDirection.Input, E_MAIL),
        BizTx.SetParam("@COMPY_NM", SqlDbType.VarChar, ParameterDirection.Input, COMPY_NM),
        BizTx.SetParam("@COMPY_POS", SqlDbType.VarChar, ParameterDirection.Input, COMPY_POS),
        BizTx.SetParam("@COMPY_ADDR", SqlDbType.VarChar, ParameterDirection.Input, COMPY_ADDR),
        BizTx.SetParam("@COMPY_TEL_NO", SqlDbType.VarChar, ParameterDirection.Input, COMPY_TEL_NO),
        BizTx.SetParam("@JOB_CD", SqlDbType.VarChar, ParameterDirection.Input, JOB_CD),
        BizTx.SetParam("@WEDDING_CD", SqlDbType.VarChar, ParameterDirection.Input, WEDDING_CD),
        BizTx.SetParam("@WEDDING_DAY", SqlDbType.VarChar, ParameterDirection.Input, WEDDING_DAY),
        BizTx.SetParam("@CHILD_CNT", SqlDbType.VarChar, ParameterDirection.Input, CHILD_CNT),
        BizTx.SetParam("@CAR_YN", SqlDbType.VarChar, ParameterDirection.Input, CAR_YN),
        BizTx.SetParam("@AUTOBY_YN", SqlDbType.VarChar, ParameterDirection.Input, AUTOBY_YN),
        BizTx.SetParam("@INCOME_CD", SqlDbType.VarChar, ParameterDirection.Input, INCOME_CD),
        BizTx.SetParam("@REG_ID", SqlDbType.VarChar, ParameterDirection.Input, REG_ID),
        BizTx.SetParam("@STR_HOBI_CD", SqlDbType.VarChar, ParameterDirection.Input, HOBI_CD),
        BizTx.SetParam("@ETC_HOBI", SqlDbType.VarChar, ParameterDirection.Input, ETC_HOBI),
        BizTx.SetParam("@ADD_PNT", SqlDbType.VarChar, ParameterDirection.Input, ADD_PNT),
        BizTx.SetParam("@PNT_RSN", SqlDbType.VarChar, ParameterDirection.Input, PNT_RSN),
        BizTx.SetParam("@IsSuccess", SqlDbType.Int, ParameterDirection.Output, "-1")
      }, "@IsSuccess");
        }

        public static int UptMemReg(
          string MEM_SEQ,
          string MEM_CARD,
          string MEM_NM,
          string GRADE_CD,
          string NAT_CD,
          string CERTI_NO,
          string MEM_BIRTHDAY,
          string AGE_CD,
          string SEX_CD,
          string MAJOR_AREA,
          string MINOR_AREA,
          string MEM_ADDR,
          string TEL_NO,
          string MOBILE_NO,
          string E_MAIL,
          string COMPY_NM,
          string COMPY_POS,
          string COMPY_ADDR,
          string COMPY_TEL_NO,
          string JOB_CD,
          string WEDDING_CD,
          string WEDDING_DAY,
          string CHILD_CNT,
          string CAR_YN,
          string AUTOBY_YN,
          string INCOME_CD,
          string REG_ID,
          string USE_YN,
          string HOBI_CD,
          string ETC_HOBI)
        {
            return new DataHandler().ExcuteSp(DacTx.USP_MOD_MEM_INFO, new List<SqlParameter>()
      {
        BizTx.SetParam("@MEM_SEQ", SqlDbType.VarChar, ParameterDirection.Input, MEM_SEQ),
        BizTx.SetParam("@MEM_CARD", SqlDbType.VarChar, ParameterDirection.Input, MEM_CARD),
        BizTx.SetParam("@MEM_NM", SqlDbType.VarChar, ParameterDirection.Input, MEM_NM),
        BizTx.SetParam("@GRADE_CD", SqlDbType.VarChar, ParameterDirection.Input, GRADE_CD),
        BizTx.SetParam("@NAT_CD", SqlDbType.VarChar, ParameterDirection.Input, NAT_CD),
        BizTx.SetParam("@CERTI_NO", SqlDbType.VarChar, ParameterDirection.Input, CERTI_NO),
        BizTx.SetParam("@MEM_BIRTHDAY", SqlDbType.VarChar, ParameterDirection.Input, MEM_BIRTHDAY),
        BizTx.SetParam("@AGE_CD", SqlDbType.VarChar, ParameterDirection.Input, AGE_CD),
        BizTx.SetParam("@SEX_CD", SqlDbType.VarChar, ParameterDirection.Input, SEX_CD),
        BizTx.SetParam("@MAJOR_AREA", SqlDbType.VarChar, ParameterDirection.Input, MAJOR_AREA),
        BizTx.SetParam("@MINOR_AREA", SqlDbType.VarChar, ParameterDirection.Input, MINOR_AREA),
        BizTx.SetParam("@MEM_ADDR", SqlDbType.VarChar, ParameterDirection.Input, MEM_ADDR),
        BizTx.SetParam("@TEL_NO", SqlDbType.VarChar, ParameterDirection.Input, TEL_NO),
        BizTx.SetParam("@MOBILE_NO", SqlDbType.VarChar, ParameterDirection.Input, MOBILE_NO),
        BizTx.SetParam("@E_MAIL", SqlDbType.VarChar, ParameterDirection.Input, E_MAIL),
        BizTx.SetParam("@COMPY_NM", SqlDbType.VarChar, ParameterDirection.Input, COMPY_NM),
        BizTx.SetParam("@COMPY_POS", SqlDbType.VarChar, ParameterDirection.Input, COMPY_POS),
        BizTx.SetParam("@COMPY_ADDR", SqlDbType.VarChar, ParameterDirection.Input, COMPY_ADDR),
        BizTx.SetParam("@COMPY_TEL_NO", SqlDbType.VarChar, ParameterDirection.Input, COMPY_TEL_NO),
        BizTx.SetParam("@JOB_CD", SqlDbType.VarChar, ParameterDirection.Input, JOB_CD),
        BizTx.SetParam("@WEDDING_CD", SqlDbType.VarChar, ParameterDirection.Input, WEDDING_CD),
        BizTx.SetParam("@WEDDING_DAY", SqlDbType.VarChar, ParameterDirection.Input, WEDDING_DAY),
        BizTx.SetParam("@CHILD_CNT", SqlDbType.VarChar, ParameterDirection.Input, CHILD_CNT),
        BizTx.SetParam("@CAR_YN", SqlDbType.VarChar, ParameterDirection.Input, CAR_YN),
        BizTx.SetParam("@AUTOBY_YN", SqlDbType.VarChar, ParameterDirection.Input, AUTOBY_YN),
        BizTx.SetParam("@INCOME_CD", SqlDbType.VarChar, ParameterDirection.Input, INCOME_CD),
        BizTx.SetParam("@USE_YN", SqlDbType.VarChar, ParameterDirection.Input, USE_YN),
        BizTx.SetParam("@REG_ID", SqlDbType.VarChar, ParameterDirection.Input, REG_ID),
        BizTx.SetParam("@STR_HOBI_CD", SqlDbType.VarChar, ParameterDirection.Input, HOBI_CD),
        BizTx.SetParam("@ETC_HOBI", SqlDbType.VarChar, ParameterDirection.Input, ETC_HOBI),
        BizTx.SetParam("@IsSuccess", SqlDbType.Int, ParameterDirection.Output, "-1")
      }, "@IsSuccess");
        }

        public static int InsAdmLog(string ADMIN_ID, string MENU_ID)
        {
            return new DataHandler().ExcuteSp(DacTx.USP_SAVE_USER_LOG, new List<SqlParameter>()
      {
        BizTx.SetParam("@ADMIN_ID", SqlDbType.VarChar, ParameterDirection.Input, ADMIN_ID),
        BizTx.SetParam("@MENU_ID", SqlDbType.VarChar, ParameterDirection.Input, MENU_ID),
        BizTx.SetParam("@IsSuccess", SqlDbType.Int, ParameterDirection.Output, "-1")
      }, "@IsSuccess");
        }

        public static int InsAdmCD(
          string MAJOR_CD,
          string CD_ID,
          string CD_NM,
          string CD_VAL,
          string CD_DESC,
          string ORD_ORD,
          string REG_ID)
        {
            return new DataHandler().ExcuteSp(DacTx.USP_SAVE_ADMIN_CD_INFO, new List<SqlParameter>()
      {
        BizTx.SetParam("@MAJOR_CD", SqlDbType.VarChar, ParameterDirection.Input, MAJOR_CD),
        BizTx.SetParam("@CD_ID", SqlDbType.VarChar, ParameterDirection.Input, CD_ID),
        BizTx.SetParam("@CD_NM", SqlDbType.VarChar, ParameterDirection.Input, CD_NM),
        BizTx.SetParam("@CD_VAL", SqlDbType.VarChar, ParameterDirection.Input, CD_VAL),
        BizTx.SetParam("@CD_DESC", SqlDbType.VarChar, ParameterDirection.Input, CD_DESC),
        BizTx.SetParam("@ORD_ORD", SqlDbType.VarChar, ParameterDirection.Input, ORD_ORD),
        BizTx.SetParam("@REG_ID", SqlDbType.VarChar, ParameterDirection.Input, REG_ID),
        BizTx.SetParam("@IsSuccess", SqlDbType.Int, ParameterDirection.Output, "-1")
      }, "@IsSuccess");
        }

        public static int UptAdmCD(
          string CD_ID,
          string UP_CD,
          string CD_NM,
          string CD_VAL,
          string CD_DESC,
          string ORD_ORD,
          string USE_YN,
          string MOD_ID)
        {
            return new DataHandler().ExcuteSp(DacTx.USP_MOD_ADMIN_CD_INFO, new List<SqlParameter>()
      {
        BizTx.SetParam("@CD_ID", SqlDbType.VarChar, ParameterDirection.Input, CD_ID),
        BizTx.SetParam("@UP_CD", SqlDbType.VarChar, ParameterDirection.Input, UP_CD),
        BizTx.SetParam("@CD_NM", SqlDbType.VarChar, ParameterDirection.Input, CD_NM),
        BizTx.SetParam("@CD_VAL", SqlDbType.VarChar, ParameterDirection.Input, CD_VAL),
        BizTx.SetParam("@CD_DESC", SqlDbType.VarChar, ParameterDirection.Input, CD_DESC),
        BizTx.SetParam("@ORD_ORD", SqlDbType.VarChar, ParameterDirection.Input, ORD_ORD),
        BizTx.SetParam("@USE_YN", SqlDbType.VarChar, ParameterDirection.Input, USE_YN),
        BizTx.SetParam("@MOD_ID", SqlDbType.VarChar, ParameterDirection.Input, MOD_ID),
        BizTx.SetParam("@IsSuccess", SqlDbType.Int, ParameterDirection.Output, "-1")
      }, "@IsSuccess");
        }

        private static SqlParameter SetParam(
          string paramName,
          SqlDbType currType,
          ParameterDirection currDirc,
          string currValue)
        {
            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.SqlDbType = currType;
            sqlParameter.Direction = currDirc;
            sqlParameter.ParameterName = paramName;
            sqlParameter.Value = (object)currValue;
            return sqlParameter;
        }

        public static int InsPoint(
          string MEM_CARD,
          string ADD_PNT,
          string CHG_RSN_CD,
          string PNT_RSN,
          string ADMIN_ID)
        {
            return new DataHandler().ExcuteSp(DacTx.USP_SAVE_POINT, new List<SqlParameter>()
      {
        BizTx.SetParam("@MEM_CARD", SqlDbType.VarChar, ParameterDirection.Input, MEM_CARD),
        BizTx.SetParam("@ADD_PNT", SqlDbType.VarChar, ParameterDirection.Input, ADD_PNT),
        BizTx.SetParam("@CHG_RSN_CD", SqlDbType.VarChar, ParameterDirection.Input, CHG_RSN_CD),
        BizTx.SetParam("@PNT_RSN", SqlDbType.VarChar, ParameterDirection.Input, PNT_RSN),
        BizTx.SetParam("@ADMIN_ID", SqlDbType.VarChar, ParameterDirection.Input, ADMIN_ID),
        BizTx.SetParam("@IsSuccess", SqlDbType.Int, ParameterDirection.Output, "-1")
      }, "@IsSuccess");
        }
    }
}