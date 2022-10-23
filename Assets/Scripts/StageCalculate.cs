using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCalculate : MonoBehaviour
{
    public static StageCalculate instance { get { return _instance; } }
    private static StageCalculate _instance;

    public int DBsave;
    public int nextStage;
    public int currentStage;

    int Stagetest()
    {
        DBsave = StageManager.instance.GetDBStage();
       
        return DBsave ;
    }


    public int StageCal()
    {
        Stagetest();
        nextStage = GameManager.Instance.GetClearStage();
        
        if (DBsave > nextStage)
        {
            currentStage = DBsave;
            
        }
        else if (DBsave < nextStage)
        {
            currentStage = nextStage;
        }
        else
        {
            currentStage = DBsave;
        }
        return currentStage;
    }


}
