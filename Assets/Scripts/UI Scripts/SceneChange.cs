using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    private int SceneNumber;
    private string TargetSceneName;
    [SerializeField] private Fade mainFade;

    
    public void CurrentSceneChange()
    {
        mainFade.FadeOutIn();
        Invoke("CurrentScene", 1.2f);
    }
    public void CurrentScene()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
            TargetSceneName = "Village Scene";
        else if (SceneManager.GetActiveScene().name == "Village Scene")
            TargetSceneName = "Dungeon Scene";
        else if (SceneManager.GetActiveScene().name == "Dungeon Scene" )
            TargetSceneName = "Village Scene";
        
        
           
        SceneManager.LoadScene(TargetSceneName);
    }

}
