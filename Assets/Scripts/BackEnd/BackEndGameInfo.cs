using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

public class BackEndGameInfo : MonoBehaviour
{
    string firstKey = string.Empty;
    GameManager gameManager;
    public int DBLevel;
    public int DBEXP;
    public int DBMaxEXP;
    public int DBHP;
    public int DBMaxHP;
    public int DBMP;
    public int DBMaxMP;
    public int DBSTR;
    public int DBINT;
    public int DBFIT;
    public int DBAPPoint;
    void update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            OnClickGameInfoUpdate();
        }
    }
    public void OnClickInsertData()
    {
        int charLevel = GameManager.Instance.getLevel();
        string charname = GameManager.Instance.getName();
        int charEXP = GameManager.Instance.getExp();
        int charMaxEXP = GameManager.Instance.getmaxExp();
        int charHP = GameManager.Instance.getHp();
        int charMaxHP = GameManager.Instance.getmaxHp();
        int charMP = GameManager.Instance.getMp();
        int charMaxMp = GameManager.Instance.getmaxMp();
        int charSTR = GameManager.Instance.getSTR();
        int charINT = GameManager.Instance.getINT();
        int charFIT = GameManager.Instance.getFIT();
        int charAPPoint = GameManager.Instance.getAPPoint();

        Param param = new Param();

        param.Add("Level", charLevel);
        param.Add("Name", charname);
        param.Add("EXP", charEXP);
        param.Add("MaxEXP", charMaxEXP);
        param.Add("HP", charHP);
        param.Add("MaxHP", charMaxHP);
        param.Add("MP", charMP);
        param.Add("MaxMP", charMaxMp);
        param.Add("STR", charSTR);
        param.Add("INT", charINT);
        param.Add("FIT", charFIT);
        param.Add("APPoint", charAPPoint);

        Dictionary<string, int> character = new Dictionary<string, int>
        {
            { "Level",charLevel },
            { "EXP", charEXP },
            { "MaxEXP", charMaxEXP },
            { "HP", charHP },
            { "MaxHP", charMaxHP },
            { "MP", charMP },
            { "MaxMP", charMaxMp },
            { "STR",charSTR },
            { "INT",charINT },
            { "FIT",charFIT },
            { "APPoint", charAPPoint }
        };


        param.Add("character", character);
        BackendReturnObject BRO = Backend.GameData.Insert("character", param);
        

        if (BRO.IsSuccess())
        {
            Debug.Log("indate:" + BRO.GetInDate());
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
        BackendReturnObject BRO = Backend.GameData.Get("character", new Where(), 10);

        if (BRO.IsSuccess())
        {
            GetGameInfo(BRO.GetReturnValuetoJSON());
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
                JsonData rows = returnData["rows"];

                for (int i = 0; i < rows.Count; i++)
                {
                    
                    GetData(rows[i]);
                    
                }
                
            }
            
            else if (returnData.Keys.Contains("row"))
            {
                JsonData row = returnData["row"];
                GetData(row[0]);
            }
        }
        else
        {
            Debug.Log("데이터가 없습니다.");
        }
        

    }

    void GetData(JsonData data)
    {
        //var nickname = data["name"][0];
        var character = data["character"];
        DBLevel = (int)data["Level"][0];
        if (data.Keys.Contains("Level"))
        {
            Debug.Log(data["Level"][0]);
            Debug.Log(data["STR"][0]);
        }
        else
        {
            Debug.Log("Error");
        }

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
                    Debug.Log(DBLevel);
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
    public void OnClickGameInfoUpdate()
    {

        int charLevel = GameManager.Instance.getLevel();
        string charname = GameManager.Instance.getName();
        int charEXP = GameManager.Instance.getExp();
        int charMaxEXP = GameManager.Instance.getmaxExp();
        int charHP = GameManager.Instance.getHp();
        int charMaxHP = GameManager.Instance.getmaxHp();
        int charMP = GameManager.Instance.getMp();
        int charMaxMp = GameManager.Instance.getmaxMp();
        int charSTR = GameManager.Instance.getSTR();
        int charINT = GameManager.Instance.getINT();
        int charFIT = GameManager.Instance.getFIT();
        int charAPPoint = GameManager.Instance.getAPPoint();

        Param param = new Param();
        param.Add("Level", charLevel);
        param.Add("EXP", charEXP);
        param.Add("MaxEXP", charMaxEXP);
        param.Add("HP", charHP);
        param.Add("MaxHP", charMaxHP);
        param.Add("MP", charMP);
        param.Add("MaxMP", charMaxMp);
        param.Add("STR", charSTR);
        param.Add("INT", charINT);
        param.Add("FIT", charFIT);
        param.Add("APPoint", charAPPoint);

        Dictionary<string, int> character = new Dictionary<string, int>
        {
            { "Level",charLevel },
            { "EXP", charEXP },
            { "MaxEXP", charMaxEXP },
            { "HP", charHP },
            { "MaxHP", charMaxHP },
            { "MP", charMP },
            { "MaxMP", charMaxMp },
            { "STR",charSTR },
            { "INT",charINT },
            { "FIT",charFIT },
            { "APPoint", charAPPoint }
        };
        param.Add("character", character);
        BackendReturnObject BRO = Backend.GameInfo.Update("character", System.DateTime.Now.ToString("yyyy-MM-DD"), param);
        if (BRO.IsSuccess())
        {
            Debug.Log("수정 완료");
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

}
