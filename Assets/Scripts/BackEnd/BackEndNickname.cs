using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using System.Text.RegularExpressions;


public class BackEndNickname : MonoBehaviour
{
    public InputField nickNameInput;

    //ÇÑ±Û,¿µ¾î,¼ıÀÚ¸¸ ÀÔ·Â
    private bool CheckNickName()
    {
        // ÇÑ±Û, ¿µ¾î, ¼ıÀÚ ÀÎÁö °Ë»ç
        return Regex.IsMatch(nickNameInput.text, "^[0-9a-zA-Z°¡-ÆR]*$");
    }

    public void OnclickCreateName()
    {
        // ÇÑ±Û, ¿µ¾î, ¼ıÀÚ·Î¸¸ ´Ğ³×ÀÓÀ» ¸¸µé¾ú´ÂÁö Ã¼Å©
        if (CheckNickName() == false)
        {
            Debug.Log("´Ğ³×ÀÓÀº ÇÑ±Û, ¿µ¾î, ¼ıÀÚ¸¸ °¡´É");
            return;
        }

        BackendReturnObject BRO = Backend.BMember.CreateNickname(nickNameInput.text);

        if (BRO.IsSuccess())
        {
            Debug.Log("´Ğ³×ÀÓ »ı¼º ¿Ï·á");
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "409":
                    Debug.Log("ÀÌ¹Ì Áßº¹µÈ ´Ğ³×ÀÓÀÌ ÀÖ´Â °æ¿ì");
                    break;

                case "400":
                    if (BRO.GetMessage().Contains("too long")) Debug.Log("20ÀÚ ÀÌ»óÀÇ ´Ğ³×ÀÓÀÎ °æ¿ì");
                    else if (BRO.GetMessage().Contains("blank")) Debug.Log("´Ğ³×ÀÓ¿¡ ¾Õ/µÚ °ø¹éÀÌ ÀÖ´Â°æ¿ì");
                    break;

                default:
                    Debug.Log("¼­¹ö °øÅë ¿¡·¯ ¹ß»ı: " + BRO.GetErrorCode());
                    break;
            }
        }
    
    }
}
