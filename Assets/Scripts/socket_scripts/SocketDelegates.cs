namespace PRELOGIN
{
    public delegate void OnReqVerifyCodeHandler(bool bSuccess);
    public delegate void OnCheckVerifyCodeHandler(bool bRight);
    public delegate void OnResetPasswordHandler(byte iResCode);
    public delegate void OnReqServerGroupHandler(string sServerGroup);
}