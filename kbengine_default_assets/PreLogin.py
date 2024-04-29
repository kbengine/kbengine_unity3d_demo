# -*- coding: utf-8 -*-


import socket
from KBEDebug import *
import urllib.parse


"""
Process the logic before logging into the KBE server, because the 
KBE server cannot communicate with the client before logging in
"""


class Poller:
    """
    Demo:
    You can register a socket with kbengine, and the network module of the engine layer handles asynchronous notification sending and receiving.
    usage:
    from Poller import Poller
    poller = Poller()

    Turn on (can be executed in onBaseappReady)
    poller.start("localhost", 12345)

    stop
    poller.stop()
    """
    REQ_VERIFY_CODE = "XXX1"
    RESET_PASSWORD = "XXX2"
    REQ_SERVER_GROUP = "XXX3"

    def __init__(self):
        self._socket = None
        self._clients = {}

    def start(self, port):
        """
        virtual method.
        """
        self._socket = socket.socket()
        self._socket.bind(('0.0.0.0', port))
        self._socket.listen(10)

        KBEngine.registerReadFileDescriptor(self._socket.fileno(), self.onRecv)
        # KBEngine.registerWriteFileDescriptor(self._socket.fileno(), self.onWrite)

    def stop(self):
        if self._socket:
            KBEngine.deregisterReadFileDescriptor(self._socket.fileno())
            self._socket.close()
            self._socket = None

    def onWrite(self, fileno):
        pass

    def onRecv(self, fileno):
        if self._socket.fileno() == fileno:
            sock, addr = self._socket.accept()
            self._clients[sock.fileno()] = (sock, addr)
            KBEngine.registerReadFileDescriptor(sock.fileno(), self.onRecv)
            # DEBUG_MSG("Poller::onRecv: new channel[%s/%i]" % (addr, sock.fileno()))
        else:
            sock, addr = self._clients.get(fileno, None)
            if sock is None:
                return

            data = sock.recv(2048)
            # DEBUG_MSG("Poller::onRecv: %s/%i get data, size=%i" % (addr, sock.fileno(), len(data)))

            #########################################################################
            #---------------------------ProcessData Begin---------------------------#
            #########################################################################
            try:
                usData = urllib.parse.unquote(data.decode('utf-8'))  # 解析html格式
            except:
                return
            if usData[:256] == self.REQ_VERIFY_CODE:
                lTmp = usData[256:].split('&')
                import hwsms
                hwsms.SendCode(lTmp[0], lTmp[1])
                sRes = "SUCCESS"
                sock.sendall(sRes.encode())

                KBEngine.deregisterReadFileDescriptor(sock.fileno())
                sock.close()
                del self._clients[fileno]
            elif usData[:256] == self.RESET_PASSWORD:
                def _SqlResetPwd(result, rows, insertid, error):
                    sRes = "2"
                    sock.sendall(sRes.encode())
                    KBEngine.deregisterReadFileDescriptor(sock.fileno())
                    sock.close()
                    del self._clients[fileno]

                def _CheckActAndTel(result, rows, insertid, error):
                    if int(str(result[0][0], encoding='utf-8')) != 1:
                        sRes = "0"
                        sock.sendall(sRes.encode())
                        KBEngine.deregisterReadFileDescriptor(sock.fileno())
                        sock.close()
                        del self._clients[fileno]
                    elif int(str(result[1][0], encoding='utf-8')) != 1:
                        sRes = "1"
                        sock.sendall(sRes.encode())
                        KBEngine.deregisterReadFileDescriptor(sock.fileno())
                        sock.close()
                        del self._clients[fileno]
                    else:
                        sSQLCmd = f"update kbe_accountinfos,tbl_Account set password='{sPwd}' where sm_act='{sAct}' and sm_tel='{sTel}' and sm_act=accountName;"
                        KBEngine.executeRawDatabaseCommand(sSQLCmd, _SqlResetPwd)

                lTmp = usData[256:].split('&')
                import mxytool
                sAct = lTmp[0]
                sPwd = mxytool.GetMD5Password(lTmp[1])
                sTel = lTmp[2]
                sSQLCmd = f"select count(*) from tbl_Account where sm_act='{sAct}' union all select count(*) from tbl_Account where sm_act='{sAct}' and sm_tel='{sTel}';"
                KBEngine.executeRawDatabaseCommand(sSQLCmd, _CheckActAndTel)
            elif usData[:256] == self.REQ_SERVER_GROUP:  # Request server group information
                sRes = KBEngine.globalData["ServerData"].sServerGroup
                sock.sendall(sRes.encode())

                KBEngine.deregisterReadFileDescriptor(sock.fileno())
                sock.close()
                del self._clients[fileno]
            else:
                KBEngine.deregisterReadFileDescriptor(sock.fileno())
                sock.close()
                del self._clients[fileno]
            #########################################################################
            # ---------------------------ProcessData End----------------------------#
            #########################################################################
