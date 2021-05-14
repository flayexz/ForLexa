using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Gun : MonoBehaviour, IGun
{
    public float Offset;
    public GameObject Bullet;
    public Transform ShotPoint;

    private float currentTimeBetweenShoot;
    public float TimeBetweenShotForGun;
    private AudioSource sound;
    public bool InHandsPlayer;
    [SerializeField] private int ammo;
    public int currentAmmo;

    [SerializeField] private string name;

    public string Name => name;
    

    void Start()
    {
        currentAmmo = ammo;
        sound = GetComponent<AudioSource>();
        currentTimeBetweenShoot = TimeBetweenShotForGun;
    }

    void Update()
    {
        
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
            if (Input.GetMouseButton(0) && currentAmmo > 0)
            {
                Instantiate(Bullet, ShotPoint.position, transform.rotation);
                if(sound != null && !sound.isPlaying)
                    sound.Play();
                currentAmmo--; 
                currentTimeBetweenShoot = 0;
            }
            else if (sound != null)
                sound.Stop();
        }
        else
            currentTimeBetweenShoot += Time.fixedDeltaTime;
    }

    public void AddCartiges()
    {
        currentAmmo += ammo;
    }
}
