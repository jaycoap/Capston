using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private Fade mainFade;

    public void GameLogin()
    {
        GameManager.Instance.Ingame();
        mainFade.FadeOutIn();
        Invoke("HideMainMenu", 1.2f); ;
    }

    public void LoadGame()
    {
       // 불러오기 기능구현 해야함.
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 유니티 에디터 상 종료
#else
        Application.Quit(); // 빌드시 종료
#endif
    }
    public void HideMainMenu()
    {
        MainMenuPanel.SetActive(false);
    }


}
