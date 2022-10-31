using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class StageUI : MonoBehaviour
{
    private SceneChange IngameSceneChange;
    GameObject[] EnemyCount;
    [SerializeField]
    private GameObject ClearPanel;
    [SerializeField]
    private GameObject Subfade;
    private int EnemyCountNum = 0;
    private bool StageClear = false;
    

    [Space(8)]
    public Button _Stage1;
    public Button _Stage2;
    public Button _Stage3;
    public Button _Stage4;
    public Button _Stage5;
    public Button _Stage6;
    public Button _Stage7;
    public Button _Stage8;
    public Button GotoTown;
    [SerializeField]
    public int CurrentStage;

    private void Start()
    {
        IngameSceneChange = this.gameObject.GetComponent<SceneChange>();
        _Stage1.onClick.AddListener(() => StageChange(1));
        _Stage2.onClick.AddListener(() => StageChange(2));
        _Stage3.onClick.AddListener(() => StageChange(3));
        _Stage4.onClick.AddListener(() => StageChange(4));
        _Stage5.onClick.AddListener(() => StageChange(5));
        _Stage6.onClick.AddListener(() => StageChange(6));
        _Stage7.onClick.AddListener(() => StageChange(7));
        _Stage8.onClick.AddListener(() => StageChange(8));
        GotoTown.onClick.AddListener(() => Cleardungeon());
    }
    public void StageChange(int CrtStage)
    {
        if (CrtStage <= GameManager.Instance.GetClearStage() + 1)
        {
            IngameSceneChange.CurrentSceneChange();
            GameObject.Find("Dynamic UI").transform.Find("Stage Panel").gameObject.SetActive(false);
        }
        CurrentStage = CrtStage;
        Debug.Log(CurrentStage);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Dungeon Scene") //던전클리어시 나와야하는 이벤트 입력
        {
            StartCoroutine(EnemyCountCheck());
        }

        //if(GameManager.Instance.getHp() <= 0)
        //{
        //    Subfade.SetActive(true);
        //    GameManager.Instance.setPlayerHP(GameManager.Instance.getmaxHp());
        //    Invoke("Dead", 3.0f);
        //}
    }
    void Cleardungeon() // 던전 클리어시 씬 변경
    {
        GameManager.Instance.SetClearStage(CurrentStage);
        ClearPanel.SetActive(false);
        IngameSceneChange.CurrentSceneChange();
        Debug.Log(GameManager.Instance.GetClearStage());
    }

    public IEnumerator EnemyCountCheck()
    {
        EnemyCount = GameObject.FindGameObjectsWithTag("Enemy");
        EnemyCountNum = Int32.Parse(EnemyCount.Length.ToString());
        if (EnemyCountNum == 0)
        {
            StageClear = true;
            if (StageClear == true)
            {
                ClearPanel.SetActive(true);

                StageClear = false;
            }
        }
        else
        {
            EnemyCountNum = Int32.Parse(EnemyCount.Length.ToString());
        }

        yield break;
    }

    //private void Dead()
    //{
    //    Subfade.SetActive(false);
    //    IngameSceneChange.CurrentSceneChange();
    //}
}