using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageHUD : MonoBehaviour
{
    TextMeshProUGUI damageText;
    RectTransform hudRectTrans;

    Vector3 targetPos;
    [SerializeField]
    Vector3 additionalPos;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float maxAnimTime;

    void Awake()
    {
        damageText = GetComponent<TextMeshProUGUI>();
        hudRectTrans = GetComponent<RectTransform>();
    }

    public void Show(Vector3 worldPos, string damage)
    {
        hudRectTrans.position = worldPos;
        damageText.text = damage;
        gameObject.SetActive(true);

        targetPos = new Vector3
        (
            hudRectTrans.position.x + additionalPos.x,
            hudRectTrans.position.y + additionalPos.y,
            0f
        );
        StartCoroutine(Coroutine_TextAnimation());
    }
    
    IEnumerator Coroutine_TextAnimation()
    {
        float timer = 0f;
        
        while (true)
        {
            timer += Time.deltaTime;
            hudRectTrans.position += moveSpeed * Time.deltaTime * targetPos.normalized;
            
            if (timer > maxAnimTime)
            {
                DamageHUDManager.Instance.HideDamageHUD(this);
                yield break;
            }

            yield return null;
        }
    }
}
