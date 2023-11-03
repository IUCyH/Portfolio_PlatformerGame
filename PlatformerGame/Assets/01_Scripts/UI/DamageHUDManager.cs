using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageHUDManager : Singleton<DamageHUDManager>
{
    Camera mainCam;
    [SerializeField]
    Canvas hudCanvas;
    [SerializeField]
    GameObject hudTextPrefab;
    ObjectPool<DamageHUD> hudTextPool;

    protected override void OnStart()
    {
        hudTextPool = new ObjectPool<DamageHUD>(3, () =>
        {
            var obj = Instantiate(hudTextPrefab);
            var damageHUD = obj.GetComponent<DamageHUD>();

            obj.transform.SetParent(hudCanvas.transform);
            obj.transform.position = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.SetActive(false);

            return damageHUD;
        });
        
        mainCam = Camera.main;
    }

    public void ShowDamageHUD(Vector3 pos, string damage)
    {
        var worldPos = mainCam.WorldToScreenPoint(pos);
        var hud = hudTextPool.Get();
        
        hud.Show(worldPos, damage);
    }

    public void HideDamageHUD(DamageHUD damageHUD)
    {
        damageHUD.gameObject.SetActive(false);
        hudTextPool.Set(damageHUD);
    }
}
