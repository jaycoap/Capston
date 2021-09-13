using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject StatsPanel;
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(StatsPanel.activeSelf == false)
            {
                StatsPanel.SetActive(true);
            }
            else
            {
                StatsPanel.SetActive(false);
            }
        }
    }
}
