using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject dummy;
    public GameObject boss;
    GameManager gameManager;
    public GameObject[] enemy;
    public void SponEnemy(int enemy_code, int num, Vector2 pos)
    {
        Instantiate(enemy[enemy_code], new Vector2(pos.x, pos.y), Quaternion.identity);
    }



}
