using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private SceneChange MainMenuChanage;
    [SerializeField] private GameObject StoryUI;
    [SerializeField] private GameObject SkipText;
    [SerializeField] private Fade StoryFade;

    private float StoryDownSpeed = 0f;


    RectTransform StoryUIRectTransform;

    private void Start()
    {
        MainMenuPanel.SetActive(false);
        StoryUIRectTransform = StoryUI.GetComponent<RectTransform>();
        StartCoroutine(RunMoveStoryUI());
    }

    public void GameLogin()
    {
        Invoke("HideMainMenu", 1.2f);
        MainMenuChanage.CurrentSceneChange();
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

    public void HideStoryUI()
    {
        StoryUI.SetActive(false);
        SkipText.SetActive(false);
    }

    IEnumerator RunMoveStoryUI()
    {
        while(StoryUIRectTransform.anchoredPosition.y < 2108f)
        {
            if (Input.GetKey(KeyCode.G))
            {
               break;
            }
            else if (Input.anyKey)
            {
                StoryDownSpeed = 15f;
            }
            else
            {
                StoryDownSpeed = 1.25f;
            }
            yield return new WaitForSeconds(0.02f);
            StoryUIRectTransform.anchoredPosition
                = new Vector2(StoryUIRectTransform.anchoredPosition.x
                , StoryUIRectTransform.anchoredPosition.y + 1f + StoryDownSpeed);
        }
        Invoke("HideStoryUI", 1.2f);
        StoryFade.FadeOutIn();
        MainMenuPanel.SetActive(true);
    }

}
