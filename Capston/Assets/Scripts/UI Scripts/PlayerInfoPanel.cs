using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject StatsPanel;
    bool ActiveInfoPanel = false;
    [SerializeField] private Text NameText;
    [SerializeField] private Text LevelText;
    [SerializeField] private Text StrText;
    [SerializeField] private Text IntText;
    [SerializeField] private Text FitText;
    [SerializeField] private Text HpText;
    [SerializeField] private Text MpText;
    [SerializeField] private Text ExpText;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            ActiveInfoPanel = !ActiveInfoPanel;
            StatsPanel.SetActive(ActiveInfoPanel);
        }

        UpdateInfoPanel();
    }

    private void UpdateInfoPanel()
    {
        NameText.text = GameManager.Instance.getName();
        LevelText.text = GameManager.Instance.getLevel();

        HpText.text = GameManager.Instance.getHp() + " / " + GameManager.Instance.getmaxHp();
        MpText.text = GameManager.Instance.getMp() + " / " + GameManager.Instance.getmaxMp();

        StrText.text = GameManager.Instance.getSTR().ToString();
        IntText.text = GameManager.Instance.getINT().ToString();
        FitText.text = GameManager.Instance.getFIT().ToString();

        ExpText.text = GameManager.Instance.getExp() + " / " + GameManager.Instance.getmaxExp();

    }
}
