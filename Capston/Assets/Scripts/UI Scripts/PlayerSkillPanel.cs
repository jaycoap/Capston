using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject SkillPanel;
    bool ActiveSkillPanel = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ActiveSkillPanel = !ActiveSkillPanel;
            SkillPanel.SetActive(ActiveSkillPanel);
        }
    }
}
