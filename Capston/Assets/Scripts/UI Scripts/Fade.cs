using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{

    [SerializeField] private Image FadePanel;
    private float time = 0f;
    private float F_time = 1.0f;
    // Start is called before the first frame update

    public void FadeOutIn()
    {
        StartCoroutine(FadeFlow());
    }

    IEnumerator FadeFlow()
    {
        FadePanel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = FadePanel.color;
        while (alpha.a < 1.0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            FadePanel.color = alpha;
            yield return null;
        }
        time = 0f;
        yield return new WaitForSeconds(1f);

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            FadePanel.color = alpha;
            yield return null;
        }
        FadePanel.gameObject.SetActive(false);
        yield return null;
    }
}
