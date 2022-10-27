using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextBlink : MonoBehaviour
{
    Text flashingText;
    float time = 0;

    void Start()
    {
        flashingText = GetComponent<Text>();
    }
    private void Update()
    {
       if(time < 0.5f)
        {
            flashingText.color = new Color(1, 1, 1, 1 - time);
        }
       else
        {
            flashingText.color = new Color(1, 1, 1, time);
            if(time > 1f)
            {
                time = 0;
            }
        }
        time += Time.deltaTime / 3;
    }
}