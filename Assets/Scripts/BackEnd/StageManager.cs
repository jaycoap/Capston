using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;


public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    private void Awake(){ instance = this; }

    // 스테이지 설정 변수
    public int startStage = 0;

    public int clearStage = 0;

    public int endStage = 8;

    public string getIndate;


    public void OnClickInsertData()
    {
        int clearstage = clearStage;

        Param param = new Param();

        param.Add("ClearStage", clearstage);

        Dictionary<string, int> stage = new Dictionary<string, int>
        {
            {"ClearStage",clearstage }
        };

        param.Add("stage", stage);
        BackendReturnObject BRO = Backend.GameData.Insert("stage", param);

        if (BRO.IsSuccess())
        {
            Debug.Log("indate:" + BRO.GetInDate());
            getIndate = BRO.GetInDate();

        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "404":
                    Debug.Log("존재하지 않는 tableName인 경우");
                    break;

                case "412":
                    Debug.Log("비활성화 된 tableName인 경우");
                    break;

                case "413":
                    Debug.Log("하나의 row( column들의 집합 )이 400KB를 넘는 경우");
                    break;

                default:
                    Debug.Log("서버 공통 에러 발생: " + BRO.GetMessage());
                    break;
            }
        }
    }


}
