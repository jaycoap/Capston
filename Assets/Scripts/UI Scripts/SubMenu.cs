using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject Submenu;
    [SerializeField]
    private GameObject Subfade;
    bool ActiveSubMenu;
    // Start is called before the first frame update
    void Start()
    {
        ActiveSubMenu = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActiveSubMenu = !ActiveSubMenu;
            Submenu.SetActive(ActiveSubMenu);
            Subfade.SetActive(ActiveSubMenu);
        }
    }

    public void Save()
    {
        BackEndGameInfo.instance.OnClickGameInfoUpdate();
        StageManager.instance.OnClickStageGameInfoUpdate();
    }
}
