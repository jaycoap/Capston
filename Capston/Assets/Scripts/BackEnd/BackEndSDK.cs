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

    
}
