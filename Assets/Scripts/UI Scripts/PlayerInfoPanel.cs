using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject StatsPanel;
    [SerializeField] private GameObject Button;
    [SerializeField] private Text NameText;
    [SerializeField] private Text LevelText;
    [SerializeField] private Text StrText;
    [SerializeField] private Text IntText;
    [SerializeField] private Text FitText;
    [SerializeField] private Text HpText;
    [SerializeField] private Text MpText;
    [SerializeField] private Text ExpText;
    [SerializeField] private Text APText;

    private bool ActiveStatsPanel = false;
    private bool ActiveButton = false;

    private void Start()
    {
    }
    //public void SetRectPosition(RectTransform InfoPanel)
    //{
    //    // 캔버스 스케일러에 따른 해상도 대응
    //    float wRatio = Screen.width / _canvasScaler.referenceResolution.x;
    //    float hRatio = Screen.height / _canvasScaler.referenceResolution.y;
    //    float ratio =
    //        wRatio * (1f - _canvasScaler.matchWidthOrHeight) +
    //        hRatio * (_canvasScaler.matchWidthOrHeight);

    //    float panelWidth = InfoPanel.rect.width * ratio;
    //    float panelHeight = InfoPanel.rect.height * ratio;

    //    _rt.position = InfoPanel.position + new Vector3(panelWidth, -panelHeight);
    //    Vector2 pos = _rt.position;

    //    float width = _rt.rect.width * ratio;
    //    float height = _rt.rect.height * ratio;
    //}

        void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            ActiveStatsPanel = !ActiveStatsPanel;
            StatsPanel.SetActive(ActiveStatsPanel);
        }

        UpdateInfoPanel();
        OnOffApButton();
    }

    private void UpdateInfoPanel()
    {
        NameText.text = GameManager.Instance.getName();
        LevelText.text = GameManager.Instance.getLevel().ToString();

        HpText.text = GameManager.Instance.getHp() + " / " + GameManager.Instance.getmaxHp();
        MpText.text = GameManager.Instance.getMp() + " / " + GameManager.Instance.getmaxMp();

        StrText.text = GameManager.Instance.getSTR().ToString();
        IntText.text = GameManager.Instance.getINT().ToString();
        FitText.text = GameManager.Instance.getFIT().ToString();

        ExpText.text = GameManager.Instance.getExp() + " / " + GameManager.Instance.getmaxExp();

        APText.text = GameManager.Instance.getAPPoint().ToString();

    }

    private void OnOffApButton()
    {
        if (1 <= GameManager.Instance.getAPPoint())
        {
            ActiveButton = true;
        }
        else
        {
            ActiveButton = false;
        }

        Button.SetActive(ActiveButton);
    }

    public void GmUpSTR()
    {
        GameManager.Instance.UpSTR();
    }

    public void GmUpFIT()
    {
        GameManager.Instance.UpFIT();
    }

    public void GmUpINT()
    {
        GameManager.Instance.UpINT();
    }
}
