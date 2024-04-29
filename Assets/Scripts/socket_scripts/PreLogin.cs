using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


public class PreLogin
{
    private static string strVerifyTel = "";
    private static string strVerifyCode = "";

    private const string strSendVerifyCode = "XXX1";
    private const string strResetPassword = "XXX2";
    private const string strReqServerGroup = "XXX3";

    public static void SendVerifyCode(string sTel)
    {
        strVerifyTel = sTel;
        System.Random r = new System.Random(GetRandomSeed());
        strVerifyCode = r.Next(1000, 10000).ToString();

        int port = GlobalData.PORT_SERVER_PRELOGIN;
        string host = GlobalData.IP_SERVER;

        IPAddress ip = IPAddress.Parse(host);
        IPEndPoint ipe = new IPEndPoint(ip, port);

        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket.Connect(ipe);

        //send message
        byte[] sendBytes = Encoding.ASCII.GetBytes(strSendVerifyCode + sTel + "&" + strVerifyCode);
        clientSocket.Send(sendBytes);

        //receive message
        string recStr = "";
        byte[] recBytes = new byte[2048];
        int bytes = clientSocket.Receive(recBytes, recBytes.Length, 0);
        recStr += Encoding.ASCII.GetString(recBytes, 0, bytes);
        if(recStr == "SUCCESS")
        {
            PRELOGIN.SocketEvents.ReqVerifyCodeCB(true);
        }
        else
        {
            PRELOGIN.SocketEvents.ReqVerifyCodeCB(false);
        }

        clientSocket.Close();
    }

    public static void CheckVerifyCode(string sTel, string sVerifyCode)
    {
        PRELOGIN.SocketEvents.CheckVerifyCodeCB(true);
        return;
        if (sTel.Length == 11 && sTel == strVerifyTel && sVerifyCode.Length == 4 && sVerifyCode == strVerifyCode)
        {
            PRELOGIN.SocketEvents.CheckVerifyCodeCB(true);
        }
        else
        {
            PRELOGIN.SocketEvents.CheckVerifyCodeCB(false);
        }
    }

    public static void ResetPassword(string sAct, string strNewPwd, string sTel)
    {
        int port = GlobalData.PORT_SERVER_PRELOGIN;
        string host = GlobalData.IP_SERVER;

        IPAddress ip = IPAddress.Parse(host);
        IPEndPoint ipe = new IPEndPoint(ip, port);

        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket.Connect(ipe);

        //send message
        byte[] sendBytes = Encoding.ASCII.GetBytes(strResetPassword + sAct + "&" + strNewPwd + "&" + sTel);
        clientSocket.Send(sendBytes);

        //receive message
        string recStr = "";
        byte[] recBytes = new byte[2048];
        int bytes = clientSocket.Receive(recBytes, recBytes.Length, 0);
        recStr += Encoding.ASCII.GetString(recBytes, 0, bytes);
        PRELOGIN.SocketEvents.ResetPasswordCB(byte.Parse(recStr));

        clientSocket.Close();
    }

    public static void ReqServerGroup()
    {
        int port = GlobalData.PORT_SERVER_PRELOGIN;
        string host = GlobalData.IP_SERVER;

        IPAddress ip = IPAddress.Parse(host);
        IPEndPoint ipe = new IPEndPoint(ip, port);

        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket.Connect(ipe);

        //send message
        byte[] sendBytes = Encoding.ASCII.GetBytes(strReqServerGroup);
        clientSocket.Send(sendBytes);

        //receive message
        string recStr = "";
        byte[] recBytes = new byte[2048];
        int bytes = clientSocket.Receive(recBytes, recBytes.Length, 0);
        //recStr += Encoding.ASCII.GetString(recBytes, 0, bytes);
        recStr += Encoding.UTF8.GetString(recBytes, 0, bytes);
        PRELOGIN.SocketEvents.ReqServerGroupCB(recStr);

        clientSocket.Close();
    }

    private static int GetRandomSeed()
    {
        byte[] bytes = new byte[4];
        System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
        rng.GetBytes(bytes);
        return BitConverter.ToInt32(bytes, 0);
    }
}
