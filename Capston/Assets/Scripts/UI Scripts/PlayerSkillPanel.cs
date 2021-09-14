using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject SkillPanel;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (SkillPanel.activeSelf == false)
            {
                SkillPanel.SetActive(true);
            }
            else
            {
                SkillPanel.SetActive(false);
            }
        }
    }
}
