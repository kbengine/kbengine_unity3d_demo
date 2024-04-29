# -*- coding: utf-8 -*-


import time
import uuid
import hashlib
import base64
import requests
from KBEDebug import *


"""
SMS service
"""


url = 'APP access address + interface access URI'
APP_KEY = "APP_Key"
APP_SECRET = "APP_Secret"

SENDER_CODE = "SMS signature channel number-Verification code class"
SENDER_NOTICE = "SMS signature channel number-Notification class"

TEMPLATE_ID_CODE = "Template ID"
TEMPLATE_ID_ORDER = "Template ID"
TEMPLATE_ID_MONEY = "Template ID"


def buildWSSEHeader(appKey, appSecret):
    """
    Construct X-WSSE parameter values
    @param appKey: string
    @param appSecret: string
    @return: string
    """
    now = time.strftime('%Y-%m-%dT%H:%M:%SZ')  # Created
    nonce = str(uuid.uuid4()).replace('-', '')  # Nonce
    digest = hashlib.sha256((nonce + now + appSecret).encode()).hexdigest()

    digestBase64 = base64.b64encode(digest.encode()).decode()  # PasswordDigest
    return 'UsernameToken Username="{}",PasswordDigest="{}",Nonce="{}",Created="{}"'.format(appKey, digestBase64, nonce, now)


def send_sms(sSender, sTels, sTemplateID, lArg):
    # Headers
    header = {'Authorization': 'WSSE realm="SDP",profile="UsernameToken",type="Appkey"',
              'X-WSSE': buildWSSEHeader(APP_KEY, APP_SECRET)}
    # Body
    formData = {'from': sSender,
                'to': sTels,
                'templateId': sTemplateID,
                'templateParas': str(lArg),
                'statusCallback': "",
                }
    #ERROR_MSG(header)

    r = requests.post(url, data=formData, headers=header, verify=False)
