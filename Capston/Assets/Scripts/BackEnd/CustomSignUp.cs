using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using UnityEngine.UI;

public class CustomSignUp : MonoBehaviour
{
    public InputField idInput;
    public InputField passInput;

    public void OnclickSignUp()
    {
        BackendReturnObject BRO = Backend.BMember.CustomSignUp(idInput.text, passInput.text, "test1");


        if (BRO.IsSuccess())
        {
            Debug.Log("회원가입완료");
        }
        else
        {
            string error = BRO.GetStatusCode();
            switch (error)
            {
                case "409":
                    Debug.Log("중복된 customId가 존재");
                    break;
                default:
                    Debug.Log("서버공통에러" + BRO.GetMessage());
                    break;
            }


        }
    }

    public void OnclickLogin()
    {
        BackendReturnObject BRO = Backend.BMember.CustomLogin(idInput.text, passInput.text);

        if (BRO.IsSuccess())
        {
            Debug.Log("로그인 완료");
        }

        else
        {
            string error = BRO.GetStatusCode();

            switch (error)
            {
                case "401":
                    Debug.Log("아이디 또는 비밀번호가 틀렸다.");
                    break;

                case "403":
                    Debug.Log("차단된 유저"+BRO.GetErrorCode());
                    break;
                default:
                    Debug.Log("서버공통에러" + BRO.GetMessage());
                    break;
            }
        }
    }
}
