using System;
using System.Collections.Generic;

namespace PRELOGIN
{
    public class SocketEvents
    {
        public static event OnReqVerifyCodeHandler OnReqVerifyCode;
        public static void ReqVerifyCodeCB(bool bSuccess)
        {
            if (OnReqVerifyCode != null)
            {
                OnReqVerifyCode(bSuccess);
            }
        }

        public static event OnCheckVerifyCodeHandler OnCheckVerifyCode;
        public static void CheckVerifyCodeCB(bool bRight)
        {
            if (OnCheckVerifyCode != null)
            {
                OnCheckVerifyCode(bRight);
            }
        }

        public static event OnResetPasswordHandler OnResetPassword;
        public static void ResetPasswordCB(byte iResCode)
        {
            if (OnResetPassword != null)
            {
                OnResetPassword(iResCode);
            }
        }

        public static event OnReqServerGroupHandler OnReqServerGroup;
        public static void ReqServerGroupCB(string sServerGroup)
        {
            if (OnReqServerGroup != null)
            {
                OnReqServerGroup(sServerGroup);
            }
        }
    }
}
