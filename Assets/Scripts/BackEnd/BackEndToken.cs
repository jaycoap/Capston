using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class BackEndToken : MonoBehaviour
{
    public void OnClickRefreshToken()
    {
        //토큰을 재발급 받는 코드
        Backend.BMember.RefreshTheBackendToken();
    }

    public bool OnClickIsTokenAlive()
    {
        //유효한 토큰이면 true, 아니면 false 리턴
        return Backend.BMember.IsAccessTokenAlive().GetMessage() == "Success"?true:false;
    }
}
