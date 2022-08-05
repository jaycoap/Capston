using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using UnityEngine.UI;
using System;


public class BackEndGameInfo : MonoBehaviour
{

    public static BackEndGameInfo instance;

    private void Awake() { instance = this; }

    string firstKey = string.Empty;
    GameManager gameManager;
    public int Level;
    public int EXP;
    public int MaxEXP;
    public int HP;
    public int MaxHP;
    public int MP;   
    public int MaxMP;
    public int STR;
    public int INT;
    public int FIT;
    public int APPoint;
    public string getIndate;
    public string getowner = "2022-07-25T05:47:22.446Z";
    public string getname = "2022-07-25T05:47:22.110Z";

    

    
    void Update()
    {
        GameManager.Instance.setLevel();
        GameManager.Instance.getLevel();
    }
    

    // 개인 프라이빗 테이블 정보 가져오기     
    public void OnClickInsertData()
    {
        int charLevel = GameManager.Instance.getLevel();
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
        BackendReturnObject BRO = Backend.GameData.Insert("character", param);
        

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

    
    public void OnClickGetPrivateContents()
    {
        BackendReturnObject BRO = Backend.GameData.GetMyData("character", new Where());
       
        if (BRO.IsSuccess())
        {
            GetGameInfo(BRO.GetReturnValuetoJSON());
            Debug.Log("indate:" + BRO.GetInDate());
            getIndate = BRO.GetInDate();
            

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
        
        Level = Int32.Parse(data["Level"][0].ToString());
        HP = Int32.Parse(data["HP"][0].ToString());
        MaxHP = Int32.Parse(data["MaxHP"][0].ToString());
        MP = Int32.Parse(data["MP"][0].ToString());
        MaxMP = Int32.Parse(data["MaxMP"][0].ToString());
        EXP = Int32.Parse(data["EXP"][0].ToString());
        MaxEXP = Int32.Parse(data["MaxEXP"][0].ToString());
        STR = Int32.Parse(data["STR"][0].ToString());
        INT = Int32.Parse(data["INT"][0].ToString());
        FIT = Int32.Parse(data["FIT"][0].ToString());
        APPoint = Int32.Parse(data["APPoint"][0].ToString());

        SetDBLevel(Level);
    }

    public void OnClickGameInfoUpdate()
    {
        Param param = new Param();
        param.Add("Level", GameManager.Instance.getLevel());
        param.Add("HP", GameManager.Instance.getHp());
        param.Add("MaxHP", GameManager.Instance.getmaxHp());
        param.Add("MP", GameManager.Instance.getMp());
        param.Add("EXP", GameManager.Instance.getExp());
        param.Add("MaxEXP", GameManager.Instance.getmaxExp());
        param.Add("STR", GameManager.Instance.getSTR());
        param.Add("INT", GameManager.Instance.getINT());
        param.Add("FIT", GameManager.Instance.getFIT());
        param.Add("APPoint", GameManager.Instance.getAPPoint());

        BackendReturnObject BRO = Backend.GameData.Update("character", getIndate, param);

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

    public int GetLevel()
    {
        
        return Level;
    }
    public int GetHP()
    {
        return HP;
    }
    public int GetMaxHP()
    {
        return MaxHP;
    }
    public int GetMP()
    {
        return MP;
    }
    public int GetMaxMP()
    {
        return MaxMP;
    }
    public int GetEXP()
    {
        return EXP;
    }
    public int GetMaxEXP()
    {
        return MaxEXP;
    }
    public int GetSTR()
    {
        return STR;
    }
    public int GetINT()
    {
        return INT;
    }
    public int GetFIT()
    {
        return FIT;
    }
    public int GetAPPoint()
    {
        return APPoint;
    }

    public int SetDBLevel(int DBLevel)
    {
        Level = DBLevel;
        return Level;
    }
    

}
