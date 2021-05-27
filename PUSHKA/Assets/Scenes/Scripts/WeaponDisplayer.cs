using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDisplayer : MonoBehaviour
{
    public Text ammoCount;
    private Player player;
    public Image additionalWeaponImage;
    public Image weaponsImage;
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    
    void Update()
    {
        if (player.CurrentGun != null)
        {
            ammoCount.text = $"Ammo: {player.CurrentGun.CurrentAmmo.ToString()}";
            weaponsImage.sprite = player.CurrentGun.weaponIcon.sprite;
        }

        if (player.additionalWeapon != null)
        {
            additionalWeaponImage.sprite = player.additionalWeapon.weaponIcon.sprite;
        }
    }
}
