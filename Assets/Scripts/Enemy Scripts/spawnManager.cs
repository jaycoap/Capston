using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
    
    
    public GameObject[] enemy;
    public void SponEnemy(int enemy_code, Vector2 pos)
    {
        Instantiate(enemy[enemy_code], new Vector2(pos.x, pos.y), Quaternion.identity);
    }



}
