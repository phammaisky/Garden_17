using System;

namespace _IQwinwin
{
    public class _IQ
    {
        #region Global.asax 
        public void Application_Start(object sender, EventArgs e)
        {
            Default_Variable();
        }
        public void Application_BeginRequest(object sender, EventArgs e)
        {
            lib.Application_BeginRequest(sender, e);
        }
        public void Application_Error(object sender, EventArgs e)
        {
            lib.Application_Error(sender, e);
        }
        #endregion


        #region --- --- --- Custom From Here --- --- ---

        #endregion


        #region Default_Variable
        public void Default_Variable()
        {
            lib.Sql_Connection_String = "Data Source='127.0.0.1'; User ID='XXX'; Password='XXX'; MultipleActiveResultSets=true; Pooling=true; Max Pool Size=32767; Min Pool Size=1;";
        }
        #endregion

        #region Membership
        public void RegisterDefaultMembership()
        {

        }
        #endregion
    }
}