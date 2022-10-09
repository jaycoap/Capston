using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
