using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

public class BackEndGameInfo : MonoBehaviour
{
    string firstKey = string.Empty;
    GameManager gameManager;
    public int q;
    public int w;
    public int e;
    public int r;
    public int t;
    public int u;   
    public int v;
    public int x;
    public int y;
    public int z;
    public int n;
    public int l;

    
    void update()
    {
        
    }
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

        q = charLevel;
        w = charEXP;
        e = charMaxEXP;
        r = charHP;
        t = charMP;
        u = charSTR;
        v = charINT;
        x = charFIT;
        y = charAPPoint;
        z = charMP;
        n = charMaxHP;
        l = charMaxMp;

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
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "404":
                    Debug.Log("???????? ???? tableName?? ????");
                    break;

                case "412":
                    Debug.Log("???????? ?? tableName?? ????");
                    break;

                case "413":
                    Debug.Log("?????? row( column???? ???? )?? 400KB?? ???? ????");
                    break;

                default:
                    Debug.Log("???? ???? ???? ????: " + BRO.GetMessage());
                    break;
            }
        }
    }

    public void OnClickGetPrivateContents()
    {
        BackendReturnObject BRO = Backend.GameInfo.GetPrivateContents("character");

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
            Debug.Log("???????? ??????????.");
            
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
            Debug.Log("???????? ????????.");
        }
        

    }

    void GetData(JsonData data)
    {
        //var nickname = data["name"][0];
        var character = data["character"];

       

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
                Debug.Log("???? ?????? ???????? ???????? ????????.");
                break;

            case "404":
                if (BRO.GetMessage().Contains("gamer not found"))
                {
                    Debug.Log("gamerIndate?? ???????? gamer?? indate?? ????");
                }
                else if (BRO.GetMessage().Contains("table not found"))
                {
                    Debug.Log("???????? ???? ??????");
                    
                }
                break;

            case "400":
                if (BRO.GetMessage().Contains("bad limit"))
                {
                    Debug.Log("limit ???? 100?????? ????");
                }

                else if (BRO.GetMessage().Contains("bad table"))
                {
                    // public Table ?????? ???? ?????? private Table ?? ???????? ?? ????
                    // private Table ?????? ???? ?????? public Table ?? ???????? ?? 
                    Debug.Log("?????? ?????? ???????? ?????????? ???? ????????.");
                }
                break;

            case "412":
                Debug.Log("?????????? ????????????.");
                break;

            default:
                Debug.Log("???? ???? ???? ????: " + BRO.GetMessage());
                break;

        }
    }
    /*public void OnClickGameInfoUpdate()
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
            Debug.Log("???? ????");
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "405":
                    Debug.Log("param?? partition, gamer_id, inDate, updatedAt ?????? ?????? ???? ????");
                    break;

                case "403":
                    Debug.Log("?????????????? ?????????? ?????????? ?????? ????");
                    break;

                case "404":
                    Debug.Log("???????? ???? tableName?? ????");
                    break;

                case "412":
                    Debug.Log("???????? ?? tableName?? ????");
                    break;

                case "413":
                    Debug.Log("?????? row( column???? ???? )?? 400KB?? ???? ????");
                    break;
            }
        }

    }*/

}
