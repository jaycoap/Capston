using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPoint : MonoBehaviour
{
    private playerManager Player;
    private cameraManager Camera;


    // Start is called before the first frame update
    void Start()
    {
            Player = FindObjectOfType<playerManager>();
            Camera = FindObjectOfType<cameraManager>();
            Player.transform.position = this.transform.position;
            Camera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, Camera.transform.position.z);
    }
}
