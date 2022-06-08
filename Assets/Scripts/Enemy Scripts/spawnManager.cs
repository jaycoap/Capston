using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
    public GameObject[] enemy;
    /*
            0: Slime
            1: SlimeBoss
    */
    
    public void SponEnemy(int enemy_code, int num, Vector2 pos)
    {
        Instantiate(enemy[enemy_code], new Vector2(pos.x, pos.y), Quaternion.identity);
    }
}
