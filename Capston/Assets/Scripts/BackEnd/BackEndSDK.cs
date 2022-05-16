using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class BackEndSDK : MonoBehaviour
{
    void Start()
    {   
        var bro = Backend.Initialize(true);
        if (bro.IsSuccess())
        {
            // 초기화 성공 시 로직
            Debug.Log("초기화 성공!");
        }
        else
        {
            // 초기화 실패 시 로직
            Debug.LogError("초기화 실패!");
        }
    }
    void Update()
    {
        Backend.AsyncPoll();
    }

    public void CustomSignUp()
    {
        string id = "test1";
        string password = "1234";

        var bro = Backend.BMember.CustomSignUp(id, password);
        if (bro.IsSuccess())
        {
            Debug.Log("가입성공");
        }
        else
        {
            Debug.LogError("실패!");
            Debug.Log(bro);
        }
    }
}
