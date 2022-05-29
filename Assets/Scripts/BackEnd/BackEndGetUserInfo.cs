using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class BackEndGetUserInfo : MonoBehaviour
{
    public void OnClickGetUserInfor()
    {
        BackendReturnObject BRO = Backend.BMember.GetUserInfo();

        if (BRO.IsSuccess())
        {
            Debug.Log(BRO.GetReturnValue());
        }
        else
        {
            Debug.Log("서버에러발생" + BRO.GetErrorCode());
        }
    }
}
