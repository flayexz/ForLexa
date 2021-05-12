using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun1 : MonoBehaviour
{
    public float Offset;
    public GameObject Bullet;
    public Transform ShotPoint;

    private float currentTimeBetweenShoot;
    public float TimeBetweenShotForGun;
    private AudioSource bas;

    void Start()
    {
        bas = GetComponent<AudioSource>();
        currentTimeBetweenShoot = TimeBetweenShotForGun;
    }

    void Update()
    {
        RotateGun();
        TakeAShot();
    }

    private void RotateGun()
    {
        var difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        var rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationZ + Offset);
    }

    private void TakeAShot()
    {
        if (currentTimeBetweenShoot >= TimeBetweenShotForGun)
        {
            if (Input.GetMouseButton(0))
            {
                Instantiate(Bullet, ShotPoint.position, transform.rotation);
                bas.Play();
                currentTimeBetweenShoot = 0;
            }
        }
        else
            currentTimeBetweenShoot += Time.deltaTime;
    }
}
