# -*- coding: utf-8 -*-


import socket
from KBEDebug import *
import urllib.parse
import mxytool


"""
Payment callback processing module
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
            self.processData(sock, data)
            KBEngine.deregisterReadFileDescriptor(sock.fileno())
            sock.close()
            del self._clients[fileno]

    def processData(self, sock, datas):
        """
        Process received data
        """
        try:
            usData = urllib.parse.unquote(datas.decode('utf-8'))
            sock.sendall("success".encode())
            idx = usData.index("out_trade_no=")
            sOrderNo = usData[idx + 13:idx + 13 + 17]
            self.OnPay(sOrderNo)
        except:
            pass

    def OnPay(self, sOrderNo):
        """
        Payment order processing function
        :param sOrderNo: Internal order number
        """
        def OnCreatePlayer(player, _, bOnline):
            if not player:
                return

            for idx in range(len(player.lOrder)):
                if player.lOrder[idx]["id"] == sOrderNo and player.lOrder[idx]["state"] == 0:
                    player.lOrder[idx]["state"] = 1
                    tp = player.lOrder[idx]["type"]
                    # todo: add your code
                    break
            if not bOnline:
                player.destroy()

        gmid = sOrderNo[-7:]
        dbid = mxytool.GetDbidByGameID(gmid)
        if dbid:
            # Create entity
            KBEngine.createEntityFromDBID("Account", dbid, OnCreatePlayer)
