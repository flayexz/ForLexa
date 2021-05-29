using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Gun : MonoBehaviour, IGun
{
    public float Offset;
    public GameObject Bullet;
    public Transform ShotPoint;
    public Image weaponIcon;

    private float currentTimeBetweenShoot;
    public float TimeBetweenShotForGun;
    [SerializeField] private AudioSource shootSound;
    [SerializeField] private AudioSource soundWhenNoAmmo;
    public bool InHandsPlayer;
    [SerializeField] private int ammo;
    public  int Ammo => ammo;
    public int CurrentAmmo;

    [SerializeField] private string name;

    private Player player;

    private Bounds triggerZone;

    public string Name => name;
    

    void Start()
    {
        CurrentAmmo = ammo;
        player = FindObjectOfType<Player>();
        currentTimeBetweenShoot = TimeBetweenShotForGun;
        triggerZone = GetComponent<Collider2D>().bounds;
    }

    void Update()
    {
        if (triggerZone.Contains(player.transform.position) && Input.GetKeyDown(KeyCode.E))
            player.PickUpWeapon(this);
    }
    
    private void FixedUpdate()
    {
        if (InHandsPlayer)
        {
            RotateGun();
            TakeAShot();
        }
    }
    

    private void RotateGun()
    {
        var difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        var rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationZ + Offset);
    }

    public void TakeAShot()
    {
        if (currentTimeBetweenShoot >= TimeBetweenShotForGun )
        {
            if (Input.GetMouseButton(0))
            {
                if (CurrentAmmo > 0)
                {
                    Instantiate(Bullet, ShotPoint.position, transform.rotation);
                    if (shootSound != null && !shootSound.isPlaying)
                        shootSound.Play();
                    CurrentAmmo--;
                    currentTimeBetweenShoot = 0;
                }
                else
                    PlaySoundIfNoAmmo();
            }
            else
            {
                shootSound.Stop();
                soundWhenNoAmmo.Stop();
            }
        }
        else
            currentTimeBetweenShoot += Time.fixedDeltaTime;
    }

    private void PlaySoundIfNoAmmo()
    {
        if(shootSound.isPlaying)
            shootSound.Stop();
        if(!soundWhenNoAmmo.isPlaying)
            soundWhenNoAmmo.Play();
    }

    public void AddCartiges()
    {
        CurrentAmmo += ammo;
    }
}
