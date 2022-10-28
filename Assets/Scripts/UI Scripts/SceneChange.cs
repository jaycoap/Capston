using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    private int SceneNumber;
    public string TargetSceneName;
    private Fade mainFade;
    private playerManager Player;
    private cameraManager Camera;
    [SerializeField]
    private GameObject DungeonSt;
    public GameObject VillageSt;

    private void Start()
    {
        mainFade = this.gameObject.GetComponent<Fade>();
        Player = FindObjectOfType<playerManager>();
        Camera = FindObjectOfType<cameraManager>();
    }
    public void CurrentSceneChange()
    {
        mainFade.FadeOutIn();
        Invoke("CurrentScene", 1.2f);


    }
    public void CurrentScene()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            TargetSceneName = "Village Scene";
        }
            
        else if (SceneManager.GetActiveScene().name == "Village Scene")
        {
            TargetSceneName = "Dungeon Scene";
            //Player.transform.position = DungeonSt.transform.position;
            //Camera.transform.position = new Vector3(DungeonSt.transform.position.x, DungeonSt.transform.position.y, Camera.transform.position.z);
        }
            
        else if (SceneManager.GetActiveScene().name == "Dungeon Scene")
        {
            TargetSceneName = "Village Scene";
            Player.transform.position = VillageSt.transform.position;
            Camera.transform.position = new Vector3(VillageSt.transform.position.x, VillageSt.transform.position.y, Camera.transform.position.z);
        }
            
        
        SceneManager.LoadScene(TargetSceneName);
    }

}
