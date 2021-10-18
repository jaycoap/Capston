using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject SkillPanel;
    bool ActiveSkillPanel = false;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ActiveSkillPanel = !ActiveSkillPanel;
            SkillPanel.SetActive(ActiveSkillPanel);
        }
    }

    public void SkillQuit()
    {
        SkillPanel.SetActive(false);
    }
}
