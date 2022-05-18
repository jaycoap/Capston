using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageText : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float alphaSpeed;
    [SerializeField] private float destroyTime;

    TextMeshPro text;
    Color alpha;
    private int dmgText;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        text.text = dmgText.ToString();
        alpha = text.color;
        Invoke("DestoryObject", destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
    }

    private void DestoryObject()
    {
        Destroy(gameObject);
    }

    public void setDmgText(int damage)
    {
        dmgText = damage;
    }
}
