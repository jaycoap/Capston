using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class BackEndOut : MonoBehaviour
{
    public InputField reasonInput;

    /*public void OnClickSignOut() //È¸¿øÅ»Åð
    {
        BackendReturnObject BRO = Backend.BMember.SignOut(reasonInput.text);

        if (BRO.IsSuccess())
        {
            Debug.Log(BRO.GetMessage());
        }
        else
        {
            BackEndSDK.MyInstance.ShowErrorUI(BRO);
        }
    }*/
    public void OnClickLogOut()
    {
        BackendReturnObject BRO = Backend.BMember.Logout();
        if (BRO.IsSuccess())
        {
            Debug.Log(BRO.GetMessage());
        }
        else
        {
            BackEndSDK.MyInstance.ShowErrorUI(BRO);
        }
    }
}
