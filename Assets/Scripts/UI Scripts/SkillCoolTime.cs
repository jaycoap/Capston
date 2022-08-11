using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolTime : MonoBehaviour
{
    public Image SkillFilter;
    public Text CoolTimeCounter;
    public GameObject CoolTimeCounterText;
    public float CoolTime;
    private float CurrentCoolTime;


    void Start()
    {
        SkillFilter.fillAmount = 0;
    }

    public void UseSkill()
    {
        SkillFilter.fillAmount = 1;
        StartCoroutine("Cooltime");

        CurrentCoolTime = CoolTime;
        CoolTimeCounter.text = "" + CurrentCoolTime;

        StartCoroutine("CooltimeCounter");
    }

    IEnumerator Cooltime()
    {
        while(SkillFilter.fillAmount > 0)
        {
            SkillFilter.fillAmount -= 1 * Time.smoothDeltaTime / CoolTime;

            yield return null;
        }
        yield break;
    }

    IEnumerator CooltimeCounter()
    {
        CoolTimeCounterText.SetActive(true);
        while (CurrentCoolTime > 0)
        {
            yield return new WaitForSeconds(1.0f);

            CurrentCoolTime -= 1.0f;
            CoolTimeCounter.text = "" + CurrentCoolTime;
        }
        CoolTimeCounterText.SetActive(false);

        yield break;
    }
}
