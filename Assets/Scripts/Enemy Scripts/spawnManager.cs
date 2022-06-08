using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject dummy;
    public GameObject boss;
    GameManager gameManager;
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        Instantiate(dummy, new Vector2(10, 10), Quaternion.identity);
        Instantiate(dummy, new Vector2(20, 10), Quaternion.identity);
        Instantiate(dummy, new Vector2(30, 10), Quaternion.identity);
    }

    

}
