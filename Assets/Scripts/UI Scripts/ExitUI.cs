using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitUI : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            for (int i = 0; i < 6; i++)
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
