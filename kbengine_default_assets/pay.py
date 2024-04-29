# -*- coding: utf-8 -*-


import requests
import hashlib
from KBEDebug import *


def DemoPay(para_list):
    key= "YOUR KEY"
    notify_url= "http:/IP_ADDRESS:PORT/"
    return_url= "http://XXX"
    sitename = 'XXX.COM'

    para_list['notify_url'] = notify_url
    para_list['return_url'] = return_url
    para_list['sitename'] = sitename
    para_list['sign'] = ""
    para_list['sign_type'] = "MD5"

    para_list_filter = {}
    for k,v in para_list.items():
        if k == "sign" or k == "sign_type" or v == "":
            continue
        else:
            para_list_filter[k] = v

    para_list_sorted = sorted(para_list_filter)

    para_str = ""
    for k in para_list_sorted:
       para_str += "&" + k + "=" + para_list_filter[k]

    sign_str = para_str[1:] + key
    sign = hashlib.md5(sign_str.encode(encoding='utf-8')).hexdigest()

    para_list['sign'] = sign

    url =  "https://XXX"
    headers = {"Content-Type":"application/x-www-form-urlencoded"}
    response_result = requests.post(url, data=(para_list), headers=headers)
    # status_code = response_result.status_code
    response_text = eval(response_result.text)

    ERROR_MSG(response_text)
    code = response_text['code']
    trade_no = response_text['trade_no']
    payurl = response_text['payurl']

    if code != 1:
        return {"ret": response_text['code']}

    payurl = payurl.replace("/", "")
    return payurl


def Pay(sOrderNo, sName, fMoney):
    dct = {
        "type": "PAY_TYPE",
        "out_trade_no": sOrderNo,
        "name": sName,
        "money": str(fMoney),
        "clientip": "YOUR IP"
    }
    return Alipay(dct)
