using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UnityEngine.SceneManagement;


public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    private void Awake(){ instance = this; }

    // 스테이지 설정 변수
    public int startStage = 0;

    public int clearStage = 0;

    public int DBStage = 0;

    

    public string stagegetIndate;


    void Update()
    {
        
         GameManager.Instance.GetClearStage();
        
        
    }
    public void OnClickStageInsertData()
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
            stagegetIndate = BRO.GetInDate();

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

    public void OnClickGetPrivateContents()
    {
        BackendReturnObject BRO = Backend.GameData.GetMyData("stage", new Where());

        if (BRO.IsSuccess())
        {
            GetGameInfo(BRO.GetReturnValuetoJSON());
            Debug.Log("indate:" + BRO.GetInDate());
            stagegetIndate = BRO.GetInDate();


        }
        else
        {
            CheckError(BRO);
        }
    }

    void GetGameInfo(JsonData returnData)
    {

        if (returnData != null)
        {
            Debug.Log("데이터가 존재합니다.");

            if (returnData.Keys.Contains("rows"))
            {
                Debug.Log("Pass");
                JsonData rows = returnData["rows"];
                for (int i = 0; i < rows.Count; i++)
                {
                    GetData(rows[i]);
                }
            }


            // row 로 전달받은 경우
            else if (returnData.Keys.Contains("rows"))
            {
                JsonData row = returnData["row"];
                Debug.Log("Check");
                GetData(row[0]);
            }
        }
        else
        {
            Debug.Log("데이터가 없습니다.");
        }


    }

    public void GetData(JsonData data)
    {

        DBStage = Int32.Parse(data["ClearStage"][0].ToString());
        Debug.Log(DBStage);
        SetDBStage(DBStage);
       
    }

    public void OnClickStageGameInfoUpdate()
    {
        Param param = new Param();
        int UpdateStage = GameManager.Instance.GetClearStage();
        

        param.Add("ClearStage", UpdateStage);

        Dictionary<string, int> stage = new Dictionary<string, int>
        {
            {"ClearStage",UpdateStage }
        };
        

        param.Add("stage", stage);
        Debug.Log(param);
        BackendReturnObject BRO1 = Backend.GameData.GetMyData("stage", new Where());
        BackendReturnObject BRO = Backend.GameData.Update("stage", BRO1.GetInDate(), param);

        if (BRO.IsSuccess())
        {
            Debug.Log("indate:" + BRO.GetInDate());
            Debug.Log("수정완료");
            

        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "405":
                    Debug.Log("param에 partition, gamer_id, inDate, updatedAt 네가지 필드가 있는 경우");
                    break;

                case "403":
                    Debug.Log("퍼블릭테이블의 타인정보를 수정하고자 하였을 경우");
                    break;

                case "404":
                    Debug.Log("존재하지 않는 tableName인 경우");
                    break;

                case "412":
                    Debug.Log("비활성화 된 tableName인 경우");
                    break;

                case "413":
                    Debug.Log("하나의 row( column들의 집합 )이 400KB를 넘는 경우");
                    break;
            }
        }

    }

    public int GetDBStage()
    {
        return DBStage;
    }
    public int SetDBStage(int stage)
    {
        DBStage = stage;
        return DBStage;
    }

    void CheckError(BackendReturnObject BRO)
    {
        switch (BRO.GetStatusCode())
        {
            case "200":
                Debug.Log("해당 유저의 데이터가 테이블에 없습니다.");
                break;

            case "404":
                if (BRO.GetMessage().Contains("gamer not found"))
                {
                    Debug.Log("gamerIndate가 존재하지 gamer의 indate인 경우");
                }
                else if (BRO.GetMessage().Contains("table not found"))
                {
                    Debug.Log("존재하지 않는 테이블");

                }
                break;

            case "400":
                if (BRO.GetMessage().Contains("bad limit"))
                {
                    Debug.Log("limit 값이 100이상인 경우");
                }

                else if (BRO.GetMessage().Contains("bad table"))
                {
                    // public Table 정보를 얻는 코드로 private Table 에 접근했을 때 또는
                    // private Table 정보를 얻는 코드로 public Table 에 접근했을 때 
                    Debug.Log("요청한 코드와 테이블의 공개여부가 맞지 않습니다.");
                }
                break;

            case "412":
                Debug.Log("비활성화된 테이블입니다.");
                break;

            default:
                Debug.Log("서버 공통 에러 발생: " + BRO.GetMessage());
                break;

        }
    }

}
